using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Another_Game_Final_no_fugazy
{
    public interface ICombatant
    {
      
        int CurrentHP { get; }
        int MaxHP { get; }
        bool IsAlive { get; }

        void TakeDamage(int amount);
        void Heal(int amount);
        void Attack(ICombatant target);
    }
}
