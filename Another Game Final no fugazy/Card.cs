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
        private Instructions instructions;
        private SpriteFont font;
        bool SelectableCard = false;
        bool CardSelected = false;

        public Card(string name, Texture2D texture, int x, int y, int width, int height, Action onClick, Color normalColor, Color hoverColor) : base(texture, x, y, width, height, onClick, normalColor, hoverColor)
        {
            this.Name = name;
        }

        public string name
        {
            get { return Name; }
        }



        public bool selectableCard
        {
            get { return SelectableCard; }
            set { SelectableCard = value; }
        }

        public bool cardSelected
        {
            get { return CardSelected; }
            set { CardSelected = value; }
        }

        public void PauseCard()
        {
            onClick = null;
        }

    }
}