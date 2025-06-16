using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.ComponentPattern
{
    public class Animator : Component
    {
        public int CurrentIndex { get; private set; }

        public float timeElapsed;

        private SpriteRenderer spriteRenderer;

        private Dictionary<string, Animation> animations = new Dictionary<string, Animation>();

        private Animation currentAnimation;
        public bool animationPaused;

        public override void Start()
        {
            spriteRenderer = (SpriteRenderer)GameObject.GetComponent<SpriteRenderer>();
            
        }



        public override void Update()
        {
            if (animationPaused == false)
            {
                timeElapsed += GameWorld.Instance.deltaTime;

                CurrentIndex = (int)(timeElapsed * currentAnimation.FPS);

                if (CurrentIndex > currentAnimation.Sprites.Length - 1)
                {
                    timeElapsed = 0;
                    CurrentIndex = 0;
                }

                spriteRenderer.Sprite = currentAnimation.Sprites[CurrentIndex];
            }
            base.Update();


        }



        public void AddAnimation(Animation animation)
        {
            animations.Add(animation.Name, animation);

            if (currentAnimation == null)
            {
                currentAnimation = animation;
            }
        }

        public void PlayAnimation(string animationName)
        {
            animationPaused = false;
            if (animationName != currentAnimation.Name)
            {
                currentAnimation = animations[animationName];
                timeElapsed = 0;
                CurrentIndex = 0;
            }
        }

        public void StopAnimationAndSetSprite(Texture2D sprite)
        {
            animationPaused = true;

            spriteRenderer.Sprite = sprite;


        }
    }
}
