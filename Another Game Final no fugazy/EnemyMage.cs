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
    internal class EnemyMage : CombatEntity
    {
        protected readonly Random random = new Random();

        public EnemyMage(Texture2D texture, Rectangle rect, Action onClick, Color normalColor, Color hoverColor, int BrawlerMaxHp, int BrawilerAttackPower) : base(texture, rect, onClick, normalColor, hoverColor, BrawlerMaxHp, BrawilerAttackPower)
        {
            this.baseTTNA = 3;
            this.turnsTillNextAction = baseTTNA;
        }

        public override void PerformAction()
        {
            int ChanceOfAction = random.Next(100);


            if (ChanceOfAction < 30)
            {
                Debug.WriteLine("Brawler Attacks!");
            }

            else if (30 < ChanceOfAction && ChanceOfAction < 80)
            {
                GiveDebuff(2);
                Debug.WriteLine($" EnemyMage DEBUFF ");
            }

            else
            {
                Heal(5);
                Debug.WriteLine($"EnemyMage HEAL ");
            }
        }
    }
}