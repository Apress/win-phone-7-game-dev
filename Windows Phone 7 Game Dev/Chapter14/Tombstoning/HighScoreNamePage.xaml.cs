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

namespace Tombstoning
{
    public partial class HighScoreNamePage : PhoneApplicationPage
    {
        public HighScoreNamePage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Prepare the page for use
        /// </summary>
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Read the most recently used name from the Settings
            nameText.Text = SettingsManager.GetValue("HighscoreName", "");
            // Initially focus on the textbox
            nameText.Focus();
            // Select any existing text so that it can be easily replaced if needed
            nameText.SelectAll();
        }


        /// <summary>
        /// The user pressed Enter in the textbox
        /// </summary>
        private void nameText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.PlatformKeyCode == 10)
            {
                AddName();
            }
        }

        /// <summary>
        /// The user clicked the Add Name appbar button
        /// </summary>
        private void addNameButton_Click(object sender, EventArgs e)
        {
            AddName();
        }

        /// <summary>
        /// The user clicked the Skip appbar button
        /// </summary>
        private void skipButton_Click(object sender, EventArgs e)
        {
            // Skip adding the name, go directly to the high score page
            MainPage.TargetGamePage = MainPage.GamePages.HighScorePage;
            NavigationService.GoBack();
        }

        /// <summary>
        /// Trigger the addition of the name to the high score table
        /// and navigate to HighScorePage
        /// </summary>
        private void AddName()
        {
            // Do we have a name?
            if (nameText.Text.Length > 0)
            {
                // Store the name into the game Settings
                SettingsManager.SetValue("HighscoreName", nameText.Text);
                // Add to the high score table and retrieve the index of the new position
                int newScoreIndex = HighScorePage.HighScores.GetTable("Default").AddEntry(nameText.Text, GamePage.GameState.Score);
                // Save the updated scores
                HighScorePage.HighScores.SaveScores();
                // Pass the new score index to the highscore page so we can highlight it
                MainPage.TargetGamePageParameters = "HighlightIndex=" + newScoreIndex.ToString();
            }
            // We need to navigate to the high score page...
            MainPage.TargetGamePage = MainPage.GamePages.HighScorePage;
            // Go back to the menu and allow it to redirect for us
            NavigationService.GoBack();
        }

    }
}