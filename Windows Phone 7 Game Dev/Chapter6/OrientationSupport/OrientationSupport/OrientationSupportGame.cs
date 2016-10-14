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

namespace OrientationSupport
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class OrientationSupportGame : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Variables required for the scene to be rendered
        private BasicEffect _effect;
        private VertexPositionTexture[] _vertices = new VertexPositionTexture[4];
        private Texture2D _texture;

        private float _angle;

        public OrientationSupportGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 30);

            // Allow portrait and both landscape rotations
            _graphics.SupportedOrientations = DisplayOrientation.Portrait |
                                    DisplayOrientation.LandscapeLeft |
                                    DisplayOrientation.LandscapeRight;

            // Set back buffer size and orientation
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 480;
            // Display in full screen mode
            _graphics.IsFullScreen = true;

            // Add a handler so that we can update the projection matrix if the orientation changes
            Window.OrientationChanged += new EventHandler<EventArgs>(Window_OrientationChanged);

          }

        /// <summary>
        /// Update the projection matrix for the new orientation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Window_OrientationChanged(object sender, EventArgs e)
        {
            //// Calculate the new screen aspect ratio
            //float aspectRatio = (float)GraphicsDevice.Viewport.Width / GraphicsDevice.Viewport.Height;
            //// Create a projection matrix
            //Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), aspectRatio, 0.1f, 1000.0f);
            //// Set the matrix into the effect
            //_effect.Projection = projection;
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
            Matrix view = Matrix.CreateLookAt(new Vector3(0, 0, 10), Vector3.Zero, Vector3.Up);

            _effect = new BasicEffect(GraphicsDevice);
            _effect.LightingEnabled = false;
            _effect.VertexColorEnabled = false;
            _effect.TextureEnabled = true;
            _effect.Projection = projection;
            _effect.View = view;
            _effect.World = Matrix.Identity;

            _vertices[0].Position = new Vector3(-1, -1, 0);
            _vertices[1].Position = new Vector3(-1, 1, 0);
            _vertices[2].Position = new Vector3(1, -1, 0);
            _vertices[3].Position = new Vector3(1, 1, 0);

            _vertices[0].TextureCoordinate = new Vector2(0, 1);
            _vertices[1].TextureCoordinate = new Vector2(0, 0);
            _vertices[2].TextureCoordinate = new Vector2(1, 1);
            _vertices[3].TextureCoordinate = new Vector2(1, 0);

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

            // Load our texture
            _texture = Content.Load<Texture2D>("Grapes");
            // Set it as the active texture within our effect
            _effect.Texture = _texture;
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

            _angle += MathHelper.ToRadians(10);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Draw three objects so that we can see how they blend together
            for (int i = 0; i < 3; i++)
            {
                // Translate a small way up or down the screen
                _effect.World = Matrix.CreateTranslation(0, i - 1, 0);
                // Set the world matrix so that the square rotates
                _effect.World = Matrix.CreateRotationZ((float)Math.Sin(_angle) * MathHelper.Pi / 10) * _effect.World;

                foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
                {
                    // Apply the pass
                    pass.Apply();
                    // Draw the square
                    GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, _vertices, 0, 2);
                }
            }

            base.Draw(gameTime);
        }
    }
}
