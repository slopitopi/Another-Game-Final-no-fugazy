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
    internal class Button : GameObject
    {

        public Button(Texture2D texture, Rectangle rect, Action onClick, Color normalColor, Color hoverColor) : base(texture, rect, onClick, normalColor, hoverColor)
        {
        }
    }
}
