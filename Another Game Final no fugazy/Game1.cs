using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Xml.Linq;

namespace Another_Game_Final_no_fugazy
{
    /// <summary>
    /// The main game class that serves as the entry point and MonoGame loop controller for the application.
    /// Inherits from MonoGame's Game class and delegates all game-specific logic to the static GameElements class.
    /// 
    /// Game1 is responsible for:
    /// - Setting up the game window (1280x720 resolution) and graphics device.
    /// - Calling GameElements.InitializeGE() during initialization to set up the initial game state.
    /// - Calling GameElements.LoadContentGE() during content loading to load all textures, fonts, and game objects.
    /// - Calling GameElements.MASTER_UpdateGE() each frame to update the active game state's logic.
    /// - Calling GameElements.MASTER_DrawGE() each frame to render the active game state's visuals.
    /// 
    /// The actual game logic (menus, gameplay, high scores, etc.) is entirely managed by GameElements,
    /// keeping Game1 as a thin wrapper around the MonoGame framework lifecycle.
    /// </summary>
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics; // Manages the graphics device settings (resolution, fullscreen, etc.) for the game window.
        private SpriteBatch _spriteBatch; // Used for batching 2D draw calls. Passed to GameElements.MASTER_DrawGE() for rendering all game visuals.

        /// <summary>
        /// Constructs the Game1 instance, initializes the graphics manager, sets the window resolution to 1280x720,
        /// and configures the content root directory for asset loading.
        /// </summary>
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);

            _graphics.PreferredBackBufferWidth = 1280; // Set the game window width to 1280 pixels
            _graphics.PreferredBackBufferHeight = 720; // Set the game window height to 720 pixels
            _graphics.ApplyChanges(); // Apply the resolution settings to the graphics device

            Content.RootDirectory = "Content"; // Set the root directory for the content pipeline (where all textures, fonts, etc. are loaded from)
            IsMouseVisible = true; // Make the mouse cursor visible since the game uses mouse input for all interactions (clicking buttons, cards, enemies)
        }


        /// <summary>
        /// Called once at startup. Initializes the game state by calling GameElements.InitializeGE(),
        /// which sets the initial state to Menu, creates the background manager, and sets the starting wave.
        /// </summary>
        protected override void Initialize()
        {
            GameElements.InitializeGE(); // Initialize all game state variables (current state, background, play phase, wave number)

            base.Initialize(); // Call the base MonoGame initialization
        }


        /// <summary>
        /// Called once after Initialize. Creates the SpriteBatch and loads all game content by calling
        /// GameElements.LoadContentGE(), which loads all textures, fonts, creates all game objects
        /// (cards, buttons, enemies, player, backgrounds, health bars, etc.), and spawns the first wave.
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice); // Create the SpriteBatch used for all 2D rendering

            GameElements.LoadContentGE(Content, Window, GraphicsDevice); // Load all game assets and initialize all game objects
        }

        /// <summary>
        /// Called when the game is exiting. Currently empty — no custom unload logic is needed since
        /// MonoGame handles disposing of content manager resources automatically.
        /// </summary>
        protected override void UnloadContent()
        {
        }



        /// <summary>
        /// Called once per frame. Checks for gamepad Back button or Escape key to exit the game,
        /// then delegates all game-state-specific update logic to GameElements.MASTER_UpdateGE().
        /// </summary>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit(); // Exit the game if the gamepad Back button or Escape key is pressed

            GameElements.MASTER_UpdateGE(gameTime); // Delegate to GameElements, which routes to the appropriate state-specific update method (Menu, Play, Instructions, HighScore)


            base.Update(gameTime); // Call the base MonoGame update
        }


        /// <summary>
        /// Called once per frame after Update. Clears the screen, begins a SpriteBatch draw session,
        /// delegates all rendering to GameElements.MASTER_DrawGE(), and ends the draw session.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue); // Clear the screen with a default blue color (will be covered by the background)
            _spriteBatch.Begin(); // Begin the SpriteBatch session for batching all 2D draw calls

            GameElements.MASTER_DrawGE(_spriteBatch); // Delegate to GameElements, which routes to the appropriate state-specific draw method (Menu, Play, Instructions, HighScore)


            _spriteBatch.End(); // End the SpriteBatch session and flush all batched draw calls to the screen
            base.Draw(gameTime); // Call the base MonoGame draw
        }
    }
}
