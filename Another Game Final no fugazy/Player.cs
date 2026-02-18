using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Another_Game_Final_no_fugazy
{
    internal class Player : CombatEntity
    {

        private int debuffTurnsRemaining = 0;


        public Player(Texture2D texture, Rectangle rect, Action onClick, Color normalColor, Color hoverColor, int maxHp, int attackPower) : base(texture, rect, onClick, normalColor, hoverColor, maxHp, attackPower)
        {

        }

        public bool IsDebuffed()
        {
            return debuffTurnsRemaining > 0;
        }


        public override void PerformAction()
        {
        }

        public override void GiveDebuff(int Turns)
        {
            debuffTurnsRemaining = Turns;
            UpdateEffectBoxText();
            Debug.WriteLine($"Player DEBUFF for {Turns} ");
        }


        public void UpdateEffectBoxText()
        {
            string text = "Player Characteristics \n\n" +
                          $"Debuff turns: {debuffTurnsRemaining} \n" +
                          $"Card increase per wave: wavenNum \n\n" +
                          $"Damage = dmg * wavemath \n" +
                          $"Health = heal * wavemath \n" +
                          $"Buff = buff * wavemath \n" +
                          $"DOT = dmg * wavemath \n";


            EffectBoxes.SetText(text);
        }


        public override void WaitTurns()
        {
            if (debuffTurnsRemaining > 0)
            {
                debuffTurnsRemaining--;
                UpdateEffectBoxText();

                if (debuffTurnsRemaining == 0)
                {
                    Debug.WriteLine("Player DEBUFF OVER");
                }
            }
        }




        public override void Update(MouseState mouseState)
        {
        }
    }
}