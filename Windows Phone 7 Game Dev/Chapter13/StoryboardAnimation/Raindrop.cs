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

namespace StoryboardAnimation
{
    public class Raindrop
    {

        // The raindrop image. Static, so only one instance is created per application,
        // rather than one per object.
        static BitmapImage _raindropBitmap;

        private Canvas _gameCanvas; // The canvas into which the sprite has been added
        private Sprite _sprite;     // The Sprite control instance used by this object

        public Raindrop(Canvas gameCanvas)
        {
            // Store the reference to the canvas
            _gameCanvas = gameCanvas;

            // Load the sprite image from the application resources if not already loaded
            if (_raindropBitmap == null)
            {
                StreamResourceInfo sr = Application.GetResourceStream(new Uri("StoryboardAnimation;component/Images/Raindrop.png", UriKind.Relative));
                _raindropBitmap = new BitmapImage();
                _raindropBitmap.SetSource(sr.Stream);
            }

            // Create and add the sprite at a random position
            _sprite = new Sprite();
            _gameCanvas.Children.Add(_sprite);
            _sprite.Source = _raindropBitmap;
            _sprite.Width = _raindropBitmap.PixelWidth;
            _sprite.Height = _raindropBitmap.PixelHeight;
            _sprite.Left = GameHelper.RandomNext(0, _gameCanvas.ActualWidth - _sprite.Width);
            _sprite.Top = 0;
            _sprite.ScaleX = GameHelper.RandomNext(0.5, 1.0);
            _sprite.ScaleY = _sprite.ScaleX;

            // Generate the overall duration for the translation
            double duration = GameHelper.RandomNext(3.0, 7.0);

            // Tell the sprite to begin its translation storyboard
            _sprite.BeginTranslate(0, -_raindropBitmap.PixelHeight, 0, _gameCanvas.ActualHeight + _sprite.Height
                        , duration, GameHelper.RandomNext(duration), new CircleEase() { EasingMode = EasingMode.EaseIn }, RepeatBehavior.Forever, false);

        }

    }
}
