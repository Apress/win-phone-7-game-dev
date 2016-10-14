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
using System.Windows.Media.Imaging;
using System.Windows.Resources;

namespace DiamondLinesPartII
{
    public class Diamond
    {

        //-------------------------------------------------------------------------------------
        // Class variables/constants

        // The pixel width and height of the diamonds
        internal const int DiamondWidth = 60;
        internal const int DiamondHeight = 60;

        // Internal object references
        private GamePage _gamePage;
        private Sprite _diamondSprite;
        private int _color;

        // A static reference to the sprite image
        private static BitmapImage _spriteImage;

        //-------------------------------------------------------------------------------------
        // Constructors

        public Diamond(GamePage mainPage, int x, int y)
        {
            
            // Store the information we have been given
            _gamePage = mainPage;
            XPos = x;
            YPos = y;

            // Load the sprite image if it has not already been loaded
            if (_spriteImage == null)
            {
                StreamResourceInfo sr = Application.GetResourceStream(new Uri("DiamondLinesPartII;component/Images/Diamonds.png", UriKind.Relative));
                _spriteImage = new BitmapImage();
                _spriteImage.SetSource(sr.Stream);
            }

            // Create a Sprite control so that the diamond is displayed on the page
            _diamondSprite = new Sprite();
            
            // Set event handlers
            _diamondSprite.TranslateCompleted += new EventHandler(_diamondSprite_TranslateCompleted);

            // Configure the sprite
            _diamondSprite.Source = _spriteImage;
            _gamePage.GameCanvas.Children.Add(_diamondSprite);
            _diamondSprite.Width = DiamondWidth;
            _diamondSprite.Height = DiamondHeight;

            // Position the sprite on the screen
            SetSpritePosition();

            // Add a handler for the MouseLeftButtonDown event
            _diamondSprite.MouseLeftButtonDown += new MouseButtonEventHandler(_diamondSprite_MouseLeftButtonDown);
        }


        //-------------------------------------------------------------------------------------
        // Properties

        /// <summary>
        /// The sprite's horizontal position within the game board (0 to 7)
        /// </summary>
        public int XPos { get; set; }
        /// <summary>
        /// The sprite's verticle position within the game board (0 to 7)
        /// </summary>
        public int YPos { get; set; }

        /// <summary>
        /// The diamond color index (0 to 5)
        /// </summary>
        public int Color
        {
            get { return _color; }
            set
            {
                _color = value;
                _diamondSprite.ImageOffsetX = _color * 60;
            }
        }

        /// <summary>
        /// The translation offset distance for the x axis
        /// </summary>
        public double XOffset
        {
            get { return _diamondSprite.TranslationX;  }
            set { _diamondSprite.TranslationX = value; }
        }
        /// <summary>
        /// The translation offset distance for the y axis
        /// </summary>
        public double YOffset
        {
            get { return _diamondSprite.TranslationY; }
            set { _diamondSprite.TranslationY = value; }
        }

        /// <summary>
        /// The easing function to use the next time BeginTranslate is called
        /// </summary>
        public EasingFunctionBase TranslateEasing { get; set; }
        /// <summary>
        /// The duration to use the next time BeginTranslate is called
        /// </summary>
        public double TranslateDuration { get; set; }

        /// <summary>
        /// Returns a reference to the underlying sprite control
        /// </summary>
        public Sprite Sprite
        {
            get { return _diamondSprite; }
        }

        //-------------------------------------------------------------------------------------
        // Event handlers

        /// <summary>
        /// Handle a click on this diamond
        /// </summary>
        void _diamondSprite_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Let the game process the click on this diamond
            _gamePage.ActivateDiamond(XPos, YPos);
        }

        /// <summary>
        /// Process the end of movement of a diamond
        /// </summary>
        void _diamondSprite_TranslateCompleted(object sender, EventArgs e)
        {
            // Are the diamonds currently dropping?
            if (_gamePage.MovementState == GamePage.MovementStates.Dropping)
            {
                // Yes, so this diamond has just landed. Play a drop sound effect.
                AudioPlayer.PlaySoundEffect("Sounds/DiamondDrop.wav", 1, GameHelper.RandomNext(0.0f, 0.5f), 0);
            }
        }

        //-------------------------------------------------------------------------------------
        // Object methods

        /// <summary>
        /// Physically position the sprite on the page
        /// </summary>
        private void SetSpritePosition()
        {
            _diamondSprite.Left = XPos * DiamondWidth;
            _diamondSprite.Top = YPos * DiamondHeight;
        }

        /// <summary>
        /// Generate and set a random color for the diamond
        /// </summary>
        internal void SetRandomColor()
        {
            Color = GenerateRandomColor();
        }

        /// <summary>
        /// Generate and return a random color.
        /// </summary>
        /// <returns>This is a static function and so can be called without a class instance</returns>
        internal static int GenerateRandomColor()
        {
            return GameHelper.RandomNext(6);
        }

        /// <summary>
        /// Begin translating the sprite from its defined XOffset,YOffset position
        /// to its origin (0,0) position using the currently stored TranslateDuration
        /// and TranslateEasing.
        /// </summary>
        internal void BeginTranslate()
        {
            double animSpeed = 1;

            // Reduce the translation duration if in fast animation mode
            if (SettingsManager.GetValue("AnimSpeed", "Normal") == "Fast") animSpeed = 0.4;

            // Is there any translation to do?
            if (XOffset != 0 || YOffset != 0)
            {
                _diamondSprite.BeginTranslate(XOffset, YOffset, 0, 0, TranslateDuration * animSpeed, 0, TranslateEasing);
            }
        }

        /// <summary>
        /// Stop any active translation storyboard that may be in use
        /// </summary>
        internal void StopTranslate()
        {
            _diamondSprite.StopTranslate();
        }

    }
}
