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
using Microsoft.Devices.Sensors;

namespace InputAccelerometer
{
    public partial class MainPage : PhoneApplicationPage
    {

        // The accelerometer object
        private Accelerometer _accelerometer = new Accelerometer();
        // The most recent reading from the accelerometer
        private double _accelerometerX;
        private double _accelerometerY;
        private double _accelerometerZ;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Add an event handler and start processing the events
            _accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
            _accelerometer.Start();

            // Add a handler for the Rendering event so that we can update the sprite
            CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);
        }

        /// <summary>
        /// Update the page ready for it to be rendered
        /// </summary>
        void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            accelerometerText.Text = "Accelerometer: " + _accelerometerX.ToString() + "," + _accelerometerY.ToString() + "," + _accelerometerZ.ToString();

            ballSprite.Left += _accelerometerX * 5;
            ballSprite.Top += _accelerometerY * 5;

            if (ballSprite.Left < 0) ballSprite.Left = 0;
            if (ballSprite.Left + ballSprite.Width > GameCanvas.ActualWidth) ballSprite.Left = GameCanvas.ActualWidth - ballSprite.Width;
            if (ballSprite.Top < 0) ballSprite.Top = 0;
            if (ballSprite.Top + ballSprite.Height > GameCanvas.ActualHeight) ballSprite.Top = GameCanvas.ActualHeight - ballSprite.Height;
        }

        /// <summary>
        /// Store the details of the updated accelerometer reading
        /// </summary>
        void Accelerometer_ReadingChanged(object sender, AccelerometerReadingEventArgs e)
        {
            _accelerometerX = e.X;
            _accelerometerY = e.Y;
            _accelerometerZ = e.Z;
        }


    }
}