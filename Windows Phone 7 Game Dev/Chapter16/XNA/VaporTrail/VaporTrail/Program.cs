using System;

namespace VaporTrail
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (VaporTrailGame game = new VaporTrailGame())
            {
                game.Run();
            }
        }
    }
#endif
}

