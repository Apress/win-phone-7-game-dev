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

namespace HighScores
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class HighScoresGame : GameHost
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // An enumeration containing the game modes that we can support
        private enum GameModes
        {
            Playing,
            HighScores
        };
        // The active game mode
        private GameModes _gameMode = GameModes.Playing;

        // The player's score
        private int _score;

        public HighScoresGame()
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

            // Initialize and load the high scores
            HighScores.InitializeTable("Normal", 20);
            HighScores.InitializeTable("Difficult", 20);
            HighScores.LoadScores();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw HighScoresures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load content
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

                case GameModes.HighScores:
                    Update_HighScores(gameTime);
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
                // Enter HighScores mode
                EnterHighScoresMode();
            }
        }

        /// <summary>
        /// Update the game in "HighScores" mode
        /// </summary>
        private void Update_HighScores(GameTime gameTime)
        {
            // Allows the user to return to the main game
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                LeaveHighScoresMode();
            }

            // Allow screen taps to leave the highscore mode too
            TouchCollection tc = TouchPanel.GetState();
            if (tc.Count == 1 && tc[0].State == TouchLocationState.Pressed)
            {
                LeaveHighScoresMode();
            }

            UpdateAll(gameTime);
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

                case GameModes.HighScores:
                    Draw_HighScores(gameTime);
                    break;
            }

            base.Draw(gameTime);
        }

        /// <summary>
        /// Draw the game in "playing" mode
        /// </summary>
        private void Draw_Playing(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkBlue);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            DrawText(gameTime, _spriteBatch);
            _spriteBatch.End();
        }

        /// <summary>
        /// Draw the game in "HighScores" mode
        /// </summary>
        private void Draw_HighScores(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.OrangeRed);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            DrawText(gameTime, _spriteBatch);
            _spriteBatch.End();
        }

        private void ResetGame()
        {
            GameObjects.Clear();

            // Generate a random score
            _score = GameHelper.RandomNext(100, 200) * 10;

            // Show the player some "game over" text
            GameObjects.Add(new TextObject(this, Fonts["WascoSans"], new Vector2(Window.ClientBounds.Width / 2, 30), "*** Game over ***", TextObject.TextAlignment.Center, TextObject.TextAlignment.Near));
            GameObjects.Add(new TextObject(this, Fonts["WascoSans"], new Vector2(Window.ClientBounds.Width / 2, 100), "Your score was " + _score.ToString(), TextObject.TextAlignment.Center, TextObject.TextAlignment.Near));
            // Is this good enough for a high score?
            if (HighScores.GetTable("Normal").ScoreQualifies(_score))
            {
                GameObjects.Add(new TextObject(this, Fonts["WascoSans"], new Vector2(Window.ClientBounds.Width / 2, 220), "You got a high score!", TextObject.TextAlignment.Center, TextObject.TextAlignment.Near));
            }
            else
            {
                GameObjects.Add(new TextObject(this, Fonts["WascoSans"], new Vector2(Window.ClientBounds.Width / 2, 220), "No high score this time...", TextObject.TextAlignment.Center, TextObject.TextAlignment.Near));
            }
            GameObjects.Add(new TextObject(this, Fonts["WascoSans"], new Vector2(Window.ClientBounds.Width / 2, 330), "Tap the screen to continue...", TextObject.TextAlignment.Center, TextObject.TextAlignment.Near));
        }

        /// <summary>
        /// Switch from the game's current mode into HighScores mode
        /// </summary>
        private void EnterHighScoresMode()
        {
            // Set the new game mode
            _gameMode = GameModes.HighScores;

            // Did the player's score qualify?
            if (HighScores.GetTable("Normal").ScoreQualifies(_score))
            {
                // Yes, so display the input dialog
                // Make sure the input dialog is not already visible
                if (!(Guide.IsVisible))
                {
                    // Show the input dialog to get text from the user
                    Guide.BeginShowKeyboardInput(PlayerIndex.One, "High score achieved", "Please enter your name", SettingsManager.GetValue("PlayerName", ""), InputCallback, null);
                }
            }
            else
            {
                // Show the highscores now. No score added so nothing to highlight
                ResetHighscoreTableDisplay(null);
            }
        }

        /// <summary>
        /// Return from HighScores mode back to Playing mode
        /// </summary>
        private void LeaveHighScoresMode()
        {
            // Reset the game ready for a new game to begin
            ResetGame();

            // Set the new game mode
            _gameMode = GameModes.Playing;
        }

        /// <summary>
        /// Set the game objects collection to display the high score table
        /// </summary>
        /// <param name="highlightEntry">If a new score has been added, pass its entry here and it will be highlighted</param>
        private void ResetHighscoreTableDisplay(HighScoreEntry highlightEntry)
        {
            // Clear any existing game objects
            GameObjects.Clear();

            // Add the title
            GameObjects.Add(new TextObject(this, Fonts["WascoSans"], new Vector2(10, 10), "High Scores"));

            // Add the score objects
            HighScores.CreateTextObjectsForTable("Normal", Fonts["WascoSans"], 0.8f, 80, 30, Color.White, Color.Blue, highlightEntry, Color.Yellow);
        }

        void InputCallback(IAsyncResult result)
        {
            string sipContent = Guide.EndShowKeyboardInput(result);
            HighScoreEntry newEntry = null;

            // Did we get some input from the user?
            if (sipContent != null)
            {
                // Add the name to the highscore
                newEntry = HighScores.GetTable("Normal").AddEntry(sipContent, _score);
                // Save the scores
                HighScores.SaveScores();
                // Store the name for later use
                SettingsManager.SetValue("PlayerName", sipContent);
            }
            // Show the highscores now and highlight the new entry if we have one
            ResetHighscoreTableDisplay(newEntry);
        } 

    }
}
