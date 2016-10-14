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
    public partial class NamePage : PhoneApplicationPage
    {
        public NamePage()
        {
            InitializeComponent();
        }


        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string yourName = "";
            // See if we can retrieve the passed name
            if (NavigationContext.QueryString.TryGetValue("YourName", out yourName))
            {
                // Display the name
                textblockName.Text = "Hello, " + yourName + "!";
            }
            else
            {
                // Couldn't locate a name
                textblockName.Text = "Couldn't find your name, sorry!";
            }
        }
    }
}