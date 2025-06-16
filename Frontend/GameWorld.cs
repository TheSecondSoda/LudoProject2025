using Common.DTOs;
using Common.Enums;
using Frontend.BuilderPattern;
using Frontend.ComponentPattern;
using Frontend.ControllerPattern;
using Frontend.FactoryPattern;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Frontend
{
    public class GameWorld : Game
    {
        private GraphicsDeviceManager _graphics;
        public SpriteBatch _spriteBatch;
        private Texture2D board;
        public Texture2D box;
        public Vector2 screenSize;

        public List<GameObject> gameObjects_Playing;
        public List<GameObject> gameObjects_SetupGame;
        public List<GameObject> gameObjects_MainMenu;
        public List<Button> buttons_MainMenu;
        public List<Button> buttons_SetupGame;
        public List<Button> buttons_Playing;

        private List<Button> newButtons;
        private List<Button> destroyedButtons;

        public List<GameObject> tiles = new List<GameObject>();
        public GameStateManager stateManager;


        private static GameWorld instance;
        private int currentTilesCount;

        private int tileSpacing = 70;

        public Dictionary<Guid,ColourEnum> playerColors;

        public Dictionary<ColourEnum, List<GameObject>> colorTiles;
        public Dictionary<ColourEnum, List<GameObject>> homeTiles;

        public Dictionary<ColourEnum, List<GameObject>> playerPieces;

        private List<GameObject> newGameObjects;
        private List<GameObject> destroyedPieces;

        public Texture2D pixel;
        public float deltaTime;

        public float FPS;

        public int playerAmount;
        private List<PlayerDTO> playerOrderInit;


        public AsyncTaskManager asyncTaskManager;
        public APIService apiService;
        public static GameWorld Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameWorld();

                }
                return instance;
            }

        }

        private GameWorld()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;


            _graphics.PreferredBackBufferHeight = 1000;
            _graphics.PreferredBackBufferWidth = 1800;

            screenSize = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

            stateManager = new GameStateManager();

            gameObjects_MainMenu = new List<GameObject>();

            gameObjects_SetupGame = new List<GameObject>();

            gameObjects_Playing = new List<GameObject>();

            newGameObjects = new List<GameObject>();
            destroyedPieces = new List<GameObject>();

            colorTiles = new Dictionary<ColourEnum, List<GameObject>>();
            homeTiles = new Dictionary<ColourEnum, List<GameObject>>();
            playerPieces=new Dictionary<ColourEnum, List<GameObject>>();

            playerColors = new Dictionary<Guid, ColourEnum>();
           

            buttons_MainMenu = new List<Button>();
            buttons_Playing = new List<Button>();
            buttons_SetupGame = new List<Button>();

            newButtons = new List<Button>();
            destroyedButtons = new List<Button>();

            asyncTaskManager = new AsyncTaskManager();

            apiService = new APIService();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            FPS = 60;
            this.TargetElapsedTime = TimeSpan.FromSeconds(1.0 / FPS);
            buttons_MainMenu.Add(new Button("NewGame", new SetupGameButton(), new Vector2(screenSize.X / 2, screenSize.Y / 2), new Vector2(1, 1)));

            CreateSetupButtons();

            foreach (GameObject go in gameObjects_Playing)
            {
                go.Awake();
            }

            base.Initialize();
        }


        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            // board=Content.Load<Texture2D>("LudoBoard");
            box = Content.Load<Texture2D>("Box");
            pixel = Content.Load<Texture2D>("pixel");
            

            CreateDice();
            stateManager.Loadcontent();
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            stateManager.Update(gameTime);

            asyncTaskManager.Update();
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {


            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);



            stateManager.Draw(_spriteBatch);

            // FOR SHOWCASE USE ONLY

            // _spriteBatch.Draw(box, homeTiles[ColourEnum.Blue][2].Transform.Position, null, Color.Green, 0, new Vector2(box.Width/2, box.Height/ 2), new Vector2(0.1f, 0.1f), SpriteEffects.None, 0);

           // _spriteBatch.Draw(box, tiles[0].Transform.Position, null, Color.Green, 0, new Vector2(box.Width / 2, box.Height / 2), new Vector2(0.1f, 0.1f), SpriteEffects.None, 0);

            //_spriteBatch.Draw(box, new Vector2(500, 500), null, Color.Red, 0, new Vector2(0, 0), new Vector2(0.2f, 0.2f), SpriteEffects.None, 0);




            _spriteBatch.End();
            base.Draw(gameTime);
        }


        public void MakeClassicTileMap()
        {
            for (int i = 0; i < 4; i++)
            {
                GameObject go = TileFactory.Instance.Create(ColourEnum.None, Content);
                go.Transform.Position = new Vector2(50 + tileSpacing + i * tileSpacing, 450);

                tiles.Add(go);
            }

            Vector2 pos1 = tiles[tiles.Count - 1].Transform.Position;

            for (int i = 0; i < 5; i++)
            {
                GameObject go = TileFactory.Instance.Create(ColourEnum.None, Content);
                go.Transform.Position = new Vector2(pos1.X + tileSpacing, pos1.Y - tileSpacing * i);

                tiles.Add(go);

            }

            Vector2 pos2 = tiles[tiles.Count - 1].Transform.Position;

            for (int i = 0; i < 2; i++)
            {
                GameObject go = TileFactory.Instance.Create(ColourEnum.None, Content);
                go.Transform.Position = new Vector2(pos2.X + tileSpacing * i, pos2.Y - tileSpacing);

                tiles.Add(go);
            }


            Vector2 pos3 = tiles[tiles.Count - 1].Transform.Position;


            for (int i = 0; i < 5; i++)
            {
                GameObject go = TileFactory.Instance.Create(ColourEnum.None, Content);
                go.Transform.Position = new Vector2(pos3.X + tileSpacing, pos3.Y + tileSpacing * i);

                tiles.Add(go);

            }

            Vector2 pos4 = tiles[tiles.Count - 1].Transform.Position;

            for (int i = 0; i < 5; i++)
            {
                GameObject go = TileFactory.Instance.Create(ColourEnum.None, Content);
                go.Transform.Position = new Vector2(pos4.X + i * tileSpacing, pos4.Y + tileSpacing);

                tiles.Add(go);
            }


            Vector2 pos5 = tiles[tiles.Count - 1].Transform.Position;

            for (int i = 0; i < 2; i++)
            {
                GameObject go = TileFactory.Instance.Create(ColourEnum.None, Content);
                go.Transform.Position = new Vector2(pos5.X + tileSpacing, pos5.Y + tileSpacing * i);

                tiles.Add(go);

            }

            Vector2 pos6 = tiles[tiles.Count - 1].Transform.Position;

            for (int i = 0; i < 5; i++)
            {
                GameObject go = TileFactory.Instance.Create(ColourEnum.None, Content);
                go.Transform.Position = new Vector2(pos6.X - i * tileSpacing, pos6.Y + tileSpacing);

                tiles.Add(go);
            }

            Vector2 pos7 = tiles[tiles.Count - 1].Transform.Position;

            for (int i = 0; i < 5; i++)
            {
                GameObject go = TileFactory.Instance.Create(ColourEnum.None, Content);
                go.Transform.Position = new Vector2(pos7.X - tileSpacing, pos7.Y + i * tileSpacing);

                tiles.Add(go);
            }

            Vector2 pos8 = tiles[tiles.Count - 1].Transform.Position;

            for (int i = 0; i < 2; i++)
            {
                GameObject go = TileFactory.Instance.Create(ColourEnum.None, Content);
                go.Transform.Position = new Vector2(pos8.X - tileSpacing * i, pos8.Y + tileSpacing);

                tiles.Add(go);

            }

            Vector2 pos9 = tiles[tiles.Count - 1].Transform.Position;

            for (int i = 0; i < 5; i++)
            {
                GameObject go = TileFactory.Instance.Create(ColourEnum.None, Content);
                go.Transform.Position = new Vector2(pos9.X - tileSpacing, pos9.Y - i * tileSpacing);

                tiles.Add(go);
            }

            Vector2 pos10 = tiles[tiles.Count - 1].Transform.Position;

            for (int i = 0; i < 5; i++)
            {
                GameObject go = TileFactory.Instance.Create(ColourEnum.None, Content);
                go.Transform.Position = new Vector2(pos10.X - i * tileSpacing, pos10.Y - tileSpacing);

                tiles.Add(go);
            }

            Vector2 pos11 = tiles[tiles.Count - 1].Transform.Position;

            for (int i = 0; i < 3; i++)
            {
                GameObject go = TileFactory.Instance.Create(ColourEnum.None, Content);
                go.Transform.Position = new Vector2(pos11.X - tileSpacing, pos11.Y - tileSpacing * i);

                tiles.Add(go);

            }

            for (int i = 0; i < playerOrderInit.Count; i++)
            {

            
                switch (playerOrderInit[i].colour)
                {
                    case ColourEnum.None:
                        break;
                    case ColourEnum.Red:
                        SpriteRenderer startTile1 = (SpriteRenderer)tiles[i*tiles.Count/4].GetComponent<SpriteRenderer>();

                        startTile1.Color = Color.Red;
                        break;
                    case ColourEnum.Blue:
                        SpriteRenderer startTile2 = (SpriteRenderer)tiles[i * tiles.Count / 4].GetComponent<SpriteRenderer>();

                        startTile2.Color = Color.Blue;
                        break;
                    case ColourEnum.Green:
                        SpriteRenderer startTile3 = (SpriteRenderer)tiles[i * tiles.Count / 4].GetComponent<SpriteRenderer>();

                        startTile3.Color = Color.Green;
                        break;
                    case ColourEnum.Yellow:

                        SpriteRenderer startTile4 = (SpriteRenderer)tiles[i * tiles.Count / 4].GetComponent<SpriteRenderer>();

                        startTile4.Color = Color.Yellow;
                        break;
                    default:
                        break;
                }
            }

          

          


            CreateColorTiles(playerColors.Values.ToList());
            CreateHomeTiles(playerColors.Values.ToList());


        }



        private void CreateColorTiles(List<ColourEnum> ColourEnums)
        {
            for (int i = 0; i < ColourEnums.Count; i++)
            {


                List<GameObject> colorTileGameObjects = new List<GameObject>();
                Vector2 startColorPos = tiles[(tiles.Count-2 + i * 12)%tiles.Count].Transform.Position;
                GameObject go=new GameObject();

                ColourEnum actualColor = ColourEnum.None; // Temporary variable to track the actual color used
                // Colors are set in terms of going around the clock instead of left to right which is how the colorEnums list is set.
                // Therefore indexes are set accordingly. 

                for (int j = 0; j < 5; j++)
                {
                 

                    switch (i)
                    {
                        case 0:
                            actualColor = ColourEnums[i];
                            go = TileFactory.Instance.Create(actualColor, Content);
                            go.Transform.Position = new Vector2(startColorPos.X + tileSpacing + tileSpacing * j, startColorPos.Y);
                           
                            break;

                        case 1:
                            actualColor = ColourEnums[i];
                            go = TileFactory.Instance.Create(actualColor, Content);
                            go.Transform.Position = new Vector2(startColorPos.X, startColorPos.Y + tileSpacing + tileSpacing * j);

                            break;
                        case 2:
                            actualColor = ColourEnums[i];
                            go = TileFactory.Instance.Create(actualColor, Content);
                            go.Transform.Position = new Vector2(startColorPos.X - tileSpacing - tileSpacing * j, startColorPos.Y);

                            break;
                        case 3:
                            actualColor = ColourEnums[i];
                            go = TileFactory.Instance.Create(actualColor, Content);
                            go.Transform.Position = new Vector2(startColorPos.X, startColorPos.Y - tileSpacing - tileSpacing * j);
                            break;



                        default:
                            break;
                    }




                    colorTileGameObjects.Add(go);

                }

                colorTiles.Add(actualColor, colorTileGameObjects);
                
            }



        }

        private void CreateHomeTiles(List<ColourEnum> ColourEnums)
        {
            List<Vector2> startpositions = new List<Vector2>() {new Vector2(112,225),new Vector2(760,225), new Vector2(760, 740),new Vector2(112,740), };


  
            for (int i = 0; i < ColourEnums.Count; i++)
            {
                

                List<GameObject> tiles = new List<GameObject>();
                for (int j = 0; j < 4; j++)
                {
                    GameObject go = TileFactory.Instance.Create(ColourEnum.None, Content);

                    switch (j)
                    {
                        case 0:
                            go.Transform.Position = new Vector2(startpositions[i].X, startpositions[i].Y);
                            break;

                        case 1:
                            go.Transform.Position = new Vector2(startpositions[i].X+ tileSpacing, startpositions[i].Y);
                            break;
                        case 2:
                            go.Transform.Position = new Vector2(startpositions[i].X, startpositions[i].Y+ tileSpacing);
                            break;
                        case 3:
                            go.Transform.Position = new Vector2(startpositions[i].X + tileSpacing, startpositions[i].Y+tileSpacing);
                            break;



                        default:
                            break;
                    }
                    tiles.Add(go);

                }

                homeTiles.Add(ColourEnums[i], tiles);


            }

        }



        private void CreateSetupButtons()
        {
            NonInteractableGOBuilder SelectAmountOfPlayersSign = new NonInteractableGOBuilder("SelectAmountOfPlayers", 1, new Vector2(screenSize.X / 2, 100), Color.White);

            List<Button> playerAmountButtons = new List<Button>();

            for (int i = 0; i < 3; i++)
            {
                //temporarily 4 since the logic for other amount of players has to be re-planned to work so frontend and backend works together. 
                int playerAmt = 4;
                Button playerAmountBtn = new Button((playerAmt).ToString(), new SetPlayerAmountButton(playerAmt), new Vector2(750 + 100 * i, 200), new Vector2(1, 1));


                playerAmountButtons.Add(playerAmountBtn);
                buttons_SetupGame.Add(playerAmountBtn);
            }

            foreach (var btn in playerAmountButtons)
            {
                btn.linkedButtons = playerAmountButtons;
            }
            // default button selected is 2
            playerAmountButtons[0].buttonAction.DoAction();

            Button rollForPlayerOrderBtn = new Button("RollForPlayerOrder", new RollForPlayerOrderButton(), new Vector2(screenSize.X / 2, 300), new Vector2(1, 1));

            buttons_SetupGame.Add(rollForPlayerOrderBtn);

            gameObjects_SetupGame.Add(SelectAmountOfPlayersSign.BuildGameObject());

        }


        private void CreateDice()
        {
            GameObject diceGO = new GameObject();
            SpriteRenderer sr=(SpriteRenderer)diceGO.AddComponent(new SpriteRenderer());
            sr.SetSprite("Dice6",0,1,Color.White,Content);
            diceGO.AddComponent(new Animator());
            diceGO.AddComponent(new Dice(new List<string>() { "Dice1", "Dice2", "Dice3", "Dice4", "Dice5", "Dice6" },1));

            diceGO.Transform.Position = new Vector2(1200, 600);

            gameObjects_Playing.Add(diceGO);
            


        }


        public void APIStartGame(int playerNumbers, int BoardSize)
        {
            // CREATE DATA DTO

            asyncTaskManager.EnqueueTask(async () =>
            {
                try
                {
                    await apiService.GetAsync<List<PlayerDTO>>("https://localhost:7221/api/Ludo/StartGame?PlayerNumber="+playerNumbers.ToString()+"&BoardSize="+BoardSize.ToString(), 
                    onSuccess: (responseObj) =>
                    {
                        playerOrderInit = responseObj;

                        foreach (var item in playerOrderInit)
                        {
                            playerColors.Add(item.id, item.colour);

                            List<GameObject> pieces = new List<GameObject>();
                            for (int i = 0; i < 4; i++)
                            {
                                GameObject go = new GameObject();
                                SpriteRenderer sr = (SpriteRenderer)go.AddComponent(new SpriteRenderer());
                                switch (item.colour)
                                {
                                    case ColourEnum.None:
                                        sr.SetSprite("Box", 0, 0.1f, Color.White, Content);
                                        break;
                                    case ColourEnum.Red:
                                        sr.SetSprite("Box", 0, 0.1f, Color.Red, Content);
                                        break;
                                    case ColourEnum.Blue:
                                        sr.SetSprite("Box", 0, 0.1f, Color.Blue, Content);
                                        break;
                                    case ColourEnum.Green:
                                        sr.SetSprite("Box", 0, 0.1f, Color.Green, Content);
                                        break;
                                    case ColourEnum.Yellow:
                                        sr.SetSprite("Box", 0, 0.1f, Color.Yellow, Content);
                                        break;
                                    default:
                                        break;
                                }


                                go.AddComponent(new PlayerPiece(item.id, item.colour, i, item.startTile.Index, item.pieces[i].ID, item.pieces[i].IsInPlay, item.pieces[i].IsFinished));

                                pieces.Add(go);



                            }


                            playerPieces.Add(item.colour, pieces);

                        }
                        // SETS THE CURRENT PLAYER TO BE THE FIRST ON THE LIST IN THE PLAYERORDER
                        UpdateCurrentPlayer(playerOrderInit[0].id);

                        ///SHOW PLAYERORDER METHOD USED HERE 
                        ///
                        ShowPlayerOrder(playerOrderInit.Select(p => p.colour).ToList());

                        Button startGameButton = new Button("StartGame", new StartGameButton(), new Vector2(GameWorld.Instance.screenSize.X / 2, 600), new Vector2(1, 1));

                        GameWorld.Instance.Instantiate(startGameButton);


                    },
                    onError: (error) =>
                    {
                        Console.WriteLine("POST failed: " + error.Message);
                    }
                    );
                    
                }
                catch (Exception ex)
                {

                    Console.WriteLine("Error: " + ex.Message);
                }
            });
        }

        public void ShowPlayerOrder(List<ColourEnum> playerOrderColours)
        {
            List<NonInteractableGOBuilder> playerColorBoxes = new List<NonInteractableGOBuilder>();
            

            for (int i = 0; i < playerOrderColours.Count; i++)
            {
                switch (playerOrderColours[i])
                {
                    case ColourEnum.None:
                        break;
                    case ColourEnum.Red:
                       
                        playerColorBoxes.Add(new NonInteractableGOBuilder("Box", 0.2f, new Vector2(700 + 100 * i, 400), Color.Red));
                        break;
                    case ColourEnum.Blue:
                      
                        playerColorBoxes.Add( new NonInteractableGOBuilder("Box", 0.2f, new Vector2(700 + 100 * i, 400), Color.Blue));
                        break;
                    case ColourEnum.Green:
                      
                        playerColorBoxes.Add(new NonInteractableGOBuilder("Box", 0.2f, new Vector2(700 + 100 * i, 400), Color.Green));
                        break;
                    case ColourEnum.Yellow:
                        playerColorBoxes.Add(new NonInteractableGOBuilder("Box", 0.2f, new Vector2(700 + 100 * i, 400), Color.Yellow));
                        break;
                    default:
                        break;
                }

            }


            foreach (var item in playerColorBoxes)
            {
                Instantiate(item.BuildGameObject());
            }

        }

        public void StartGame()
        {
            // Send numberOfPlayers to backend and return a list of players in an order

            int playerAmountTOSENDTOBACKEND = playerAmount;

            APIStartGame(playerAmount, 48);
            //// TEMPORARY PLAYERORDER
            //List<PlayerDTO> backendPlayerOrder = new List<PlayerDTO>()
            //{

            //new PlayerDTO() { colour = ColourEnum.Red, startTile = new PosIndex() { Index = 0*(tiles.Count/4)}, id=Guid.NewGuid(), pieces=new List<PieceDTO>(){
            //    new PieceDTO() {ID=Guid.NewGuid(),IsFinished=false,IsInPlay=false},
            //    new PieceDTO() {ID=Guid.NewGuid(),IsFinished=false,IsInPlay=false},
            //    new PieceDTO() {ID=Guid.NewGuid(),IsFinished=false,IsInPlay=false},
            //    new PieceDTO() {ID=Guid.NewGuid(),IsFinished=false,IsInPlay=false},
            //}},

            //new PlayerDTO() { colour = ColourEnum.Green, startTile = new PosIndex() { Index = 1*(tiles.Count/4)}, id=Guid.NewGuid(),pieces=new List<PieceDTO>(){
            //    new PieceDTO() {ID=Guid.NewGuid(),IsFinished=false,IsInPlay=false},
            //    new PieceDTO() {ID=Guid.NewGuid(),IsFinished=false,IsInPlay=false},
            //    new PieceDTO() {ID=Guid.NewGuid(),IsFinished=false,IsInPlay=false},
            //    new PieceDTO() {ID=Guid.NewGuid(),IsFinished=false,IsInPlay=false},
            //} },

            //new PlayerDTO() { colour = ColourEnum.Blue, startTile = new PosIndex() { Index = 2*(tiles.Count/4)}, id=Guid.NewGuid(),pieces=new List<PieceDTO>(){
            //    new PieceDTO() {ID=Guid.NewGuid(),IsFinished=false,IsInPlay=false},
            //    new PieceDTO() {ID=Guid.NewGuid(),IsFinished=false,IsInPlay=false},
            //    new PieceDTO() {ID=Guid.NewGuid(),IsFinished=false,IsInPlay=false},
            //    new PieceDTO() {ID=Guid.NewGuid(),IsFinished=false,IsInPlay=false},
            //} },

            //new PlayerDTO() { colour = ColourEnum.Yellow, startTile = new PosIndex() { Index = 3*(tiles.Count/4)}, id=Guid.NewGuid(),pieces=new List<PieceDTO>(){
            //    new PieceDTO() {ID=Guid.NewGuid(),IsFinished=false,IsInPlay=false},
            //    new PieceDTO() {ID=Guid.NewGuid(),IsFinished=false,IsInPlay=false},
            //    new PieceDTO() {ID=Guid.NewGuid(),IsFinished=false,IsInPlay=false},
            //    new PieceDTO() {ID=Guid.NewGuid(),IsFinished=false,IsInPlay=false},
            //} },
            

            //};

            // TEMPORARY PLAYERORDER

           
        }


        public void MakePiecesMoveable(List<Guid> moveAblePieces)
        {

            if (moveAblePieces.Count > 0)
            {
                foreach (var gameObjects in playerPieces.Values)
                {
                    foreach (var go in gameObjects)
                    {
                        PlayerPiece piece = (PlayerPiece)go.GetComponent<PlayerPiece>();


                        if (moveAblePieces.Contains(piece.pieceID))
                        {
                            piece.MakeMoveAble();
                        }


                    }



                }
            }
            

        }



        public void MakePiecesUnMoveable(ColourEnum pieceColor)
        {

                foreach (var gameObject in playerPieces[pieceColor])
                {
                 
                    PlayerPiece piece = (PlayerPiece)gameObject.GetComponent<PlayerPiece>();

                    piece.MakeUnMoveAble();
                        
                }
           


        }

        public void UpdatePieces(List<PieceDTO> piecesToUpdate)
        {

            foreach (var item in piecesToUpdate)
            {
                foreach (var go in playerPieces[item.Colour])
                {
                    PlayerPiece p =(PlayerPiece) go.GetComponent<PlayerPiece>();
                   
                    if ( p.pieceID==item.ID)
                    {

                        p.UpdatePiece(item);
                       
                    }
                }
               

            

            }

            

        }


        public void UpdateCurrentPlayer(Guid nextPlayersTurn)
        {
            switch (playerColors[nextPlayersTurn])
            {
                case ColourEnum.None:
                    stateManager.currentColor = Color.White;
                    break;
                case ColourEnum.Red:
                    stateManager.currentColor = Color.Red;
                    break;
                case ColourEnum.Blue:
                    stateManager.currentColor = Color.Blue;
                    break;
                case ColourEnum.Green:
                    stateManager.currentColor = Color.Green;
                    break;
                case ColourEnum.Yellow:
                    stateManager.currentColor = Color.Yellow;
                    break;
                default:
                    break;
            }
           
        }

        public void Instantiate(GameObject go)
        {
            newGameObjects.Add(go);
        }

        public void Instantiate(Button button)
        {
            newButtons.Add(button);
        }

       

        public void Destroy(Button button)
        {
            destroyedButtons.Add(button);
        }

        public void DestroyPiece(GameObject go)
        {
            destroyedPieces.Add(go);

          
        }

        public void Cleanup()
        {
            for (int i = 0; i < newGameObjects.Count; i++)
            {
                if(stateManager.currentState==GameState.SetupGame)
                {
                    gameObjects_SetupGame.Add(newGameObjects[i]);
                }

                if(stateManager.currentState==GameState.Playing)
                {
                    gameObjects_Playing.Add(newGameObjects[i]);
                }
                
                newGameObjects[i].Awake();
                newGameObjects[i].Start();

            }

            for (int i = 0; i < destroyedPieces.Count; i++)
            {
              
                PlayerPiece pieceToDestroy = (PlayerPiece)destroyedPieces[i].GetComponent<PlayerPiece>();
                
               

                foreach (var item in playerPieces[pieceToDestroy.pieceColor])
                {
                    PlayerPiece p = (PlayerPiece)item.GetComponent<PlayerPiece>();

                    if (p.pieceID == pieceToDestroy.pieceID)
                    {
                        playerPieces[pieceToDestroy.pieceColor].Remove(item);
                        break;
                    }
                }
              
            }

            destroyedPieces.Clear();
            newGameObjects.Clear();

            for (int i = 0; i < newButtons.Count; i++)
            {
                if (stateManager.currentState == GameState.SetupGame)
                {
                    buttons_SetupGame.Add(newButtons[i]);
                }

                if (stateManager.currentState == GameState.Playing)
                {
                    buttons_Playing.Add(newButtons[i]);
                }

                newButtons[i].LoadContent();
            }

            

            destroyedButtons.Clear();
            newButtons.Clear();
           
        }

    }
}
