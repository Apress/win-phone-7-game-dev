using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using SLGameFramework;

namespace SettingsExample
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// When the application navigates to this page, put all the existing settings values
        /// into the form controls
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Put the current settings values into the form controls
            SelectListItem(difficultyList, SettingsManager.GetValue("Difficulty", "Normal"));
            SelectListItem(speedList, SettingsManager.GetValue("Speed", "Slow"));
            musicCheckbox.IsChecked = SettingsManager.GetValue("Music", true);
            soundCheckbox.IsChecked = SettingsManager.GetValue("Sound", true);
        }

        /// <summary>
        /// When navigating away from the page, save the form control values back to the settings
        /// </summary>
        protected override void OnNavigatingFrom(System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);

            // Save each setting
            SettingsManager.SetValue("Difficulty", GetListItemText(difficultyList.SelectedItem));
            SettingsManager.SetValue("Speed", GetListItemText(speedList.SelectedItem));
            SettingsManager.SetValue("Music", musicCheckbox.IsChecked.Value);
            SettingsManager.SetValue("Sound", soundCheckbox.IsChecked.Value);
        }

        /// <summary>
        /// Select one of the items within a list based on its item text
        /// </summary>
        /// <param name="list">The list whose item is to be selected</param>
        /// <param name="searchText">The text to search for</param>
        private void SelectListItem(ListBox list, string searchText)
        {
            // Loop for each list item
            foreach (object listitem in list.Items)
            {
                // Does the text match?
                if (GetListItemText(listitem) == searchText)
                {
                    // Yes, so select this item and return
                    list.SelectedItem = listitem;
                    return;
                }
            }
        }

        /// <summary>
        /// Returns the text of the provided listbox item.
        /// </summary>
        private string GetListItemText(object listitem)
        {
            // Is the item null? If so, return null.
            if (listitem == null) return null;

            // Is this a ListBoxItem?
            if (listitem is ListBoxItem)
            {
                // Yes, so return its content text
                return (listitem as ListBoxItem).Content.ToString();
            }
            else
            {
                // No, so call ToString on the item itself
                return listitem.ToString();
            }
        }

    }
}