using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Another_Game_Final_no_fugazy
{
    internal class Button
    {
        private Texture2D texture;
        private Rectangle rect;
        private Color normalColor;
        private Color hoverColor;
        private Color currentColor;
        private Action onClick;
        private bool wasPressed;

        public Button(Texture2D texture, int x, int y, int width, int height, Action onClick, Color? normalColor = null, Color? hoverColor = null)
        {
            this.texture = texture;
            this.rect = new Rectangle(x, y, width, height);
            this.onClick = onClick;
            this.normalColor = normalColor ?? Color.White;
            this.hoverColor = hoverColor ?? Color.Red;
            this.currentColor = this.normalColor;
            this.wasPressed = false;
        }




        /// <summary>
        /// Checks for mouse hover and click events to update button state and trigger actions.
        /// 
        /// It changes button color and makes sure it registers the click and exicutes the action.
        /// </summary>
        /// <param name="mouseState"></param>
        public void Update(MouseState mouseState)
        {
            bool isHovering = rect.Contains(mouseState.X, mouseState.Y);

            // Update color based on hover state
            currentColor = isHovering ? hoverColor : normalColor;

            // Handle click with press/release detection
            if (isHovering && mouseState.LeftButton == ButtonState.Pressed)
            {
                wasPressed = true;
            }

            else if (wasPressed && mouseState.LeftButton == ButtonState.Released)
            {
                wasPressed = false;
                onClick?.Invoke(); // Trigger the action
            }

            else if (!isHovering)
            {
                wasPressed = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rect, currentColor);
        }
    }
}
