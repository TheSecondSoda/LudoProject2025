using Common.DTOs;
using Common.DTOs.ResponseDTOs;
using Common.Enums;
using Frontend.ControllerPattern;
using Frontend.FactoryPattern;
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

namespace Frontend.ComponentPattern
{
    public class PlayerPiece : Component
    {
        private string spriteName;
        private Vector2 position;
        private Vector2 scale;
        private Texture2D sprite;
        private Texture2D pixel;
        private Rectangle rectangle;
        private Vector2 origin;
        private bool down;

        public Guid pieceID { get; set; }

        private bool isInplay;
        public ColourEnum pieceColor;
        private int startTileIndex;
        public Guid playerId;
        public bool moveAble = false;
        private int index;
        private bool moveToColorZone = false;
        private int hometilePosIndex;
        private bool isFinished;

        public PlayerPiece(Guid playerID,ColourEnum pieceColor, int hometileIndex, int startTileIndex,Guid pieceId,bool isInplay,bool isFinished)
        {

            this.hometilePosIndex = hometileIndex;
            this.pieceColor = pieceColor;
            this.startTileIndex = startTileIndex;
            this.playerId = playerID;
            this.pieceID = pieceId;
            this.isInplay = isInplay;
            this.isFinished = isFinished;

        }

        public void MakeMoveAble()
        {
            moveAble = true;
            SpriteRenderer sr=(SpriteRenderer)GameObject.GetComponent<SpriteRenderer>();

            sr.ShowRectangle = true;

        }

        public void MakeUnMoveAble()
        {
            moveAble = false;
            SpriteRenderer sr = (SpriteRenderer)GameObject.GetComponent<SpriteRenderer>();

            sr.ShowRectangle = false;
        }

        public void MoveToStartTile()
        {
            if (sprite == null || scale == Vector2.Zero)
            {
                SpriteRenderer sr = (SpriteRenderer)GameObject.GetComponent<SpriteRenderer>();
                this.sprite = sr.Sprite;
                this.scale = new Vector2(sr.Scale, sr.Scale);
            }

            GameObject.Transform.Position = GameWorld.Instance.tiles[startTileIndex].Transform.Position;

            this.position = GameObject.Transform.Position;

            rectangle = new Rectangle((int)(position.X - sprite.Width * scale.X / 2), (int)(position.Y - sprite.Height * scale.Y / 2), (int)(sprite.Width * scale.X), (int)(sprite.Height * scale.Y));
            
        }

        public void RemoveFromGame()
        {
            GameWorld.Instance.DestroyPiece(GameObject);
           

        }

        public void MoveToColorZone()
        {
            moveToColorZone = true;
        }

        public void MoveToHomeTiles()
        {


               
            GameObject.Transform.Position = GameWorld.Instance.homeTiles[pieceColor][hometilePosIndex].Transform.Position;
            SpriteRenderer sr = (SpriteRenderer)GameWorld.Instance.homeTiles[pieceColor][hometilePosIndex].GetComponent<SpriteRenderer>();
            sr.ShowRectangle = true;




            this.position = GameObject.Transform.Position;

            rectangle = new Rectangle((int)(position.X - sprite.Width * scale.X / 2), (int)(position.Y - sprite.Height * scale.Y / 2), (int)(sprite.Width * scale.X), (int)(sprite.Height * scale.Y));

        }

        public void UpdatePiece(PieceDTO updatedPiece)
        {
            if(updatedPiece.IsInPlay==false)
            {
                MoveToHomeTiles();
            }
            else if (updatedPiece.IsInPlay!=isInplay)
            {
                isInplay = updatedPiece.IsInPlay;
                MoveToStartTile();

            }


            if (updatedPiece.IsFinished)
            {
                RemoveFromGame();
            }

            if (updatedPiece.PosIndex!= null)
            {
                index = updatedPiece.PosIndex.Index;

                if (updatedPiece.PosIndex.Colour == pieceColor)
                {
                    MoveToColorZone();
                }

            }

            this.position = GameObject.Transform.Position;

            rectangle = new Rectangle((int)(position.X - sprite.Width * scale.X / 2), (int)(position.Y - sprite.Height * scale.Y / 2), (int)(sprite.Width * scale.X), (int)(sprite.Height * scale.Y));

            GameWorld.Instance.MakePiecesUnMoveable(pieceColor);


        }

        public override void Start()
        {
          
            this.position = GameObject.Transform.Position;
            SpriteRenderer sr = (SpriteRenderer)GameObject.GetComponent<SpriteRenderer>();
            this.scale = new Vector2(sr.Scale, sr.Scale);
            this.spriteName = sr.Sprite.Name;
            this.sprite = sr.Sprite;
            pixel = GameWorld.Instance.Content.Load<Texture2D>("pixel");
      
            origin = new Vector2(sprite.Width * scale.X / 2, sprite.Height * scale.Y / 2);

            MoveToHomeTiles();
        }

        public override void Update()
        {

           

            if (rectangle.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y)) && Mouse.GetState().LeftButton == ButtonState.Pressed && moveAble == true)
            {

                down = true;

            }
            if (rectangle.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y)) && Mouse.GetState().LeftButton == ButtonState.Released && down)
            {


                //SEND PIECEID TO BACKEND AND GET A LIST OF PIECEDTO THAT REPRESENTS THE UPDATED PIECES

                APIMoveSelectedPiece(pieceID);


                //TEMPORARY 


                //List<PieceDTO> piecesToUpdate = new List<PieceDTO>() { new PieceDTO() { Colour = pieceColor, ID = pieceID, PosIndex = new PosIndex { Colour = ColourEnum.None, Index = startTileIndex }, IsFinished = true } };


                //TEMPORARY

              
                moveAble = false;
                down = false;




                //SEND NEXT TURN TO BACKEND AND UPDATE WHOS TURN IT IS BASED ON A PLAYER GUID

                //TEMPORARY
                //Guid nextPlayersTurn = GameWorld.Instance.playerColors.Keys.ToList()[1];
                //TEMPORARY

               

            }


        }


        public void APIMoveSelectedPiece(Guid pieceID)
        {

            GameWorld.Instance.asyncTaskManager.EnqueueTask(async () =>
            {
                try
                {
                    var result = GameWorld.Instance.apiService.GetAsync<MoveSelectedPieceResponseDTO> ("https://localhost:7221/api/Ludo/MoveSelectedPiece?pieceID=" + pieceID.ToString(),
                    onSuccess: (responseObj) =>
                    {

                        GameWorld.Instance.UpdatePieces(responseObj.affectedPieces);


                        if (moveToColorZone == false)
                        {
                            GameObject.Transform.Position = GameWorld.Instance.tiles[index].Transform.Position;
                        }
                        else
                        {
                            GameObject.Transform.Position = GameWorld.Instance.colorTiles[pieceColor][index].Transform.Position;
                        }

                        this.position = GameObject.Transform.Position;

                        rectangle = new Rectangle((int)(position.X - sprite.Width * scale.X / 2), (int)(position.Y - sprite.Height * scale.Y / 2), (int)(sprite.Width * scale.X), (int)(sprite.Height * scale.Y));

                        //SEND NEXT TURN TO BACKEND AND UPDATE WHOS TURN IT IS BASED ON A PLAYER GUID

                        GameWorld.Instance.UpdateCurrentPlayer(responseObj.nextPlayerID);



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



        public void APINextTurn()
        {

            GameWorld.Instance.asyncTaskManager.EnqueueTask(async () =>
            {
                try
                {
                    await GameWorld.Instance.apiService.GetAsync<Guid>("https://localhost:7221/api/Ludo/NextTurn",
                    onSuccess: (responseObj) =>
                    {

                        GameWorld.Instance.UpdateCurrentPlayer(responseObj);

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

    }
}
