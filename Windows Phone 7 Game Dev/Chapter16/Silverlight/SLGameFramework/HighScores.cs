using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SLGameFramework
{
    public class HighScores
    {
        // A Dictionary of all the known highscore tables.
        private Dictionary<string, HighScoreTable> _highscoreTables;

        //-------------------------------------------------------------------------------------
        // Class constructor

        public HighScores()
        {
            // Set default property values
            FileName = "Scores.dat";

            // Initialize the highscore tables
            Clear();
        }

        //-------------------------------------------------------------------------------------
        // Property access

        /// <summary>
        /// The filename to and from which the highscore data will be written.
        /// This can be either a fully specified path and filename, or just
        /// a filename alone (in which case the file will be written to the
        /// game engine assembly directory).
        /// </summary>
        public string FileName { get; set; }

        //-------------------------------------------------------------------------------------
        // Class functions

        /// <summary>
        /// Initialize a named high score table
        /// </summary>
        /// <param name="tableName">The name for the table to initialize</param>
        /// <param name="tableSize">The number of entries to store in this table</param>
        public void InitializeTable(string tableName, int tableSize)
        {
            // Delegate to the other version of this function
            InitializeTable(tableName, tableSize, "");
        }
        /// <summary>
        /// Initialize a named high score table
        /// </summary>
        /// <param name="tableName">The name for the table to initialize</param>
        /// <param name="tableSize">The number of entries to store in this table</param>
        /// <param name="tableDescription">A description of this table to show to the player</param>
        public void InitializeTable(string tableName, int tableSize, string tableDescription)
        {
            if (!_highscoreTables.ContainsKey(tableName))
            {
                _highscoreTables.Add(tableName, new HighScoreTable(tableSize, tableDescription));
            }
        }

        /// <summary>
        /// Retrieve the high score table with the specified name
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public HighScoreTable GetTable(string tableName)
        {
            if (_highscoreTables.ContainsKey(tableName))
            {
                return _highscoreTables[tableName];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Remove all high score tables from the object
        /// </summary>
        /// <remarks>To clear the scores for an individual table, retrieve the
        /// table object using GetTable and call the Clear method on that instead.</remarks>
        public void Clear()
        {
            // Create the table dictionary if it doesn't already exist
            if (_highscoreTables == null)
            {
                _highscoreTables = new Dictionary<string, HighScoreTable>();
            }

            // Tell any known tables to clear their content
            foreach (HighScoreTable table in _highscoreTables.Values)
            {
                table.Clear();
            }
        }

        /// <summary>
        /// Load the high scores from the storage file
        /// </summary>
        /// <remarks>Ensure that the tables have been created using InitializeTable
        /// prior to loading the scores.</remarks>
        public void LoadScores()
        {
            string fileContent;
            HighScoreTable table;

            // Just in case we have any problems...
            try
            {
                // Clear any existing scores
                Clear();

                // Get access to the isolated storage
                using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (!store.FileExists(FileName))
                    {
                        // The score file doesn't exist
                        return;
                    }
                    // Read the contents of the file
                    using (StreamReader sr = new StreamReader(store.OpenFile(FileName, FileMode.Open)))
                    {
                        fileContent = sr.ReadToEnd();
                    }
                }

                // Parse the content XML that was loaded
                XDocument xDoc = XDocument.Parse(fileContent);
                // Create a query to read the score details from the xml
                var result = from c in xDoc.Root.Descendants("entry")
                                select new
                                {
                                    TableName = c.Parent.Parent.Element("name").Value,
                                    Name = c.Element("name").Value,
                                    Score = c.Element("score").Value,
                                    Date = c.Element("date").Value
                                };
                // Loop through the resulting elements
                foreach (var el in result)
                {
                    // Add the entry to the table.
                    table = GetTable(el.TableName);
                    if (table != null) table.AddEntry(el.Name, int.Parse(el.Score), DateTime.Parse(el.Date));
                }
            }
            catch
            {
                // A problem occurred, but don't re-throw the exception or the
                // user won't be able to relaunch the game. Instead just ignore
                // the error and carry on regardless.
                // We will ensure that a partial load hasn't taken place however
                // which could cause unexpected problems, we'll reset back to defaults
                // instead.
                Clear();
            }
        }

        /// <summary>
        /// Save the scores to the storage file
        /// </summary>
        public void SaveScores()
        {
            StringBuilder sb = new StringBuilder();
            XmlWriter xmlWriter = XmlWriter.Create(sb);
            HighScoreTable table;

            // Begin the document
            xmlWriter.WriteStartDocument();
            // Write the HighScores root element
            xmlWriter.WriteStartElement("highscores");

            // Loop for each table
            foreach (string tableName in _highscoreTables.Keys)
            {
                // Retrieve the table object for this table name
                table = _highscoreTables[tableName];

                // Write the Table element
                xmlWriter.WriteStartElement("table");
                // Write the table Name element
                xmlWriter.WriteStartElement("name");
                xmlWriter.WriteString(tableName);
                xmlWriter.WriteEndElement();    // name

                // Create the Entries element
                xmlWriter.WriteStartElement("entries");

                // Loop for each entry
                foreach (HighScoreEntry entry in table.Entries)
                {
                    // Make sure the entry is not blank
                    if (entry.Date != DateTime.MinValue)
                    {
                        // Write the Entry element
                        xmlWriter.WriteStartElement("entry");
                        // Write the score, name and date
                        xmlWriter.WriteStartElement("score");
                        xmlWriter.WriteString(entry.Score.ToString());
                        xmlWriter.WriteEndElement();    // score
                        xmlWriter.WriteStartElement("name");
                        xmlWriter.WriteString(entry.Name);
                        xmlWriter.WriteEndElement();    // name
                        xmlWriter.WriteStartElement("date");
                        xmlWriter.WriteString(entry.Date.ToString("yyyy-MM-ddTHH:mm:ss"));
                        xmlWriter.WriteEndElement();    // date
                        // End the Entry element
                        xmlWriter.WriteEndElement();    // entry
                    }
                }

                // End the Entries element
                xmlWriter.WriteEndElement();    // entries

                // End the Table element
                xmlWriter.WriteEndElement();    // table
            }

            // End the root element
            xmlWriter.WriteEndElement();    // highscores
            xmlWriter.WriteEndDocument();

            // Close the xml writer, which will put the finished document into the stringbuilder
            xmlWriter.Close();

            // Get access to the isolated storage
            using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                // Create a file and attach a streamwriter
                using (StreamWriter sw = new StreamWriter(store.CreateFile(FileName)))
                {
                    // Write the XML string to the streamwriter
                    sw.Write(sb.ToString());
                }
            }
        }


        /// <summary>
        /// Display the high scores in the provided grid
        /// </summary>
        /// <param name="scoresGrid">The grid inside which the scores are to be displayed</param>
        /// <param name="tableName">The name of the high score table whose scores are to be shown</param>
        public void ShowScores(Grid scoresGrid, string tableName)
        {
            ShowScores(scoresGrid, tableName, -1);
        }
        /// <summary>
        /// Display the high scores in the provided grid, highlighting the score at the specified index
        /// </summary>
        /// <param name="scoresGrid">The grid inside which the scores are to be displayed</param>
        /// <param name="tableName">The name of the high score table whose scores are to be shown</param>
        /// <param name="highlightIndex">The index (from 0 to Entries-1) of a newly-added score, which will be highlighted.
        /// Pass as -1 for no highlight.</param>
        public void ShowScores(Grid scoresGrid, string tableName, int highlightIndex)
        {
            TextBlock highlightedName = null;

            // Get a reference to the specified table
            HighScoreTable table = GetTable(tableName);

            //Clear any controls contained in the grid
            scoresGrid.Children.Clear();

            // Reset the grid columns
            scoresGrid.ColumnDefinitions.Clear();
            scoresGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            scoresGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            scoresGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });

            // Reset the grid rows
            scoresGrid.RowDefinitions.Clear();

            // Loop for each high score table entry
            for (int i = 0; i < table.Entries.Count; i++)
            {
                // Add a new auto-sized row for this line of the scores
                scoresGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

                // Create the position textblock
                TextBlock scoreText = new TextBlock();
                scoreText.Text = (i + 1).ToString() + ". ";
                scoreText.TextAlignment = TextAlignment.Right;
                // Add the textblock to the grid and set its position
                scoresGrid.Children.Add(scoreText);
                Grid.SetRow(scoreText, i);
                Grid.SetColumn(scoreText, 0);

                // Create the name textblock
                scoreText = new TextBlock();
                scoreText.Text = table.Entries[i].Name;
                // Add the textblock to the grid and set its position
                scoresGrid.Children.Add(scoreText);
                Grid.SetRow(scoreText, i);
                Grid.SetColumn(scoreText, 1);
                // Is this the name to highlight?
                if (i == highlightIndex)
                {
                    // Yes, so store a reference to this textblock so we can animate it later
                    highlightedName = scoreText;
                    // Set the name to be in bold text to make it stand out
                    scoreText.FontWeight = FontWeights.Bold;
                }

                // Create the score textblock
                scoreText = new TextBlock();
                scoreText.Text = table.Entries[i].Score.ToString();
                scoreText.TextAlignment = TextAlignment.Right;
                // Add the textblock to the grid and set its position
                scoresGrid.Children.Add(scoreText);
                Grid.SetRow(scoreText, i);
                Grid.SetColumn(scoreText, 2);
            }

            // Did we find a name to highlight?
            if (highlightedName != null)
            {
                // Yes, so create a storyboard and a double animation to fade its opacity
                Storyboard scoreFader = new Storyboard();
                DoubleAnimation faderAnim = new DoubleAnimation();

                // Set the target object and target property of the fader animation
                Storyboard.SetTarget(faderAnim, highlightedName);
                Storyboard.SetTargetProperty(faderAnim, new PropertyPath("Opacity"));
                // Set the other animation parameters
                faderAnim.Duration = new Duration(new TimeSpan(0, 0, 1));
                faderAnim.RepeatBehavior = RepeatBehavior.Forever;
                faderAnim.AutoReverse = true;
                faderAnim.From = 1;
                faderAnim.To = 0;
                // Add the fader to the storyboard
                scoreFader.Children.Add(faderAnim);
                // Add the storyboard to the grid
                scoresGrid.Resources.Add("scorefader", scoreFader);
                // Begin the storyboard
                scoreFader.Begin();
            }
        }
    }
}
