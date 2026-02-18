using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Another_Game_Final_no_fugazy
{
    internal class EnemyBrawler : CombatEntity
    {
        protected readonly Random random = new Random();
        private Player player;



        public EnemyBrawler(Texture2D texture, Rectangle rect, Action onClick, Color normalColor, Color hoverColor, int BrawlerMaxHp, int BrawilerAttackPower, Player player) : base(texture, rect, onClick, normalColor, hoverColor, BrawlerMaxHp, BrawilerAttackPower)
        {
            this.baseTTNA = 3;
            this.turnsTillNextAction = baseTTNA;

            this.player = player;
        }

        public override void PerformAction()
        {
            int ChanceOfAction = random.Next(100);


            if (ChanceOfAction < 70)
            {
                player.GiveDebuff(2);
                player.HealthBar.UpdateHealth();
                Debug.WriteLine("Brawler Debuffed!");
            }

            else
            {
                Heal(5);
                Debug.WriteLine($"BRAWLER HEAL ");
            }
        }
    }
}