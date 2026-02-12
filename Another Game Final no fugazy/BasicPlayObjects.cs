using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Another_Game_Final_no_fugazy
{
    internal abstract class GameObject
    {
        protected Texture2D texture;
        protected Vector2 vector;
        //--------------------------------konstruktor--------------------------//

        public GameObject(Texture2D texture, float X, float Y)
        {
            this.texture = texture;
            vector.X = X;
            vector.Y = Y;
        }

        //--------------------------------konstruktor END--------------------------//



        //--------------------------draw-----------------------//
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, vector, Color.White);
        }
        //--------------------------draw END-----------------------//




        //------------------egenskaper----------------------//
        public float X
        {
            get { return vector.X; }
        }

        public float Y
        {
            get { return vector.Y; }
        }

        public float Width
        {
            get { return texture.Width; }
        }

        public float Height
        {
            get { return texture.Height; }
        }
        //------------------egenskaper Slut----------------------//
    }
}