using Common.DTOs.ResponseDTOs;
using Common.Enums;
using Frontend.ControllerPattern;
using Microsoft.Xna.Framework;
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
    public class Dice:Component
    {
        List<string> numberSpriteNames;
        List<Texture2D> numberSprites;
        Vector2 scale;
        Vector2 position;
        private Texture2D spriteForSizeRef;
        private Rectangle rectangle;
        private Vector2 origin;
        private Animator animator;
        private Random random;
        private bool down;
        private bool animationStarted;
        private float timeElapsed;
        private float rollTime;

        public Dice(List<string> numberSpriteNames,float rollTime)
        {
            this.numberSpriteNames = numberSpriteNames;

            this.rollTime = rollTime;

            numberSprites = new List<Texture2D>();

        }

        public override void Start()
        {
            position = GameObject.Transform.Position;
            SpriteRenderer sr = (SpriteRenderer)GameObject.GetComponent<SpriteRenderer>();
            scale = new Vector2(sr.Scale, sr.Scale);
            foreach (var name in numberSpriteNames )
            {
                numberSprites.Add(GameWorld.Instance.Content.Load<Texture2D>(name));
            }
            
            spriteForSizeRef = numberSprites[0];
            rectangle = new Rectangle((int)(position.X - spriteForSizeRef.Width / 2), (int)(position.Y - spriteForSizeRef.Height / 2), (int)(spriteForSizeRef.Width * scale.X), (int)(spriteForSizeRef.Height * scale.Y));
            origin = new Vector2(spriteForSizeRef.Width * scale.X / 2, spriteForSizeRef.Height * scale.Y / 2);
            animator = (Animator)GameObject.GetComponent<Animator>();
            animator.animationPaused = true;

            animator.AddAnimation(new Animation("RollDice", numberSprites.ToArray(), 10));
            random = new Random();

          
            
            base.Start();
        }

       
        public override void Update()
        {
            if (rectangle.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y)) && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {

                down = true;

            }
            if (rectangle.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y)) && Mouse.GetState().LeftButton == ButtonState.Released && down)
            {
                animator.PlayAnimation("RollDice");
                animationStarted = true;
             
                //Debug.Write("down");
                
                down = false;
            }

            if(animationStarted==true)
            {
                timeElapsed += GameWorld.Instance.deltaTime;

                if(timeElapsed>=rollTime)
                {
                 
                 

                    APIFindValidMoves();

                    animationStarted = false;
                    //TEMPORARY GET back a list of moveable pieces
                    //List<Guid> moveAblePieces = new List<Guid>();
                    //moveAblePieces = GameWorld.Instance.playerPieces[ColourEnum.Red].Select(go => {
                    //    PlayerPiece piece = (PlayerPiece)go.GetComponent<PlayerPiece>();
                    //    return piece.pieceID;
                    //}).ToList();
                    //TEMPORARY

                
                }


            }
        }


        public void APIFindValidMoves()
        {

            GameWorld.Instance.asyncTaskManager.EnqueueTask(async () =>
            {
                try
                {
                    await GameWorld.Instance.apiService.GetAsync<RollDieAndFindValidMovesResponseDTO>("https://localhost:7221/api/Ludo/RollDieAndFindValidMoves",
                    onSuccess: (responseObj) =>
                    {
                       
                      
                        GameWorld.Instance.MakePiecesMoveable(responseObj.validPieces);



                        animator.StopAnimationAndSetSprite(numberSprites[responseObj.diceroll-1]);


                        timeElapsed = 0;
                        animationStarted = false;

                        if (responseObj.canReroll == false && responseObj.validPieces.Count==0)
                        {
                            APIEndTurn();
                        }

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

        public void APIEndTurn()
        {

            GameWorld.Instance.asyncTaskManager.EnqueueTask(async () =>
            {
                try
                {
                    await GameWorld.Instance.apiService.GetAsync<EndTurnResponseDTO>("https://localhost:7221/api/Ludo/EndTurn",
                    onSuccess: (responseObj) =>
                    {

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


    }
}
