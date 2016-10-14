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

namespace Navigation
{
    public partial class MainPage : Page
    {

        //-------------------------------------------------------------------------------------
        // Class variables/constants

        /// <summary>
        /// An enumeration of all the pages that we can navigate to within the game.
        /// Note that the enumeration names must exactly match the page names.
        /// </summary>
        internal enum GamePages
        {
            MainPage = 0,
            GamePage,
            SettingsPage,
            HighScorePage,
            AboutPage,
        }


        //-------------------------------------------------------------------------------------
        // Constructors

        public MainPage()
        {
            InitializeComponent();
        }


        //-------------------------------------------------------------------------------------
        // Navigation functions

        /// <summary>
        /// The page that should next be opened when the menu page is navigated to
        /// </summary>
        static internal GamePages TargetGamePage { get; set; }
        static internal string TargetGamePageParameters { get; set; }

        /// <summary>
        /// Process navigation to this page from elsewhere in the game
        /// </summary>
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Show or hide the resume button depending on whether a game is active
            resumeButton.Visibility = (GamePage.GameState.IsGameActive ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed);

            // Navigate to whichever page has been set to be opened next
            NavigateToPage(TargetGamePage, TargetGamePageParameters);
            // Reset the target page and parameters to be the menu so that by default
            // when other pages navigate back, they return to the menu
            TargetGamePage = GamePages.MainPage;
            TargetGamePageParameters = null;
        }

        /// <summary>
        /// Navigate to the specified page
        /// </summary>
        private void NavigateToPage(GamePages toPage)
        {
            NavigateToPage(toPage, null);
        }
        /// <summary>
        /// Navigate to the specified page, passing the provided parameters
        /// </summary>
        private void NavigateToPage(GamePages toPage, string parameters)
        {
            string uriString;

            // Are we navigating to the menu page?
            if (toPage == GamePages.MainPage)
            {
                // We are already on the menu page, no navigation required
                // Show the menu so that it is displayed within the page
                this.Visibility = System.Windows.Visibility.Visible;
                return;
            }

            // Build the URI for navigation
            uriString = "/" + toPage.ToString() + ".xaml";
            // Include parameters if there are any
            if (!string.IsNullOrEmpty(parameters))
            {
                uriString += "?" + parameters;
            }

            // Navigate to the specified page
            NavigationService.Navigate(new Uri(uriString, UriKind.Relative));

            // Hide the page content so that it doesn't briefly appear when navigating
            // directly between other pages
            this.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void resumeButton_Click(object sender, RoutedEventArgs e)
        {
            // Resume the game
            NavigateToPage(GamePages.GamePage, "GameState=Resume");
        }

        private void newGameButton_Click(object sender, RoutedEventArgs e)
        {
            // Warn the player if an existing game is in progress
            if (GamePage.GameState.IsGameActive)
            {
                if (MessageBox.Show("Starting a new game will end the game that is already in progress -- are you sure?", "Start New Game", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                {
                    // Cancel
                    return;
                }
            }

            // Begin a new game
            GamePage.GameState.ResetGame();

            // Show the game page
            NavigateToPage(GamePages.GamePage);
        }

        private void optionsButton_Click(object sender, RoutedEventArgs e)
        {
            // Show the options page
            NavigateToPage(GamePages.SettingsPage);
        }

        private void highScoresButton_Click(object sender, RoutedEventArgs e)
        {
            // Show the high scores page
            NavigateToPage(GamePages.HighScorePage);
        }

        private void aboutButton_Click(object sender, RoutedEventArgs e)
        {
            // Show the about page
            NavigateToPage(GamePages.AboutPage);
        }

    }
}