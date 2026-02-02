using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Another_Game_Final_no_fugazy
{
    internal abstract class BackgroundAsset
    {
            public Texture2D[] Textures { get; }
            public bool IsAnimated { get; }
            public float FrameDuration { get; }

            public BackgroundAsset(Texture2D[] textures, bool isAnimated = false, float frameDuration = 0.1f)
            {
                Textures = textures;
                IsAnimated = isAnimated && textures.Length > 1;
                FrameDuration = frameDuration;
            }
    }
}