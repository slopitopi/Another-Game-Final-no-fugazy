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
    internal class EnemyHealer : CombatEntity
    {
        protected readonly Random random = new Random();

        public EnemyHealer(Texture2D texture, Rectangle rect, Action onClick, Color normalColor, Color hoverColor, int HealerMaxHp, int HealerAttackPower) : base(texture, rect, onClick, normalColor, hoverColor, HealerMaxHp, HealerAttackPower)
        {
            this.baseTTNA = 2;
            this.turnsTillNextAction = baseTTNA;
        }

        public override void PerformAction()
        {
            int ChanceOfAction = random.Next(100);


            if (ChanceOfAction < 30)
            {
                Debug.WriteLine("Healer Attacks!");
            }

            else
            {
                Heal(5);
                Debug.WriteLine($"BRAWLER HEAL ");
            }
        }
    }
}