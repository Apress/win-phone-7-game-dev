using System;

namespace FireAndSmoke
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (FireAndSmokeGame game = new FireAndSmokeGame())
            {
                game.Run();
            }
        }
    }
#endif
}

