using Backend.Domains.BoardDomain;
using Backend.Domains.PlayerDomain;
using Backend.Services.PlayerServices.Interfaces;

namespace Backend.Services.GameRulesService
{
    public class GameRulesService : IGameRulesService
    {
        private readonly IPlayerService playerService;
        public GameRulesService(IPlayerService playerService) => this.playerService = playerService;


        public bool CanRollAgain(Player currentPlayer, int rollsTaken, Board board, bool movedPiece)
        {
            if (currentPlayer.LastRoll == 6)
                return true;

            bool noPiecesInPlay = !playerService.AnyPiecesInPlay(currentPlayer);
            bool underRollLimit = rollsTaken < 3;

            if (noPiecesInPlay && underRollLimit)
                return !movedPiece;

            return false;
        }
    }
}
