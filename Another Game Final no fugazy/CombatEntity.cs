using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Another_Game_Final_no_fugazy
{
    internal abstract class CombatEntity : GameObject
    {
        protected int EnemyHP;
        protected int EnemyMaxHP;
        protected int AttackPower;
        protected int DefensePower;
        protected int TurnsTillNextAction;
        protected int BaseTTNA;

        public CombatEntity(Texture2D texture, int x, int y, int width, int height, Action onClick, Color normalColor, Color hoverColor, int enemyHp, int enemyMaxHp, int attackPower, int defensePower) : base(texture, x, y, width, height, onClick, normalColor, hoverColor)
        {
            this.EnemyHP = enemyHp;
            this.EnemyMaxHP = enemyMaxHp;
            this.AttackPower = attackPower;
            this.DefensePower = defensePower;
            this.BaseTTNA = 2;
            this.TurnsTillNextAction = BaseTTNA;
        }




        public virtual void TakeDamage(int CardDamage)
        {
            EnemyHP -= CardDamage;
        }

        public virtual void Heal(int HealAmount)
        {
            EnemyHP += HealAmount;
            if (EnemyHP > EnemyMaxHP)
            {
                EnemyHP = EnemyMaxHP;
            }
        }

        public virtual void Attack(int AttackAmount)
        {
            AttackAmount = AttackPower;
        }

        public virtual void PerformAction()
        {
            // Placeholder for enemy action logic
        }

        public virtual void WaitTurns()
        {
            if (TurnsTillNextAction > 0)
            {
                TurnsTillNextAction--;
            }

            if (TurnsTillNextAction == 0)
            {
                PerformAction();
                TurnsTillNextAction = BaseTTNA;
            }
        }

        public override void OnClick()
        {
            TakeDamage(10); // Example damage value, can be modified based on card effects
        }
    }
}