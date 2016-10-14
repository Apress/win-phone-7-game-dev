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
using System.Windows.Media.Imaging;
using System.Windows.Resources;

namespace StoryboardSprites
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            Sprite runtimeSprite;

            // Load the sprite image from the application resources
            StreamResourceInfo sr = Application.GetResourceStream(new Uri("StoryboardSprites;component/Images/SmileyFace.png", UriKind.Relative));
            BitmapImage spriteImage = new BitmapImage();
            spriteImage.SetSource(sr.Stream);

            // First row: translating and rotating sprite
            runtimeSprite = new Sprite();
            GameCanvas.Children.Add(runtimeSprite);
            runtimeSprite.Source = spriteImage;
            runtimeSprite.Width = spriteImage.PixelWidth;
            runtimeSprite.Height = spriteImage.PixelHeight;
            runtimeSprite.BeginTranslate(60, 60, 300, 60, 5);

            runtimeSprite = new Sprite();
            GameCanvas.Children.Add(runtimeSprite);
            runtimeSprite.Source = spriteImage;
            runtimeSprite.Width = spriteImage.PixelWidth;
            runtimeSprite.Height = spriteImage.PixelHeight;
            runtimeSprite.Left = 380;
            runtimeSprite.Top = 60;
            runtimeSprite.TransformCenterX = runtimeSprite.Width / 2;
            runtimeSprite.TransformCenterY = runtimeSprite.Height / 2;
            runtimeSprite.BeginRotate(0, 360, 5);

            // Second row: bounce-ease translation with repeat forever
            runtimeSprite = new Sprite();
            GameCanvas.Children.Add(runtimeSprite);
            runtimeSprite.Source = spriteImage;
            runtimeSprite.Width = spriteImage.PixelWidth;
            runtimeSprite.Height = spriteImage.PixelHeight;
            runtimeSprite.BeginTranslate(60, 210, 350, 210, 3, 0, new BounceEase(), RepeatBehavior.Forever, false);

            // Third row: sine-ease translation with repeat forever
            runtimeSprite = new Sprite();
            GameCanvas.Children.Add(runtimeSprite);
            runtimeSprite.Source = spriteImage;
            runtimeSprite.Width = spriteImage.PixelWidth;
            runtimeSprite.Height = spriteImage.PixelHeight;
            runtimeSprite.BeginTranslate(60, 360, 350, 360, 4, 0, new SineEase() { EasingMode = EasingMode.EaseInOut }, RepeatBehavior.Forever, true);

            // Fourth row: combined translation and rotation with easing
            runtimeSprite = new Sprite();
            GameCanvas.Children.Add(runtimeSprite);
            runtimeSprite.Source = spriteImage;
            runtimeSprite.Width = spriteImage.PixelWidth;
            runtimeSprite.Height = spriteImage.PixelHeight;
            runtimeSprite.TransformCenterX = runtimeSprite.Width / 2;
            runtimeSprite.TransformCenterY = runtimeSprite.Height / 2;
            runtimeSprite.BeginTranslate(60, 510, 350, 510, 7, 0, new ElasticEase() { EasingMode = EasingMode.EaseInOut }, RepeatBehavior.Forever, true);
            runtimeSprite.BeginRotate(0, 540, 7, 0, new ElasticEase() { EasingMode = EasingMode.EaseInOut }, RepeatBehavior.Forever, true);
        }
    }
}