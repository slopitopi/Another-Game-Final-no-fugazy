using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Another_Game_Final_no_fugazy
{
    /// <summary>
    /// A specialized Instructions subclass that visually represents an entity's health as a colored bar with text.
    /// Inherits from Instructions to reuse the text-on-background rendering, but overrides Draw to render a two-layer
    /// health bar: a black background rectangle and a green fill rectangle whose width is proportional to the entity's
    /// current HP percentage.
    /// 
    /// Each CombatEntity (player and enemies) has a HealthBar instance created and linked in GameElements
    /// (LoadContentGE for the player, SpawnEnemys for enemies). The HealthBar reads HP values from its linked entity
    /// and updates the displayed text via UpdateHealth().
    /// </summary>
    internal class HealthBar : Instructions
    {
        private CombatEntity entity; // Reference to the CombatEntity this health bar is linked to, used to read current HP and max HP for display and fill calculation.
        private Color fillColor = Color.Green; // Color of the health bar fill portion (the green bar that shrinks as HP decreases).
        private Color backColor = Color.Black; // Color of the health bar background (the black bar behind the green fill, always full width).

        /// <summary>
        /// Constructs a new HealthBar linked to the given entity, positioned at the specified location.
        /// Initializes the display text with the entity's current HP/MaxHP using the static helper method.
        /// The graphicsDevice is passed to the base Instructions constructor to create the 1x1 pixel background texture.
        /// </summary>
        public HealthBar(SpriteFont font, GraphicsDevice graphicsDevice, Vector2 position, CombatEntity entity) : base(font, GetHealthText(entity), graphicsDevice, position)
        {
            this.entity = entity; // Store the entity reference for ongoing HP reads
           
        }

        /// <summary>
        /// Static helper method that generates the initial health text string from an entity's HP values.
        /// Used only during construction to provide the base Instructions class with initial text.
        /// </summary>
        public static string GetHealthText(CombatEntity entity)
        {
            return $"{entity.EnemyHP}/{entity.EnemyMaxHP}";
        }




        /// <summary>
        /// Updates the health bar's display text to reflect the entity's current HP and max HP.
        /// Called by GameElements whenever the entity's health changes (after taking damage, healing, etc.)
        /// to keep the displayed values in sync.
        /// </summary>
        public void UpdateHealth()
        {
            Text = $"{entity.EnemyHP} / {entity.EnemyMaxHP}";
        }


        /// <summary>
        /// Draws the health bar to the screen. Overrides the base Instructions.Draw to render:
        /// 1. A fixed-width (80px) black background rectangle.
        /// 2. A green fill rectangle whose width is proportional to the entity's current HP percentage.
        /// 3. The HP text (e.g., "75 / 100") centered on top of the bar in white.
        /// 
        /// The text position is recalculated each frame to stay centered within the background rectangle.
        /// </summary>
        public override void Draw(SpriteBatch spriteBatch)
        {
            BackgroundRect.Width = 80; // Set a fixed width for the health bar background

            // Center the text within the health bar rectangle
            TextPosition = new Vector2(
                BackgroundRect.X + BackgroundRect.Width / 2 - Font.MeasureString(Text).X / 2,
                BackgroundRect.Y + BackgroundRect.Height / 2 - Font.MeasureString(Text).Y / 2
                );


            spriteBatch.Draw(BackgroundTexture, BackgroundRect, backColor); // Draw the black background bar at full width


            float healthProcent = (float)entity.EnemyHP / entity.EnemyMaxHP; // Calculate the HP percentage (0.0 to 1.0)

            int backW = (int)(BackgroundRect.Width * healthProcent); // Scale the fill width by the HP percentage
            backW = Math.Max(0, Math.Min(BackgroundRect.Width, backW)); // Clamp the fill width between 0 and the full bar width to prevent visual glitches

            
            Rectangle fillRectangle = new Rectangle(BackgroundRect.X, BackgroundRect.Y, backW, BackgroundRect.Height); // Create the green fill rectangle with the calculated width
            spriteBatch.Draw(BackgroundTexture, fillRectangle, fillColor); // Draw the green fill bar on top of the black background
            

            spriteBatch.DrawString(Font, Text, TextPosition, Color.White); // Draw the HP text (e.g., "75 / 100") centered on the bar in white
        }
    }
}