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
    /// <summary>
    /// Represents the player character in the game. Inherits from CombatEntity to share health, damage, and turn-based behavior
    /// with enemies, but overrides specific methods to implement player-specific logic such as debuff tracking, wave-scaled
    /// effect display, and a no-op for Update and PerformAction (since the player is controlled via card selection, not AI).
    /// </summary>
    internal class Player : CombatEntity
    {

        private int debuffTurnsRemaining = 0; // Tracks how many turns the player's debuff lasts. When greater than 0, the player's damage output is reduced.
        private int currentWave; // Stores the current wave number, used to scale the player's card damage and healing display in the effect box.


        /// <summary>
        /// Constructs a new Player instance with the given texture, position, colors, max HP, and attack power.
        /// The player is effectively a non-clickable placeholder entity — all interaction happens through cards in GameElements.
        /// </summary>
        public Player(Texture2D texture, Rectangle rect, Action onClick, Color normalColor, Color hoverColor, int maxHp, int AttackPower) : base(texture, rect, onClick, normalColor, hoverColor, maxHp, AttackPower)
        {
        }


        /// <summary>
        /// Gets or sets the current wave number for the player. When set, it also refreshes the effect box text
        /// so that the displayed card damage and healing values reflect the wave-based scaling.
        /// </summary>
        public int CurrentWave
        {
            get { return currentWave; }
            set { currentWave = value; UpdateEffectBoxText(); }
        }


        /// <summary>
        /// Returns whether the player is currently debuffed. Used by GameElements when playing a DamageCard
        /// to determine if the player's damage output should be reduced.
        /// </summary>
        public bool IsDebuffed()
        {
            return debuffTurnsRemaining > 0;
        }


        /// <summary>
        /// The player does not perform autonomous actions (unlike enemies), so this method is intentionally left empty.
        /// All player actions are handled through the card system in GameElements.
        /// </summary>
        public override void PerformAction()
        {
        }

        /// <summary>
        /// Applies a debuff to the player for the specified number of turns. While debuffed, the player's
        /// DamageCard deals reduced damage. Also updates the effect box text to reflect the new debuff status.
        /// Called by enemy PerformAction methods (e.g., EnemyMage, EnemyHealer) when they choose to debuff the player.
        /// </summary>
        public override void GiveDebuff(int Turns)
        {
            debuffTurnsRemaining = Turns;
            UpdateEffectBoxText(); // Refresh the effect box to show the debuff duration and adjusted damage values.
            Debug.WriteLine($"Player DEBUFF for {Turns} ");
        }


        /// <summary>
        /// Updates the player's effect box text to display current debuff status, card damage values, and heal values.
        /// The text dynamically adjusts based on whether the player is debuffed and the current wave number,
        /// showing both the base values and wave bonuses so the player can see how their cards scale.
        /// </summary>
        public void UpdateEffectBoxText()
        {
            string currentDebuff;
            string currentDamage;
            if (debuffTurnsRemaining > 0) // If debuffed, show reduced damage and remaining debuff turns
            {
                currentDebuff = $"Debuff turns: {debuffTurnsRemaining}";
                currentDamage = $"Damage card: 10 {-2 - currentWave} :Total: {(10 + currentWave -1) -2 - currentWave} \n";
            }
            else // If not debuffed, show full damage with the wave bonus
            {
                currentDebuff = $"Has no debuffs";
                currentDamage = $"Damage card: 10 + {currentWave - 1} :Total: {10 + currentWave -1}\n";
            }

            // Compose the full effect box text with all player characteristics
            string text = "Player Characteristics \n\n" +
                      $"{currentDebuff}\n\n\n" +
                      $"Card Effect : WaveBonus\n" +
                      currentDamage +
                      $"Heal card: 5 + {currentWave -1}\n\n";



            EffectBoxes.SetText(text); // Push the updated text to the Instructions-based effect box for rendering.
        }


        /// <summary>
        /// Decrements the player's debuff counter by one turn. Called every time the player plays a card (both self-targeting
        /// and enemy-targeting) in GameElements to simulate the passage of a turn for debuff purposes.
        /// When the debuff expires (reaches 0), the effect box text is updated to reflect the removal of the debuff.
        /// </summary>
        public override void WaitTurns()
        {
            if (debuffTurnsRemaining > 0)
            {
                debuffTurnsRemaining--;
                UpdateEffectBoxText(); // Refresh text to show the decremented debuff counter.

                if (debuffTurnsRemaining == 0)
                {
                    Debug.WriteLine("Player DEBUFF OVER");
                }
            }
        }




        /// <summary>
        /// The player does not respond to mouse input directly (it cannot be clicked or hovered over),
        /// so this Update method is intentionally left empty. All player interaction is handled through the card system.
        /// </summary>
        public override void Update(MouseState mouseState)
        {
        }
    }
}