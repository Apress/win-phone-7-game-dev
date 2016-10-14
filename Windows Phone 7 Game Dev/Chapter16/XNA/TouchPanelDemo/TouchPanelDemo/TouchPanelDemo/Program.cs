using System;

namespace TouchPanelDemo
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (TouchPanelGame game = new TouchPanelGame())
            {
                game.Run();
            }
        }
    }
#endif
}

