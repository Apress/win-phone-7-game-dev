using System;
using System.Collections.Generic;

namespace GameFramework
{
    public class HighScoreEntry : IComparer<HighScoreEntry>
    {
        
        //-------------------------------------------------------------------------------------
        // Class constructor

        /// <summary>
        /// Class constructor. Scope is internal so external code cannot create instances.
        /// </summary>
        internal HighScoreEntry()
        {
            // Set the default values for the new entry
            Name = "";
            Score = 0;
            Date = DateTime.MinValue;
        }

        //-------------------------------------------------------------------------------------
        // Property access

        /// <summary>
        /// Return the entry Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Return the entry Score
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Return the entry Date
        /// </summary>
        public DateTime Date { get; set; }

        //-------------------------------------------------------------------------------------
        // Class functions

        /// <summary>
        /// Compare two highscore entries. This provides a simple mechanism for sorting the
        /// entries into descending order for display.
        /// </summary>
        /// <param name="x">The first score entry to compare</param>
        /// <param name="y">The second score entry to compare</param>
        /// <returns>1 if x is greater than y, -1 if x is less than y, 0 of x and y are equal</returns>
        public int Compare(HighScoreEntry x, HighScoreEntry y)
        {
            // Is the score in x less than the score in y? If so, return 1
            if (x.Score < y.Score)
            {
                return 1;
            }
            // Is the score in x greater than the score in y? If so, return -1
            else if (x.Score > y.Score)
            {
                return -1;
            }
            else
            {
                // The scores match, so we will put the oldest one first
                if (x.Date < y.Date)
                {
                    return -1;
                }
                else if (x.Date > y.Date)
                {
                    return 1;
                }
                else
                {
                    // Scores and dates match to just keep the existing sort order
                    return 0;
                }
            }
        }

    }
}