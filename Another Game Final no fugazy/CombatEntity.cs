using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Another_Game_Final_no_fugazy
{
    internal abstract class CombatEntity : GameObject
    {
        protected int enemyHP;
        protected int enemyMaxHP;
        protected int attackPower;
        protected int turnsTillNextAction;
        protected int baseTTNA;
        protected bool GivenDebuff;



        public CombatEntity(Texture2D texture, Rectangle rect, Action onClick, Color normalColor, Color hoverColor, int enemyMaxHp, int attackPower) : base(texture, rect, onClick, normalColor, hoverColor)
        {
            this.enemyMaxHP = enemyMaxHp;
            this.enemyHP = enemyMaxHP;

            this.attackPower = attackPower;

            this.baseTTNA = 2;
            this.turnsTillNextAction = baseTTNA;
        }


        public bool IsAlive
        {
            get { return enemyHP > 0; }
        }

        public int EnemyHP
        {
            get { return enemyHP; }
        }
        public int EnemyMaxHP
        {
            get { return enemyMaxHP; }
        }

        public HealthBar HealthBar
        {
            get;
            set;
        }

        public Instructions EffectBoxes
        {
            get; 
            set;
        }






        public virtual void TakeDamage(int CardDamage)
        {
            enemyHP -= CardDamage;
            if (enemyHP < 0)
            {
                enemyHP = 0;
            }
            Debug.WriteLine($"{EnemyHP} ");
        }

        public virtual void Heal(int HealAmount)
        {
            enemyHP += HealAmount;
            if (enemyHP > EnemyMaxHP)
            {
                enemyHP = EnemyMaxHP;
            }

        }

        public virtual void GiveDebuff(int Turns)  //Change to abstract method Later
        {
            GivenDebuff = true;
        }



        public abstract void PerformAction();


        public virtual void WaitTurns()
        {
            if (turnsTillNextAction > 0)
            {
                turnsTillNextAction--;
            }

            if (turnsTillNextAction == 0)
            {
                PerformAction();
                turnsTillNextAction = baseTTNA;
            }
        }
    }
}