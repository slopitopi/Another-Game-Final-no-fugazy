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
        private SpriteFont _font;
        private string _text;
        private Vector2 _textPosition;
        private Rectangle _backgroundRect;
        private Texture2D _backgroundTexture;
        private int _padding = 20; // Padding around the text

        /// <summary>
        /// Creates an instructions with centered text.
        /// </summary>
        public Instructions(SpriteFont font, string text, GraphicsDevice graphicsDevice)
        {
            _font = font;
            _text = text;

            // Center the text on screen (1280x720)
            Vector2 textSize = _font.MeasureString(_text);
            _textPosition = new Vector2(
                (1280 - textSize.X) / 2,
                (720 - textSize.Y) / 2 - 50
            );

            // Create background rectangle with padding
            _backgroundRect = new Rectangle(
                (int)_textPosition.X - _padding,
                (int)_textPosition.Y - _padding,
                (int)textSize.X + _padding * 2,
                (int)textSize.Y + _padding * 2
            );

            // Create a 1x1 pixel texture for the background
            _backgroundTexture = new Texture2D(graphicsDevice, 1, 1);
            _backgroundTexture.SetData(new[] { Color.White });
        }



        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_backgroundTexture, _backgroundRect, Color.Black * 0.6f);

            // Draw the text on top
            spriteBatch.DrawString(_font, _text, _textPosition, Color.White);
        }
    }
}