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

namespace BackgroundMusic
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class BackgroundMusicGame : GameFramework.GameHost
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public BackgroundMusicGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 30);

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

            // Load fonts
            Fonts.Add("Miramonte", Content.Load<SpriteFont>("Miramonte"));

            // Load songs.
            // Are we in control of the media player?
            if (MediaPlayer.GameHasControl)
            {
                // Load our song
                Songs.Add("2020", Content.Load<Song>("Breadcrumbs_2020"));

                // Play the song, repeating
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Play(Songs["2020"]);
            }

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

            TimeSpan currentPosition;
            TimeSpan duration;

            // Are we playing a song?
            if (MediaPlayer.GameHasControl)
            {
                // Yes, so read the position and duraction
                currentPosition = MediaPlayer.PlayPosition;
                duration = Songs["2020"].Duration;
                // Display the details in our text object
                TextObject positionText = (TextObject)GameObjects[0];
                positionText.Text = "Song position: "
                            + new DateTime(currentPosition.Ticks).ToString("mm:ss") + "/"
                            + new DateTime(duration.Ticks).ToString("mm:ss");
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
            DrawText(gameTime, _spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }


        private void ResetGame()
        {
            if (Songs.Count == 0)
            {
                // Not currently playing
                GameObjects.Add(new TextObject(this, Fonts["Miramonte"], new Vector2(10, 50), "Game is not in control of MediaPlayer"));
            }
            else
            {
                // Add a text object that we can use to display the song position
                GameObjects.Add(new TextObject(this, Fonts["Miramonte"], new Vector2(10, 50)));
            }
        }

    }
}
