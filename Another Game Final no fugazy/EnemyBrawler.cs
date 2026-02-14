using System;
using System.Collections.Generic;
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
        protected int BrawlerAttack;


        public EnemyBrawler(Texture2D texture, int x, int y, int width, int height, Action onClick, Color normalColor, Color hoverColor, int BrawlerHP, int BrawlerMaxHp, int BrawilerAttackPower, int BrawlerDefensePower) : base(texture, x, y, width, height, onClick, normalColor, hoverColor, BrawlerHP, BrawlerMaxHp, BrawilerAttackPower, BrawlerDefensePower)
        {
            BrawlerHP = BrawlerMaxHp;
            BrawlerMaxHp = 30;
            BrawilerAttackPower = 8;
            BrawlerAttack = BrawilerAttackPower;
            this.BaseTTNA = 3;
            this.TurnsTillNextAction = BaseTTNA;
        }


        public override void PerformAction()
        {

            int ChanceOfAction = random.Next(100);


            if (ChanceOfAction < 70)
            {
                Attack(BrawlerAttack);
            }
            else
            {
                Heal(5);
            }
        }


        public void BattleLogic()
        {
            WaitTurns();
        }
    }
}