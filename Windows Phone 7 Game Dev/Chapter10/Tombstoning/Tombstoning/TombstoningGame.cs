using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Phone.Shell;

namespace Tombstoning
{

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class TombstoningGame : GameHost
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private int _score = 0;
        private TextObject _scoreText;

        public TombstoningGame()
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

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw Tombstoningures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load content
            Textures.Add("Ball", Content.Load<Texture2D>("Ball"));
            Fonts.Add("WascoSans", Content.Load<SpriteFont>("WascoSans"));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        protected override void GameLaunching()
        {
            // Reset the game
            ResetGame();
        }

        protected override void GameDeactivated()
        {
            // Write the game objects to the phone state
            WriteGameObjectsToPhoneState();

            // Write other data to the phone state
            PhoneApplicationService.Current.State.Add("_score", _score);
        }

        protected override void GameActivated()
        {
            // Read the game objects back from the phone state
            ReadGameObjectsFromPhoneState();
            
            // Read other state data
            _score = (int)PhoneApplicationService.Current.State["_score"];

            // Restore class-level game object references
            _scoreText = (TextObject)GetObjectByTag("_scoreText");
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
            {
                this.Exit();
            }

            UpdateAll(gameTime);

            // Increase the player's score
            if (_scoreText != null)
            {
                _score += 1;
                _scoreText.Text = "Your score: " + _score.ToString();
            }

            base.Update(gameTime);
        }



        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DeepSkyBlue);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            DrawSprites(gameTime, _spriteBatch);
            DrawText(gameTime, _spriteBatch);
            _spriteBatch.End();
        }

        /// <summary>
        /// Reset the game to its default state
        /// </summary>
        private void ResetGame()
        {
            GameObjects.Clear();


            // Show the player some text
            GameObjects.Add(new TextObject(this, Fonts["WascoSans"], new Vector2(Window.ClientBounds.Width / 2, 30), "I feel fantastic and I'm still alive!", TextObject.TextAlignment.Center, TextObject.TextAlignment.Near));
            // Show the player's score
            _scoreText = new TextObject(this, Fonts["WascoSans"], new Vector2(Window.ClientBounds.Width / 2, 120), "Your score: 0", TextObject.TextAlignment.Center, TextObject.TextAlignment.Near);
            _scoreText.Tag = "_scoreText";
            GameObjects.Add(_scoreText);

            for (int i = 0; i < 10; i++)
            {
                GameObjects.Add(new BallObject(this, Textures["Ball"], 1));
            }
        }

    }
}
