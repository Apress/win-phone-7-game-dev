using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

namespace GameFramework
{
    [System.Runtime.Serialization.DataContract]
    public abstract class MatrixParticleObjectBase : MatrixObjectBase
    {

        //-------------------------------------------------------------------------------------
        // Class constructors

        public MatrixParticleObjectBase()
            : base()
        {
        }

        public MatrixParticleObjectBase(GameHost game)
            : base(game)
        {
            // Default to active
            IsActive = true;
        }

        public MatrixParticleObjectBase(GameHost game, Texture2D texture, Vector3 position, Vector3 scale)
            : this(game)
        {
            ObjectTexture = texture;
            Position = position;
            Scale = scale;
        }

        //-------------------------------------------------------------------------------------
        // Properties

        /// <summary>
        /// Is this object active or dormant?
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public bool IsActive { get; set; }

        /// <summary>
        /// By default, exclude particles from being stored if the game is tombstoned
        /// </summary>
        public override bool WriteToPhoneState
        {
            get { return false; }
        }

    }
}
