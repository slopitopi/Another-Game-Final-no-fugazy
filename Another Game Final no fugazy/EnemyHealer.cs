using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Another_Game_Final_no_fugazy
{
    /// <summary>
    /// Represents the Healer enemy type in the game. Inherits from CombatEntity for shared health, damage, and turn-based behavior.
    /// 
    /// The Healer's primary focus is supporting other enemies by healing the ally with the lowest HP. Its action distribution is:
    /// 60% chance to heal the lowest-HP ally, 20% chance to attack the player, and 20% chance to debuff the player.
    /// 
    /// The Healer has a slower action cadence (baseTTNA = 4, meaning it acts every 4 turns) compared to other enemy types.
    /// Like all enemies, it can be debuffed by the player's DebuffCard, which halves its attack damage for a number of turns.
    /// 
    /// Created and managed by GameElements.SpawnEnemys(), which passes in references to the player and the full enemy list
    /// so the Healer can target allies for healing and the player for attacks/debuffs.
    /// </summary>
    internal class EnemyHealer : CombatEntity
    {
        protected readonly Random random = new Random(); // Random number generator used to determine which action the healer performs each turn (heal, attack, or debuff).
        private Player player; // Reference to the player, used for attacking and debuffing the player during PerformAction.
        private int debuffTurnsRemaining; // Tracks how many turns remain on the healer's debuff. While debuffed, its attack damage is halved.
        private int HealAmount; // The amount of HP the healer restores when it heals an ally. Scales with the wave number (set in SpawnEnemys).
        private List<CombatEntity> allEnemies; // Reference to the full list of enemies on the field, used to find the lowest-HP ally for healing.

        /// <summary>
        /// Constructs a new EnemyHealer with the given texture, position, colors, max HP, attack power, heal amount,
        /// and references to the player and enemy list. Sets the base turns till next action to 4 (slowest enemy type).
        /// </summary>
        public EnemyHealer(Texture2D texture, Rectangle rect, Action onClick, Color normalColor, Color hoverColor, int HealerMaxHp, int HealerAttackPower, int healAmount, Player player, List<CombatEntity> allEnemies) : base(texture, rect, onClick, normalColor, hoverColor, HealerMaxHp, HealerAttackPower)
        {
            this.baseTTNA = 4; // Healer acts every 4 turns, making it the slowest enemy type
            this.turnsTillNextAction = baseTTNA; // Initialize the turn counter

            this.player = player; // Store reference to the player for attacks and debuffs
            this.HealAmount = healAmount; // Store the heal amount, which scales with the wave
            this.allEnemies = allEnemies; // Store reference to all enemies so the healer can find the lowest-HP ally to heal
        }

        /// <summary>
        /// Returns the number of debuff turns remaining. A value greater than 0 means the healer is debuffed
        /// and its attack damage will be halved.
        /// </summary>
        public int IsDebuffed()
        {
            return debuffTurnsRemaining;
        }


        /// <summary>
        /// Applies a debuff to the healer for the specified number of turns. While debuffed, its attack damage
        /// is halved in PerformAction. Also updates the effect box text to reflect the new debuff status.
        /// Called by GameElements when the player plays a DebuffCard targeting this enemy.
        /// </summary>
        public override void GiveDebuff(int Turns)
        {
            debuffTurnsRemaining = Turns;
            UpdateEffectBoxText(); // Refresh the effect box to show debuff duration and adjusted damage
        }


        /// <summary>
        /// Heals the ally with the lowest current HP among all living enemies. Iterates through the allEnemies list
        /// to find the target, then restores HP and updates the target's health bar.
        /// Called when the healer's PerformAction rolls a heal (60% chance).
        /// </summary>
        public void ApplyHealerHeal(int healAmount)
        {
            CombatEntity target = null; // Will hold the enemy with the lowest HP
            int lowestHP = int.MaxValue; // Start with the highest possible value so any enemy's HP will be lower

            foreach (var enemy in allEnemies) // Search through all enemies to find the one with the lowest HP that is still alive
            {
                if (enemy.EnemyHP < lowestHP && enemy.IsAlive)
                {
                    lowestHP = enemy.EnemyHP;
                    target = enemy; // Update the target to this enemy since it has the lowest HP so far
                }
            }


            target.Heal(healAmount); // Heal the lowest-HP ally by the healer's heal amount
            target.HealthBar.UpdateHealth(); // Update the healed ally's health bar to reflect the restored HP
        }



        /// <summary>
        /// Updates the healer's effect box text to display its current type, action distribution, damage, heal amount,
        /// and debuff status. Dynamically adjusts based on whether the healer is debuffed (showing halved damage if so).
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

            // Compose the full effect box text showing the healer's characteristics and action probabilities
            string text = "Enemy Characteristics \n\n" +
                          "Type: Healer \n" +
                          "Focus healing lowest enemy \n" +
                          "60% Heal / 20% debuff /20% Attack \n" +
                          currentDamage +
                          $"Heal: {HealAmount} \n" +
                          currentDebuff;

            EffectBoxes.SetText(text); // Push the updated text to the Instructions-based effect box for rendering
        }



        /// <summary>
        /// Called every turn by GameElements when the player plays a card. Calls the base WaitTurns() to handle
        /// the action timer (decrement counter, perform action when it reaches 0), and then handles the healer's
        /// own debuff countdown. Decrements debuffTurnsRemaining and updates the effect box text accordingly.
        /// </summary>
        public override void WaitTurns()
        {
            base.WaitTurns(); // Handle the turn counter and potentially trigger PerformAction
            if (debuffTurnsRemaining > 0) // If the healer is debuffed, decrement the debuff counter
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
        /// Defines the healer's action when its turn counter reaches 0. Randomly selects one of three actions:
        /// - 60% chance: Heal the ally with the lowest HP using ApplyHealerHeal.
        /// - 20% chance: Attack the player (damage halved if the healer is debuffed).
        /// - 20% chance: Debuff the player for 2 turns, reducing the player's damage output.
        /// Called automatically by WaitTurns() via the base CombatEntity turn system.
        /// </summary>
        public override void PerformAction()
        {
            int ChanceOfAction = random.Next(100); // Roll a random number 0-99 to determine which action to take


            if (ChanceOfAction < 60) // 60% chance: Heal the lowest-HP ally
            {
                ApplyHealerHeal(HealAmount);
                Debug.WriteLine("Healer used Heal!");
                return;
            }

            else if (60 < ChanceOfAction && ChanceOfAction < 80) // 20% chance: Attack the player
            {
                if (debuffTurnsRemaining == 0) // If NOT debuffed, deal full damage to the player
                {
                    player.TakeDamage(attackPower);
                    player.HealthBar.UpdateHealth();
                    Debug.WriteLine("Healer is Debuffed and attacked!");

                    return;
                }

                else // If debuffed, deal half damage to the player
                {
                    player.TakeDamage(attackPower / 2);
                    player.HealthBar.UpdateHealth();
                    Debug.WriteLine("Healer Attacks!");
                    return;
                }
            }

            else // 20% chance: Debuff the player for 2 turns
            {
                player.GiveDebuff(2);
                Debug.WriteLine("Healer Debuffed you!");

            }
        }
    }
}