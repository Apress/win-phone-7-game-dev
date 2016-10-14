using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using GameFramework;
using Microsoft.Phone.Shell;

namespace TombstoneEvents
{

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class TombstoneEventsGame : GameHost
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public TombstoneEventsGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            _graphics.IsFullScreen = true;
            _graphics.PreferredBackBufferWidth = 480;
            _graphics.PreferredBackBufferHeight = 800;

            // Set up application lifecycle event handlers
            PhoneApplicationService.Current.Launching += GameLaunching;
            PhoneApplicationService.Current.Closing += GameClosing;
            PhoneApplicationService.Current.Deactivated += GameDeactivated;
            PhoneApplicationService.Current.Activated += GameActivated;

        }


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw TombstoneEventsures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }



        private void GameLaunching(object sender, Microsoft.Phone.Shell.LaunchingEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Game launching");
        }

        private void GameClosing(object sender, Microsoft.Phone.Shell.ClosingEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Game closing");
        }

        private void GameDeactivated(object sender, Microsoft.Phone.Shell.DeactivatedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Game deactivated");

            // Generate a random number
            int rand = GameHelper.RandomNext(10000);
            // Write the number to the state object
            PhoneApplicationService.Current.State.Clear();
            PhoneApplicationService.Current.State.Add("RandomNumber", rand);
        }

        private void GameActivated(object sender, Microsoft.Phone.Shell.ActivatedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Game activated");

            // Recover the random number
            int rand = (int)PhoneApplicationService.Current.State["RandomNumber"];
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.Exit();
            }

            UpdateAll(gameTime);

            base.Update(gameTime);
        }



        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
        }

    }
}
