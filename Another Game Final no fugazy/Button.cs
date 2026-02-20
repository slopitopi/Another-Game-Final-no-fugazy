using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Another_Game_Final_no_fugazy
{
    /// <summary>
    /// Represents a clickable UI button in the game. Inherits all behavior from GameObject, including
    /// texture rendering, hover color changes, and click detection.
    /// 
    /// Button is a thin wrapper around GameObject that provides no additional functionality — all click/hover
    /// logic is handled by the base class. It exists as a separate class for semantic clarity, distinguishing
    /// UI buttons from other GameObjects like cards and enemies.
    /// 
    /// Button instances are created in GameElements.LoadContentGE() for menu navigation (Play, HighScore,
    /// Instructions, Quit) and for the back button used in the Instructions and HighScore screens.
    /// Each button's onClick action is a lambda that changes the game state (e.g., switching to Play or Menu).
    /// </summary>
    internal class Button : GameObject
    {

        /// <summary>
        /// Constructs a new Button with the given texture, position, click action, and color states.
        /// Delegates all initialization to the base GameObject constructor.
        /// </summary>
        /// <param name="texture">The texture to display for the button.</param>
        /// <param name="rect">The screen rectangle defining the button's position and size (also used for click/hover detection).</param>
        /// <param name="onClick">The action to invoke when the button is clicked (e.g., changing the game state).</param>
        /// <param name="normalColor">The tint color when the mouse is not hovering over the button.</param>
        /// <param name="hoverColor">The tint color when the mouse is hovering over the button.</param>
        public Button(Texture2D texture, Rectangle rect, Action onClick, Color normalColor, Color hoverColor) : base(texture, rect, onClick, normalColor, hoverColor)
        {
        }
    }
}
