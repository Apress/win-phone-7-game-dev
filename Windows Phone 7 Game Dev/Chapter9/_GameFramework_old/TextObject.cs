using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

namespace GameFramework
{
    [System.Runtime.Serialization.DataContract]
    public class TextObject : SpriteObject
    {

        //-------------------------------------------------------------------------------------
        // Class variables

        // The text to display
        private string _text;
        // Text alignment
        private TextAlignment _horizontalAlignment = TextAlignment.Manual;
        private TextAlignment _verticalAlignment = TextAlignment.Manual;

        //-------------------------------------------------------------------------------------
        // Enumerations

        /// <summary>
        /// Controls the alignment of text.
        /// </summary>
        public enum TextAlignment
        {
            /// <summary>
            /// Manual perform alignment using the Origin property.
            /// </summary>
            Manual,
            /// <summary>
            /// Align to the Top or Left
            /// </summary>
            Near,
            /// <summary>
            /// Align to the Center
            /// </summary>
            Center,
            /// <summary>
            /// Align to the Bottom or Right
            /// </summary>
            Far
        };

        //-------------------------------------------------------------------------------------
        // Class constructors

        public TextObject()
            : base()
        {
        }

        public TextObject(GameHost game)
            : base(game)
        {
            ScaleX = 1;
            ScaleY = 1;
            SpriteColor = Color.White;
        }

        public TextObject(GameHost game, SpriteFont font)
            : this(game)
        {
            Font = font;
        }

        public TextObject(GameHost game, SpriteFont font, Vector2 position)
            : this(game, font)
        {
            PositionX = position.X;
            PositionY = position.Y;
        }

        public TextObject(GameHost game, SpriteFont font, Vector2 position, String text)
            : this(game, font, position)
        {
            Text = text;
        }

        public TextObject(GameHost game, SpriteFont font, Vector2 position, String text, TextAlignment horizontalAlignment, TextAlignment verticalAlignment)
            : this(game, font, position, text)
        {
            HorizontalAlignment = horizontalAlignment;
            VerticalAlignment = verticalAlignment;
        }

        //-------------------------------------------------------------------------------------
        // Properties

        /// <summary>
        /// The underlying texture being used by this sprite
        /// </summary>
        private SpriteFont _font;
        /// <summary>
        /// A reference to the default texture used by this sprite
        /// </summary>
        public SpriteFont Font
        {
            get
            {
                // Do we have a texture name but no actual texture?
                // This will be the case after recovering from being tombstoned.
                if (_font == null && !String.IsNullOrEmpty(_fontName) && Game != null)
                {
                    // Get the texture back from the Textures collection
                    _font = Game.Fonts[_fontName];
                }
                // Return the sprite texture
                return _font;
            }
            set
            {
                // Has the font changed from whatever we already have stored?
                if (_font != value)
                {
                    // Yes, so store the new font and recalculate the origin if needed
                    _font = value;
                    CalculateAlignmentOrigin();
                    // Set the font name
                    _fontName = Game.GetContentObjectName<SpriteFont>(Game.Fonts, value);
                }
            }
        }

        /// <summary>
        /// The name of the texture being used by this sprite
        /// </summary>
        private string _fontName;
        /// <summary>
        /// Sets or returns the sprite texture name. This can be serialized when the game is tombstoned.
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public virtual string FontName
        {
            get
            {
                // Return the texture name
                return _fontName;
            }
            set
            {
                // Is this font name different to the one we are already using?
                if (_fontName != value)
                {
                    // Store the name
                    _fontName = value;
                    // Clear the stored font so that the name is processed the name
                    // time the Font property is called
                    _font = null;
                }
            }
        }

        /// <summary>
        /// The text to display
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public String Text
        {
            get { return _text; }
            set
            {
                // Has the text changed from whatever we already have stored?
                if (_text != value)
                {
                    // Yes, so store the new text and recalculate the origin if needed
                    _text = value;
                    CalculateAlignmentOrigin();
                }
            }
        }

        /// <summary>
        /// Allows the horizontal alignment of the text to be automatically calculated
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public TextAlignment HorizontalAlignment
        {
            get { return _horizontalAlignment; }
            set
            {
                if (_horizontalAlignment != value)
                {
                    _horizontalAlignment = value;
                    CalculateAlignmentOrigin();
                }
            }
        }
        /// <summary>
        /// Allows the vertical alignment of the text to be automatically calculated
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public TextAlignment VerticalAlignment
        {
            get { return _verticalAlignment; }
            set
            {
                if (_verticalAlignment != value)
                {
                    _verticalAlignment = value;
                    CalculateAlignmentOrigin();
                }
            }
        }


      
        //-------------------------------------------------------------------------------------
        // Game functions

        /// <summary>
        /// Draw the text using its default settings
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Do we have a font? And some text? If not then there is nothing to draw...
            if (Font != null && Text != null && Text.Length > 0)
            {
                // Draw the text
                spriteBatch.DrawString(Font, Text, Position, SpriteColor, Angle, Origin, Scale, SpriteEffects.None, LayerDepth);
            }
        }

        /// <summary>
        /// Calculate a simple bounding box for the text
        /// </summary>
        /// <remarks>Note that this doesn't currently take rotation into account so that
        /// the box size remains constant when rotating.</remarks>
        public override Rectangle BoundingBox
        {
            get
            {
                Rectangle result;
                Vector2 size;

                // Measure the string
                size = Font.MeasureString(Text);

                // Build a rectangle whose position and size matches that of the sprite
                // (taking scaling into account for the size)
                result = new Rectangle((int)PositionX, (int)PositionY, (int)(size.X * ScaleX), (int)(size.Y * ScaleY));

                // Offset the sprite by the origin
                result.Offset((int)(-OriginX * ScaleX), (int)(-OriginY * ScaleY));

                // Return the finished rectangle
                return result;
            }
        }


        /// <summary>
        /// Set the alignment of the text (automatically sets the origin)
        /// </summary>
        private void CalculateAlignmentOrigin()
        {
            Vector2 size;

            // Is the alignment being performed manually?
            if (HorizontalAlignment == TextAlignment.Manual && VerticalAlignment == TextAlignment.Manual)
            {
                // Yes, so there's nothing to do
                return;
            }

            // Make sure we have a font and some text
            if (Font == null || Text == null || Text.Length == 0)
            {
                // Nothing to render
                return;
            }

            // Measure the string
            size = Font.MeasureString(Text);

            // Set the origin as appropriate
            switch (HorizontalAlignment)
            {
                case TextAlignment.Near: OriginX = 0; break;
                case TextAlignment.Center: OriginX = size.X / 2; break;
                case TextAlignment.Far: OriginX = size.X; break;
            }
            switch (VerticalAlignment)
            {
                case TextAlignment.Near: OriginY = 0; break;
                case TextAlignment.Center: OriginY = size.Y / 2; break;
                case TextAlignment.Far: OriginY = size.Y; break;
            }
        }



    }
}
