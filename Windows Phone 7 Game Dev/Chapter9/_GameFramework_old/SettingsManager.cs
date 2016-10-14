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

        // The dictionary into which all of our settings will be written
        private Dictionary<string, string> _settings = new Dictionary<string, string>();


        //-------------------------------------------------------------------------------------
        // Class constructor

        /// <summary>
        /// Class constructor. Scope is internal so external code cannot create instances.
        /// </summary>
        internal SettingsManager(GameHost game)
        {
            // Store the game reference
            _game = game;

            // Provide a sensible default filename
            FileName = "Settings.dat";
        }

        //-------------------------------------------------------------------------------------
        // Properties

        /// <summary>
        /// The filename to and from which the settings data will be written.
        /// This can be either a fully specified path and filename, or just
        /// a filename alone (in which case the file will be written to the
        /// game engine assembly directory).
        /// </summary>
        public string FileName { get; set; }


        //-------------------------------------------------------------------------------------
        // Class functions

        /// <summary>
        /// Add a new setting or update a setting value in the object
        /// </summary>
        /// <param name="SettingName">The name of the setting to add or update</param>
        /// <param name="Value">The new value for the setting</param>
        public void SetValue(string SettingName, string Value)
        {
            // Update the setting's value item if it already exists
            if (_settings.ContainsKey(SettingName.ToLower()))
            {
                _settings[SettingName.ToLower()] = Value;
            }
            else
            {
                // Add the value
                _settings.Add(SettingName.ToLower(), Value);
            }
        }
        /// <summary>
        /// Add or update a setting as an integer value
        /// </summary>
        public void SetValue(string SettingName, int Value)
        {
            SetValue(SettingName, Value.ToString());
        }

        /// <summary>
        /// Add or update a setting as a float value
        /// </summary>
        public void SetValue(string SettingName, float Value)
        {
            SetValue(SettingName, Value.ToString());
        }

        /// <summary>
        /// Add or update a setting as a bool value
        /// </summary>
        public void SetValue(string SettingName, bool Value)
        {
            SetValue(SettingName, Value.ToString());
        }

        /// <summary>
        /// Add or update a setting as a date value
        /// </summary>
        public void SetValue(string SettingName, DateTime Value)
        {
            SetValue(SettingName, Value.ToString("yyyy-MM-ddTHH:mm:ss"));
        }


        /// <summary>
        /// Retrieve a setting value from the object
        /// </summary>
        /// <param name="SettingName">The name of the setting to be retrieved</param>
        /// <param name="DefaultValue">A value to return if the requested setting does not exist</param>
        public string GetValue(string SettingName, string DefaultValue)
        {
            // Do we have this setting in the dictionary?
            if (_settings.ContainsKey(SettingName.ToLower()))
            {
                return _settings[SettingName.ToLower()];
            }
            // The setting does not exist, so return the DefaultValue
            return DefaultValue;
        }

        /// <summary>
        /// Retrieve a setting as an int value
        /// </summary>
        public int GetValue(string SettingName, int DefaultValue)
        {
            return int.Parse(GetValue(SettingName, DefaultValue.ToString()));
        }

        /// <summary>
        /// Retrieve a setting as a float value
        /// </summary>
        public float GetValue(string SettingName, float DefaultValue)
        {
            return float.Parse(GetValue(SettingName, DefaultValue.ToString()));
        }

        /// <summary>
        /// Retrieve a setting as a bool value
        /// </summary>
        public bool GetValue(string SettingName, bool DefaultValue)
        {
            return bool.Parse(GetValue(SettingName, DefaultValue.ToString()));
        }

        /// <summary>
        /// Retrieve a setting as a date value
        /// </summary>
        public DateTime GetValue(string SettingName, DateTime DefaultValue)
        {
            return DateTime.Parse(GetValue(SettingName, DefaultValue.ToString("yyyy-MM-ddTHH:mm:ss")));
        }

        /// <summary>
        /// Delete a setting value
        /// </summary>
        /// <param name="SettingName">The name of the setting to be deleted</param>
        public void DeleteValue(string SettingName)
        {
            // Do we have this setting in the dictionary?
            if (_settings.ContainsKey(SettingName.ToLower()))
            {
                _settings.Remove(SettingName.ToLower());
            }
        }


        public void RetrieveValues()
        {
            // Clear any existing values in the settings dictionary
            _settings.Clear();

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


        /// <summary>
        /// Save settings to a data file
        /// </summary>
        public void SaveSettings()
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
        /// Load settings from a data file
        /// </summary>
        public void LoadSettings()
        {
            string settingsContent;

            // Just in case we have any problems...
            try
            {
                // Clear any existing settings
                _settings.Clear();

                // Get access to the isolated storage
                using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (!store.FileExists(FileName))
                    {
                        // The settings file doesn't exist
                        return;
                    }
                    // Read the contents of the file
                    using (StreamReader sr = new StreamReader(store.OpenFile(FileName, FileMode.Open)))
                    {
                        settingsContent = sr.ReadToEnd();
                    }
                }

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

    }
}
