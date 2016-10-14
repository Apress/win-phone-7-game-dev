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

namespace FadeToBlack
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Random _rand = new Random();

        private Texture2D _smileyTexture;
        private Texture2D _faderTexture;

        private Vector2[] _smileyPositions;

        private int _faderAlpha = 0;
        private int _faderAlphaAdd = 10;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Pre-autoscale settings.
            _graphics.PreferredBackBufferWidth = 480;
            _graphics.PreferredBackBufferHeight = 800;

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

            // Load graphics
            _smileyTexture = Content.Load<Texture2D>("SmileyFace");

            // Create the texture for our fader sprite with a size of 1 x 1 pixel
            _faderTexture = new Texture2D(GraphicsDevice, 1, 1);
            // Create an array of colors for the texture -- just one color
            // as the texture consists of only one pixel
            Color[] faderColors = new Color[] {Color.White};
            // Set the color data into the texture
            _faderTexture.SetData<Color>(faderColors);
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

            // If the smiley positions are not yet initialized, randomize them now
            if (_smileyPositions == null)
            {
                _smileyPositions = new Vector2[5];
                RandomizeSmileys();
            }

            // Update the fader alpha level
            _faderAlpha += _faderAlphaAdd;
            // Have we reached the extent of the fade?
            // If so, reverse the fade direction
            if (_faderAlpha <= 0)
            {
                // The fader is at its invisible point
                // Make sure we don't go below zero...
                _faderAlpha = 0;
                // Reverse direction
                _faderAlphaAdd = -_faderAlphaAdd;
            }
            if (_faderAlpha >= 255)
            {
                // The fader is at its opaque point
                // Make sure we don't go above 255
                _faderAlpha = 255;
                // Reverse direction
                _faderAlphaAdd = -_faderAlphaAdd;
                // Get some new smiley positions too while the smileys are hidden
                RandomizeSmileys();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Generate random positions for each of the smileys
        /// </summary>
        private void RandomizeSmileys()
        {
            for (int i = 0; i < _smileyPositions.Length; i++)
            {
                _smileyPositions[i].X = _rand.Next(0, Window.ClientBounds.Width - _smileyTexture.Width);
                _smileyPositions[i].Y = _rand.Next(0, Window.ClientBounds.Height - _smileyTexture.Height);
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Begin the spriteBatch
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            
            // Draw the smiley face sprites
            for (int i = 0; i < _smileyPositions.Length; i++)
            {
                _spriteBatch.Draw(_smileyTexture, _smileyPositions[i], Color.White);
            }

            // Draw the fader
            _spriteBatch.Draw(_faderTexture, this.Window.ClientBounds, new Color(0, 0, 0, _faderAlpha));
            
            // End the spriteBatch
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
