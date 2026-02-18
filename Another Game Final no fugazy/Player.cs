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
        public bool Debuffed = false;


        public Player(Texture2D texture, Rectangle rect, Action onClick, Color normalColor, Color hoverColor, int maxHp, int attackPower) : base(texture, rect, onClick, normalColor, hoverColor, maxHp, attackPower)
        {

        }

        public bool IsDebuffed()
        {
            return Debuffed;
        }


        public override void PerformAction()
        {
        }

        public override void GiveDebuff(int Turns)
        {
            Debuffed = true;

            for (int i = 0; i < Turns; i++)
            {
                Debug.WriteLine($"Player DEBUFF for {Turns - i} ");

                if (i == Turns - 1)
                {
                    Debuffed = false;
                    Debug.WriteLine("Player DEBUFF EXPIRED");
                }
            }
        }

        public override void Update(MouseState mouseState)
        {

        }
    }
}