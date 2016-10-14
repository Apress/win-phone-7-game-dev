using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SLGameFramework
{
    public partial class Sprite : UserControl
    {

        //-------------------------------------------------------------------------------------
        // Class variables

        // Track whether a translation, scaling or rotation storyboard is currently active
        private bool _isTranslating = false;
        private bool _isScaling = false;
        private bool _isRotating = false;

        //-------------------------------------------------------------------------------------
        // Class events

        public event EventHandler TranslateCompleted;
        public event EventHandler ScaleCompleted;
        public event EventHandler RotateCompleted;

        //-------------------------------------------------------------------------------------
        // Constructors

        public Sprite()
        {
            InitializeComponent();

            // Set default property values
            ScaleX = 1;
            ScaleY = 1;

            // Use the bitmap cache by default
            UseBitmapCache = true;

            // Set event handlers
            SBTranslate.Completed += new EventHandler(SBTranslate_Completed);
            SBScale.Completed += new EventHandler(SBScale_Completed);
            SBRotate.Completed += new EventHandler(SBRotate_Completed);
        }


        //-------------------------------------------------------------------------------------
        // Properties

        // ---- General properties ---- 

        /// <summary>
        /// Sets or returns a value controlling whether a bitmap cache should be applied to this sprite
        /// </summary>
        public bool UseBitmapCache
        {
            get { return this.CacheMode != null; }
            set
            {
                // If set to true and no cache already in place...
                if (value == true && this.CacheMode == null)
                {
                    // ...create a cache now
                    this.CacheMode = new BitmapCache();
                }
                else if (value == false)
                {
                    // Clear the cache
                    this.CacheMode = null;
                }
            }
        }

        // ---- Image properties ---- 

        /// <summary>
        /// The height of the sprite's image. Set to 0 for auto
        /// </summary>
        public double ImageHeight
        {
            get { return (double.IsNaN(spriteImage.Height) ? 0 : spriteImage.Height); }
            set { spriteImage.Height = (value == 0 ? double.NaN : value); }
        }

        /// <summary>
        /// The width of the sprite's image. Set to 0 for auto
        /// </summary>
        public double ImageWidth
        {
            get { return (double.IsNaN(spriteImage.Width) ? 0 : spriteImage.Width); }
            set { spriteImage.Width = (value == 0 ? double.NaN : value); }
        }

        /// <summary>
        /// The horizontal offset across the sprite image
        /// </summary>
        public double ImageOffsetX
        {
            get { return -Canvas.GetLeft(spriteImage); }
            set { Canvas.SetLeft(spriteImage, -value); }
        }

        /// <summary>
        /// The vertical offset across the sprite image
        /// </summary>
        public double ImageOffsetY
        {
            get { return -Canvas.GetTop(spriteImage); }
            set { Canvas.SetTop(spriteImage, -value); }
        }

        /// <summary>
        /// The image to use to display the sprite
        /// </summary>
        public ImageSource Source
        {
            get { return spriteImage.Source; }
            set { spriteImage.Source = value; }
        }

        // ---- Position properties ---- 

        /// <summary>
        /// The Left position within the containing Canvas
        /// </summary>
        public double Left
        {
            get
            {
                // Make sure the parent actually is a Canvas. If so, return the position
                if (Parent is Canvas) return Canvas.GetLeft(this);
                // Not in a Canvas, so return 0
                return 0;
            }
            set
            {
                // Make sure the parent actually is a Canvas.
                if (Parent is Canvas) Canvas.SetLeft(this, value);
            }
        }

        /// <summary>
        /// The Top position within the containing Canvas
        /// </summary>
        public double Top
        {
            get
            {
                // Make sure the parent actually is a Canvas. If so, return the position
                if (Parent is Canvas) return Canvas.GetTop(this);
                // Not in a Canvas, so return 0
                return 0;
            }
            set
            {
                // Make sure the parent actually is a Canvas.
                if (Parent is Canvas) Canvas.SetTop(this, value);
            }
        }

        // ---- Transform properties ---- 

        /// <summary>
        /// The Transform horizontal center
        /// </summary>
        public double TransformCenterX
        {
            get { return SpriteTransform.CenterX; }
            set { SpriteTransform.CenterX = value; }
        }

        /// <summary>
        /// The Transform vertical center
        /// </summary>
        public double TransformCenterY
        {
            get { return SpriteTransform.CenterY; }
            set { SpriteTransform.CenterY = value; }
        }

        /// <summary>
        /// The Transform horizontal translation
        /// </summary>
        public double TranslationX
        {
            get { return SpriteTransform.TranslateX; }
            set { SpriteTransform.TranslateX = value; }
        }

        /// <summary>
        /// The Transform vertical translation
        /// </summary>
        public double TranslationY
        {
            get { return SpriteTransform.TranslateY; }
            set { SpriteTransform.TranslateY = value; }
        }

        /// <summary>
        /// The Transform horizontal scale
        /// </summary>
        public double ScaleX
        {
            get { return SpriteTransform.ScaleX; }
            set { SpriteTransform.ScaleX = value; }
        }

        /// <summary>
        /// The Transform vertical scale
        /// </summary>
        public double ScaleY
        {
            get { return SpriteTransform.ScaleY; }
            set { SpriteTransform.ScaleY = value; }
        }

        /// <summary>
        /// The Transform rotation angle
        /// </summary>
        public double Rotation
        {
            get { return SpriteTransform.Rotation; }
            set { SpriteTransform.Rotation = value; }
        }

        // ---- Storyboard properties ---- 

        /// <summary>
        /// Indicates whether a translation storyboard is currently active
        /// </summary>
        public bool IsTranslating
        {
            get { return _isTranslating; }
        }

        /// <summary>
        /// Indicates whether a scaling storyboard is currently active
        /// </summary>
        public bool IsScaling
        {
            get { return _isScaling; }
        }

        /// <summary>
        /// Indicates whether a rotation storyboard is currently active
        /// </summary>
        public bool IsRotating
        {
            get { return _isRotating; }
        }

        //-------------------------------------------------------------------------------------
        // Event processing

        private void spriteCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Set the clipping rectangle to match the size of the Canvas
            canvasRect.Rect = new Rect(0, 0, e.NewSize.Width, e.NewSize.Height);
        }


        void SBTranslate_Completed(object sender, EventArgs e)
        {
            // Translation has finished so indicate that we are no longer translating
            _isTranslating = false;
            // Call the OnTranslateCompleted function to raise the TranslateCompleted event
            OnTranslateCompleted(sender, e);
        }
        protected virtual void OnTranslateCompleted(object sender, EventArgs e)
        {
            // Raise the TranslationCompleted event
            if (TranslateCompleted != null) TranslateCompleted(this, e);
        }


        void SBScale_Completed(object sender, EventArgs e)
        {
            // Scaling has finished so indicate that we are no longer scaling
            _isScaling = false;
            // Call the OnScaleCompleted function to raise the ScaleCompleted event
            OnScaleCompleted(sender, e);
        }
        protected virtual void OnScaleCompleted(object sender, EventArgs e)
        {
            // Raise the ScaleCompleted event
            if (ScaleCompleted != null) ScaleCompleted(this, e);
        }

        void SBRotate_Completed(object sender, EventArgs e)
        {
            // Rotation has finished so indicate that we are no longer rotating
            _isRotating = false;
            // Call the OnRotateCompleted function to raise the RotateCompleted event
            OnRotateCompleted(sender, e);
        }
        protected virtual void OnRotateCompleted(object sender, EventArgs e)
        {
            // Raise the RotationCompleted event
            if (RotateCompleted != null) RotateCompleted(this, e);
        }


        //-------------------------------------------------------------------------------------
        // Sprite methods

        /// <summary>
        /// Begin the Translate storyboard
        /// </summary>
        /// <param name="fromX">The starting X coordinate</param>
        /// <param name="fromY">The starting Y coordinate</param>
        /// <param name="toX">The destination X coordinate</param>
        /// <param name="toY">The destination X coordinate</param>
        /// <param name="duration">The storyboard duration, in seconds</param>
        public void BeginTranslate(double fromX, double fromY, double toX, double toY, double duration)
        {
            // Delegate to the next overload
            BeginTranslate(fromX, fromY, toX, toY, duration, 0);
        }
        /// <summary>
        /// Begin the Translate storyboard
        /// </summary>
        /// <param name="fromX">The starting X coordinate</param>
        /// <param name="fromY">The starting Y coordinate</param>
        /// <param name="toX">The destination X coordinate</param>
        /// <param name="toY">The destination X coordinate</param>
        /// <param name="duration">The storyboard duration, in seconds</param>
        /// <param name="startTime">The time through the duration at which the storyboard should begin, in seconds</param>
        public void BeginTranslate(double fromX, double fromY, double toX, double toY, double duration, double startTime)
        {
            // Delegate to the next overload
            BeginTranslate(fromX, fromY, toX, toY, duration, startTime, null);
        }
        /// <summary>
        /// Begin the Translate storyboard
        /// </summary>
        /// <param name="fromX">The starting X coordinate</param>
        /// <param name="fromY">The starting Y coordinate</param>
        /// <param name="toX">The destination X coordinate</param>
        /// <param name="toY">The destination X coordinate</param>
        /// <param name="duration">The storyboard duration, in seconds</param>
        /// <param name="startTime">The time through the duration at which the storyboard should begin, in seconds</param>
        /// <param name="easingFunction">An easing function to apply to the translation</param>
        public void BeginTranslate(double fromX, double fromY, double toX, double toY, double duration, double startTime, EasingFunctionBase easingFunction)
        {
            // Delegate to the next overload
            BeginTranslate(fromX, fromY, toX, toY, duration, startTime, easingFunction, new RepeatBehavior(1), false);
        }
        /// <summary>
        /// Begin the Translate storyboard
        /// </summary>
        /// <param name="fromX">The starting X coordinate</param>
        /// <param name="fromY">The starting Y coordinate</param>
        /// <param name="toX">The destination X coordinate</param>
        /// <param name="toY">The destination X coordinate</param>
        /// <param name="duration">The storyboard duration, in seconds</param>
        /// <param name="startTime">The time through the duration at which the storyboard should begin, in seconds</param>
        /// <param name="easingFunction">An easing function to apply to the translation</param>
        /// <param name="repeatBehavior">The repeat behavior for the storyboard</param>
        /// <param name="autoReverse">Whether the storyboard should auto-reverse when it reaches its end</param>
        public void BeginTranslate(double fromX, double fromY, double toX, double toY, double duration, double startTime, EasingFunctionBase easingFunction, RepeatBehavior repeatBehavior, bool autoReverse)
        {
            // Stop the storyboard
            StopTranslate();

            // Configure the x axis translation
            SBTranslateAnimX.From = fromX;
            SBTranslateAnimX.To = toX;
            SBTranslateAnimX.EasingFunction = easingFunction;
            SBTranslateAnimX.Duration = new Duration(new TimeSpan(0, 0, 0, 0, (int)(duration * 1000)));
            // Configure the y axis translation
            SBTranslateAnimY.From = fromY;
            SBTranslateAnimY.To = toY;
            SBTranslateAnimY.EasingFunction = easingFunction;
            SBTranslateAnimY.Duration = new Duration(new TimeSpan(0, 0, 0, 0, (int)(duration * 1000)));

            // Ensure the repeat behavior is valid
            if (repeatBehavior.HasCount && repeatBehavior.Count == 0) repeatBehavior = new RepeatBehavior(1);
            // Set the repeat and reverse properties
            SBTranslate.RepeatBehavior = repeatBehavior;
            SBTranslate.AutoReverse = autoReverse;

            // Begin the storyboard
            _isTranslating = true;
            SBTranslate.Begin();
            // Seek to the specified start position
            SBTranslate.Seek(new TimeSpan(0, 0, 0, 0, (int)(startTime * 1000)));
        }
        /// <summary>
        /// Stop the Translate storyboard
        /// </summary>
        public void StopTranslate()
        {
            SBTranslate.Stop();
            _isTranslating = false;
        }


        /// <summary>
        /// Begin the Scale storyboard
        /// </summary>
        /// <param name="fromX">The starting X scale</param>
        /// <param name="fromY">The starting Y scale</param>
        /// <param name="toX">The destination X scale</param>
        /// <param name="toY">The destination X scale</param>
        /// <param name="duration">The storyboard duration, in seconds</param>
        public void BeginScale(double fromX, double fromY, double toX, double toY, double duration)
        {
            // Delegate to the next overload
            BeginScale(fromX, fromY, toX, toY, duration, 0);
        }
        /// <summary>
        /// Begin the Scale storyboard
        /// </summary>
        /// <param name="fromX">The starting X scale</param>
        /// <param name="fromY">The starting Y scale</param>
        /// <param name="toX">The destination X scale</param>
        /// <param name="toY">The destination X scale</param>
        /// <param name="duration">The storyboard duration, in seconds</param>
        /// <param name="startTime">The time through the duration at which the storyboard should begin, in seconds</param>
        public void BeginScale(double fromX, double fromY, double toX, double toY, double duration, double startTime)
        {
            // Delegate to the next overload
            BeginScale(fromX, fromY, toX, toY, duration, startTime, null);
        }
        /// <summary>
        /// Begin the Scale storyboard
        /// </summary>
        /// <param name="fromX">The starting X scale</param>
        /// <param name="fromY">The starting Y scale</param>
        /// <param name="toX">The destination X scale</param>
        /// <param name="toY">The destination X scale</param>
        /// <param name="duration">The storyboard duration, in seconds</param>
        /// <param name="startTime">The time through the duration at which the storyboard should begin, in seconds</param>
        /// <param name="easingFunction">An easing function to apply to the scale</param>
        public void BeginScale(double fromX, double fromY, double toX, double toY, double duration, double startTime, EasingFunctionBase easingFunction)
        {
            // Delegate to the next overload
            BeginScale(fromX, fromY, toX, toY, duration, startTime, easingFunction, new RepeatBehavior(1), false);
        }
        /// <summary>
        /// Begin the Scale storyboard
        /// </summary>
        /// <param name="fromX">The starting X scale</param>
        /// <param name="fromY">The starting Y scale</param>
        /// <param name="toX">The destination X scale</param>
        /// <param name="toY">The destination X scale</param>
        /// <param name="duration">The storyboard duration, in seconds</param>
        /// <param name="startTime">The time through the duration at which the storyboard should begin, in seconds</param>
        /// <param name="easingFunction">An easing function to apply to the scale</param>
        /// <param name="repeatBehavior">The repeat behavior for the storyboard</param>
        /// <param name="autoReverse">Whether the storyboard should auto-reverse when it reaches its end</param>
        public void BeginScale(double fromX, double fromY, double toX, double toY, double duration, double startTime, EasingFunctionBase easingFunction, RepeatBehavior repeatBehavior, bool autoReverse)
        {
            // Stop the storyboard
            StopScale();

            // Configure the x axis scaling
            SBScaleAnimX.From = fromX;
            SBScaleAnimX.To = toX;
            SBScaleAnimX.EasingFunction = easingFunction;
            SBScaleAnimX.Duration = new Duration(new TimeSpan(0, 0, 0, 0, (int)(duration * 1000)));
            // Configure the y axis scaling
            SBScaleAnimY.From = fromY;
            SBScaleAnimY.To = toY;
            SBScaleAnimY.EasingFunction = easingFunction;
            SBScaleAnimY.Duration = new Duration(new TimeSpan(0, 0, 0, 0, (int)(duration * 1000)));

            // Ensure the repeat behavior is valid
            if (repeatBehavior.HasCount && repeatBehavior.Count == 0) repeatBehavior = new RepeatBehavior(1);
            // Set the repeat and reverse properties
            SBScale.RepeatBehavior = repeatBehavior;
            SBScale.AutoReverse = autoReverse;
            
            // Begin the storyboard
            _isScaling = true;
            SBScale.Begin();
            // Seek to the specified start position
            SBScale.Seek(new TimeSpan(0, 0, 0, 0, (int)(startTime * 1000)));
        }
        /// <summary>
        /// Stop the Scale storyboard
        /// </summary>
        public void StopScale()
        {
            SBScale.Stop();
            _isScaling = false;
        }

        /// <summary>
        /// Begin the Rotate storyboard
        /// </summary>
        /// <param name="fromAngle">The starting angle</param>
        /// <param name="toAngle">The destination angle</param>
        /// <param name="duration">The storyboard duration, in seconds</param>
        public void BeginRotate(double fromAngle, double toAngle, double duration)
        {
            // Delegate to the next overload
            BeginRotate(fromAngle, toAngle, duration, 0);
        }
        /// <summary>
        /// Begin the Rotate storyboard
        /// </summary>
        /// <param name="fromAngle">The starting angle</param>
        /// <param name="toAngle">The destination angle</param>
        /// <param name="duration">The storyboard duration, in seconds</param>
        /// <param name="startTime">The time through the duration at which the storyboard should begin, in seconds</param>
        public void BeginRotate(double fromAngle, double toAngle, double duration, double startTime)
        {
            BeginRotate(fromAngle, toAngle, duration, startTime, null);
        }
        /// <summary>
        /// Begin the Rotate storyboard
        /// </summary>
        /// <param name="fromAngle">The starting angle</param>
        /// <param name="toAngle">The destination angle</param>
        /// <param name="duration">The storyboard duration, in seconds</param>
        /// <param name="startTime">The time through the duration at which the storyboard should begin, in seconds</param>
        /// <param name="easingFunction">An easing function to apply to the Rotate</param>
        public void BeginRotate(double fromAngle, double toAngle, double duration, double startTime, EasingFunctionBase easingFunction)
        {
            BeginRotate(fromAngle, toAngle, duration, startTime, easingFunction, new RepeatBehavior(1), false);
        }
        /// <summary>
        /// Begin the Rotate storyboard
        /// </summary>
        /// <param name="fromAngle">The starting angle</param>
        /// <param name="toAngle">The destination angle</param>
        /// <param name="duration">The storyboard duration, in seconds</param>
        /// <param name="startTime">The time through the duration at which the storyboard should begin, in seconds</param>
        /// <param name="easingFunction">An easing function to apply to the Rotate</param>
        /// <param name="repeatBehavior">The repeat behavior for the storyboard</param>
        /// <param name="autoReverse">Whether the storyboard should auto-reverse when it reaches its end</param>
        public void BeginRotate(double fromAngle, double toAngle, double duration, double startTime, EasingFunctionBase easingFunction, RepeatBehavior repeatBehavior, bool autoReverse)
        {
            // Stop the storyboard
            StopRotate();

            // Configure the rotation
            SBRotateAnim.From = fromAngle;
            SBRotateAnim.To = toAngle;
            SBRotateAnim.EasingFunction = easingFunction;
            SBRotateAnim.Duration = new Duration(new TimeSpan(0, 0, 0, 0, (int)(duration * 1000)));

            // Ensure the repeat behavior is valid
            if (repeatBehavior.HasCount && repeatBehavior.Count == 0) repeatBehavior = new RepeatBehavior(1);
            // Set the repeat and reverse properties
            SBRotate.RepeatBehavior = repeatBehavior;
            SBRotate.AutoReverse = autoReverse;

            // Begin the storyboard
            _isRotating = true;
            SBRotate.Begin();
            // Seek to the specified start position
            SBRotate.Seek(new TimeSpan(0, 0, 0, 0, (int)(startTime * 1000)));
        }
        /// <summary>
        /// Stop the Rotate storyboard
        /// </summary>
        public void StopRotate()
        {
            SBRotate.Stop();
            _isRotating = false;
        }


    }
}
