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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace InputGestures
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            TouchPanel.EnabledGestures = GestureType.Tap | GestureType.DoubleTap | GestureType.FreeDrag | GestureType.Flick;
        }

        private void PhoneApplicationPage_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            // See if there are any gestures waiting to be processed
            ProcessGestures();
        }

        private void PhoneApplicationPage_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            // See if there are any gestures waiting to be processed
            ProcessGestures();
        }

        private void ProcessGestures()
        {
            // Are there any gestures queued?
            while (TouchPanel.IsGestureAvailable)
            {
                // Yes, so read the gesture
                GestureSample gesture = TouchPanel.ReadGesture();

                // Display information on the screen
                gestureText.Text = "Gesture status: " + gesture.GestureType.ToString() + " @ " + gesture.Position.ToString();
            }
        }


    }
}