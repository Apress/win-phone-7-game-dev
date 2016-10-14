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
using Microsoft.Devices.Sensors;

namespace AccelerometerDemo
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class AccelerometerGame : GameFramework.GameHost
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Accelerometer _accelerometer;
        private TextObject _accText;
        private BallObject _accBall;

        internal Vector3 AccelerometerData { get; set; }

        public AccelerometerGame()
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

            // Instantiate the accelerometer
            _accelerometer = new Accelerometer();
            // Add an event handler
            _accelerometer.ReadingChanged += AccelerometerReadingChanged;
            // Start the accelerometer
            _accelerometer.Start();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load game content
            Textures.Add("ShinyBall", Content.Load<Texture2D>("ShinyBall"));
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

            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(240, 240, 240));

            _spriteBatch.Begin();
            DrawSprites(gameTime, _spriteBatch);
            DrawText(gameTime, _spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void ResetGame()
        {
            // Create a single text object
            GameObjects.Clear();

            // Add a text object to display the accelerometer vector
            _accText = new TextObject(this, Fonts["Miramonte"], new Vector2(20, 100), "Accelerometer data:");
            _accText.SpriteColor = Color.DarkBlue;
            GameObjects.Add(_accText);

            // Add a ball to roll around the screen
            _accBall = new BallObject(this, new Vector2(240, 400), Textures["ShinyBall"]);
            _accBall.Origin = new Vector2(_accBall.SpriteTexture.Width, _accBall.SpriteTexture.Height) / 2;
            GameObjects.Add(_accBall);
        }

        /// <summary>
        /// Event that fires each time a new reading is received from the accelerometer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AccelerometerReadingChanged(object sender, AccelerometerReadingEventArgs e)
        {
            if (Microsoft.Devices.Environment.DeviceType == Microsoft.Devices.DeviceType.Device)
            {
                AccelerometerData = new Vector3((float)e.X, (float)e.Y, (float)e.Z);
            }
            else
            {

                // Use the touch screen to simulate the accelerometer
                float x, y;
                TouchCollection touches;
                touches = TouchPanel.GetState();
                if (touches.Count > 0)
                {
                    x = (touches[0].Position.X - Window.ClientBounds.Width / 2) / (Window.ClientBounds.Width / 2);
                    y = -(touches[0].Position.Y - Window.ClientBounds.Height / 2) / (Window.ClientBounds.Height / 2);
                    AccelerometerData = new Vector3(x, y, 0);
                }

                //// Use the cursor keys on the keyboard to simulate the accelerometer
                //Vector3 accData = AccelerometerData;
                //if (Keyboard.GetState().IsKeyDown(Keys.Left)) accData.X -= 0.05f;
                //if (Keyboard.GetState().IsKeyDown(Keys.Right)) accData.X += 0.05f;
                //if (Keyboard.GetState().IsKeyDown(Keys.Up)) accData.Y += 0.05f;
                //if (Keyboard.GetState().IsKeyDown(Keys.Down)) accData.Y -= 0.05f;
                //// Ensure that the data stays within valid bounds of -1 to 1 on each axis
                //accData.X = MathHelper.Clamp(accData.X, -1, 1);
                //accData.Y = MathHelper.Clamp(accData.Y, -1, 1);
                //// Put the vector back into the AccelerometerData property
                //AccelerometerData = accData;
            }

            // Update the content of the text object
            _accText.Text = "Accelerometer data: " + AccelerometerData.X.ToString("0.000")
                                                    + ", " + AccelerometerData.Y.ToString("0.000")
                                                    + ", " + AccelerometerData.Z.ToString("0.000");
        } 
    }
}
