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
    internal class HealthBar : Instructions
    {
        private CombatEntity entity;
        private Color fillColor = Color.Green;
        private Color backColor = Color.Black;

        public HealthBar(SpriteFont font, GraphicsDevice graphicsDevice, Vector2 position, CombatEntity entity) : base(font, GetHealthText(entity), graphicsDevice, position)
        {
            this.entity = entity;
        }

        public static string GetHealthText(CombatEntity entity)
        {
            return $"{entity.EnemyHP}/{entity.EnemyMaxHP}";
        }




        public void UpdateHealth()
        {
            Text = $"{entity.EnemyHP} / {entity.EnemyMaxHP}";
        }




        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(BackgroundTexture, BackgroundRect, backColor);

            float healthProcent = (float)entity.EnemyHP / entity.EnemyMaxHP;
            int backW = (int)(BackgroundRect.Width * healthProcent);
            backW = Math.Max(0, Math.Min(BackgroundRect.Width, backW));

            if (backW > 0)
            {
                Rectangle fillRectangle = new Rectangle(BackgroundRect.X, BackgroundRect.Y, backW, BackgroundRect.Height);
                spriteBatch.Draw(BackgroundTexture, fillRectangle, fillColor);
            }

            spriteBatch.DrawString(Font, Text, TextPosition, Color.White);
        }
    }
}