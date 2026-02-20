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
    /// <summary>
    /// Represents the Mage enemy type in the game. Inherits from CombatEntity for shared health, damage, and turn-based behavior.
    /// 
    /// The Mage's primary focus is debuffing the player to reduce their damage output. Its action distribution is:
    /// 60% chance to debuff the player (for 5 turns), 20% chance to attack the player, and 20% chance to heal itself for 5 HP.
    /// 
    /// The Mage has the fastest action cadence (baseTTNA = 2, meaning it acts every 2 turns), making it a frequent threat
    /// even though its individual attacks are weaker. Like all enemies, it can be debuffed by the player's DebuffCard,
    /// which halves its attack damage for a number of turns.
    /// 
    /// Created and managed by GameElements.SpawnEnemys(), which passes in a reference to the player so the Mage can attack and debuff it.
    /// </summary>
    internal class EnemyMage : CombatEntity
    {
        protected readonly Random random = new Random(); // Random number generator used to determine which action the mage performs each turn (debuff, attack, or heal).
        private Player player; // Reference to the player, used for attacking and debuffing the player during PerformAction.
        private int debuffTurnsRemaining; // Tracks how many turns remain on the mage's debuff. While debuffed, its attack damage is halved.

        /// <summary>
        /// Constructs a new EnemyMage with the given texture, position, colors, max HP, attack power, and a reference to the player.
        /// Sets the base turns till next action to 2 (fastest enemy type).
        /// </summary>
        public EnemyMage(Texture2D texture, Rectangle rect, Action onClick, Color normalColor, Color hoverColor, int MageMaxHp, int MageAttackPower, Player player) : base(texture, rect, onClick, normalColor, hoverColor, MageMaxHp, MageAttackPower)
        {
            this.baseTTNA = 2; // Mage acts every 2 turns, making it the fastest enemy type
            this.turnsTillNextAction = baseTTNA; // Initialize the turn counter

            this.player = player; // Store reference to the player for attacks and debuffs
        }

        /// <summary>
        /// Returns the number of debuff turns remaining. A value greater than 0 means the mage is debuffed
        /// and its attack damage will be halved.
        /// </summary>
        public int IsDebuffed()
        {
            return debuffTurnsRemaining;
        }


        /// <summary>
        /// Applies a debuff to the mage for the specified number of turns. While debuffed, its attack damage
        /// is halved in PerformAction. Also updates the effect box text to reflect the new debuff status.
        /// Called by GameElements when the player plays a DebuffCard targeting this enemy.
        /// </summary>
        public override void GiveDebuff(int Turns)
        {
            debuffTurnsRemaining = Turns;
            UpdateEffectBoxText(); // Refresh the effect box to show debuff duration and adjusted damage
        }

        /// <summary>
        /// Updates the mage's effect box text to display its current type, action distribution, damage,
        /// and debuff status. Dynamically adjusts based on whether the mage is debuffed (showing halved damage if so).
        /// Called whenever the debuff state changes (GiveDebuff, WaitTurns).
        /// </summary>
        public void UpdateEffectBoxText()
        {
            string currentDebuff;
            string currentDamage;
            if (debuffTurnsRemaining > 0) // If debuffed, show reduced damage and remaining debuff turns
            {
                currentDebuff = $"Debuff turns: {debuffTurnsRemaining}";
                currentDamage = $"Damage: {attackPower/2} \n";
            }
            else // If not debuffed, show full damage
            {
                currentDebuff = $"Has no debuffs";
                currentDamage = $"Damage: {attackPower} \n";
            }

            // Compose the full effect box text showing the mage's characteristics and action probabilities
            string text = "Enemy Characteristics \n\n" +
                          "Type: Mage \n" +
                          "Focus is on Debuffing \n" +
                          "60% Debuff / 20% Heal /20% Attack \n" +
                          currentDamage +
                          currentDebuff;

            EffectBoxes.SetText(text); // Push the updated text to the Instructions-based effect box for rendering
        }



        /// <summary>
        /// Called every turn by GameElements when the player plays a card. Calls the base WaitTurns() to handle
        /// the action timer (decrement counter, perform action when it reaches 0), and then handles the mage's
        /// own debuff countdown. Decrements debuffTurnsRemaining and updates the effect box text accordingly.
        /// </summary>
        public override void WaitTurns()
        {
            base.WaitTurns(); // Handle the turn counter and potentially trigger PerformAction



            if (debuffTurnsRemaining > 0) // If the mage is debuffed, decrement the debuff counter
            {
                Debug.WriteLine($"Enemy DEBUFF for {debuffTurnsRemaining} ");

                debuffTurnsRemaining--;
                UpdateEffectBoxText(); // Refresh the effect box to show the decremented debuff counter
                if (debuffTurnsRemaining == 0)
                {
                    Debug.WriteLine("Enemy DEBUFF OVER");
                }
            }

            else
            {
            }
        }






        /// <summary>
        /// Defines the mage's action when its turn counter reaches 0. Randomly selects one of three actions:
        /// - 60% chance: Debuff the player for 5 turns, reducing the player's damage output.
        /// - 20% chance: Attack the player (damage halved if the mage is debuffed).
        /// - 20% chance: Heal itself for 5 HP.
        /// Called automatically by WaitTurns() via the base CombatEntity turn system.
        /// </summary>
        public override void PerformAction()
        {
            int ChanceOfAction = random.Next(100); // Roll a random number 0-99 to determine which action to take


            if (ChanceOfAction < 60) // 60% chance: Debuff the player for 5 turns
            {
                player.GiveDebuff(5);
                player.HealthBar.UpdateHealth();
                Debug.WriteLine("Mage used Debuffed!");
                return;
            }

            else if (60 < ChanceOfAction && ChanceOfAction < 80) // 20% chance: Attack the player
            {
                if (debuffTurnsRemaining == 0) // If NOT debuffed, deal full damage to the player
                {
                    player.TakeDamage(attackPower);
                    player.HealthBar.UpdateHealth();
                    Debug.WriteLine("Mage is Debuffed and attacked!");

                    return;
                }

                else // If debuffed, deal half damage to the player
                {
                    player.TakeDamage(attackPower / 2);
                    player.HealthBar.UpdateHealth();
                    Debug.WriteLine("Mage Attacks!");
                    return;
                }
            }

            else // 20% chance: Heal itself for 5 HP
            {
                Heal(5);
                Debug.WriteLine($"MAGE HEAL ");
            }
        }

    }
}