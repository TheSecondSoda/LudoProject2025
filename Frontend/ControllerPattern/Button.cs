using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace Frontend.ControllerPattern
{
    public class Button
    {
        public IButtonAction buttonAction;
        private string spriteName;
        public Vector2 position;
        private Texture2D sprite;
        private Texture2D pixel;
        private Rectangle rectangle;
        private bool down;
        private Vector2 origin;

        public bool selected = false;

        public List<Button> linkedButtons;

        public Vector2 Scale { get; private set; }

        public Button(string spriteName, IButtonAction buttonAction, Vector2 position,Vector2 scale)
        {
          
            this.spriteName = spriteName;
            this.position = position;
            this.Scale = scale;

            AddButtonAction(buttonAction);
           
        }

        public void DeSelectLinkedButtons()
        {
            foreach (var btn in linkedButtons)
            {
                btn.selected = false; 
            }
        }

        public void AddButtonAction(IButtonAction buttonAction)
        {
            buttonAction.ButtonObject = this;
            this.buttonAction = buttonAction;
        }

        public void Update()
        {

            if (rectangle.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y)) && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {

                down = true;

            }
            if (rectangle.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y)) && Mouse.GetState().LeftButton == ButtonState.Released && down)
            {

                //Debug.Write("down");
                buttonAction.DoAction();
                down = false;
            }


        }

        public void LoadContent()
        {
            sprite = GameWorld.Instance.Content.Load<Texture2D>(spriteName);
            pixel = GameWorld.Instance.Content.Load<Texture2D>("pixel");

            rectangle = new Rectangle((int)(position.X - sprite.Width / 2), (int)(position.Y - sprite.Height / 2), (int)(sprite.Width*Scale.X), (int)(sprite.Height*Scale.Y));
            origin = new Vector2(sprite.Width*Scale.X / 2, sprite.Height*Scale.Y / 2);


            

        }

        public void Draw(SpriteBatch spriteBatch)
        {
           
            spriteBatch.Draw(sprite, position, null, Color.White, 0, origin, Scale, SpriteEffects.None, 1);


            if (selected == true)
            {
                DrawRectangle(rectangle, spriteBatch);
            }
        }


        private void DrawRectangle(Rectangle collisionBox, SpriteBatch spriteBatch)
        {
            Rectangle topLine = new Rectangle(collisionBox.X, collisionBox.Y, collisionBox.Width, 3);
            Rectangle bottomLine = new Rectangle(collisionBox.X, collisionBox.Y + collisionBox.Height, collisionBox.Width, 3);
            Rectangle rightLine = new Rectangle(collisionBox.X + collisionBox.Width, collisionBox.Y, 3, collisionBox.Height);
            Rectangle leftLine = new Rectangle(collisionBox.X, collisionBox.Y, 3, collisionBox.Height);

            spriteBatch.Draw(pixel, topLine, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(pixel, bottomLine, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(pixel, rightLine, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(pixel, leftLine, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1);
        }

    }
}
