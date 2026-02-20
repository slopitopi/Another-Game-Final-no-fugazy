using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
    /// Manages the game's background rendering for all game states (Menu, Play, Instructions, HighScore).
    /// Supports both animated backgrounds (cycling through multiple texture frames at a configurable speed)
    /// and static backgrounds (a single texture).
    /// 
    /// Each game state can have its own BackgroundAsset registered via AddBackground(). When the game state changes,
    /// GameElements calls SetState() to switch the active background, resetting the animation frame and timer.
    /// The Update() method advances animated backgrounds frame by frame, and Draw() renders the current frame
    /// stretched to fill the full 1280x720 game window.
    /// 
    /// Backgrounds are configured and registered in GameElements.LoadContentGE().
    /// </summary>
    internal class Background
    {
        private Dictionary<GameElements.State, BackgroundAsset> _assets; // Maps each game state to its corresponding BackgroundAsset (textures and animation settings).
        private BackgroundAsset _currentAsset; // The currently active BackgroundAsset, set by SetState() when the game state changes.
        private float _elapsedTime; // Accumulates elapsed time (in seconds) since the last animation frame change. Reset when a new frame is displayed.
        private int _currentFrame; // Index of the current animation frame in the active BackgroundAsset's texture array. Wraps around using modulo.

        /// <summary>
        /// Constructs a new Background manager with an empty dictionary of background assets.
        /// Assets are added later via AddBackground() during GameElements.LoadContentGE().
        /// </summary>
        public Background()
        {
            _assets = new Dictionary<GameElements.State, BackgroundAsset>();
        }



        /// <summary>
        /// Registers a BackgroundAsset for a specific game state. Called from GameElements.LoadContentGE()
        /// to associate each state (Menu, Play, Instructions, HighScore) with its background textures and animation settings.
        /// </summary>
        /// <param name="state">The game state this background is associated with.</param>
        /// <param name="asset">The BackgroundAsset containing textures and animation configuration.</param>
        public void AddBackground(GameElements.State state, BackgroundAsset asset)
        {
            _assets[state] = asset;
        }



        /// <summary>
        /// Switches the active background to the one associated with the given game state.
        /// Resets the animation frame to 0 and the elapsed timer to 0 so the animation starts fresh.
        /// Called by GameElements.MASTER_UpdateGE() whenever the game state changes.
        /// </summary>
        /// <param name="state">The new game state to set the background for.</param>
        public void SetState(GameElements.State state)
        {
            if (_assets.TryGetValue(state, out var asset)) // Look up the asset for the given state; if found, activate it
            {
                _currentAsset = asset;
                _currentFrame = 0; // Start from the first frame
                _elapsedTime = 0f; // Reset the animation timer
            }
        }

        /// <summary>
        /// Advances the animation frame if the current background is animated. Accumulates elapsed time each frame
        /// and advances to the next texture frame when the frame duration is exceeded. Non-animated backgrounds
        /// are not affected.
        /// Called once per frame by GameElements update methods (Menu_UpdateGE, Play_UpdateGE, etc.).
        /// </summary>
        public void Update(GameTime gameTime)
        {
            if (_currentAsset == null || !_currentAsset.IsAnimated) // Skip update if there's no asset or it's not animated
                return;

            _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds; // Accumulate time since last frame change

            if (_elapsedTime < _currentAsset.FrameDuration) // Don't advance the frame until enough time has passed
                return;

            _currentFrame = (_currentFrame + 1) % _currentAsset.Textures.Length; // Advance to the next frame, wrapping around to 0 at the end of the array
            _elapsedTime = 0f; // Reset the timer for the next frame interval
        }

        /// <summary>
        /// Draws the current background frame stretched to fill the entire 1280x720 game window.
        /// For animated backgrounds, draws the current frame based on _currentFrame index.
        /// For static backgrounds, always draws the first (and only) texture.
        /// Called once per frame by GameElements draw methods (Menu_DrawGE, Play_DrawGE, etc.).
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (_currentAsset?.Textures == null || _currentAsset.Textures.Length == 0) // Safety check: skip drawing if no textures are loaded
                return;

            // Select the appropriate texture: current animation frame for animated backgrounds, first texture for static ones
            var texture = _currentAsset.IsAnimated
                ? _currentAsset.Textures[_currentFrame]
                : _currentAsset.Textures[0];

            spriteBatch.Draw(texture, new Rectangle(0, 0, 1280, 720), Color.White); // Draw the texture stretched to fill the full game window
        }
    }
}
