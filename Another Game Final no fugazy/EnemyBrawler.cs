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
    /// Represents the Brawler enemy type in the game. Inherits from CombatEntity for shared health, damage, and turn-based behavior.
    /// 
    /// The Brawler is the heavy-hitting enemy type with the highest base damage. Its action distribution is:
    /// 70% chance to attack the player and 30% chance to heal itself for 5 HP.
    /// 
    /// The Brawler acts every 3 turns (baseTTNA = 3) and starts with the highest max HP among enemies (30 + wave bonus).
    /// Like all enemies, it can be debuffed by the player's DebuffCard, which halves its attack damage for a number of turns.
    /// 
    /// Created and managed by GameElements.SpawnEnemys(), which passes in a reference to the player so the Brawler can attack it.
    /// </summary>
    internal class EnemyBrawler : CombatEntity
    {
        protected readonly Random random = new Random(); // Random number generator used to determine which action the brawler performs each turn (attack or heal).
        private Player player; // Reference to the player, used for attacking the player during PerformAction.
        private int debuffTurnsRemaining; // Tracks how many turns remain on the brawler's debuff. While debuffed, its attack damage is halved.




        /// <summary>
        /// Constructs a new EnemyBrawler with the given texture, position, colors, max HP, attack power, and a reference to the player.
        /// Sets the base turns till next action to 3 (moderate action speed).
        /// </summary>
        public EnemyBrawler(Texture2D texture, Rectangle rect, Action onClick, Color normalColor, Color hoverColor, int BrawlerMaxHp, int BrawilerAttackPower, Player player) : base(texture, rect, onClick, normalColor, hoverColor, BrawlerMaxHp, BrawilerAttackPower)
        {
            this.baseTTNA = 3; // Brawler acts every 3 turns
            this.turnsTillNextAction = baseTTNA; // Initialize the turn counter

            this.player = player; // Store reference to the player for attacks
        }

        /// <summary>
        /// Returns the number of debuff turns remaining. A value greater than 0 means the brawler is debuffed
        /// and its attack damage will be halved.
        /// </summary>
        public int IsDebuffed()
        {
            return debuffTurnsRemaining;
        }


        /// <summary>
        /// Applies a debuff to the brawler for the specified number of turns. While debuffed, its attack damage
        /// is halved in PerformAction. Also updates the effect box text to reflect the new debuff status.
        /// Called by GameElements when the player plays a DebuffCard targeting this enemy.
        /// </summary>
        public override void GiveDebuff(int Turns)
        {
            debuffTurnsRemaining = Turns;
            UpdateEffectBoxText(); // Refresh the effect box to show debuff duration and adjusted damage
        }

        /// <summary>
        /// Updates the brawler's effect box text to display its current type, action distribution, damage,
        /// and debuff status. Dynamically adjusts based on whether the brawler is debuffed (showing halved damage if so).
        /// Called whenever the debuff state changes (GiveDebuff, WaitTurns).
        /// </summary>
        public void UpdateEffectBoxText()
        {
            string currentDebuff;
            string currentDamage;

            if (debuffTurnsRemaining > 0) // If debuffed, show reduced damage and remaining debuff turns
            {
                currentDebuff = $"Debuff turns: {debuffTurnsRemaining}";
                currentDamage = $"Damage: {attackPower / 2} \n";
            }
            else // If not debuffed, show full damage
            {
                currentDebuff = $"Has no debuffs";
                currentDamage = $"Damage: {attackPower} \n";
            }

            // Compose the full effect box text showing the brawler's characteristics and action probabilities
            string text = "Enemy Characteristics \n\n" +
                          "Type: Brawler \n" +
                          "Focus is on attack \n" +
                          "70% Attack / 30% Heal \n" +
                          currentDamage +
                          currentDebuff;

            EffectBoxes.SetText(text); // Push the updated text to the Instructions-based effect box for rendering
        }


        /// <summary>
        /// Called every turn by GameElements when the player plays a card. Calls the base WaitTurns() to handle
        /// the action timer (decrement counter, perform action when it reaches 0), and then handles the brawler's
        /// own debuff countdown. Decrements debuffTurnsRemaining and updates the effect box text accordingly.
        /// </summary>
        public override void WaitTurns()
        {
            base.WaitTurns(); // Handle the turn counter and potentially trigger PerformAction



            if (debuffTurnsRemaining > 0) // If the brawler is debuffed, decrement the debuff counter
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
        /// Defines the brawler's action when its turn counter reaches 0. Randomly selects one of two actions:
        /// - 70% chance: Attack the player (damage halved if the brawler is debuffed).
        /// - 30% chance: Heal itself for 5 HP.
        /// Called automatically by WaitTurns() via the base CombatEntity turn system.
        /// </summary>
        public override void PerformAction()
        {
            int ChanceOfAction = random.Next(100); // Roll a random number 0-99 to determine which action to take


            if (ChanceOfAction < 70) // 70% chance: Attack the player
            {
                if (debuffTurnsRemaining == 0) // If NOT debuffed, deal full damage to the player
                {
                    player.TakeDamage(attackPower);
                    player.HealthBar.UpdateHealth();
                    Debug.WriteLine("Brawler Debuffed!");

                    return;
                }

                else // If debuffed, deal half damage to the player
                {
                    player.TakeDamage(attackPower/2);
                    player.HealthBar.UpdateHealth();
                    Debug.WriteLine("Brawler Attacks!");
                    return;
                }

            }

            else // 30% chance: Heal itself for 5 HP
            {
                Heal(5);
                Debug.WriteLine($"BRAWLER HEAL ");
            }
        }
    }
}