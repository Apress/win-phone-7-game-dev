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
    public partial class AboutPage : PhoneApplicationPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));

            //// Can we navigate backward?
            //if (NavigationService.CanGoBack)
            //{
            //    // Yes, so go there now
            //    NavigationService.GoBack();
            //}
        }
    }
}