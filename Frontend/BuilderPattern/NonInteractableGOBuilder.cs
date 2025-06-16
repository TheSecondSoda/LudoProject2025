using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace Frontend.BuilderPattern
{
    internal class NonInteractableGOBuilder : IBuilder
    {
        string spriteName;
        float scale;
        Vector2 pos;
        Color color;
        public NonInteractableGOBuilder(string spriteName,float scale,Vector2 position,Color color)
        {
            this.spriteName = spriteName;
            this.scale = scale;
            this.pos = position;
            this.color = color;
        }
        public GameObject BuildGameObject()
        {
            GameObject GO = new GameObject();

            SpriteRenderer sr=(SpriteRenderer)GO.AddComponent(new SpriteRenderer());
            sr.SetSprite(spriteName,0,scale,color,GameWorld.Instance.Content);

            GO.Transform.Position = pos;
            return GO;
        }
    }
}
