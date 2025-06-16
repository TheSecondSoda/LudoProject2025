using Backend.Domains.BoardDomain;
using Backend.Domains.PieceDomain;
using Backend.Domains.PlayerDomain;

namespace Backend.Services.GameManagerService
{
    public interface IGameManagerService
    {
        (Board Board, List<Player> Players) CreateNewGame(int playerCount, int boardSize, int lengthOfColourZone);
        List<Player> RollForPlayerOrder();
        void Roll(int roll);
        List<Guid>? GetMovablePieces();
        List<Piece> MovePiece(Guid pieceId);
        bool CanRollAgain();
        Guid NextTurn();
        Player GetCurrentPlayer();
    }
}
