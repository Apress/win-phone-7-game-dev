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

namespace MultiplePages
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to the Name page
            NavigationService.Navigate(new Uri("/NamePage.xaml?YourName=" + HttpUtility.UrlEncode(textName.Text), UriKind.Relative));
        }

        private void buttonAbout_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/AboutPage.xaml", UriKind.Relative));
        }
    }
}