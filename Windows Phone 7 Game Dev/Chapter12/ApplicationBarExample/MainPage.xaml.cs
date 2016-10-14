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
using Microsoft.Phone.Shell;

namespace ApplicationBarExample
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You clicked " + (sender as ApplicationBarIconButton).Text);
        }

        private void ApplicationBarMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You selected " + (sender as ApplicationBarMenuItem).Text);
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("You clicked the background image");
        }
    }
}