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
        private int debuffTurnsRemaining;




        public EnemyBrawler(Texture2D texture, Rectangle rect, Action onClick, Color normalColor, Color hoverColor, int BrawlerMaxHp, int BrawilerAttackPower, Player player) : base(texture, rect, onClick, normalColor, hoverColor, BrawlerMaxHp, BrawilerAttackPower)
        {
            this.baseTTNA = 3;
            this.turnsTillNextAction = baseTTNA;

            this.player = player;
        }

        public int IsDebuffed()
        {
            return debuffTurnsRemaining;
        }


        public override void GiveDebuff(int Turns)
        {
            debuffTurnsRemaining = Turns;
            UpdateEffectBoxText();
        }

        public void UpdateEffectBoxText()
        {
            string text = "Enemy Characteristics \n\n" +
                          "Type: Brawler \n" +
                          "Focus is on attack \n" +
                          "70% Attack / 30% Heal \n" +
                          $"Damage: {attackPower} \n" +
                          $"Debuff turns: {debuffTurnsRemaining} \n";

            EffectBoxes.SetText(text);
        }


        public override void WaitTurns()
        {
            base.WaitTurns();



            if (debuffTurnsRemaining > 0)
            {
                Debug.WriteLine($"Enemy DEBUFF for {debuffTurnsRemaining} ");

                debuffTurnsRemaining--;
                UpdateEffectBoxText();
                if (debuffTurnsRemaining == 0)
                {
                    Debug.WriteLine("Enemy DEBUFF OVER");
                }
            }

            else
            {
            }
        }






        public override void PerformAction()
        {
            int ChanceOfAction = random.Next(100);


            if (ChanceOfAction < 70)
            {
                if (debuffTurnsRemaining == 0)
                {
                    player.TakeDamage(attackPower);
                    player.HealthBar.UpdateHealth();
                    Debug.WriteLine("Brawler Debuffed!");

                    return;
                }

                else
                {
                    player.TakeDamage(attackPower - 2);
                    player.HealthBar.UpdateHealth();
                    Debug.WriteLine("Brawler Attacks!");
                    return;
                }

            }

            else
            {
                Heal(5);
                Debug.WriteLine($"BRAWLER HEAL ");
            }
        }
    }
}