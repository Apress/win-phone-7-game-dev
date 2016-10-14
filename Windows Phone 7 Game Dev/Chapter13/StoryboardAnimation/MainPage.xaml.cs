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

namespace StoryboardAnimation
{
    public partial class MainPage : PhoneApplicationPage
    {
        // The number of raindrops to display
        private const int RaindropCount = 20;

        // A list inside which all our Raindrop objects will be stored
        private List<Raindrop> _raindropSprites = new List<Raindrop>();

        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize the page content once the page is loaded
        /// </summary>
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Create and initialize the sprites
            for (int i = 0; i < RaindropCount; i++)
            {
                _raindropSprites.Add(new Raindrop(GameCanvas));
            }
        }

        /// <summary>
        /// Ensure that the canvas clipping rect matches its size
        /// </summary>
        private void GameCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            GameCanvasClipRect.Rect = new Rect(0, 0, e.NewSize.Width, e.NewSize.Height);
        }

    }
}