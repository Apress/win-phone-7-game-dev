using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace VersionNumber
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Get the assembly name
            string name = Assembly.GetExecutingAssembly().FullName;
            // Use this to obtain its version
            Version version = new AssemblyName(name).Version;
            // Display the version on the page
            versionText.Text = "Version " + version.ToString();

        }

    }
}