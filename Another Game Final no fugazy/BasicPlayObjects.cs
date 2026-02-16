using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Another_Game_Final_no_fugazy
{
    internal class GameObject
    {


        //--------------------------------Game State Management--------------------------------//
        protected Texture2D texture;
        protected Rectangle rect;
        protected Color normalColor;
        protected Color hoverColor;
        protected Color currentColor;
        protected Action onClick;
        protected bool wasPressed;
        //--------------------------------Game State Management--------------------------------//

        public GameObject(Texture2D texture, int x, int y, int width, int height, Action onClick, Color normalColor, Color hoverColor)
        {
            this.texture = texture;
            this.rect = new Rectangle(x, y, width, height);
            this.onClick = onClick;
            this.normalColor = normalColor;
            this.hoverColor = hoverColor;
            this.currentColor = this.normalColor;
            this.wasPressed = false;
        }



        public virtual void Update(MouseState mouseState)
        {
            bool isHovering = rect.Contains(mouseState.X, mouseState.Y);


            if (isHovering)
            {
                currentColor = hoverColor;
            }
            else
            {
                currentColor = normalColor;
            }



            if (isHovering && mouseState.LeftButton == ButtonState.Pressed)
            {
                wasPressed = true;
            }

            else if (wasPressed && mouseState.LeftButton == ButtonState.Released)
            {
                wasPressed = false;
                onClick.Invoke();
            }

            else if (!isHovering)
            {
                wasPressed = false;
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rect, currentColor);
        }

        public virtual void OnClick()
        {
            onClick.Invoke();
        }


    }
}