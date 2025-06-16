using Backend.Domains.BoardDomain;
using Backend.Domains.PieceDomain;
using Backend.Domains.PlayerDomain;

namespace Backend.Services.GameManagerServicesTemp.Interfaces
{
	public interface IGameManagerService
	{
		(Board Board, List<Player> Players) CreateNewGame(int playerCount, int boardSize, int lengthOfColourZone);
		void AddPlayers(int playerCount);
		List<Player> RollForPlayerOrder();
		void Roll(int roll);
		List<Guid>? GetPossibleMoves();
		List<Piece> MovePiece(Guid pieceId);
		Guid NextTurn();
	}
}
  