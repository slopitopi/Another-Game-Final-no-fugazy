using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Another_Game_Final_no_fugazy
{
    internal class Instructions
    {
        private SpriteFont Font;
        private string Text;
        private Vector2 TextPosition;
        private Rectangle BackgroundRect;
        private Texture2D BackgroundTexture;
        private int Padding = 20; // Padding around the text

        /// <summary>
        /// Creates an instructions with centered text.
        /// </summary>
        public Instructions(SpriteFont font, string text, GraphicsDevice graphicsDevice, Vector2 textPosition)
        {
            this.Font = font;
            this.Text = text;
            this.TextPosition = textPosition;

            // Center the text on screen (1280x720)
            Vector2 textSize = Font.MeasureString(Text);


            // Create background rectangle with padding
            BackgroundRect = new Rectangle(
                (int)TextPosition.X - Padding,
                (int)TextPosition.Y - Padding,
                (int)textSize.X + Padding * 2,
                (int)textSize.Y + Padding * 2
                );
            

            // Create a 1x1 pixel texture for the background
            BackgroundTexture = new Texture2D(graphicsDevice, 1, 1);
            BackgroundTexture.SetData(new[] { Color.White });
        }



        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(BackgroundTexture, BackgroundRect, Color.Black * 0.6f);

            // Draw the text on top
            spriteBatch.DrawString(Font, Text, TextPosition, Color.White);
        }
    }
}