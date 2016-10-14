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

namespace ColorFade
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void rectangle1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            GradientStop gradStop;
            Random rand = new Random();

            // Clear the existing gradient stops
            FadeBrush.GradientStops.Clear();

            // Add a new stop with offset 0 (radial center)
            gradStop = new GradientStop();
            gradStop.Color = Color.FromArgb(255, (byte)rand.Next(256), (byte)rand.Next(256), (byte)rand.Next(256));
            gradStop.Offset = 0;
            FadeBrush.GradientStops.Add(gradStop);

            // Add a new stop with offset 1 (radial edge)
            gradStop = new GradientStop();
            gradStop.Color = Color.FromArgb(255, (byte)rand.Next(256), (byte)rand.Next(256), (byte)rand.Next(256));
            gradStop.Offset = 1;
            FadeBrush.GradientStops.Add(gradStop);

            // Add a new stop with a random offset
            gradStop = new GradientStop();
            gradStop.Color = Color.FromArgb(255, (byte)rand.Next(256), (byte)rand.Next(256), (byte)rand.Next(256));
            gradStop.Offset = rand.Next(100) / 100.0f;
            FadeBrush.GradientStops.Add(gradStop);
        }
    }
}