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
using GameFramework;

namespace SoftInputPanel
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SIPGame : GameFramework.GameHost
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public SIPGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 30);

            // Set back buffer size and orientation
            _graphics.PreferredBackBufferWidth = 480;
            _graphics.PreferredBackBufferHeight = 800;
            // Display in full screen mode
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

            Fonts.Add("Miramonte", Content.Load<SpriteFont>("Miramonte"));

            ResetGame();
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

            UpdateAll(gameTime);

            // Has the user touched the screen?
            if (TouchPanel.GetState().Count > 0)
            {
                // Make sure the input dialog is not already visible
                if (!(Guide.IsVisible))
                {
                    // Show the input dialog to get text from the user
                    Guide.BeginShowKeyboardInput(PlayerIndex.One, "High score achieved", "Please enter your name", "My name", InputCallback, null);
                }  
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This function will be called when the SIP name entry is completed or cancelled
        /// </summary>
        /// <param name="result"></param>
        void InputCallback(IAsyncResult result)
        {
            string sipContent = Guide.EndShowKeyboardInput(result);

            // Did we get some input from the user?
            if (sipContent != null)
            {
                // Store it in the text object
                ((TextObject)GameObjects[0]).Text = "Your name is " + sipContent;
            }
            else
            {
                // The SIP was cancelled
                ((TextObject)GameObjects[0]).Text = "Name entry was canceled.";
            }
        } 

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            DrawText(gameTime, _spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void ResetGame()
        {
            // Create a single text object
            GameObjects.Clear();

            GameObjects.Add(new TextObject(this, Fonts["Miramonte"], new Vector2(20, 100), "No name entered, tap the screen..."));
        }


    }
}
