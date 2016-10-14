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

namespace DiamondLines
{
    public partial class HostPage : UserControl
    {
        public HostPage()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            hostFrame.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }
    }
}
