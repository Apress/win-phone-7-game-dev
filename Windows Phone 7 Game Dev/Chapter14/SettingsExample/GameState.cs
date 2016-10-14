using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using SLGameFramework;

namespace SettingsExample
{
    public class GameState
    {

        /// <summary>
        /// Is a game currently in progress?
        /// </summary>
        public bool IsGameActive { get; set; }

        /// <summary>
        /// The only piece of information used by this game -- the player's score
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Reset the game to its initial state
        /// </summary>
        internal void ResetGame()
        {
            // Set a random score
            Score = GameHelper.RandomNext(0, 10000);
            // The game is now active
            IsGameActive = true;
        }


    }
}
