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

namespace ColorFade
{
    public partial class HostPage : UserControl
    {
        public HostPage()
        {
            InitializeComponent();

            // Navigate to MainPage
            hostFrame.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }
    }
}
