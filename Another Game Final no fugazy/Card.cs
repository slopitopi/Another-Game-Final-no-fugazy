using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Another_Game_Final_no_fugazy
{
    internal class Card : GameObject
    {
        protected string Name;
        protected int pos3Index;
        private Instructions instructions;
        private SpriteFont font;

        public Card(string name, Texture2D texture, int x, int y, int width, int height, Action onClick, Color normalColor, Color hoverColor) : base(texture, x, y, width, height, onClick, normalColor, hoverColor)
        {
            this.Name = name;
        }

        public string name
        {
            get { return Name; }
        }

        public void CardInstruction(GraphicsDevice graphicsDevice)
        {
            instructions = new Instructions(font, "Select an Enemy", graphicsDevice, new Vector2(0, 50));
        }

        public override void OnClick()
        {
            SelectEnemy();
        }

        public void SelectEnemy()
        {
            
        }
    }
}