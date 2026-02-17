using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Another_Game_Final_no_fugazy
{
    internal class EnemyBrawler : CombatEntity
    {
        protected readonly Random random = new Random();

        public EnemyBrawler(Texture2D texture, int x, int y, int width, int height, Action onClick, Color normalColor, Color hoverColor, int BrawlerMaxHp, int BrawilerAttackPower, int BrawlerDefensePower) : base(texture, x, y, width, height, onClick, normalColor, hoverColor, BrawlerMaxHp, BrawilerAttackPower, BrawlerDefensePower)
        {
            this.baseTTNA = 3;
            this.turnsTillNextAction = baseTTNA;
        }

        public override void PerformAction()
        {
            int ChanceOfAction = random.Next(100);


            if (ChanceOfAction < 70)
            {
                Debug.WriteLine("Brawler Attacks!");
            }
            else
            {
                Heal(5);
            }
        }
    }
}