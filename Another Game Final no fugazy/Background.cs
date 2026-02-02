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
    internal class Background: BackgroundAsset
    {
        private readonly Dictionary<GameElements.State, BackgroundAssets> _assets;
        private BackgroundAssets _currentAssets;
        private float _elapsedTime;
        private int _currentFrame;

        public Background() : base(null)
        {
            _assets = new Dictionary<GameElements.State, BackgroundAssets>();
        }

        public void LoadAllBackgrounds(ContentManager content)
        {
            // Pre-load all backgrounds for all states
            _assets[GameElements.State.Menu] = new BackgroundAssets(
                textures: new[] { content.Load<Texture2D>("images/menu/MenBackground") },
                isAnimated: true
            );
            //_assets[GameElements.State.Play] = new BackgroundAssets(
            //    textures: new[] { content.Load<Texture2D>("images/Play/bg1") },
            //    isAnimated: false
            //);
            //_assets[GameElements.State.Instructions] = new BackgroundAssets(
            //    textures: new[] { content.Load<Texture2D>("images/Instructions/bg1") },
            //    isAnimated: false
            //);
            //_assets[GameElements.State.HighScore] = new BackgroundAssets(
            //    textures: new[] { content.Load<Texture2D>("images/HighScore/bg1") },
            //    isAnimated: false
            //);
        }



        public void SetState(GameElements.State state)
        {
            if (_assets.TryGetValue(state, out var assets))
            {
                _currentAssets = assets;
                _currentFrame = 0;
                _elapsedTime = 0f;
            }
        }



        public void Update(GameTime gameTime)
        {
            if (_currentAssets == null || !_currentAssets.IsAnimated)
                return;

            _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_elapsedTime < _currentAssets.FrameDuration)
                return;

            _currentFrame = (_currentFrame + 1) % _currentAssets.Textures.Length;
            _elapsedTime = 0f;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_currentAssets?.Textures == null || _currentAssets.Textures.Length == 0)
                return;

            var texture = _currentAssets.IsAnimated
                ? _currentAssets.Textures[_currentFrame]
                : _currentAssets.Textures[0];

            spriteBatch.Draw(texture, new Rectangle(0, 0, 1280, 720), Color.White);
        }

        private class BackgroundAssets
        {
            public Texture2D[] Textures { get; }
            public bool IsAnimated { get; }
            public float FrameDuration { get; }

            public BackgroundAssets(Texture2D[] textures, bool isAnimated = false, float frameDuration = 0.1f)
            {
                Textures = textures;
                IsAnimated = isAnimated && textures.Length > 1;
                FrameDuration = frameDuration;
            }
        }
    }
}
