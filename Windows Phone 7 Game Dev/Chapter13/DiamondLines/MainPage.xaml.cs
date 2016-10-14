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

namespace DiamondLines
{
    public partial class MainPage : PhoneApplicationPage
    {

        //-------------------------------------------------------------------------------------
        // Class variables/constants

        // Dimensions of the board
        private const int BoardWidth = 8;
        private const int BoardHeight = 8;

        // An array to hold the diamond objects for the board
        private Diamond[,] _gameBoard = new Diamond[BoardWidth, BoardHeight];

        // The board coordinate of the diamond that is currently selected by the user
        private int _selectedDiamondX = -1;
        private int _selectedDiamondY = -1;

        // The board coordinates of the diamonds that are currently being swapped
        private int _lastSwapX1;
        private int _lastSwapY1;
        private int _lastSwapX2;
        private int _lastSwapY2;

        // The current state that the game is in (idle, swapping diamonds, dropping diamonds, etc.)
        private GameStates _gameState = GameStates.Idle;
        private enum GameStates
        {
            Idle,               // The game is waiting for player input
            Swapping,           // The game is swapping two diamonds
            SwappingBack,       // The game is undoing the swapping of diamonds as the swap did not form a line
            Dropping            // The game is dropping diamonds into the space left when a line was removed
        }


        //-------------------------------------------------------------------------------------
        // Constructors

        public MainPage()
        {
            InitializeComponent();
        }


        //-------------------------------------------------------------------------------------
        // Event handlers

        private void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            // Now that the page has loaded, reset the game so that we have some diamonds to display
            ResetGame();

            // Add an event handler for the CompositionTarget's Rendering event so that
            // we can update the graphics on the screen each frame
            CompositionTarget.Rendering += new EventHandler(compositionTarget_Rendering);
        }

        /// <summary>
        /// Respond to the page being rendered
        /// </summary>
        private void compositionTarget_Rendering(Object sender, EventArgs e)
        {
            // What state are we in?
            switch (_gameState)
            {
                case GameStates.Swapping:
                    // Have the diamonds finished swapping?
                    if (!DiamondsAreMoving())
                    {
                        // Yes, so see if we can remove any diamonds from the board
                        if (FindLines(true))
                        {
                            // Switch to the dropping state while the diamonds fall into place
                            _gameState = GameStates.Dropping;
                        }
                        else
                        {
                            // Couldn't remove any diamonds, so this swap was not valid
                            // Swap back to the previous positions
                            SwapDiamonds(_lastSwapX1, _lastSwapY1, _lastSwapX2, _lastSwapY2);
                            _gameState = GameStates.SwappingBack;
                        }
                    }
                    break;

                case GameStates.SwappingBack:
                    // Have the diamonds finished swapping back?
                    if (!DiamondsAreMoving())
                    {
                        // Yes, so go back to an idle state
                        _gameState = GameStates.Idle;
                    }
                    break;

                case GameStates.Dropping:
                    // Have the diamonds finished dropping?
                    if (!DiamondsAreMoving())
                    {
                        // Are there more lines to remove?
                        if (FindLines(true))
                        {
                            // Yes, so switch to dropping state once again
                            _gameState = GameStates.Dropping;
                        }
                        else
                        {
                            // No, so go back to an idle state
                            _gameState = GameStates.Idle;
                        }
                    }
                    break;
            }
        }


        /// <summary>
        /// Handle the New Game application button
        /// </summary>
        private void buttonNewGame_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to start a new game?", "Diamond Lines", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                ResetGame();
            }
        }


        //-------------------------------------------------------------------------------------
        // Game functions

        /// <summary>
        /// Reset the game board ready for a new game to begin
        /// </summary>
        private void ResetGame()
        {
            // Stop the fader animation if it is activated
            Fader.Stop();

            // Remove all existing sprites from the canvas
            GameCanvas.Children.Clear();

            // Add a new set of random diamonds
            for (int x = 0; x < BoardWidth; x++)
            {
                for (int y = 0; y < BoardHeight; y++)
                {
                    _gameBoard[x, y] = new Diamond(this, x, y);
                }
            }
            // All the time there are lines left on the board, remove them
            while (FindLines(false));

            // Switch to idle state
            _gameState = GameStates.Idle;
        }


        /// <summary>
        /// The player has touched one of the diamonds so process the touch as appropriate
        /// </summary>
        /// <param name="XPos">The x board position of the touched diamond</param>
        /// <param name="YPos">The y board position of the touched diamond</param>
        internal void ActivateDiamond(int XPos, int YPos)
        {
            // Diamond activation can only be performed when the game is idle
            if (_gameState != GameStates.Idle) return;

            // Stop the fader animation if it is activated
            Fader.Stop();

            // Do we already have a selected (fading) diamond?
            if (_selectedDiamondX != -1)
            {
                // Ensure the previously-selected diamond's Opacity is 1
                _gameBoard[_selectedDiamondX, _selectedDiamondY].Sprite.Opacity = 1;
            }

            // Is this the diamond that is already selected?
            if (_selectedDiamondX == XPos && _selectedDiamondY == YPos)
            {
                // Yes, so cancel the selection
                _selectedDiamondX = -1;
                _selectedDiamondY = -1;
                // Done
                return;
            }

            // Determine whether the selected diamond is adjacent to the one that has been touched
            if (PositionsAreAdjacent(XPos, YPos, _selectedDiamondX, _selectedDiamondY))
            {
                // Yes, so swap the selected and the touched diamonds
                SwapDiamonds(XPos, YPos, _selectedDiamondX, _selectedDiamondY);
                // Cancel the selection now that it has been processed
                _selectedDiamondX = -1;
                _selectedDiamondY = -1;
                // Done
                return;
            }

            // The touched diamond was not adjacent to the selected diamond, or there
            // is no current selected diamond, so select this one.
            _selectedDiamondX = XPos;
            _selectedDiamondY = YPos;

            // Attach the fader animation to the touched diamond
            Storyboard.SetTarget(FaderAnim, _gameBoard[XPos, YPos].Sprite);
            Fader.Begin();
        }

        /// <summary>
        /// Determine whether the two game board positions are adjacent
        /// </summary>
        /// <returns>Returns true if the positions are adjacent, false if they are not</returns>
        private bool PositionsAreAdjacent(int x1, int y1, int x2, int y2)
        {
            // If any coordinate contains -1 then this is not a coordinate
            // within the board, so the two positions cannot be ajacent.
            if (x1 == -1 || y1 == -1 || x2 == -1 || y2 == -1) return false;

            // See if the positions match on one axis and differ by one position on the other
            if (x1 == x2 && (y1 == y2 - 1 || y1 == y2 + 1)) return true;
            if (y1 == y2 && (x1 == x2 - 1 || x1 == x2 + 1)) return true;
            
            // No match found
            return false;
        }

        /// <summary>
        /// Determine whether any diamonds are moving
        /// </summary>
        /// <returns>Returns true if any of the diamonds in the game board are moving, false if none are moving</returns>
        private bool DiamondsAreMoving()
        {
            for (int x = 0; x < BoardWidth; x++)
            {
                for (int y = 0; y < BoardHeight; y++)
                {
                    // Is this diamond currently translating?
                    if (_gameBoard[x, y] != null && _gameBoard[x, y].Sprite.IsTranslating) return true;
                }
            }
            // No translation found, so the diamonds are not moving
            return false;
        }

        /// <summary>
        /// Swap the diamonds in the specified board positions
        /// </summary>
        private void SwapDiamonds(int x1, int y1, int x2, int y2)
        {
            int colorTemp;
            
            // Swap over the gems within the board
            colorTemp = _gameBoard[x1, y1].Color;
            _gameBoard[x1, y1].Color = _gameBoard[x2, y2].Color;
            _gameBoard[x2, y2].Color = colorTemp;

            // Set translation details for the two gems so that they translate
            // from their previous positions to their current positions
            _gameBoard[x1, y1].TranslateEasing = new BackEase();
            _gameBoard[x1, y1].TranslateDuration = 0.5;
            _gameBoard[x2, y2].TranslateEasing = new BackEase();
            _gameBoard[x2, y2].TranslateDuration = 0.5;

            // Set the initial offsets so that the gems continue to appear in their
            // original unswapped positions on the screen
            if (x1 != x2)
            {
                if (x1 < x2)
                {
                    _gameBoard[x1, y1].XOffset = Diamond.DiamondWidth;
                    _gameBoard[x2, y2].XOffset = -Diamond.DiamondWidth;
                }
                else
                {
                    _gameBoard[x1, y1].XOffset = -Diamond.DiamondWidth;
                    _gameBoard[x2, y2].XOffset = Diamond.DiamondWidth;
                }
            }
            if (y1 != y2)
            {
                if (y1 < y2)
                {
                    _gameBoard[x1, y1].YOffset = Diamond.DiamondHeight;
                    _gameBoard[x2, y2].YOffset = -Diamond.DiamondHeight;
                }
                else
                {
                    _gameBoard[x1, y1].YOffset = -Diamond.DiamondHeight;
                    _gameBoard[x2, y2].YOffset = Diamond.DiamondHeight;
                }
            }

            // Begin the translation storyboard on both of the swapped gems
            _gameBoard[x1, y1].BeginTranslate();
            _gameBoard[x2, y2].BeginTranslate();

            // Store the swap coordinates in case this swap is invalid
            _lastSwapX1 = x1;
            _lastSwapY1 = y1;
            _lastSwapX2 = x2;
            _lastSwapY2 = y2;

            // Indicate that we are swapping
            _gameState = GameStates.Swapping;
        }

        /// <summary>
        /// Search for and remove lines within the game board
        /// </summary>
        /// <param name="animate">If true, the board will be animated using translation storyboards.
        /// If false, the lines will be removed immediately (for when the board is being reset)</param>
        /// <returns>Returns true if lines were found and removed, false if no lines were located.</returns>
        private bool FindLines(bool animate)
        {
            int x, y;
            int lastColor;
            int matchCount;
            bool linePresent = false;

            // Set up an array to count how many lines of each length were found
            int[] matchSizeCounts = new int[Math.Max(BoardWidth, BoardHeight) + 1];
            // Set up an array indicating whether each diamond on the board forms part of a line
            bool[,] removeDiamonds = new bool[BoardWidth, BoardHeight];

            // Scan horizontally
            for (y = 0; y < BoardHeight; y++)
            {
                // Set the last color to be that of the first diamond on this row
                lastColor = _gameBoard[0, y].Color;
                // Set matchCount to 1 for the first diamond of the row
                matchCount = 1;
                // Loop for each of the remaining diamonds in this row
                for (x = 1; x < BoardWidth; x++)
                {
                    // Does this match the previous diamond?
                    if (_gameBoard[x, y].Color == lastColor)
                    {
                        // Yes, so increase the match count
                        matchCount += 1;
                        // Is this a match of 3 or more?
                        if (matchCount == 3)
                        {
                            // We have a length of 3, so increase the count for this match size
                            matchSizeCounts[3] += 1;
                            // At least one line is present
                            linePresent = true;
                            // We will need to remove the diamond here and the previous two
                            removeDiamonds[x - 2, y] = true;
                            removeDiamonds[x - 1, y] = true;
                            removeDiamonds[x, y] = true;
                        }
                        else if (matchCount > 3)
                        {
                            // We have a length greater than 3.
                            // Reduce the count for the smaller size already counted...
                            matchSizeCounts[matchCount - 1] -= 1;
                            // ...and increase the count for this size
                            matchSizeCounts[matchCount] += 1;
                            // We will need to remove this diamond
                            removeDiamonds[x, y] = true;
                        }
                    }
                    else
                    {
                        // No match, so remember the color of this diamond and reset the match count
                        lastColor = _gameBoard[x, y].Color;
                        matchCount = 1;
                    }
                }
            }

            // Scan vertically
            for (x = 0; x < BoardWidth; x++)
            {
                // Set the last color to be that of the first diamond on this column
                lastColor = _gameBoard[x, 0].Color;
                // Set matchCount to 1 for the first diamond of the column
                matchCount = 1;
                // Loop for each of the remaining diamonds in this column
                for (y = 1; y < BoardHeight; y++)
                {
                    // Does this match the previous diamond?
                    if (_gameBoard[x, y].Color == lastColor)
                    {
                        // Yes, so increase the match count
                        matchCount += 1;
                        // Is this a match of 3 or more?
                        if (matchCount == 3)
                        {
                            // We have a length of 3, so increase the count for this match size
                            matchSizeCounts[3] += 1;
                            // At least one line is present
                            linePresent = true;
                            // We will need to remove the diamond here and the previous two
                            removeDiamonds[x, y - 2] = true;
                            removeDiamonds[x, y - 1] = true;
                            removeDiamonds[x, y] = true;
                        }
                        else if (matchCount > 3)
                        {
                            // We have a length greater than 3.
                            // Reduce the count for the smaller size already counted...
                            matchSizeCounts[matchCount - 1] -= 1;
                            // ...and increase the count for this size
                            matchSizeCounts[matchCount] += 1;
                            // We will need to remove this diamond
                            removeDiamonds[x, y] = true;
                        }
                    }
                    else
                    {
                        // No match, so remember the color of this diamond and reset the match count
                        lastColor = _gameBoard[x, y].Color;
                        matchCount = 1;
                    }
                }
            }

            // Were any matches formed?
            if (linePresent == false)
            {
                // No, so return false without doing anything more
                return false;
            }

            // Remove the diamonds from the board
            RemoveDiamonds(removeDiamonds, animate);

            // Lines were found so return true
            return true;
        }

        /// <summary>
        /// Remove from the board any diamonds whose positions are marked as true in the provided array
        /// </summary>
        /// <param name="removeDiamonds">An array indicating which diamonds are to be removed</param>
        /// <param name="animate">Pass true to animate, false to update immediately</param>
        private void RemoveDiamonds(bool[,] removeDiamonds, bool animate)
        {
            int x, y;

            // Loop across the board
            for (x = 0; x < BoardWidth; x++)
            {
                // Loop down the columns
                for (y = 0; y < BoardHeight; y++)
                {
                    // Reset all animation for this diamond
                    _gameBoard[x, y].StopTranslate();
                    _gameBoard[x, y].XOffset = 0;
                    _gameBoard[x, y].YOffset = 0;
                    _gameBoard[x, y].TranslateDuration = 0.3 + (x / 40.0);
                }
            }

            // Loop across the board updating the diamond offsets
            for (x = 0; x < BoardWidth; x++)
            {
                // Loop down the columns
                for (y = 0; y < BoardHeight; y++)
                {
                    // Is this diamond to be removed?
                    if (removeDiamonds[x,y])
                    {
                        // Yes, so shift the diamonds in this column and from this position
                        // towards the top of the column down one position, inserting
                        // a new diamond at the top position
                        RemoveDiamonds_DropColumn(x, y, animate);
                    }
                }
            }

            // Begin all of the storyboards. This will set all diamonds whose
            // XOffset or YOffset values have been amended into motion.
            for (x = 0; x < BoardWidth; x++)
            {
                for (y = 0; y < BoardHeight; y++)
                {
                    _gameBoard[x, y].BeginTranslate();
                    _gameBoard[x, y].BeginTranslate();
                }
            }
        }

        /// <summary>
        /// Remove the diamond at the specified location and drop all the diamonds
        /// above it down one space.
        /// </summary>
        /// <param name="x">The x board position of the diamond to be removed</param>
        /// <param name="y">The y board position of the diamond to be removed</param>
        /// <param name="animate">Pass true to animate with storyboards, false to update immediately.</param>
        private void RemoveDiamonds_DropColumn(int x, int y, bool animate)
        {
            // Loop backward through the board so that we work from the specified y
            // position back to the top of the board
            for (int updatePos = y; updatePos >= 0; updatePos--)
            {
                // Is this the top board position?
                if (updatePos == 0)
                {   // Yes, so randomly generate a new diamond color for this position
                    _gameBoard[x, updatePos].SetRandomColor();
                }
                else
                {
                    // No, so drop the diamond above into this position
                    _gameBoard[x, updatePos].Color = _gameBoard[x, updatePos - 1].Color;
                }
                // If we were asked to animate...
                if (animate)
                {
                    // Set up the offsets and translation for the diamond.
                    _gameBoard[x, updatePos].TranslateEasing = new PowerEase()  { EasingMode = EasingMode.EaseIn };
                    if (updatePos > 0)
                    {
                        _gameBoard[x, updatePos].YOffset = _gameBoard[x, updatePos - 1].YOffset - Diamond.DiamondHeight;
                    }
                    else
                    {
                        _gameBoard[x, updatePos].YOffset -= Diamond.DiamondHeight;
                    }
                    _gameBoard[x, updatePos].TranslateDuration += 0.1;
                }
            }
        }

    }
}