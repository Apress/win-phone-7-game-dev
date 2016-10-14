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

namespace TrialMode
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            SetTrialModeDisplay();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void SetTrialModeDisplay()
        {
            // Are we in full or trial mode?
            if (App.IsTrialMode)
            {
                // Show the 'trial' content
                trialText.Visibility = System.Windows.Visibility.Visible;
                buyButton.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                // Hide the 'trial' content
                trialText.Visibility = System.Windows.Visibility.Collapsed;
                buyButton.Visibility = System.Windows.Visibility.Collapsed;
            }
        }


        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            // Work out what type of game to start
            if (App.IsTrialMode)
            {
                MessageBox.Show("Starting the trial version of the game...");
            }
            else
            {
                MessageBox.Show("Starting the full version of the game...");
            }
        }

        private void buyButton_Click(object sender, RoutedEventArgs e)
        {
            // Show the Marketplace
            Microsoft.Xna.Framework.GamerServices.Guide.ShowMarketplace(Microsoft.Xna.Framework.PlayerIndex.One);

            // In case we are in simulation mode, update the IsTrialMode property and the screen display
            App.RefreshTrialModeProperty();
            SetTrialModeDisplay();
        }

        private void PhoneApplicationPage_GotFocus(object sender, RoutedEventArgs e)
        {
            // In case we are in simulation mode, update the IsTrialMode property and the screen display
            App.RefreshTrialModeProperty();
            SetTrialModeDisplay();
        }
    }
}