using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Another_Game_Final_no_fugazy
{

    //Purly for the dictionary to function
    internal class BackgroundAsset
    {
        public Texture2D[] Textures { get; }
        public bool IsAnimated { get; }
        public float FrameDuration { get; }

        public BackgroundAsset(Texture2D[] textures, bool isAnimated = false, float frameDuration = 0.1f)
        {
            Textures = textures;
            IsAnimated = isAnimated && textures != null && textures.Length > 1;
            FrameDuration = frameDuration;
        }
    }
}