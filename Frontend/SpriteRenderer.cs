using Frontend.ComponentPattern;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend
{
    public class SpriteRenderer : Component
    {
        private Texture2D pixel;
        private Rectangle rectangle;

        public Texture2D Sprite { get; set; }

        public float Rotate { get; set; }
        public float Scale { get; set; }

        public Color Color { get; set; }

        public Vector2 Origin { get; set; }

        public bool ShowRectangle { get; set; }


        public override void Start()
        {
            pixel = GameWorld.Instance.Content.Load<Texture2D>("pixel");
            ShowRectangle = false;
        
        }

        public void SetSprite(string spriteName, float rotate, float scale,Color color,ContentManager content)
        {
            Sprite = content.Load<Texture2D>(spriteName);
            Rotate = rotate;
            Scale = scale;
            this.Color = color;

            Origin = new Vector2(Sprite.Width / 2, Sprite.Height/ 2);
        }




        public override void Draw(SpriteBatch spriteBatch)
        {
            //this is to draw the sprites
            spriteBatch.Draw(Sprite, GameObject.Transform.Position, null, Color, Rotate, Origin, Scale, SpriteEffects.None, 1);
            

            if(ShowRectangle == true)
            {   
                rectangle = new Rectangle((int)(GameObject.Transform.Position.X - Sprite.Width*Scale / 2), (int)(GameObject.Transform.Position.Y - Sprite.Height*Scale / 2), (int)(Sprite.Width * Scale), (int)(Sprite.Height * Scale));
                DrawRectangle(rectangle, spriteBatch,10);
            }
        }

        private void DrawRectangle(Rectangle collisionBox, SpriteBatch spriteBatch,int lineThickness)
        {
            Rectangle topLine = new Rectangle(collisionBox.X, collisionBox.Y, collisionBox.Width, lineThickness);
            Rectangle bottomLine = new Rectangle(collisionBox.X, collisionBox.Y + collisionBox.Height, collisionBox.Width, lineThickness);
            Rectangle rightLine = new Rectangle(collisionBox.X + collisionBox.Width, collisionBox.Y, lineThickness, collisionBox.Height);
            Rectangle leftLine = new Rectangle(collisionBox.X, collisionBox.Y, lineThickness, collisionBox.Height);

            spriteBatch.Draw(pixel, topLine, null, Color.Black, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(pixel, bottomLine, null, Color.Black, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(pixel, rightLine, null, Color.Black, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(pixel, leftLine, null, Color.Black, 0, Vector2.Zero, SpriteEffects.None, 1);
        }


    }
}
