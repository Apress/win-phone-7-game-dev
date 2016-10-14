using GameFramework;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

namespace Tombstoning
{
    [System.Runtime.Serialization.DataContract]
    public class BallObject : SpriteObject
    {

        //-------------------------------------------------------------------------------------
        // Class constructors

        public BallObject()
            : base()
        {
        }

        internal BallObject(TombstoningGame game, Texture2D texture, float speed)
            : base(game, Vector2.Zero, texture)
        {
            // Set a random position
            PositionX = GameHelper.RandomNext(0, Game.Window.ClientBounds.Width);
            PositionY = GameHelper.RandomNext(0, Game.Window.ClientBounds.Height);

            // Set the origin
            Origin = new Vector2(texture.Width, texture.Height) / 2;

            // Set a random color
            SpriteColor = new Color(GameHelper.RandomNext(0, 256), GameHelper.RandomNext(0, 256), GameHelper.RandomNext(0, 256));

            // Store the movement speed
            Speed = speed;

            // Set a horizontal movement speed for the box
            XAdd = GameHelper.RandomNext(-5.0f, 5.0f);

            Wobble = 2.34f;
            PositionX = 99;
        }

        //-------------------------------------------------------------------------------------
        // Properties

        // The ball's movement speed
        [System.Runtime.Serialization.DataMember]
        public float Speed { get; set; }

        // The ball's current x and y velocity
        [System.Runtime.Serialization.DataMember]
        public float XAdd { get; set; }
        [System.Runtime.Serialization.DataMember]
        public float YAdd { get; set; }

        // The level of "wobble" applied to the ball
        [System.Runtime.Serialization.DataMember]
        public float Wobble { get; set; }

        //-------------------------------------------------------------------------------------
        // Game functions

        public override void Update(GameTime gameTime)
        {
            // Allow the base class to do any work it needs
            base.Update(gameTime);

            // Update the position of the ball
            PositionX += XAdd * Speed;
            PositionY += YAdd * Speed;

            // If we reach the side of the window, reverse the x velocity so that the ball bounces back
            if (PositionX < OriginX)
            {
                // Reset back to the left edge
                PositionX = OriginX;
                // Reverse the x velocity
                XAdd = -XAdd;
                // Add to the wobble
                Wobble += Math.Abs(XAdd);
            }
            if (PositionX > Game.Window.ClientBounds.Width - OriginX)
            {
                // Reset back to the right edge
                PositionX = Game.Window.ClientBounds.Width - OriginX;
                // Reverse the x velocity
                XAdd = -XAdd;
                // Add to the wobble
                Wobble += Math.Abs(XAdd);
            }

            // If we reach the bottom of the window, reverse the y velocity so that the ball bounces upwards
            if (PositionY >= Game.Window.ClientBounds.Bottom - OriginY)
            {
                // Reset back to the bottom of the window
                PositionY = Game.Window.ClientBounds.Bottom - OriginY;
                // Reverse the y-velocity
                YAdd = -YAdd; // +0.3f;
                // Add to the wobble
                Wobble += Math.Abs(YAdd);
            }
            else
            {
                // Increase the y velocity to simulate gravity
                YAdd += 0.3f;
            }

            // Is there any wobble?
            if (Wobble == 0)
            {
                // No, so reset the scale
                Scale = Vector2.One;
            }
            else
            {
                const float WobbleSpeed = 20.0f;
                const float WobbleIntensity = 0.015f;

                // Yes, so calculate the scaling on the x and y axes
                ScaleX = (float)Math.Sin(MathHelper.ToRadians(UpdateCount * WobbleSpeed)) * Wobble * WobbleIntensity + 1;
                ScaleY = (float)Math.Sin(MathHelper.ToRadians(UpdateCount * WobbleSpeed + 180.0f)) * Wobble * WobbleIntensity + 1;
                // Reduce the wobble level
                Wobble -= 0.2f;
                // Don't allow the wobble to fall below zero or to rise too high
                if (Wobble < 0) Wobble = 0;
                if (Wobble > 50) Wobble = 50;
            }
        }

        public override bool IsPointInObject(Vector2 point)
        {
            throw new NotImplementedException();
        }

    }
}
