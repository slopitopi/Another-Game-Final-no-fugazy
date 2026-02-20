using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Another_Game_Final_no_fugazy
{
    /// <summary>
    /// Represents a playable card in the player's hand. Inherits from GameObject to get clickable/hoverable behavior.
    /// Each card has a name (e.g., "DamageCard", "HealCard", "DebuffCard") and a target type ("Self" or "Enemy")
    /// which determines whether the card requires the player to select an enemy target or applies its effect immediately.
    /// 
    /// Cards are organized into three position arrays (pos1, pos2, pos3) in GameElements, and one card from each position
    /// is randomly selected each turn. When clicked, the card sets its PosSelectedCard flag to true, which GameElements
    /// checks during the SelectCard phase to determine which card the player chose.
    /// </summary>
    internal class Card : GameObject
    {
        protected string Name; // The name/type of the card (e.g., "DamageCard", "HealCard", "DebuffCard"), used by GameElements to determine which effect to apply.
        bool posSelectedCard = false; // Flag that is set to true when this card is clicked. GameElements reads this flag to identify which card in that position was selected, then resets it to false.
        string Target; // Determines the targeting mode: "Self" means the card applies its effect immediately (e.g., healing), "Enemy" means the player must click an enemy to apply the card's effect.

        /// <summary>
        /// Constructs a new Card with the given name, target type, texture, position, click action, and color states.
        /// The onClick action is typically a lambda that sets this card's PosSelectedCard to true, defined in GameElements.LoadContentGE.
        /// </summary>
        public Card(string name, string target, Texture2D texture, Rectangle rect, Action onClick, Color normalColor, Color hoverColor) : base(texture, rect, onClick, normalColor, hoverColor)
        {
            this.Name = name;
            this.Target = target;
        }

        /// <summary>
        /// Gets the name of the card (e.g., "DamageCard", "HealCard", "DebuffCard").
        /// Used by GameElements in the card system switch statements to determine which effect to apply.
        /// </summary>
        public string name
        {
            get { return Name; }
        }

        /// <summary>
        /// Gets the target type of the card ("Self" or "Enemy").
        /// Used by GameElements to decide whether to immediately apply the card effect or transition to the SelectEnemy phase.
        /// </summary>
        public string target
        {
            get { return Target; }
        }

        /// <summary>
        /// Gets or sets whether this card has been selected by the player in its position slot.
        /// Set to true by the card's onClick action when clicked, and reset to false by GameElements after the selection is processed.
        /// </summary>
        public bool PosSelectedCard
        {
            get { return posSelectedCard; }
            set { posSelectedCard = value; }
        }
    }
}