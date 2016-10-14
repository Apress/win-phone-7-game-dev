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

namespace Text
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private SpriteFont _fontMiramonte;

        private float _angle;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Pre-autoscale settings.
            _graphics.PreferredBackBufferWidth = 480;
            _graphics.PreferredBackBufferHeight = 800;
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

            // Load the spritefont
            _fontMiramonte = Content.Load<SpriteFont>("Miramonte");
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

            // Rotate the text
            _angle += 5;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            Vector2 textsize;
            Color textcolor;
            string textString;

            GraphicsDevice.Clear(Color.Blue);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

            // Display some static text with a shadow
            _spriteBatch.DrawString(_fontMiramonte, "Hello world", new Vector2(101, 101), Color.Black);
            _spriteBatch.DrawString(_fontMiramonte, "Hello world", new Vector2(100, 100), Color.White);

            // Draw some rotated, scaled text with alpha blending...
            // Calculate the size of the text
            textString = "Text in XNA!";
            textsize = _fontMiramonte.MeasureString(textString);
            // Draw it lots of times
            for (int i = 25; i >= 0; i--)
            {
                // For the final iteration, use black text;
                // otherwise use white text with gradually increasing alpha levels
                if (i > 0)
                {
                    textcolor = new Color(255, 255, 255, 255 - i * 10);
                }
                else
                {
                    textcolor = Color.Black;
                }
                // Draw our text with its origin at the middle of the screen and
                // in the center of the text, rotated and scaled based on the
                // iteration number.
                _spriteBatch.DrawString(_fontMiramonte, textString, new Vector2(240, 400), textcolor, MathHelper.ToRadians(_angle * ((i + 5) * 0.1f)), textsize / 2, 1 + (i / 7.0f), SpriteEffects.None, 0);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
