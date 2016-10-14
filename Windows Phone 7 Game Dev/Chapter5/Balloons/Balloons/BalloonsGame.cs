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


namespace Balloons
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class BalloonsGame : GameFramework.GameHost
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public BalloonsGame()
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

            // Load textures
            Textures.Add("Balloon", Content.Load<Texture2D>("Balloon"));

            // Load sounds
            SoundEffects.Add("Pop1", Content.Load<SoundEffect>("Pop1"));
            SoundEffects.Add("Pop2", Content.Load<SoundEffect>("Pop2"));
            SoundEffects.Add("Pop3", Content.Load<SoundEffect>("Pop3"));

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
            TouchCollection touchPoints;
            SpriteObject touchSprite;
            SoundEffect sound = null;

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // Update the game objects
            UpdateAll(gameTime);

            // Has a new touch point been pressed?
            touchPoints = TouchPanel.GetState();
            if (touchPoints.Count > 0 && touchPoints[0].State == TouchLocationState.Pressed)
            {
                // Find the front-most sprite at this position
                touchSprite = GetSpriteAtPoint(touchPoints[0].Position);
                // Did we find a balloon?
                if (touchSprite is BalloonObject)
                {
                    // Yes, so pop up...
                    // Randomize the sprite to effectively remove it and create another balloon
                    ((BalloonObject)touchSprite).Randomize();
                    // Play a pop sound effect
                    switch (GameHelper.RandomNext(3))
                    {
                        case 0: sound = SoundEffects["Pop1"]; break;
                        case 1: sound = SoundEffects["Pop2"]; break;
                        case 2: sound = SoundEffects["Pop3"]; break;
                    }
                    // Play at a randomised pitch, panned to the position of the balloon across the screen
                    sound.Play(1.0f, GameHelper.RandomNext(-0.1f, 0.1f), touchPoints[0].Position.X / Window.ClientBounds.Width * 2 - 1);
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Draw all of the balloons, sorted by their LayerDepth from back to front
            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            DrawSprites(gameTime, _spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Reset the game to its default state
        /// </summary>
        private void ResetGame()
        {
            GameObjects.Clear();

            // Add 50 new balloons
            for (int i = 0; i < 50; i++)
            {
                GameObjects.Add(new BalloonObject(this, Textures["Balloon"]));
            }
        }


    }
}
