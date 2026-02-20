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
    /// <summary>
    /// Abstract base class for all combat participants in the game (player and enemies).
    /// Inherits from GameObject to get drawing and click/hover behavior, and adds health management,
    /// turn-based action timing, damage/healing, and debuff support.
    /// 
    /// Each CombatEntity has HP, max HP, attack power, and a turn counter that controls when it performs
    /// its action (via PerformAction). Subclasses (Player, EnemyBrawler, EnemyMage, EnemyHealer) override
    /// PerformAction to define their specific behavior, and may override GiveDebuff and WaitTurns for
    /// debuff-specific logic.
    /// 
    /// CombatEntity also holds references to its HealthBar and EffectBoxes (Instructions-based UI elements),
    /// which are created and linked in GameElements during LoadContent and SpawnEnemys.
    /// </summary>
    internal abstract class CombatEntity : GameObject
    {
        protected int enemyHP; // Current health points of this entity. When this reaches 0, the entity is considered dead (IsAlive returns false).
        protected int enemyMaxHP; // Maximum health points, set at construction. Used by the health bar to calculate the fill percentage and to cap healing.
        protected int attackPower; // Base attack damage dealt when this entity performs an attack action. May be halved when debuffed, depending on the subclass.
        protected int turnsTillNextAction; // Counter that decrements each turn via WaitTurns(). When it reaches 0, PerformAction() is called and the counter resets to baseTTNA.
        protected int baseTTNA; // Base "turns till next action" — the number of turns this entity must wait between actions. Set by subclasses (e.g., Brawler=3, Mage=2, Healer=4).
        protected bool GivenDebuff; // Flag indicating whether this entity has been given a debuff. Used by the base GiveDebuff implementation.



        /// <summary>
        /// Constructs a new CombatEntity with the specified texture, position, click action, colors, max HP, and attack power.
        /// Initializes HP to max, and sets the default turn timer to 2 turns (subclasses may override baseTTNA in their constructors).
        /// </summary>
        public CombatEntity(Texture2D texture, Rectangle rect, Action onClick, Color normalColor, Color hoverColor, int enemyMaxHp, int attackPower) : base(texture, rect, onClick, normalColor, hoverColor)
        {
            this.enemyMaxHP = enemyMaxHp;
            this.enemyHP = enemyMaxHP; // Start at full health

            this.attackPower = attackPower;

            this.baseTTNA = 2; // Default turns between actions; subclasses override this as needed
            this.turnsTillNextAction = baseTTNA; // Initialize the turn counter to the base value so the entity waits before its first action
        }


        /// <summary>
        /// Returns true if the entity's HP is above 0, meaning it is still alive and should be updated and drawn.
        /// Used by GameElements to skip dead enemies in update/draw loops and to check wave completion.
        /// </summary>
        public bool IsAlive
        {
            get { return enemyHP > 0; }
        }

        /// <summary>
        /// Gets the current health points of this entity. Used by HealthBar to display the current HP value.
        /// </summary>
        public int EnemyHP
        {
            get { return enemyHP; }
        }

        /// <summary>
        /// Gets the maximum health points of this entity. Used by HealthBar to display the max HP and calculate fill percentage.
        /// </summary>
        public int EnemyMaxHP
        {
            get { return enemyMaxHP; }
        }

        /// <summary>
        /// Gets or sets the HealthBar UI component linked to this entity. Created in GameElements (LoadContentGE for the player,
        /// SpawnEnemys for enemies) and used to visually display the entity's current health as a colored bar.
        /// </summary>
        public HealthBar HealthBar
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the EffectBoxes UI component (an Instructions instance) linked to this entity.
        /// Created in GameElements and used to display active debuffs, damage values, and other status information
        /// next to the entity on screen.
        /// </summary>
        public Instructions EffectBoxes
        {
            get;
            set;
        }





        /// <summary>
        /// Reduces this entity's HP by the specified damage amount. Clamps HP to a minimum of 0.
        /// Called by GameElements when the player plays a DamageCard targeting this enemy,
        /// or by enemy PerformAction methods when enemies attack the player.
        /// </summary>
        public virtual void TakeDamage(int CardDamage)
        {
            enemyHP -= CardDamage;
            if (enemyHP < 0) // Prevent HP from going negative
            {
                enemyHP = 0;
            }
            Debug.WriteLine($"{EnemyHP} ");
        }

        /// <summary>
        /// Increases this entity's HP by the specified amount. Clamps HP to the maximum (enemyMaxHP).
        /// Called by GameElements when the player plays a HealCard (for the player), or by enemy
        /// PerformAction methods when enemies heal themselves or allies.
        /// </summary>
        public virtual void Heal(int HealAmount)
        {
            enemyHP += HealAmount;
            if (enemyHP > EnemyMaxHP) // Prevent HP from exceeding the maximum
            {
                enemyHP = EnemyMaxHP;
            }

        }

        /// <summary>
        /// Base implementation for applying a debuff to this entity. Sets the GivenDebuff flag to true.
        /// Subclasses (EnemyBrawler, EnemyMage, EnemyHealer, Player) override this to track debuff turns
        /// and update their effect box text accordingly.
        /// Called by GameElements when the player plays a DebuffCard, or by enemy actions that debuff the player.
        /// </summary>
        public virtual void GiveDebuff(int Turns)
        {
            GivenDebuff = true;
        }



        /// <summary>
        /// Abstract method that defines what this entity does when its turn counter reaches 0.
        /// Each subclass implements its own action logic (e.g., Brawler attacks/heals, Mage debuffs/attacks, Healer heals allies).
        /// Called automatically by WaitTurns() when turnsTillNextAction reaches 0.
        /// </summary>
        public abstract void PerformAction();


        /// <summary>
        /// Decrements the turn counter by one. When it reaches 0, calls PerformAction() and resets the counter to baseTTNA.
        /// This is called by GameElements every time the player plays a card (both self-targeting and enemy-targeting),
        /// effectively advancing the turn for all entities. Subclasses call base.WaitTurns() and then handle their own
        /// debuff decrementing logic.
        /// </summary>
        public virtual void WaitTurns()
        {
            if (turnsTillNextAction > 0)
            {
                turnsTillNextAction--; // Count down toward the next action
            }

            if (turnsTillNextAction == 0) // Time to act
            {
                PerformAction(); // Execute this entity's specific action logic
                turnsTillNextAction = baseTTNA; // Reset the counter for the next action cycle
            }
        }
    }
}