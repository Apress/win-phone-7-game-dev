using GameFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Text;
using System.Xml;

#if WINDOWS_PHONE
using Microsoft.Phone.Shell;
#endif

namespace GameFramework
{
    // Derive GameHost from the XNA Game class.
    // Our actual game classes can then derive from GameHost
    // in order to pick up all of the functionality added here.
    [System.Runtime.Serialization.DataContract]
    public class GameHost : Microsoft.Xna.Framework.Game
    {

        //-------------------------------------------------------------------------------------
        // Class variables

        private GameObjectBase[] _objectArray;

        // A stack of gameobjects lists so that we can work with multiple object sets
        private Stack<List<GameObjectBase>> _gameObjectsStack;

        // The following variables store the graphics device state values
        // that were present prior to sprite rendering so that they can be
        // restored afterwards
        private BlendState _preSpriteBlendState = BlendState.Opaque;
        private DepthStencilState _preSpriteDepthStencilState = DepthStencilState.Default;
        private RasterizerState _preSpriteRasterizerState = RasterizerState.CullCounterClockwise;
        private SamplerState _preSpriteSamplerState = SamplerState.LinearWrap;


        //-------------------------------------------------------------------------------------
        // Constructors

        public GameHost()
        {
            // Create new collections
            Textures = new Dictionary<string, Texture2D>();
            Fonts = new Dictionary<string, SpriteFont>();
            Models = new Dictionary<string, Model>();
            SoundEffects = new Dictionary<string, SoundEffect>();
            Songs = new Dictionary<string, Song>();
            // The list of active game objects
            GameObjects = new List<GameObjectBase>();

            // Create the stack of GameObjects lists
            _gameObjectsStack = new Stack<List<GameObjectBase>>();

            // Create other objects
            SettingsManager = new SettingsManager(this);
            HighScores = new HighScores(this);

            // Set up application lifecycle event handlers
#if WINDOWS_PHONE
            PhoneApplicationService.Current.Launching += GameLaunchingEvent;
            PhoneApplicationService.Current.Closing += GameClosingEvent;
            PhoneApplicationService.Current.Deactivated += GameDeactivatedEvent;
            PhoneApplicationService.Current.Activated += GameActivatedEvent;
#else
            GameLaunching();
#endif
        }


        //-------------------------------------------------------------------------------------
        // Properties

        // A dictionary of loaded textures. Using a dictionary allows us to easily access the texture by name
        public Dictionary<string, Texture2D> Textures { get; set; }
        // A dictionary of loaded fonts.
        public Dictionary<string, SpriteFont> Fonts { get; set; }
        // A dictionary of loaded models.
        public Dictionary<string, Model> Models { get; set; }

        // A list of active game objects.
        public List<GameObjectBase> GameObjects { get; set; }


        // A dictionary of loaded sound effects.
        public Dictionary<string, SoundEffect> SoundEffects { get; set; }
        // A dictionary of loaded songs.
        public Dictionary<string, Song> Songs { get; set; }

        // A camera object, if one is required
        public MatrixCameraObject Camera;
        // A sky box, if one is required
        public MatrixSkyboxObject Skybox;

        // A pre-defined instance of the SettingsManager class
        public SettingsManager SettingsManager;
        // A pre-defined instance of the HighScores class
        public HighScores HighScores;


        //-------------------------------------------------------------------------------------
        // Event handlers

#if WINDOWS_PHONE
        /// <summary>
        /// Handle the Launching event
        /// </summary>
        private void GameLaunchingEvent(object sender, Microsoft.Phone.Shell.LaunchingEventArgs e)
        {
            // Call the virtual function so that the game can provide its own handling code
            GameLaunching();
        }

        /// <summary>
        /// Handle the Closing event
        /// </summary>
        private void GameClosingEvent(object sender, Microsoft.Phone.Shell.ClosingEventArgs e)
        {
            // Call the virtual function so that the game can provide its own handling code
            GameClosing();
        }

        /// <summary>
        /// Handle the Deactivated event
        /// </summary>
        private void GameDeactivatedEvent(object sender, Microsoft.Phone.Shell.DeactivatedEventArgs e)
        {
            // Call the virtual function so that the game can provide its own handling code
            GameDeactivated();
        }

        /// <summary>
        /// Handle the Activated event
        /// </summary>
        private void GameActivatedEvent(object sender, Microsoft.Phone.Shell.ActivatedEventArgs e)
        {
            // Call the virtual function so that the game can provide its own handling code
            GameActivated();
        }
#endif

        /// <summary>
        /// Virtual function to allow the game to handle the Launching event
        /// </summary>
        protected virtual void GameLaunching()
        {
        }

        /// <summary>
        /// Virtual function to allow the game to handle the Closing event
        /// </summary>
        protected virtual void GameClosing()
        {
        }

        /// <summary>
        /// Virtual function to allow the game to handle the Deactivated event
        /// </summary>
        protected virtual void GameDeactivated()
        {
        }

        /// <summary>
        /// Virtual function to allow the game to handle the Activated event
        /// </summary>
        protected virtual void GameActivated()
        {
        }

        //-------------------------------------------------------------------------------------
        // Game functions

        /// <summary>
        /// Call the Update method on all objects in the GameObjects collection
        /// </summary>
        /// <param name="gameTime"></param>
        protected virtual void UpdateAll(GameTime gameTime)
        {
            int i;
            int objectCount;

            // First build our array of objects.
            // We will iterate across this rather than across the actual GameObjects
            // collection so that the collection can be modified by the game objects'
            // Update code.
            // First of all, do we have an array?
            if (_objectArray == null)
            {
                // No, so allocate it.
                // Allocate 20% more objects than we currently have, or 20 objects, whichever is more
                _objectArray = new GameObjectBase[(int)MathHelper.Max(20, GameObjects.Count * 1.2f)];
            }
            else if (GameObjects.Count > _objectArray.Length)
            {
                // The number of game objects has exceeded the array size.
                // Reallocate the array, adding 20% free space for further expansion.
                _objectArray = new GameObjectBase[(int)(GameObjects.Count * 1.2f)];
            }

            // Store the current object count for performance
            objectCount = GameObjects.Count;

            // Transfer the object references into the array
            for (i = 0; i < _objectArray.Length; i++)
            {
                // Is there an active object at this position in the GameObjects collection?
                if (i < objectCount)
                {
                    // Yes, so copy it to the array
                    _objectArray[i] = GameObjects[i];
                }
                else
                {
                    // No, so clear any reference stored at this index position
                    _objectArray[i] = null;
                }
            }

            // Loop for each element within the array
            for (i = 0; i < objectCount; i++)
            {
                // Update the object at this array position
                _objectArray[i].Update(gameTime);
            }

            // Finally, if we have a camera or sky box, update them too
            if (Camera != null) Camera.Update(gameTime);
            if (Skybox != null) Skybox.Update(gameTime);
        }

        /// <summary>
        /// Push the current collection of game objects on to the stack
        /// and create a new game objects list to work with.
        /// </summary>
        public void PushGameObjects()
        {
            // Push the current GameObjects list on to the stack
            _gameObjectsStack.Push(GameObjects);
            // Create a new empty list for the game to work with
            GameObjects = new List<GameObjectBase>();
        }

        /// <summary>
        /// Pop the topmost collection of game objects off the stack and
        /// make them active once again.
        /// </summary>
        public void PopGameObjects()
        {
            // Pop the list from the top of the stack and set it as the GameObjects list
            GameObjects = _gameObjectsStack.Pop();
        }

        /// <summary>
        /// Scan the GameObjects list looking for an object with the specified tag
        /// </summary>
        /// <param name="tag">The tag to search for</param>
        /// <returns>The tagged object if one is found, or null if there was no match</returns>
        public GameObjectBase GetObjectByTag(string tag)
        {
            // Loop through the objects
            foreach (GameObjectBase obj in GameObjects)
            {
                // Does this tag match?
                if (tag.Equals(obj.Tag))
                {
                    // Yes, so return the object
                    return obj;
                }
            }
            // No matching tag
            return null;
        }

#if WINDOWS_PHONE
        /// <summary>
        /// Write all of the game objects to the phone state.
        /// Call when the game is being Deactivated to store state for when it resumes.
        /// </summary>
        protected void WriteGameObjectsToPhoneState()
        {
            // Clear any previous state
            PhoneApplicationService.Current.State.Clear();

            // Keep track of the index so we can generate a unique dictionary key for each object
            int objIndex = 0;
            foreach (GameObjectBase obj in GameObjects)
            {
                // Is this object interested in being added to the phone state?
                if (obj.WriteToPhoneState)
                {
                    // Generate a name and add the object
                    PhoneApplicationService.Current.State.Add("_obj" + objIndex.ToString(), obj);
                    // Move to the next object index number
                    objIndex += 1;
                }
            }
            // Save the camera and skybox too
            if (Camera != null) PhoneApplicationService.Current.State.Add("_camera", Camera);
            if (Skybox != null) PhoneApplicationService.Current.State.Add("_skybox", Skybox);
        }

        /// <summary>
        /// Read all of the game objects back from the phone state
        /// </summary>
        protected void ReadGameObjectsFromPhoneState()
        {
            GameObjectBase obj;
            // Loop for each state item key/value
            foreach (KeyValuePair<string, object> stateItem in PhoneApplicationService.Current.State)
            {
                // Is the value a game object?
                if (stateItem.Value is GameObjectBase)
                {
                    // It is. Retrieve a reference to the object
                    obj = (GameObjectBase)stateItem.Value;
                    // Set its Game property so that it can see the GameHost
                    obj.Game = this;
                    // Is it a camera, skybox or general object? Deploy as appropriate
                    if (obj is MatrixCameraObject)
                    {
                        Camera = (MatrixCameraObject)obj;
                    }
                    else if (obj is MatrixSkyboxObject)
                    {
                        Skybox = (MatrixSkyboxObject)obj;
                    }
                    else
                    {
                        // Add the object back into the GameObjects collection
                        GameObjects.Add(obj);
                    }
                }
            }
        }
#endif



        /// <summary>
        /// Retrieve the key of the provided object from the specified dictionary
        /// </summary>
        /// <typeparam name="T">The type of object to be examined</typeparam>
        /// <param name="objectDictionary">The dictionary whose objects are to be scanned</param>
        /// <param name="contentObject">The object whose name is to be returned</param>
        /// <returns>Returns the object name</returns>
        public string GetContentObjectName<T>(Dictionary<string, T> objectDictionary, T contentObject)
        {
            // Loop for each key/value pair
            foreach (KeyValuePair<string, T> dictItem in objectDictionary)
            {
                // Is this item's value the object that we are searching for?
                if (EqualityComparer<T>.Default.Equals(dictItem.Value, contentObject))
                {
                    return dictItem.Key;
                }
            }
            // Couldn't find the requested object
            return null;
        }

        //-------------------------------------------------------------------------------------
        // Sprite functions

        /// <summary>
        /// Call the Draw method on all SpriteObject-based objects in the game host.
        /// </summary>
        /// <param name="gameTime"></param>
        public void DrawSprites(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Call the other DrawSprites overload, passing null for restrictToTexture
            DrawSprites(gameTime, spriteBatch, null);
        }

        /// <summary>
        /// Call the Draw method on all SpriteObject-based objects in the game host
        /// whose texture matches the one provided.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void DrawSprites(GameTime gameTime, SpriteBatch spriteBatch, Texture2D restrictToTexture)
        {
            GameObjectBase obj;
            int objectCount;

            // Loop for each sprite object
            objectCount = _objectArray.Length;
            for (int i = 0; i < objectCount; i++)
            {
                obj = _objectArray[i];
                // Is this a sprite object, and not a text object (which is handled separately using DrawText)?
                if (obj is SpriteObject && !(obj is TextObject))
                {
                    // If we are restricting to a texture, does the texture match?
                    if (restrictToTexture == null || ((SpriteObject)obj).SpriteTexture == restrictToTexture)
                    {
                        // Yes, so call its Draw method
                        ((SpriteObject)obj).Draw(gameTime, spriteBatch);
                    }
                }
            }
        }

        /// <summary>
        /// Scan the sprites to find those that contain the specified position.
        /// Returns an array of all matching sprites.
        /// </summary>
        /// <param name="testPosition"></param>
        public SpriteObject[] GetSpritesAtPoint(Vector2 testPosition)
        {
            SpriteObject spriteObj;
            SpriteObject[] hits = new SpriteObject[GameObjects.Count];
            int hitCount = 0;

            // Loop for all of the SelectableSpriteObjects
            foreach (GameObjectBase obj in GameObjects)
            {
                // Is this a SpriteObject?
                if (obj is SpriteObject)
                {
                    // Yes... Cast it to a SelectableSpriteObject
                    spriteObj = (SpriteObject)obj;
                    // Is the point in the object?
                    if (spriteObj.IsPointInObject(testPosition))
                    {
                        // Add to the array
                        hits[hitCount] = spriteObj;
                        hitCount += 1;
                    }
                }
            }

            // Trim the empty space from the end of the array
            Array.Resize(ref hits, hitCount);

            return hits;
        }

        /// <summary>
        /// Scan the sprites to find those that contain the specified position.
        /// Returns an array of all matching sprites.
        /// </summary>
        /// <param name="testPosition"></param>
        public SpriteObject GetSpriteAtPoint(Vector2 testPosition)
        {
            SpriteObject spriteObj;
            SpriteObject ret = null;
            float lowestLayerDepth = float.MaxValue;

            // Loop for all of the SelectableSpriteObjects
            foreach (GameObjectBase obj in GameObjects)
            {
                // Is this a SpriteObject?
                if (obj is SpriteObject)
                {
                    // Yes... Cast it to a SelectableSpriteObject
                    spriteObj = (SpriteObject)obj;
                    // Is its layerdepth the same or lower than the lowest we have seen so far?
                    // If not, previously encountered objects are in front of this one
                    // and so we have no need to check it.
                    if (spriteObj.LayerDepth <= lowestLayerDepth)
                    {
                        // Is the point in the object?
                        if (spriteObj.IsPointInObject(testPosition))
                        {
                            // Mark this as the current frontmost object
                            // and remember its layerdepth for future checks
                            ret = spriteObj;
                            lowestLayerDepth = spriteObj.LayerDepth;
                        }
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Store all of the graphics device state values that are modified by
        /// the sprite batch. These can be restored by later calling the
        /// RestoreStateAfterSprites function.
        /// </summary>
        protected void StoreStateBeforeSprites()
        {
            _preSpriteBlendState = GraphicsDevice.BlendState;
            _preSpriteDepthStencilState = GraphicsDevice.DepthStencilState;
            _preSpriteRasterizerState = GraphicsDevice.RasterizerState;
            _preSpriteSamplerState = GraphicsDevice.SamplerStates[0];
        }

        /// <summary>
        /// Restore all of the graphics device state values that are modified by
        /// the sprite batch to their previous values, as saved by an earlier call to
        /// StoreStateBeforeSprites function.
        /// </summary>
        protected void RestoreStateAfterSprites()
        {
            GraphicsDevice.BlendState = _preSpriteBlendState;
            GraphicsDevice.DepthStencilState = _preSpriteDepthStencilState;
            GraphicsDevice.RasterizerState = _preSpriteRasterizerState;
            GraphicsDevice.SamplerStates[0] = _preSpriteSamplerState;
        }


        //-------------------------------------------------------------------------------------
        // Text functions

        /// <summary>
        /// Call the Draw method on all TextObject-based objects in the game host
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public virtual void DrawText(GameTime gameTime, SpriteBatch spriteBatch)
        {
            GameObjectBase obj;
            int objectCount;

            // Draw each sprite object
            objectCount = _objectArray.Length;
            for (int i = 0; i < objectCount; i++)
            {
                obj = _objectArray[i];
                // Is this a text object?
                if (obj is TextObject)
                {
                    // Yes, so call its Draw method
                    ((TextObject)obj).Draw(gameTime, spriteBatch);
                }
            }
        }

        //-------------------------------------------------------------------------------------
        // Matrix-based object functions

        /// <summary>
        /// Call the Draw method on all matrix objects in the game
        /// </summary>
        public virtual void DrawObjects(GameTime gameTime, Effect effect)
        {
            DrawMatrixObjects(gameTime, effect, false, false, null);
        }

        /// <summary>
        /// Call the Draw method on all matrix objects in the game that use
        /// the specified texture. Pass as null to draw only objects that do
        /// not have a texture specified at all.
        /// </summary>
        public virtual void DrawObjects(GameTime gameTime, Effect effect, Texture2D restrictToTexture)
        {
            DrawMatrixObjects(gameTime, effect, true, false, restrictToTexture);
        }

        /// <summary>
        /// Call the Draw method on all matrix particle objects in the game
        /// </summary>
        public virtual void DrawParticles(GameTime gameTime, Effect effect)
        {
            DrawMatrixObjects(gameTime, effect, false, true, null);
        }

        /// <summary>
        /// Call the Draw method on all matrix particle objects in the game that use
        /// the specified texture. Pass as null to draw only objects that do
        /// not have a texture specified at all.
        /// </summary>
        public virtual void DrawParticles(GameTime gameTime, Effect effect, Texture2D restrictToTexture)
        {
            DrawMatrixObjects(gameTime, effect, true, true, restrictToTexture);
        }

        /// <summary>
        /// Draw the specified objects
        /// </summary>
        private void DrawMatrixObjects(GameTime gameTime, Effect effect, bool specifiedTextureOnly, bool renderParticles, Texture2D restrictToTexture)
        {
            GameObjectBase obj;
            int objectCount;

            // If we have a camera, draw it first
            if (Camera != null) Camera.Draw(gameTime, effect);

            // Now draw the sky box if there is one
            if (Skybox != null && Skybox._renderedThisFrame == false) Skybox.Draw(gameTime, effect);

            // Draw each matrix-based object
            objectCount = _objectArray.Length;
            for (int i = 0; i < objectCount; i++)
            {
                obj = _objectArray[i];
                // Is this a matrix object?
                if (obj is MatrixObjectBase)
                {
                    // Check whether this is a particle, and whether we are supposed to render it
                    if ((renderParticles && obj is MatrixParticleObjectBase) || (!renderParticles && !(obj is MatrixParticleObjectBase)))
                    {
                        // Does this object use the required texture?
                        if (specifiedTextureOnly == false || ((MatrixObjectBase)obj).ObjectTexture == restrictToTexture)
                        {
                            ((MatrixObjectBase)obj).Draw(gameTime, effect);
                        }
                    }
                }
            }
        }

    }
}
