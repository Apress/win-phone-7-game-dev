using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Text;
using System.Runtime.Serialization;

namespace GameFramework
{
    [DataContract]
    public abstract class GameObjectBase
    {

        //-------------------------------------------------------------------------------------
        // Class constructors

        public GameObjectBase()
        {
        }

        /// <summary>
        /// Constructor for the object
        /// </summary>
        /// <param name="game">A reference to the XNA Game class inside which the object resides</param>
        public GameObjectBase(GameHost game)
            : this()
        {
            // Store a reference to the game
            Game = game;
        }

        //-------------------------------------------------------------------------------------
        // Property access

        /// <summary>
        /// A reference back to the game that owns the object
        /// </summary>
        public GameHost Game { get; set; }

        /// <summary>
        /// The number of calls that have been made to the Update method
        /// </summary>
        [DataMember]
        public int UpdateCount { get; set; }

        /// <summary>
        /// A string that can be used to tag the object for later identification
        /// </summary>
        [DataMember]
        public string Tag { get; set; }

        /// <summary>
        /// Track whether this object should be written to the phone state if the game is tombstoned.
        /// For classes whose objects should not, this can be overridden and modified to return false instead.
        /// </summary>
        public virtual bool WriteToPhoneState
        {
            get { return true; }
        }

        //-------------------------------------------------------------------------------------
        // Game functions


        /// <summary>
        /// Update the object state
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
            // Increment the UpdateCount
            UpdateCount += 1;
        }

        /// <summary>
        /// Determine whether the specified position is contained within the object
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public abstract bool IsPointInObject(Vector2 point);

    }
}
