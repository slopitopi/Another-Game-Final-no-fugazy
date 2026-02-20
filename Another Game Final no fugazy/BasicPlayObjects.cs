using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Another_Game_Final_no_fugazy
{
    /// <summary>
    /// Base class for all interactive game objects that can be drawn, hovered over, and clicked.
    /// Provides core functionality for texture rendering with color tinting, mouse hover detection,
    /// and click handling using a press-then-release pattern.
    /// 
    /// Subclasses include:
    /// - Button: UI navigation buttons (menu buttons, back button).
    /// - Card: Playable cards in the player's hand (damage, heal, debuff).
    /// - CombatEntity: Abstract base for combat participants (Player, EnemyBrawler, EnemyMage, EnemyHealer),
    ///   which further overrides Update for custom input handling.
    /// 
    /// The click detection uses a two-step approach: the mouse must first be pressed while hovering (wasPressed = true),
    /// then released while still in bounds to trigger the onClick action. This prevents accidental clicks from
    /// dragging the mouse onto a button and also prevents repeated triggers from holding the mouse button down.
    /// </summary>
    internal class GameObject
    {


        //--------------------------------Game Object Properties--------------------------------//
        protected Texture2D texture; // The texture (image) drawn for this object on screen.
        protected Rectangle rect; // The screen rectangle defining this object's position and size. Also used as the bounding box for mouse hover/click detection.
        protected Color normalColor; // The tint color applied when the mouse is NOT hovering over this object.
        protected Color hoverColor; // The tint color applied when the mouse IS hovering over this object, providing visual feedback.
        protected Color currentColor; // The currently active tint color (either normalColor or hoverColor), updated each frame in Update().
        protected Action onClick; // The action (lambda/delegate) invoked when this object is clicked. Set during construction or via SetOnClick(). Can be null for non-interactive objects (e.g., the player sprite).
        protected bool wasPressed; // Tracks whether the mouse was pressed while hovering over this object. Used for the press-then-release click detection pattern.
        //--------------------------------Game Object Properties END--------------------------------//

        /// <summary>
        /// Constructs a new GameObject with the given texture, position, click action, and color states.
        /// Initializes the current color to the normal (non-hover) color and sets wasPressed to false.
        /// </summary>
        /// <param name="texture">The texture to render for this object.</param>
        /// <param name="rect">The screen rectangle for position, size, and click/hover bounds.</param>
        /// <param name="onClick">The action to invoke on click. Can be null for non-interactive objects.</param>
        /// <param name="normalColor">The tint color when not hovered.</param>
        /// <param name="hoverColor">The tint color when hovered.</param>
        public GameObject(Texture2D texture, Rectangle rect, Action onClick, Color normalColor, Color hoverColor)
        {
            this.texture = texture;
            this.rect = rect;
            this.onClick = onClick;
            this.normalColor = normalColor;
            this.hoverColor = hoverColor;
            this.currentColor = this.normalColor; // Start with the normal (non-hover) color
            this.wasPressed = false; // No press tracked initially
        }



        /// <summary>
        /// Updates this object's hover state and handles click detection based on the current mouse state.
        /// 
        /// Hover logic: If the mouse cursor is within the object's rectangle, apply the hover color; otherwise, apply the normal color.
        /// 
        /// Click logic (press-then-release pattern):
        /// 1. If hovering and left mouse button is pressed → mark wasPressed as true.
        /// 2. If wasPressed is true and left mouse button is released → invoke onClick and reset wasPressed.
        /// 3. If the mouse moves outside the rectangle → reset wasPressed (cancel the click).
        /// 
        /// Called once per frame by GameElements update methods for active objects (menu buttons, cards, enemies).
        /// Subclasses (e.g., Player) may override this to disable input handling.
        /// </summary>
        public virtual void Update(MouseState mouseState)
        {
            bool isHovering = rect.Contains(mouseState.X, mouseState.Y); // Check if the mouse cursor is within this object's bounding rectangle


            if (isHovering) // Apply the appropriate color based on hover state
            {
                currentColor = hoverColor;
            }
            else
            {
                currentColor = normalColor;
            }



            if (isHovering && mouseState.LeftButton == ButtonState.Pressed) // Step 1: Mouse is hovering and pressed — start tracking the click
            {
                wasPressed = true;
            }

            else if (wasPressed && mouseState.LeftButton == ButtonState.Released) // Step 2: Mouse was previously pressed and is now released — trigger the click action
            {
                wasPressed = false;
                onClick.Invoke(); // Execute the click action (e.g., select a card, navigate to a different game state)
            }

            else if (!isHovering) // Step 3: Mouse moved outside the rectangle — cancel the click tracking
            {
                wasPressed = false;
            }
        }

        /// <summary>
        /// Draws this object's texture at its rectangle position with the current tint color (normal or hover).
        /// Called once per frame by GameElements draw methods for active objects.
        /// </summary>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rect, currentColor);
        }

        /// <summary>
        /// Replaces this object's onClick action with a new one. Used by GameElements.SpawnEnemys() to set
        /// enemy click handlers after construction, since the lambda needs to capture a reference to the
        /// fully-constructed enemy instance (which isn't available during the constructor call).
        /// </summary>
        /// <param name="action">The new action to invoke when this object is clicked.</param>
        public void SetOnClick(Action action)
        {
            onClick = action;
        }


    }
}