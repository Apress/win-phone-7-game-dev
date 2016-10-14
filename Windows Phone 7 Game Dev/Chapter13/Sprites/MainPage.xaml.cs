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

namespace Sprites
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Load the sprite image from the application resources
            StreamResourceInfo sr = Application.GetResourceStream(new Uri("Sprites;component/Images/Ball.png", UriKind.Relative));
            BitmapImage spriteImage = new BitmapImage();
            spriteImage.SetSource(sr.Stream);

            // Create and initialize the sprites
            for (int x = 20; x < 400; x += 20)
            {
                Sprite runtimeSprite = new Sprite();
                GameCanvas.Children.Add(runtimeSprite);
                runtimeSprite.Source = spriteImage;
                runtimeSprite.Width = 85;
                runtimeSprite.Height = 85;
                runtimeSprite.Left = x;
                runtimeSprite.Top = 520;
            }
        }

    }
}