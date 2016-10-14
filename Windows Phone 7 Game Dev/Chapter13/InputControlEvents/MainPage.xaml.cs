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

namespace InputControlEvents
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void Sprite_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ballText.Text = "Ball: MouseLeftButtonDown @ " + e.GetPosition(ballSprite);
        }

        private void ballSprite_MouseMove(object sender, MouseEventArgs e)
        {
            ballText.Text = "Ball: MouseMove @ " + e.GetPosition(ballSprite);
        }

        private void ballSprite_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ballText.Text = "Ball: MouseLeftButtonUp @ " + e.GetPosition(ballSprite);
        }

        private void smileySprite_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            smileyText.Text = "Smiley: ManipulationStarted\r origin = " + e.ManipulationOrigin.ToString();
            e.Handled = true;
        }

        private void smileySprite_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            smileyText.Text = "Smiley: ManipulationDelta\r trantlation delta = " + e.DeltaManipulation.Translation.ToString() + ", scale delta =" + e.DeltaManipulation.Scale.ToString();

            // Add the delta translation
            smileySprite.Left += e.DeltaManipulation.Translation.X;
            smileySprite.Top += e.DeltaManipulation.Translation.Y;

            // Apply the delta scaling
            if (e.DeltaManipulation.Scale.X != 0)
            {
                smileySprite.Width *= e.DeltaManipulation.Scale.X;
            }
            if (e.DeltaManipulation.Scale.Y != 0)
            {
                smileySprite.Height *= e.DeltaManipulation.Scale.Y;
            }
            // Set the image to match the sprite size
            smileySprite.ImageWidth = smileySprite.Width;
            smileySprite.ImageHeight = smileySprite.Height;
        }

        private void smileySprite_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            smileyText.Text = "Smiley: ManipulationCompleted\r total translation = " + e.TotalManipulation.Translation.ToString() + ", total scale = " + e.TotalManipulation.Scale.ToString();
        }

    }
}