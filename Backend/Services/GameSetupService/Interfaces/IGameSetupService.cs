using Backend.Domains.BoardDomain;
using Backend.Domains.PlayerDomain;

namespace Backend.Services.GameSetupService.Interfaces
{
	public interface IGameSetupService
	{
		List<Player> RollForPlayerOrder(List<Player> players);
		List<Player> AddPlayers(int playerCount);
		(Board Board, List<Player> Players) CreateNewGame(int playerCount, int boardSize, int lengthOfColorZone);

    }
}
