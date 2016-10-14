using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace SLGameFramework
{
    public static class SettingsManager
    {

        //-------------------------------------------------------------------------------------
        // Class functions

        /// <summary>
        /// Add a new setting or update a setting value
        /// </summary>
        /// <param name="settingName">The name of the setting to add or update</param>
        /// <param name="value">The new value for the setting</param>
        public static void SetValue(string settingName, string value)
        {
            // Convert the setting name to lower case so that names are case-insensitive
            settingName = settingName.ToLower();

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
        }
        /// <summary>
        /// Add or update a setting as an integer value
        /// </summary>
        public static void SetValue(string settingName, int value)
        {
            SetValue(settingName, value.ToString());
        }

        /// <summary>
        /// Add or update a setting as a float value
        /// </summary>
        public static void SetValue(string settingName, float value)
        {
            SetValue(settingName, value.ToString());
        }

        /// <summary>
        /// Add or update a setting as a bool value
        /// </summary>
        public static void SetValue(string settingName, bool value)
        {
            SetValue(settingName, value.ToString());
        }

        /// <summary>
        /// Add or update a setting as a date value
        /// </summary>
        public static void SetValue(string settingName, DateTime value)
        {
            SetValue(settingName, value.ToString("yyyy-MM-ddTHH:mm:ss"));
        }

        /// <summary>
        /// Add or update other object types
        /// </summary>
        /// <param name="settingName"></param>
        /// <param name="value"></param>
        public static void SetObjectValue(string settingName, object value)
        {
            // Convert the setting name to lower case so that names are case-insensitive
            settingName = settingName.ToLower();

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
        }


        /// <summary>
        /// Retrieve a setting value from the object
        /// </summary>
        /// <param name="settingName">The name of the setting to be retrieved</param>
        /// <param name="defaultValue">A value to return if the requested setting does not exist</param>
        public static string GetValue(string settingName, string defaultValue)
        {
            // Convert the setting name to lower case so that names are case-insensitive
            settingName = settingName.ToLower();

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
        }

        /// <summary>
        /// Retrieve a setting as an int value
        /// </summary>
        public static int GetValue(string settingName, int defaultValue)
        {
            return int.Parse(GetValue(settingName, defaultValue.ToString()));
        }

        /// <summary>
        /// Retrieve a setting as a float value
        /// </summary>
        public static float GetValue(string settingName, float defaultValue)
        {
            return float.Parse(GetValue(settingName, defaultValue.ToString()));
        }

        /// <summary>
        /// Retrieve a setting as a bool value
        /// </summary>
        public static bool GetValue(string settingName, bool defaultValue)
        {
            return bool.Parse(GetValue(settingName, defaultValue.ToString()));
        }

        /// <summary>
        /// Retrieve a setting as a date value
        /// </summary>
        public static DateTime GetValue(string settingName, DateTime defaultValue)
        {
            return DateTime.Parse(GetValue(settingName, defaultValue.ToString("yyyy-MM-ddTHH:mm:ss")));
        }

        /// <summary>
        /// Retrieve a setting value for other object types
        /// </summary>
        /// <param name="settingName">The name of the setting to be retrieved</param>
        /// <param name="defaultValue">A value to return if the requested setting does not exist</param>
        public static object GetObjectValue(string settingName, object defaultValue)
        {
            // Convert the setting name to lower case so that names are case-insensitive
            settingName = settingName.ToLower();

            // Does a setting with this name exist?
            if (IsolatedStorageSettings.ApplicationSettings.Contains(settingName))
            {
                // Yes, so return it
                return IsolatedStorageSettings.ApplicationSettings[settingName];
            }
            else
            {
                // No, so return the default value
                return defaultValue;
            }
        }


        /// <summary>
        /// Clear all current setting values
        /// </summary>
        public static void ClearValues()
        {
            IsolatedStorageSettings.ApplicationSettings.Clear();
        }

        /// <summary>
        /// Delete a setting value
        /// </summary>
        /// <param name="settingName">The name of the setting to be deleted</param>
        public static void DeleteValue(string settingName)
        {
            // Do we have this setting in the dictionary?
            if (IsolatedStorageSettings.ApplicationSettings.Contains(settingName.ToLower()))
            {
                // Yes, so remove it
                IsolatedStorageSettings.ApplicationSettings.Remove(settingName);
            }
        }

    }
}
