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
    public partial class HighScorePage : PhoneApplicationPage
    {

        //-------------------------------------------------------------------------------------
        // Class constructor

        public HighScorePage()
        {
            InitializeComponent();
        }


        //-------------------------------------------------------------------------------------
        // Static properties

        private static HighScores _highScores;
        /// <summary>
        /// Returns a reference to the game's GameState object
        /// </summary>
        internal static HighScores HighScores
        {
            get
            {
                // Do we already have a highscore instance?
                if (_highScores == null)
                {
                    // No, so create one
                    _highScores = new HighScores();
                    // Add the tables
                    _highScores.InitializeTable("Default", 20);
                    // Load any existing scores
                    _highScores.LoadScores();
                }
                return _highScores;
            }
        }


        //-------------------------------------------------------------------------------------
        // Events
        
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            string paramValue;
            int newScoreIndex;

            // Are we highlighting an entry?
            NavigationContext.QueryString.TryGetValue("HighlightIndex", out paramValue);
            if (!int.TryParse(paramValue, out newScoreIndex))
            {
                // Nothing found or not numeric, so don't highlight anything
                newScoreIndex = -1;
            }

            // Get the HighScores class to show the scores inside the scoresGrid control
            HighScores.ShowScores(scoresGrid, "Default", newScoreIndex);
        }


    }
}