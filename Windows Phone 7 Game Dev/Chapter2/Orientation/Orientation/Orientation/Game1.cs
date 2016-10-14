using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

namespace Orientation
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private SpriteFont _miramonteFont;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Force portrait orientation
            _graphics.PreferredBackBufferWidth = 480;
            _graphics.PreferredBackBufferHeight = 800;

            //// Allow portrait and both landscape rotations
            //_graphics.SupportedOrientations = DisplayOrientation.Portrait |
            //                        DisplayOrientation.LandscapeLeft |
            //                        DisplayOrientation.LandscapeRight;

            // Add a handler to the OrientationChanged event
            Window.OrientationChanged += Window_OrientationChanged;

            _graphics.IsFullScreen = true;
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
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _miramonteFont = Content.Load<SpriteFont>("Miramonte");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
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
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Display some information about the orientation
            _spriteBatch.Begin();
            _spriteBatch.DrawString(_miramonteFont, "Current orientation: " + Window.CurrentOrientation.ToString(), new Vector2(10, 100), Color.White);
            _spriteBatch.DrawString(_miramonteFont, "Window size: " + Window.ClientBounds.Width + ", " + Window.ClientBounds.Height, new Vector2(10, 130), Color.White);
            _spriteBatch.DrawString(_miramonteFont, "Back buffer size: " + _graphics.GraphicsDevice.PresentationParameters.BackBufferWidth + ", " + _graphics.GraphicsDevice.PresentationParameters.BackBufferHeight, new Vector2(10, 160), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Respond to the orientation changing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Window_OrientationChanged(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Orientation changed to " + Window.CurrentOrientation.ToString());
        }

    }
}
