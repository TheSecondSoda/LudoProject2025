using Backend.Domains.PieceDomain;
using Backend.Domains.PlayerDomain;
using Backend.Services.BoardServices.Interfaces;
using Backend.Services.GameManagerService;
using Common.DTOs;
using Common.Enums;
using Backend.Domains.BoardDomain;
using Backend.Services.DiceServices.Interfaces;
using Backend.Services.GameSetupService.Interfaces;
using Backend.Services.PlayerServices.Interfaces;
using Backend.Services.GameRulesService;


namespace Backend.Domains.GameManagerDomain
{
    public class GameManager : IGameManagerService
    {
        private Board Board { get; set; }
        private List<Player> Players { get; set; } = new List<Player>();
        private Player CurrentPlayer { get; set; }
        private int rollsTaken = 0;
        private bool movedPiece;

        private readonly IGameSetupService gameSetupService;
        private readonly IPlayerService playerService;
        private readonly IGameRulesService gameRulesService;

        public GameManager(IGameSetupService gameSetupService, IPlayerService playerService, IGameRulesService gameRulesService)
        {
            this.gameSetupService = gameSetupService;
            this.playerService = playerService;
            this.gameRulesService = gameRulesService;
        }

        // Step 1 Create board
        public (Board Board, List<Player> Players) CreateNewGame(int playerCount, int boardSize, int lengthOfColourZone)
        {
            (Board, Players) = gameSetupService.CreateNewGame(playerCount, boardSize, lengthOfColourZone);
            return (this.Board, this.Players);
        }

        // Step 2 Roll for player order
        public List<Player> RollForPlayerOrder()
        {
            var playerOrder = gameSetupService.RollForPlayerOrder(Players);
            this.Players = playerOrder;
            this.CurrentPlayer = Players[0];
            return playerOrder;
        }

        // Step 3 Player rolls dice
        public void Roll(int roll)
        {
            this.CurrentPlayer.LastRoll = roll;
            this.rollsTaken++;
        }

        // Step 4 Return possible moves
        public List<Guid>? GetMovablePieces()
        {
            var availablePieces = Board.FindValidPiecesToMove(CurrentPlayer.Colour, CurrentPlayer.LastRoll);
            var ids = availablePieces.Select(p => p.ID).ToList();
            return ids;
        }

        // Step 5 Move piece
        public List<Piece> MovePiece(Guid pieceId)
        {
            movedPiece = true;
            return Board.MovePiece(pieceId, CurrentPlayer.Colour,CurrentPlayer.LastRoll);
        }

        // Step 6 Check if current player can roll again due to 6'er rule or no pieces in play rule
        public bool CanRollAgain()
        {
            return gameRulesService.CanRollAgain(CurrentPlayer, rollsTaken, Board, movedPiece); 
        }

        // Step 7 End turn
        public Guid NextTurn()
        {
            if (CanRollAgain())
            {
                return CurrentPlayer.Id;
            }

            // Move on to the next player and reset the roll
            int index = Players.IndexOf(CurrentPlayer);
            do
            {
                index = (index + 1) % Players.Count;
                CurrentPlayer = Players[index];
            } while (playerService.HasFinished(CurrentPlayer));

            movedPiece = false;
            rollsTaken = 0;
            this.CurrentPlayer.LastRoll = 0;

            return CurrentPlayer.Id;
        }

        // Support method, currently used during reqnroll
        public Player GetCurrentPlayer()
        {
            return CurrentPlayer;
        }
    } 
}
