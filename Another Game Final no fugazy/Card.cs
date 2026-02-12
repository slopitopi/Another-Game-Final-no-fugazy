using Microsoft.Xna.Framework.Graphics;
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
    internal class Card : Button
    {
        protected string Name;

        public Card(string name, Texture2D texture, int x, int y, int width, int height, Action onClick, Color normalColor, Color hoverColor) : base(texture, x, y, width, height, onClick, normalColor, hoverColor)
        {
            this.Name = name;
        }

        public string name
        {
            get { return Name; }
        }

    }
}