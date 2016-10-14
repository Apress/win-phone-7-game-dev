using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using GameFramework;
using System.Runtime.Serialization;

namespace VaporTrailTS
{

    [System.Runtime.Serialization.DataContract]
    public class GroundObject : MatrixObjectBase
    {

        //-------------------------------------------------------------------------------------
        // Class variables

        // Declare a static array of vertices.
        private static VertexPositionNormalTexture[] _vertices;

        //-------------------------------------------------------------------------------------
        // Class constructors

        public GroundObject()
            : base()
        {
        }

        public GroundObject(VaporTrailTSGame game, Texture2D texture)
            : base(game)
        {
            // Set other object properties
            ObjectTexture = texture;
        }

        /// <summary>
        /// A dummy property to force serialization of this class
        /// </summary>
        [DataMember]
        public bool _SerializeDummy { get; set; }


        //-------------------------------------------------------------------------------------
        // Object Functions

        /// <summary>
        /// Retrieve the vertices array
        /// </summary>
        private VertexPositionNormalTexture[] Vertices
        {
            get
            {
                // Have we already built the ground vertex array in a previous instance?
                if (_vertices == null)
                {
                    // No, so build it now
                    BuildVertices();
                }
                return _vertices;
            }
        }

        /// <summary>
        /// Update the ground position and calculate its transformation matrix
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Calculate the transformation matrix
            SetIdentity();
            // Now apply the standard transformations
            ApplyStandardTransformations();
        }

        /// <summary>
        /// Draw the ground
        /// </summary>
        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime, Effect effect)
        {
            // Prepare the effect for drawing
            PrepareEffect(effect);

            // Draw the object
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                // Apply the pass
                pass.Apply();
                // Draw the square
                effect.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vertices, 0, Vertices.Length / 3);
            }
        }


        /// <summary>
        /// Build the vertex array that stores the positions and colors of the ground vertices
        /// </summary>
        private void BuildVertices()
        {
            int i;
            Color thisColor = Color.Black;

            // Create and initialize the vertices
            _vertices = new VertexPositionNormalTexture[6];

            // Set the vertex positions for the ground
            i = 0;
            _vertices[i++].Position = new Vector3(-25.0f, 0.0f, -25.0f);
            _vertices[i++].Position = new Vector3(25.0f, 0.0f, -25.0f);
            _vertices[i++].Position = new Vector3(-25.0f, 0.0f, 25.0f);
            _vertices[i++].Position = new Vector3(25.0f, 0.0f, -25.0f);
            _vertices[i++].Position = new Vector3(25.0f, 0.0f, 25.0f);
            _vertices[i++].Position = new Vector3(-25.0f, 0.0f, 25.0f);
            // Set the texture coordinates for the ground
            i = 0;
            _vertices[i++].TextureCoordinate = new Vector2(0.0f, 0.0f);
            _vertices[i++].TextureCoordinate = new Vector2(8.0f, 0.0f);
            _vertices[i++].TextureCoordinate = new Vector2(0.0f, 8.0f);
            _vertices[i++].TextureCoordinate = new Vector2(8.0f, 0.0f);
            _vertices[i++].TextureCoordinate = new Vector2(8.0f, 8.0f);
            _vertices[i++].TextureCoordinate = new Vector2(0.0f, 8.0f);
            // Set the normals
            for (i = 0; i < _vertices.Length; i++)
            {
                _vertices[i].Normal = new Vector3(0, 1, 0);
            }
        }

    }

}
