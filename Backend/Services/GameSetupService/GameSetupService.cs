using Backend.Domains.BoardDomain;
using Backend.Domains.PieceDomain;
using Backend.Domains.PlayerDomain;
using Backend.Services.DiceServices.Interfaces;
using Backend.Services.GameSetupService.Interfaces;
using Common.Enums;
using System.Drawing;

namespace Backend.Services.GameSetupService
{
	public class GameSetupService : IGameSetupService
	{
		public GameSetupService(IDiceService diceService)
		{
			DiceService = diceService;
		}

		public IDiceService DiceService { get; }

		List<Player> IGameSetupService.RollForPlayerOrder(List<Player> players)
		{
			// Make sure there are players in the list
			if (players.Count() < 1)
			{
				throw new Exception("Need players to roll");
			}

			var firstPlayer = RollForFirstPlayer(players);

			// Reorder list so firstPlayer is first, maintaining original order
			int firstPlayerIndex = players.IndexOf(firstPlayer);
			var orderedPlayers = players.Skip(firstPlayerIndex)
										.Concat(players.Take(firstPlayerIndex))
										.ToList();

			return orderedPlayers;
		}


		private Player RollForFirstPlayer(List<Player> players)
		{
			Player result = null;
			var playersWithRolls = RollDieForEachPlayer(players);

			var highestRoll = playersWithRolls.Max(e => e.diceValue);
			if (playersWithRolls.Where(e => e.diceValue == highestRoll).Count() > 1)
			{
				result = RollForFirstPlayer(playersWithRolls.Where(e => e.diceValue == highestRoll).Select(e => e.player).ToList());
			}
			else
			{
				result = playersWithRolls.Where(e => e.diceValue == highestRoll).FirstOrDefault().player;
			}

			return result;
		}

		private List<(Player player, int diceValue)> RollDieForEachPlayer(List<Player> players)
		{
			// Roll a die for each player
			var result = new List<(Player player, int diceValue)>();
			foreach (var player in players)
			{
				var DiceValue = DiceService.Roll();

				result.Add((player, DiceValue));
			}
			return result;
		}

        public List<Player> AddPlayers(int playerCount)
        {
            var players = new List<Player>();
            for (int i = 0; i < playerCount; i++)
            {
                var color = (ColourEnum)(i + 1);
                players.Add(new Player(color));
            }
            return players;
        }

        public (Board Board, List<Player> Players) CreateNewGame(int playerCount, int boardSize, int lengthOfColorZone)
		{
			var players = AddPlayers(playerCount);
			var colors = players.Select(p => p.Colour).ToList();
            var pieces = players.SelectMany(p => p.Pieces).ToList();
            var board = new Board(boardSize, lengthOfColorZone, colors, pieces);
            players.ForEach(a => a.StartTile = board.GetStartTile(a.Colour));
			return (board, players);
        }
    }
}
