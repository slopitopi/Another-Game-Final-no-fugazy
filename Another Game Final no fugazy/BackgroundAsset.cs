using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Another_Game_Final_no_fugazy
{
    /// <summary>
    /// A simple data container class that holds the texture(s) and animation settings for a single background.
    /// Used by the Background class to store and retrieve background configurations for each game state.
    /// 
    /// For animated backgrounds (e.g., the menu), Textures contains multiple frames and IsAnimated is true.
    /// For static backgrounds (e.g., play, instructions, highscore), Textures contains a single texture and IsAnimated is false.
    /// 
    /// Instances are created in GameElements.LoadContentGE() and registered with the Background class via AddBackground().
    /// </summary>
    internal class BackgroundAsset
    {
        public Texture2D[] Textures { get; } // Array of textures representing the background frames. For animated backgrounds, this contains multiple frames; for static backgrounds, a single texture.
        public bool IsAnimated { get; } // Whether this background should cycle through its textures as an animation. Only true if isAnimated was set to true AND there are at least 2 textures.
        public float FrameDuration { get; } // Time (in seconds) each frame is displayed before advancing to the next. Only relevant for animated backgrounds. Defaults to 0.1 seconds.

        /// <summary>
        /// Constructs a new BackgroundAsset with the given textures and animation settings.
        /// IsAnimated is automatically set to false if the textures array is null or has fewer than 2 entries,
        /// even if isAnimated is passed as true — a single-frame background cannot be animated.
        /// </summary>
        /// <param name="textures">Array of textures for the background. Multiple textures for animation, single for static.</param>
        /// <param name="isAnimated">Whether this background should animate through its textures.</param>
        /// <param name="frameDuration">Duration in seconds for each animation frame. Defaults to 0.1 seconds.</param>
        public BackgroundAsset(Texture2D[] textures, bool isAnimated = false, float frameDuration = 0.1f)
        {
            Textures = textures;
            IsAnimated = isAnimated && textures != null && textures.Length > 1; // Only allow animation if there are at least 2 frames to cycle through
            FrameDuration = frameDuration;
        }
    }
}