using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace GameFramework
{
    public class SettingsManager
    {

        //-------------------------------------------------------------------------------------
        // Class variables

        // A reference to the game host object
        private GameHost _game;

#if WINDOWS
        // Declare a dictionary into which all of our settings will be written
        private Dictionary<string, string> _settings = new Dictionary<string, string>();
        // The name of the file in which the settings will be stored
        private const string FileName = "Settings.dat";
#endif

        //-------------------------------------------------------------------------------------
        // Class constructor

        /// <summary>
        /// Class constructor. Scope is internal so external code cannot create instances.
        /// </summary>
        internal SettingsManager(GameHost game)
        {
            // Store the game reference
            _game = game;

#if WINDOWS
            // Load any existing stored settings
            LoadSettings();
#endif
        }


        //-------------------------------------------------------------------------------------
        // Class functions

        /// <summary>
        /// Add a new setting or update a setting value
        /// </summary>
        /// <param name="settingName">The name of the setting to add or update</param>
        /// <param name="value">The new value for the setting</param>
        public void SetValue(string settingName, string value)
        {
            // Convert the setting name to lower case so that names are case-insensitive
            settingName = settingName.ToLower();

    #if WINDOWS_PHONE
            // Does a setting with this name already exist?
            if (IsolatedStorageSettings.ApplicationSettings.Contains(settingName))
            {
                // Yes, so update its value
                IsolatedStorageSettings.ApplicationSettings[settingName] = value;
            }
            else
            {
                // No, so add it
                IsolatedStorageSettings.ApplicationSettings.Add(settingName, value);
            }
    #else
            // Does this setting already exist in the dictionary?
            if (_settings.ContainsKey(settingName))
            {
                // Update the setting's value
                _settings[settingName] = value;
            }
            else
            {
                // Add the value
                _settings.Add(settingName, value);
            }
            // Save the settings
            SaveSettings();
    #endif
        }
        /// <summary>
        /// Add or update a setting as an integer value
        /// </summary>
        public void SetValue(string settingName, int value)
        {
            SetValue(settingName, value.ToString());
        }

        /// <summary>
        /// Add or update a setting as a float value
        /// </summary>
        public void SetValue(string settingName, float value)
        {
            SetValue(settingName, value.ToString());
        }

        /// <summary>
        /// Add or update a setting as a bool value
        /// </summary>
        public void SetValue(string settingName, bool value)
        {
            SetValue(settingName, value.ToString());
        }

        /// <summary>
        /// Add or update a setting as a date value
        /// </summary>
        public void SetValue(string settingName, DateTime value)
        {
            SetValue(settingName, value.ToString("yyyy-MM-ddTHH:mm:ss"));
        }


        /// <summary>
        /// Retrieve a setting value from the object
        /// </summary>
        /// <param name="settingName">The name of the setting to be retrieved</param>
        /// <param name="defaultValue">A value to return if the requested setting does not exist</param>
        public string GetValue(string settingName, string defaultValue)
        {
            // Convert the setting name to lower case so that names are case-insensitive
            settingName = settingName.ToLower();

#if WINDOWS_PHONE
            // Does a setting with this name exist?
            if (IsolatedStorageSettings.ApplicationSettings.Contains(settingName))
            {
                // Yes, so return it
                return IsolatedStorageSettings.ApplicationSettings[settingName].ToString();
            }
            else
            {
                // No, so return the default value
                return defaultValue;
            }
#else
            // Do we have this setting in the dictionary?
            if (_settings.ContainsKey(settingName))
            {
                return _settings[settingName];
            }
            // The setting does not exist, so return the DefaultValue
            return defaultValue;
#endif
        }

        /// <summary>
        /// Retrieve a setting as an int value
        /// </summary>
        public int GetValue(string settingName, int defaultValue)
        {
            return int.Parse(GetValue(settingName, defaultValue.ToString()));
        }

        /// <summary>
        /// Retrieve a setting as a float value
        /// </summary>
        public float GetValue(string settingName, float defaultValue)
        {
            return float.Parse(GetValue(settingName, defaultValue.ToString()));
        }

        /// <summary>
        /// Retrieve a setting as a bool value
        /// </summary>
        public bool GetValue(string settingName, bool defaultValue)
        {
            return bool.Parse(GetValue(settingName, defaultValue.ToString()));
        }

        /// <summary>
        /// Retrieve a setting as a date value
        /// </summary>
        public DateTime GetValue(string settingName, DateTime defaultValue)
        {
            return DateTime.Parse(GetValue(settingName, defaultValue.ToString("yyyy-MM-ddTHH:mm:ss")));
        }

        /// <summary>
        /// Clear all current setting values
        /// </summary>
        public void ClearValues()
        {
#if WINDOWS_PHONE
            // Clear the isolated storage settings
            IsolatedStorageSettings.ApplicationSettings.Clear();
#else
            // Clear the settings dictionary
            _settings.Clear();
            SaveSettings();
#endif
        }

        /// <summary>
        /// Delete a setting value
        /// </summary>
        /// <param name="settingName">The name of the setting to be deleted</param>
        public void DeleteValue(string settingName)
        {
#if WINDOWS_PHONE
            // Do we have this setting in the dictionary?
            if (IsolatedStorageSettings.ApplicationSettings.Contains(settingName.ToLower()))
            {
                // Yes, so remove it
                IsolatedStorageSettings.ApplicationSettings.Remove(settingName);
            }
#else
            // Do we have this setting in the dictionary?
            if (_settings.ContainsKey(settingName.ToLower()))
            {
                _settings.Remove(settingName.ToLower());
                SaveSettings();
            }
#endif
        }


#if WINDOWS
        /// <summary>
        /// Save settings to a data file
        /// </summary>
        private void SaveSettings()
        {
            StringBuilder sb = new StringBuilder();
            XmlWriter xmlWriter = XmlWriter.Create(sb);

            // Begin the document
            xmlWriter.WriteStartDocument();
            // Write the root node
            xmlWriter.WriteStartElement("settings");
            // Loop through the items in the settings dictionary, creating an element for each
            foreach (KeyValuePair<string, string> setting in _settings)
            {
                // Write the setting element
                xmlWriter.WriteStartElement("setting");

                xmlWriter.WriteStartElement("name");
                xmlWriter.WriteString(setting.Key);
                xmlWriter.WriteEndElement();    // name

                xmlWriter.WriteStartElement("value");
                xmlWriter.WriteString(setting.Value);
                xmlWriter.WriteEndElement();    // value

                xmlWriter.WriteEndElement();    // setting
            }
            // End the root node
            xmlWriter.WriteEndElement();    // settings
            // End the document
            xmlWriter.WriteEndDocument();

            // Close the xml writer, which will put the finished document into the stringbuilder
            xmlWriter.Close();

            // Write the data to a file
            System.IO.File.WriteAllText(FileName, sb.ToString());
        }

        /// <summary>
        /// Load settings from a data file
        /// </summary>
        private void LoadSettings()
        {
            string settingsContent;

            // Just in case we have any problems...
            try
            {
                // Clear any existing settings
                _settings.Clear();

                // Load the data file if it exists
                if (!System.IO.File.Exists(FileName))
                {
                    // The settings file doesn't exist
                    return;
                }

                // Read the contents of the file
                settingsContent = System.IO.File.ReadAllText(FileName);

                // Parse the content XML that was loaded
                XDocument xDoc = XDocument.Parse(settingsContent);
                // Create a query to read the names and values from the setting elements
                var result = from c in xDoc.Descendants("setting")
                             select new
                             {
                                 Name = c.Element("name").Value.ToString(),
                                 Value = c.Element("value").Value.ToString()
                             };
                // Loop through the resulting elements
                foreach (var el in result)
                {
                    // Apply the setting
                    SetValue(el.Name, el.Value);
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
                _settings.Clear();
            }
        }
#endif



        /// <summary>
        /// Read the values from all SettingsItemObjects contained within the
        /// game's GameObjects collection and store them all for later retrieval.
        /// </summary>
        public void RetrieveValues()
        {
            // Clear any existing values in the settings dictionary
            //_settings.Clear();

            // Loop through the GameObjects list looking for settings objects
            foreach (GameObjectBase obj in _game.GameObjects)
            {
                // Is this a setting object?
                if (obj is SettingsItemObject)
                {
                    // It is, so apply its value to the dictionary
                    SetValue(((SettingsItemObject)obj).Name, ((SettingsItemObject)obj).SelectedValue);
                }
            }
        }

    }
}
