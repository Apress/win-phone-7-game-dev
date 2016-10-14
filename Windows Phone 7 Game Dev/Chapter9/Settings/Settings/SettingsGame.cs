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

namespace Settings
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SettingsGame : GameHost
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // An enumeration containing the game modes that we can support
        private enum GameModes
        {
            Playing,
            Settings
        };
        // The active game mode
        private GameModes _gameMode = GameModes.Playing;

        public SettingsGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            _graphics.IsFullScreen = true;
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

            // Read any existing settings
            //SettingsManager.LoadSettings();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw Settingsures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load content
            Textures.Add("Ball", Content.Load<Texture2D>("Ball"));
            Fonts.Add("WascoSans", Content.Load<SpriteFont>("WascoSans"));

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
            switch (_gameMode)
            {
                case GameModes.Playing:
                    Update_Playing(gameTime);
                    break;

                case GameModes.Settings:
                    Update_Settings(gameTime);
                    break;
            }

            UpdateAll(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// Update the game in "playing" mode
        /// </summary>
        private void Update_Playing(GameTime gameTime)
        {
            TouchCollection tc;

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.Exit();
            }

            // Has the user touched the screen?
            tc = TouchPanel.GetState();
            if (tc.Count == 1 && tc[0].State == TouchLocationState.Pressed)
            {
                // Enter settings mode
                EnterSettingsMode();
            }
        }

        /// <summary>
        /// Update the game in "settings" mode
        /// </summary>
        private void Update_Settings(GameTime gameTime)
        {
            GameObjectBase obj;
            TouchCollection tc;

            // Allows the user to return to the main game
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                LeaveSettingsMode();
            }

            UpdateAll(gameTime);

            // Has the user touched the screen?
            tc = TouchPanel.GetState();
            if (tc.Count == 1 && tc[0].State == TouchLocationState.Pressed)
            {
                // Find the object at the touch point
                obj = GetSpriteAtPoint(tc[0].Position);
                // Did we get a settings option object?
                if (obj is GameFramework.SettingsItemObject)
                {
                    // Yes, so toggle it to the next value
                    ((SettingsItemObject)obj).SelectNextValue();
                }
            }
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            switch (_gameMode)
            {
                case GameModes.Playing:
                    Draw_Playing(gameTime);
                    break;

                case GameModes.Settings:
                    Draw_Settings(gameTime);
                    break;
            }

            base.Draw(gameTime);
        }

        /// <summary>
        /// Draw the game in "playing" mode
        /// </summary>
        private void Draw_Playing(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.PaleGoldenrod);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            DrawSprites(gameTime, _spriteBatch);
            _spriteBatch.End();
        }

        /// <summary>
        /// Draw the game in "settings" mode
        /// </summary>
        private void Draw_Settings(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.OrangeRed);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            DrawText(gameTime, _spriteBatch);
            _spriteBatch.End();
        }

        private void ResetGame()
        {
            // Add some balls
            for (int i = 0; i < 10; i++)
            {
                GameObjects.Add(new BallObject(this, Textures["Ball"], SettingsManager.GetValue("Speed", 1)));
            }
        }

        /// <summary>
        /// Switch from the game's current mode into Settings mode
        /// </summary>
        private void EnterSettingsMode()
        {
            // Push the game’s objects onto the object stack
            PushGameObjects();

            // Add the title
            GameObjects.Add(new TextObject(this, Fonts["WascoSans"], new Vector2(10, 10), "Game Settings"));

            // Add some settings
            GameObjects.Add(new SettingsItemObject(this, new Vector2(30, 90), Fonts["WascoSans"], 0.9f, SettingsManager, "Speed", "Speed", "1", new string[] { "1", "2", "3" }));
            GameObjects.Add(new SettingsItemObject(this, new Vector2(30, 140), Fonts["WascoSans"], 0.9f, SettingsManager, "Difficulty", "Difficulty", "Medium", new string[] { "Easy", "Medium", "Hard" }));
            GameObjects.Add(new SettingsItemObject(this, new Vector2(30, 190), Fonts["WascoSans"], 0.9f, SettingsManager, "MusicVolume", "Music volume", "Medium", new string[] { "Off", "Quiet", "Medium", "Loud" }));
            GameObjects.Add(new SettingsItemObject(this, new Vector2(30, 240), Fonts["WascoSans"], 0.9f, SettingsManager, "SoundEffectsVolume", "Sound effects volume", "Loud", new string[] { "Off", "Quiet", "Medium", "Loud" }));
            GameObjects.Add(new SettingsItemObject(this, new Vector2(30, 290), Fonts["WascoSans"], 0.9f, SettingsManager, "FullScreen", "Full screen", "On", new string[] { "Off", "On" }));
            GameObjects.Add(new SettingsItemObject(this, new Vector2(30, 340), Fonts["WascoSans"], 0.9f, SettingsManager, "Orientation", "Screen orientation", "Auto", new string[] { "Auto", "Portrait", "Landscape" }));

            // Set the new game mode
            _gameMode = GameModes.Settings;
        }

        /// <summary>
        /// Return from Settings mode back to Playing mode
        /// </summary>
        private void LeaveSettingsMode()
        {
            // Retrieve the settings values from the settings page
            SettingsManager.RetrieveValues();

            // Retrieve the previous game objects
            PopGameObjects();
            // Set the new game mode
            _gameMode = GameModes.Playing;

            // Update the speed of each ball
            foreach (GameObjectBase obj in GameObjects)
            {
                if (obj is BallObject)
                {
                    ((BallObject)obj).Speed = SettingsManager.GetValue("Speed", 1);
                }
            }
        }

    }
}
