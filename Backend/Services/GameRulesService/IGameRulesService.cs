using Backend.Domains.BoardDomain;
using Backend.Domains.PlayerDomain;

namespace Backend.Services.GameRulesService
{
    public interface IGameRulesService
    {
        bool CanRollAgain(Player currentPlayer, int rollsTaken, Board board, bool movedPiece);
    }
}
