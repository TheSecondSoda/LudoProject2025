using Backend.Domains.BoardDomain;
using Backend.Domains.PieceDomain;
using Backend.Domains.PlayerDomain;

namespace Backend.Services.GameManagerService
{
    public class GameManagerService
    {
        public bool CanRollAgain()
        {
            throw new NotImplementedException();
        }

        public (Board Board, List<Player> Players) CreateNewGame(int playerCount, int boardSize, int lengthOfColourZone)
        {
            throw new NotImplementedException();
        }

        public List<Guid>? GetMovablePieces()
        {
            throw new NotImplementedException();
        }

        public List<Piece> MovePiece(Guid pieceId)
        {
            throw new NotImplementedException();
        }

        public Guid NextTurn()
        {
            throw new NotImplementedException();
        }

        public void Roll(int roll)
        {
            throw new NotImplementedException();
        }

        public List<Player> RollForPlayerOrder()
        {
            throw new NotImplementedException();
        }
    }
}
