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

namespace InputTouchEvents
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Add a handler for the FrameReported event
            Touch.FrameReported += new TouchFrameEventHandler(Touch_FrameReported);
        }

        void Touch_FrameReported(object sender, TouchFrameEventArgs e)
        {
            System.Text.StringBuilder status = new System.Text.StringBuilder();

            // Get a reference to the primary touch point
            TouchPoint primary = e.GetPrimaryTouchPoint(null);
            // Report on its status
            status.AppendLine("Touch status: " + primary.Action.ToString() + " @ " + primary.Position.ToString());

            // Report on the control underneath the primary touch point
            UIElement overControl = primary.TouchDevice.DirectlyOver;
            if (overControl != null)
            {
                status.AppendLine(" Over control '" + overControl.GetValue(NameProperty) + "'");
            }
            else
            {
                status.AppendLine(" Not over any control");
            }

            // Report on the total number of touch points
            status.AppendLine(" Touch point count: " + e.GetTouchPoints(null).Count.ToString());

            // Put the status into the textblock
            touchText.Text = status.ToString();
        }

    }
}