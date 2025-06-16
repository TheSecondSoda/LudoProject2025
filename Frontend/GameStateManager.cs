using Common.Enums;
using Frontend.FactoryPattern;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend
{
    public enum GameState
    {
        Playing,
        Start,
        SetupGame,
    }

    public class GameStateManager
    {
        // currentState gemmer spillets nuværende stadie.
        public GameState currentState;

        //public SpriteFont spriteFont { get; private set; }

        // constructoren der initializer stadiet som spillet starter i.

      
        public GameStateManager()
        {
            currentState = GameState.Start;
        }

        List<Color> playerColors = new List<Color>();

        public Color currentColor = Color.White;

        
        //ChangeGameState tillader at man kan ændre på stadiet fx Menu til Playing, eller omvendt.
        public void ChangeGameState(GameState newState)
        {
            currentState = newState;
            Loadcontent();
        }

        public void Loadcontent()
        {
            switch (currentState)
            {

                case GameState.Start:

                    foreach (var go in GameWorld.Instance.gameObjects_MainMenu)
                    {
                        go.Start();
                    }
                    foreach (var item in GameWorld.Instance.buttons_MainMenu)
                    {
                        item.LoadContent();
                    }
                    break;

                case GameState.SetupGame:
                    
                    foreach (var go in GameWorld.Instance.gameObjects_SetupGame)
                    {
                        go.Start();
                    }


                    foreach (var item in GameWorld.Instance.buttons_SetupGame)
                    {
                        item.LoadContent();
                    }

                    break;

                case GameState.Playing:

                    GameWorld.Instance.MakeClassicTileMap();

                    foreach (GameObject go in GameWorld.Instance.gameObjects_Playing)
                    {
                        go.Start();
                    }

                    foreach (var go in GameWorld.Instance.tiles)
                    {
                        go.Start();
                    }

                    foreach (var go in GameWorld.Instance.colorTiles)
                    {
                        foreach (var item in go.Value)
                        {
                            item.Start();
                        } 
                    }

                    foreach (var go in GameWorld.Instance.homeTiles)
                    {
                        foreach (var item in go.Value)
                        {
                            item.Start();
                        }
                    }

                    foreach (var item in GameWorld.Instance.buttons_Playing)
                    {
                        item.LoadContent();
                    }

                    foreach (var go in GameWorld.Instance.playerPieces)
                    {
                        foreach (var item in go.Value)
                        {
                            item.Start();
                        }
                    }

                    foreach (ColourEnum item in GameWorld.Instance.playerColors.Values)
                    {
                      
                        switch (item)
                        {
                            case ColourEnum.None:
                                playerColors.Add(Color.White);
                                break;
                            case ColourEnum.Red:
                                playerColors.Add(Color.Red);
                                break;
                            case ColourEnum.Blue:
                                playerColors.Add(Color.Blue);
                                break;
                            case ColourEnum.Green:
                                playerColors.Add(Color.Green);
                                break;
                            case ColourEnum.Yellow:
                                playerColors.Add(Color.Yellow);
                                break;
                            default:
                                break;
                        }


                    }
                  

                    break;

            }
        }

        public void Update(GameTime gameTime)
        {
            switch (currentState)
            {
                case GameState.Start:

                    foreach (var go in GameWorld.Instance.gameObjects_MainMenu)
                    {
                        go.Update(gameTime);
                    }

                    foreach (var item in GameWorld.Instance.buttons_MainMenu)
                    {
                        item.Update();
                    }
                    break;


                case GameState.SetupGame:

                    foreach (var go in GameWorld.Instance.gameObjects_SetupGame)
                    {
                        go.Update(gameTime);
                    }


                    foreach (var item in GameWorld.Instance.buttons_SetupGame)
                    {
                        item.Update();
                    }

                    break;

                case GameState.Playing:
                    for (int i = 0; i < GameWorld.Instance.gameObjects_Playing.Count; i++)
                    {
                        GameWorld.Instance.gameObjects_Playing[i].Update(gameTime);
                    }

                    foreach (var go in GameWorld.Instance.tiles)
                    {
                        go.Update(gameTime);
                    }

                    foreach (var go in GameWorld.Instance.colorTiles)
                    {
                        foreach (var item in go.Value)
                        {
                            item.Update(gameTime);
                        }
                    }

                    foreach (var go in GameWorld.Instance.homeTiles)
                    {
                        foreach (var item in go.Value)
                        {
                            item.Update(gameTime);
                        }
                    }

                    foreach (var item in GameWorld.Instance.buttons_Playing)
                    {
                        item.Update();
                    }

                    foreach (var go in GameWorld.Instance.playerPieces)
                    {
                        foreach (var item in go.Value)
                        {
                            item.Update(gameTime);
                        }
                    }
                    break;

            }
            GameWorld.Instance.Cleanup();
        }

        // Draw metoden Draw alt der skal ind i det nuværende stadie
        public void Draw(SpriteBatch spriteBatch)
        {
            switch (currentState)
            {

                case GameState.Start:

                    foreach (var go in GameWorld.Instance.gameObjects_MainMenu)
                    {
                        go.Draw(spriteBatch);
                    }


                    foreach (var item in GameWorld.Instance.buttons_MainMenu)
                    {
                        item.Draw(spriteBatch);
                    }
                    break;



                case GameState.SetupGame:

                    foreach (var go in GameWorld.Instance.gameObjects_SetupGame)
                    {
                        go.Draw(spriteBatch);
                    }

                    foreach (var item in GameWorld.Instance.buttons_SetupGame)
                    {
                        item.Draw(spriteBatch);
                    }

                    break;
                // Alt draw kode skal ind her
                case GameState.Playing:

                    switch (playerColors.Count)
                    {
                        case 1:
                            spriteBatch.Draw(GameWorld.Instance.box, new Vector2(20, 130), null, playerColors[0], 0, new Vector2(0, 0), new Vector2(0.8f, 0.8f), SpriteEffects.None, 0);

                            break;

                        case 2:
                            spriteBatch.Draw(GameWorld.Instance.box, new Vector2(20, 130), null, playerColors[0], 0, new Vector2(0, 0), new Vector2(0.8f, 0.8f), SpriteEffects.None, 0);

                            spriteBatch.Draw(GameWorld.Instance.box, new Vector2(665, 130), null, playerColors[1], 0, new Vector2(0, 0), new Vector2(0.8f, 0.8f), SpriteEffects.None, 0);

                            break;
                        case 3:
                            spriteBatch.Draw(GameWorld.Instance.box, new Vector2(20, 130), null, playerColors[0], 0, new Vector2(0, 0), new Vector2(0.8f, 0.8f), SpriteEffects.None, 0);

                            spriteBatch.Draw(GameWorld.Instance.box, new Vector2(665, 130), null, playerColors[1], 0, new Vector2(0, 0), new Vector2(0.8f, 0.8f), SpriteEffects.None, 0);
                            spriteBatch.Draw(GameWorld.Instance.box, new Vector2(20, 655), null, playerColors[2], 0, new Vector2(0, 0), new Vector2(0.8f, 0.8f), SpriteEffects.None, 0);

                            break;
                        case 4:
                            spriteBatch.Draw(GameWorld.Instance.box, new Vector2(20, 130), null, playerColors[0], 0, new Vector2(0, 0), new Vector2(0.8f, 0.8f), SpriteEffects.None, 0);

                            spriteBatch.Draw(GameWorld.Instance.box, new Vector2(665, 130), null, playerColors[1], 0, new Vector2(0, 0), new Vector2(0.8f, 0.8f), SpriteEffects.None, 0);
                            spriteBatch.Draw(GameWorld.Instance.box, new Vector2(665, 655), null, playerColors[2], 0, new Vector2(0, 0), new Vector2(0.8f, 0.8f), SpriteEffects.None, 0);

                            spriteBatch.Draw(GameWorld.Instance.box, new Vector2(20, 655), null, playerColors[3], 0, new Vector2(0, 0), new Vector2(0.8f, 0.8f), SpriteEffects.None, 0);


                            break;
                        default:
                            break;
                    }



                    spriteBatch.Draw(GameWorld.Instance.box, new Vector2(1165, 465), null, currentColor, 0, new Vector2(0, 0), new Vector2(0.2f, 0.2f), SpriteEffects.None, 0);




                    foreach (GameObject go in GameWorld.Instance.gameObjects_Playing)
                    {
                        go.Draw(spriteBatch);
                    }

                    foreach (var go in GameWorld.Instance.tiles)
                    {
                        go.Draw(spriteBatch);
                    }

                    foreach (var go in GameWorld.Instance.colorTiles)
                    {
                        foreach (var item in go.Value)
                        {
                            item.Draw(spriteBatch);
                        }
                    }

                    foreach (var go in GameWorld.Instance.homeTiles)
                    {
                        foreach (var item in go.Value)
                        {
                            item.Draw(spriteBatch);
                        }
                    }


                    foreach (var item in GameWorld.Instance.buttons_Playing)
                    {
                        item.Draw(spriteBatch);
                    }

                    foreach (var go in GameWorld.Instance.playerPieces)
                    {
                        foreach (var item in go.Value)
                        {
                            item.Draw(spriteBatch);
                        }
                    }

                    break;

            }
        }

       
    }

}
