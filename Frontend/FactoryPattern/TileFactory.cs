using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Common.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Frontend.FactoryPattern
{
 
    public class TileFactory : Factory
    {

        private static TileFactory instance;
        public static TileFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TileFactory();

                }
                return instance;
            }

        }
        private TileFactory()
        {

        }

        private float tileScale = 0.2f;

        public override GameObject Create(Enum type,ContentManager content)
        {
            GameObject gameObject = new GameObject();
            SpriteRenderer sr = (SpriteRenderer)gameObject.AddComponent(new SpriteRenderer());

            switch (type)
            {
                case ColourEnum.Green:
                    sr.SetSprite("Box", 0, tileScale, Color.Green,content);
                    break;

                case ColourEnum.Blue:
                    sr.SetSprite("Box", 0, tileScale,Color.Blue,content);
                 break;

                case ColourEnum.Red:
                    sr.SetSprite("Box", 0, tileScale, Color.Red,content);
                    break;

                case ColourEnum.Yellow:
                    sr.SetSprite("Box", 0, tileScale, Color.Yellow, content);
                    break;

                case ColourEnum.None:
                    sr.SetSprite("Box", 0, tileScale, Color.White,content);
                    break;

            }
        

           



            


            return gameObject;
            
        }
    }
}
