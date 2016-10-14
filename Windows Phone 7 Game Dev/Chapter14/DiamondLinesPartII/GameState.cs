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

namespace DiamondLinesPartII
{
    public class GameState
    {

        //-------------------------------------------------------------------------------------
        // Constructors

        /// <summary>
        /// Class constructor
        /// </summary>
        public GameState()
        {
            // Create all the required space in the DiamondColors array.
            // First create the outer array
            DiamondColors = new int[GamePage.BoardWidth][];
            // Now create the inner arrays at each outer array index
            for (int x = 0; x < GamePage.BoardWidth; x++)
            {
                DiamondColors[x] = new int[GamePage.BoardHeight];
            }
        }


        //-------------------------------------------------------------------------------------
        // Properties


        /// <summary>
        /// Is a game currently in progress?
        /// </summary>
        public bool IsGameActive { get; set; }

        /// <summary>
        /// The array of colors of the diamonds in play
        /// </summary>
        /// <remarks>.NET is unable to serialize multi-dimensional arrays, but it is
        /// able to serialize arrays of arrays. This is therefore implemented as an
        /// array of arrays: the outer array is for the x axis, the inner arrays for
        /// the y axis.</remarks>
        public int[][] DiamondColors { get; set; }


        //-------------------------------------------------------------------------------------
        // Object methods

        /// <summary>
        /// Generate a random set of colors for the game
        /// </summary>
        public void ResetGame()
        {
            // Loop for each diamond array entry
            for (int x = 0; x < GamePage.BoardWidth; x++)
            {
                for (int y = 0; y < GamePage.BoardWidth; y++)
                {
                    // Generate a random color for this position
                    SetDiamondColor(x, y, Diamond.GenerateRandomColor());
                }
            }

            // Set the game as being active
            IsGameActive = true;
        }


        /// <summary>
        /// Retrieve the diamond color at the specified location
        /// </summary>
        public int GetDiamondColor(int x, int y)
        {
            return DiamondColors[x][y];
        }
        /// <summary>
        /// Set the diamond color at the specified location
        /// </summary>
        public void SetDiamondColor(int x, int y, int color)
        {
            DiamondColors[x][y] = color;
        }

    }
}
