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

namespace DualTexture
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class DualTextureGame : GameFramework.GameHost
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Variables required for the scene to be rendered
        private DualTextureEffect _effect;

        public DualTextureGame()
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
            // Calculate the screen aspect ratio
            float aspectRatio = (float)GraphicsDevice.Viewport.Width / GraphicsDevice.Viewport.Height;
            // Create a projection matrix
            Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), aspectRatio, 0.1f, 1000.0f);

            // Calculate a view matrix (where we are looking from and to)
            Matrix view = Matrix.CreateLookAt(new Vector3(0, 0, 2), Vector3.Zero, Vector3.Up);

            // Create and initialize the effect
            _effect = new DualTextureEffect(GraphicsDevice);
            _effect.VertexColorEnabled = false;
            _effect.Projection = projection;
            _effect.View = view;
            _effect.World = Matrix.Identity;

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
            Textures.Add("MoireTexture1", Content.Load<Texture2D>("MoireTexture1"));
            Textures.Add("MoireTexture2", Content.Load<Texture2D>("MoireTexture2"));
            Textures.Add("Ground", Content.Load<Texture2D>("Ground"));
            Textures.Add("Lights", Content.Load<Texture2D>("Lights"));

            // Reset the game
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

            // Update all the game objects
            UpdateAll(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // Draw all objects
            DrawObjects(gameTime, _effect);

            base.Draw(gameTime);
        }

        /// <summary>
        /// Reset the game
        /// </summary>
        private void ResetGame()
        {
            // Clear any existing objects
            GameObjects.Clear();

            // Add an object
            GameObjects.Add(new SquareObject(this, Vector3.Zero, Textures["Ground"], Textures["Lights"]));
            //GameObjects.Add(new SquareObject(this, Vector3.Zero, Textures["MoireTexture1"], Textures["MoireTexture2"]));
        }

    }
}
