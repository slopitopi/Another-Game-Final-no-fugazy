using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Another_Game_Final_no_fugazy
{
    /// <summary>
    /// A UI component that displays text on a semi-transparent black background. Used throughout the game to render
    /// instructional text, effect boxes for entities (showing debuff status and stats), wave info, and card selection prompts.
    /// 
    /// The Instructions class measures the text dimensions using the provided SpriteFont, creates a padded background
    /// rectangle around the text, and draws both the background and text to the screen. The text can be dynamically
    /// updated at runtime via SetText(), which recalculates the background size.
    /// 
    /// Also serves as the base class for HealthBar, which overrides Draw to add health-bar-specific rendering.
    /// 
    /// Instances are created in GameElements.LoadContentGE (for instructions text, card instructions, wave info)
    /// and in SpawnEnemys/LoadContentGE (for entity effect boxes).
    /// </summary>
    internal class Instructions
    {
        protected SpriteFont Font; // The font used to measure and render the text. Loaded from content in ObjectPosTex and passed in during construction.
        protected string Text; // The current text string to display. Can be updated at runtime via SetText().
        protected Vector2 TextPosition; // The screen position where the text is drawn (top-left corner of the text, not the background).
        protected Rectangle BackgroundRect; // The padded rectangle drawn behind the text as a semi-transparent background.
        protected Texture2D BackgroundTexture; // A 1x1 white pixel texture used for drawing colored rectangles (background and health bar fills). Created once during construction.
        private int PaddingX = 20; // Horizontal padding (in pixels) added on each side of the text within the background rectangle.
        private int PaddingY = 10; // Vertical padding (in pixels) added on top and bottom of the text within the background rectangle.


        /// <summary>
        /// Creates a new Instructions instance with the given font, text, and screen position.
        /// Measures the text to calculate the background rectangle size with padding, and creates
        /// a 1x1 white pixel texture for drawing the background.
        /// </summary>
        /// <param name="font">The SpriteFont used to measure and render the text.</param>
        /// <param name="text">The initial text string to display.</param>
        /// <param name="graphicsDevice">The GraphicsDevice used to create the 1x1 pixel background texture.</param>
        /// <param name="textPosition">The screen position for the top-left corner of the text.</param>
        public Instructions(SpriteFont font, string text, GraphicsDevice graphicsDevice, Vector2 textPosition)
        {
            this.Font = font;
            this.Text = text;
            this.TextPosition = textPosition;

            // Measure the text dimensions to calculate the background rectangle size
            Vector2 textSize = Font.MeasureString(Text);


            // Create background rectangle with padding around the text on all sides
            BackgroundRect = new Rectangle(
                (int)TextPosition.X - PaddingX,
                (int)TextPosition.Y - PaddingY,
                (int)textSize.X + PaddingX * 2,
                (int)textSize.Y + PaddingY * 2
                );


            // Create a 1x1 pixel texture filled with white, which can be tinted to any color when drawn.
            // This is a common MonoGame technique for drawing simple colored rectangles without loading a texture file.
            BackgroundTexture = new Texture2D(graphicsDevice, 1, 1);
            BackgroundTexture.SetData(new[] { Color.White });
        }





        /// <summary>
        /// Updates the displayed text and recalculates the background rectangle to fit the new text.
        /// Called frequently by entity effect boxes (UpdateEffectBoxText), HealthBar (UpdateHealth),
        /// and the wave info display to keep the UI in sync with changing game state.
        /// </summary>
        /// <param name="newText">The new text string to display.</param>
        public void SetText(string newText)
        {
            Text = newText;
            // Re-measure the text dimensions since the new text may be a different length
            Vector2 textSize = Font.MeasureString(Text);


            // Recalculate the background rectangle with padding to fit the new text
            BackgroundRect = new Rectangle(
                (int)TextPosition.X - PaddingX,
                (int)TextPosition.Y - PaddingY,
                (int)textSize.X + PaddingX * 2,
                (int)textSize.Y + PaddingY * 2
            );
        }


        /// <summary>
        /// Draws the semi-transparent black background rectangle and the white text on top of it.
        /// Called by GameElements draw methods for each active Instructions instance (instructions screen text,
        /// card selection prompt, wave info, entity effect boxes). The background uses 60% opacity (Color.Black * 0.6f)
        /// to allow the game background to partially show through.
        /// </summary>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(BackgroundTexture, BackgroundRect, Color.Black * 0.6f); // Draw the semi-transparent black background

            // Draw the text on top of the background in white
            spriteBatch.DrawString(Font, Text, TextPosition, Color.White);
        }
    }
}