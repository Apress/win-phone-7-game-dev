using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.Xna.Framework;

namespace SLGameFramework
{
    public class XNAAsyncDispatcher : IApplicationService
    {
        // The dispatcher timer
        private static DispatcherTimer frameworkDispatcherTimer;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dispatchInterval"></param>
        private XNAAsyncDispatcher(TimeSpan dispatchInterval)
        {
            frameworkDispatcherTimer = new DispatcherTimer();
            frameworkDispatcherTimer.Tick += new EventHandler(frameworkDispatcherTimer_Tick);
            frameworkDispatcherTimer.Interval = dispatchInterval;
        }

        void IApplicationService.StartService(ApplicationServiceContext context) { frameworkDispatcherTimer.Start(); }
        void IApplicationService.StopService() { frameworkDispatcherTimer.Stop(); }
        void frameworkDispatcherTimer_Tick(object sender, EventArgs e) { Microsoft.Xna.Framework.FrameworkDispatcher.Update(); }

        /// <summary>
        /// Start the XNA Async Dispatcher. This must be called if music or sound effects are required
        /// in your game, and it MUST BE CALLED from the application's App class constructor.
        /// </summary>
        public static void Start()
        {
            // Has the timer already been initialized?
            if (frameworkDispatcherTimer == null)
            {
                // No, so create a new XNASyncDispatcher object so that it is initialized
                Application.Current.ApplicationLifetimeObjects.Add(new XNAAsyncDispatcher(TimeSpan.FromMilliseconds(50)));
            }
        }

        /// <summary>
        /// Returns a bool indicating whether the XNA Async Dispatcher is running
        /// </summary>
        public static bool IsStarted
        {
            get { return !(frameworkDispatcherTimer == null); }
        }

        /// <summary>
        /// Determines whether the XNA Async Dispatcher is running and
        /// throws an exception if it is not.
        /// </summary>
        internal static void CheckIsStarted()
        {
            // Make sure the XNA Async Dispatcher is running
            if (!XNAAsyncDispatcher.IsStarted)
            {
                throw new Exception("Cannot play music or sound effects as the XNAAsyncDispatcher is not running. Please call SLGameFramework.XNAAsyncDispatcher.Start() from your App class constructor.");
            }
        }

    }
}
