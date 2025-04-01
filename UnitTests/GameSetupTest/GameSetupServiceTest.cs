using Backend.Domains.PieceDomain;
using Backend.Domains.PlayerDomain;
using Backend.Services.DiceService.Interfaces;
using Backend.Services.GameSetupService;
using Backend.Services.GameSetupService.Interfaces;
using Backend.Services.PieceService;
using Common.Enums;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.GameSetupTest
{
	public class GameSetupServiceTest
	{
		[Fact]
		public void RollForPlayerOrder_FourPlayersInList_SamePlayersComeOutInSpecificOrder()
		{
			// Arrange
			var diceRolls = new Queue<int>(new[] { 1, 5, 3, 6 });

			var mockDice = new Mock<IDiceService>();
			mockDice.Setup(d => d.Roll()).Returns(() => diceRolls.Dequeue());

			mockDice.SetupProperty(x => x.lastRoll, 4);

			int capturedValue = 0;

			mockDice.Setup(x => x.Roll())
				.Callback(() => capturedValue = new Random().Next(1, 7))
				.Returns(() => capturedValue);



			IGameSetupService gameSetupService = new GameSetupService(mockDice.Object);

			var players = new List<Player>
			{
				CreateTestPlayer(1, ColourEnum.Red),
				CreateTestPlayer(2, ColourEnum.Blue),
				CreateTestPlayer(3, ColourEnum.Green),
				CreateTestPlayer(4, ColourEnum.Yellow)
			};

			// Act
			var result = gameSetupService.RollForPlayerOrder(players);

			// Assert
			result.Should().HaveCount(4);
			result.Select(p => p.Id).Should().Equal(4, 1, 2, 3);
		}

		[Fact]
		public void RollForPlayerOrder_ZeroPlayersInList_ThrowException()
		{
			// Arrange
			var mockDice = new Mock<IDiceService>();

			IGameSetupService gameSetupService = new GameSetupService(mockDice.Object);

			var players = new List<Player>();

			// Act
			Action act = () => gameSetupService.RollForPlayerOrder(players);

			// Assert
			act.Should().Throw<Exception>().WithMessage("*Need players to roll*");
		}

		[Fact]
		public void RollForPlayerOrder_TwoPlayersRollTheSameHighestNumber_HighestPlayersRerollForFirstPlaceInOrder()
		{
			// Arrange
			// The second and fourth player rolls the same, rerolling against each other with the second player rolling highest
			var diceRolls = new Queue<int>(new[] { 1, 6, 3, 6, 4, 3 });

			var mockDice = new Mock<IDiceService>();
			mockDice.Setup(d => d.Roll()).Returns(() => diceRolls.Dequeue());

			IGameSetupService gameSetupService = new GameSetupService(mockDice.Object);

			var players = new List<Player>
			{
				CreateTestPlayer(1, ColourEnum.Red),
				CreateTestPlayer(2, ColourEnum.Blue),
				CreateTestPlayer(3, ColourEnum.Green),
				CreateTestPlayer(4, ColourEnum.Yellow)
			};

			// Act
			var result = gameSetupService.RollForPlayerOrder(players);

			// Assert
			result.Should().HaveCount(4);
			result.Select(p => p.Id).Should().Equal(2, 3, 4, 1);
		}

		private Player CreateTestPlayer(int id, ColourEnum colour)
		{
			var pieces = new List<Piece>();

			// Add 4 Pieces to the player's list
			for (int i = 0; i < 4; i++)
			{
				pieces.Add(new Piece(i, colour));
			}

			var player = new Player(id, colour, pieces);

			return player;
		}
	}
}
