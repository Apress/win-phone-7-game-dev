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

namespace TouchPanelDemo
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class TouchPanelGame : GameFramework.GameHost
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public TouchPanelGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 30);

            _graphics.PreferredBackBufferWidth = 480;
            _graphics.PreferredBackBufferHeight = 800;
#if WINDOWS_PHONE
            _graphics.IsFullScreen = true;
#else
            this.IsMouseVisible = true;
#endif
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
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

            // Load content
            Textures.Add("TouchPoint", Content.Load<Texture2D>("TouchPoint"));
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

            // Update all game objects
            UpdateAll(gameTime);

#if WINDOWS_PHONE
            // Get the current screen touch points
            TouchCollection touches = TouchPanel.GetState();
            // Loop for each point
            for (int i = 0; i < touches.Count; i++)
            {
                // Set the position of the sprite at this index to match the touch position
                ((SpriteObject)GameObjects[i]).Position = touches[i].Position;
                ((SpriteObject)GameObjects[i]).Scale = new Vector2(2);
                // Is this the first touch point?
                if (i == 0)
                {
                    // Yes, so display its details in the debug window.

                    // First get its previous location
                    TouchLocation prevLocation;
                    bool prevLocationAvailable = touches[i].TryGetPreviousLocation(out prevLocation);
                    // Write to the debug window
                    System.Diagnostics.Debug.WriteLine("Id: " + touches[i].Id.ToString()
                                        + ", State: " + touches[i].State.ToString()
                                        + ", Position: " + touches[i].Position.ToString()
                                        + ", Previous Position: " + prevLocation.Position.ToString());
                }
            }
#elif WINDOWS
            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                // Set the position of the sprite at this index to match the touch position
                ((SpriteObject)GameObjects[0]).Position = new Vector2(mouseState.X, mouseState.Y);
                ((SpriteObject)GameObjects[0]).Scale = new Vector2(2);
                // Yes, display its details in the debug window.
            }
                
            // Write to the debug window
            System.Diagnostics.Debug.WriteLine("LeftButtonState: " + mouseState.ScrollWheelValue.ToString()
                                +", Position: " + mouseState.X.ToString() + "," + mouseState.Y.ToString());
#endif


            // Reduce the scale of all touch points so that they fade away
            for (int i = 0; i < GameObjects.Count; i++)
            {
                // Is the object at this position a sprite (and not text)?
                if (GameObjects[i] is SpriteObject && !(GameObjects[i] is TextObject))
                {
                    // Yes, so scale it down a little
                    ((SpriteObject)GameObjects[i]).Scale *= 0.95f;
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

            _spriteBatch.Begin();
            DrawSprites(gameTime, _spriteBatch);
            DrawText(gameTime, _spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }


        private void ResetGame()
        {
            int maxTouchPoints;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            SpriteObject touchPointObj;

            // Clear any existing game objects
            GameObjects.Clear();

            // Find the max touch points
#if WINDOWS_PHONE
            maxTouchPoints = TouchPanel.GetCapabilities().MaximumTouchCount;
#else
            // No actual touch points, but we'll treat the mouse as a touch point
            // so indicate that one point is available
            maxTouchPoints = 1;
#endif
            
            // Add a game object for each of the touch points
            for (int i = 0; i < maxTouchPoints; i++)
            {
                touchPointObj = new SpriteObject(this, new Vector2(-100, -100), Textures["TouchPoint"]);
                touchPointObj.Origin = new Vector2(touchPointObj.SpriteTexture.Width / 2, touchPointObj.SpriteTexture.Height / 2);
                switch (i)
                {
                    case 1: touchPointObj.SpriteColor = Color.PaleVioletRed; break;
                    case 2: touchPointObj.SpriteColor = Color.MediumBlue; break;
                    case 3: touchPointObj.SpriteColor = Color.PaleGreen; break;
                    default: touchPointObj.SpriteColor = Color.White; break;
                }
                GameObjects.Add(touchPointObj);
            }

            // Display info about the touch points on the screen
            sb.Append("MaximumTouchCount: " + maxTouchPoints.ToString());
            GameObjects.Add(new TextObject(this, Fonts["Miramonte"], new Vector2(0, 0), sb.ToString()));
        }


    }
}
