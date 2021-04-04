using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.util
{
    public static class SpriteBatchExtension
    {
        /// <summary>
        /// The texture used when drawing rectangles, lines and other 
        /// primitives. This is a 1x1 white texture created at runtime.
        /// </summary>
        public static Texture2D WhiteTexture { get; set; }

        public static void InitSpriteBatchExtension(GraphicsDevice graphicsDevice)
        {
            SpriteBatchExtension.WhiteTexture = new Texture2D(graphicsDevice, 1, 1);
            SpriteBatchExtension.WhiteTexture.SetData(new Color[] { Color.White });
        }

        /// <summary>
        /// Draw a line between the two supplied points.
        /// </summary>
        /// <param name="start">Starting point.</param>
        /// <param name="end">End point.</param>
        /// <param name="color">The draw color.</param>
        public static void DrawLine(this SpriteBatch sb, Vector2 start, Vector2 end, Color color)
        {
            float length = (end - start).Length();
            float rotation = (float)Math.Atan2(end.Y - start.Y, end.X - start.X);
            sb.Draw(SpriteBatchExtension.WhiteTexture, start, null, color, rotation, Vector2.Zero, new Vector2(length, 1), SpriteEffects.None, 0);
        }

        /// <summary>
        /// Draw a rectangle.
        /// </summary>
        /// <param name="rectangle">The rectangle to draw.</param>
        /// <param name="color">The draw color.</param>
        public static void DrawRectangle(this SpriteBatch sb, Rectangle rectangle, Color color)
        {
            sb.Draw(SpriteBatchExtension.WhiteTexture, new Rectangle(rectangle.Left, rectangle.Top, rectangle.Width, 1), color);
            sb.Draw(SpriteBatchExtension.WhiteTexture, new Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width, 1), color);
            sb.Draw(SpriteBatchExtension.WhiteTexture, new Rectangle(rectangle.Left, rectangle.Top, 1, rectangle.Height), color);
            sb.Draw(SpriteBatchExtension.WhiteTexture, new Rectangle(rectangle.Right, rectangle.Top, 1, rectangle.Height + 1), color);
        }

        /// <summary>
        /// Fill a rectangle.
        /// </summary>
        /// <param name="rectangle">The rectangle to fill.</param>
        /// <param name="color">The fill color.</param>
        public static void FillRectangle(this SpriteBatch sb, Rectangle rectangle, Color color)
        {
            sb.Draw(SpriteBatchExtension.WhiteTexture, rectangle, color);
        }
    }
}
