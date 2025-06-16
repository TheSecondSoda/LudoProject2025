using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Frontend.ComponentPattern.Animation;

namespace Frontend.ComponentPattern
{
    public class Animation : Animator
    {
        public float FPS { get; private set; }
        public string Name { get; private set; }
        public Texture2D[] Sprites { get; set; }

        public Animation(string name, Texture2D[] sprites, float fps)
        {
            Name = name;
            Sprites = sprites;
            FPS = fps;
        }
    }

}
