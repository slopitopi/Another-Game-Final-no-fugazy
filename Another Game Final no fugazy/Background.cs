using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Another_Game_Final_no_fugazy
{
    internal class Background : BackgroundAsset
    {
        private readonly Dictionary<GameElements.State, BackgroundAsset> _assets;
        private BackgroundAsset _currentAsset;
        private float _elapsedTime;
        private int _currentFrame;

        public Background() : base(null)
        {
            _assets = new Dictionary<GameElements.State, BackgroundAsset>();
        }

        /// <summary>
        /// Adds a background asset for a specific game state.
        /// Called from GameElements during LoadContent.
        /// </summary>
        public void AddBackground(GameElements.State state, BackgroundAsset asset)
        {
            _assets[state] = asset;
        }

        /// <summary>
        /// Sets the current background based on game state.
        /// </summary>
        public void SetState(GameElements.State state)
        {
            if (_assets.TryGetValue(state, out var asset))
            {
                _currentAsset = asset;
                _currentFrame = 0;
                _elapsedTime = 0f;
            }
        }

        /// <summary>
        /// Updates animation frame if the background is animated.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            if (_currentAsset == null || !_currentAsset.IsAnimated)
                return;

            _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_elapsedTime < _currentAsset.FrameDuration)
                return;

            _currentFrame = (_currentFrame + 1) % _currentAsset.Textures.Length;
            _elapsedTime = 0f;
        }

        /// <summary>
        /// Draws the current background frame.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (_currentAsset?.Textures == null || _currentAsset.Textures.Length == 0)
                return;

            var texture = _currentAsset.IsAnimated
                ? _currentAsset.Textures[_currentFrame]
                : _currentAsset.Textures[0];

            spriteBatch.Draw(texture, new Rectangle(0, 0, 1280, 720), Color.White);
        }
    }
}
