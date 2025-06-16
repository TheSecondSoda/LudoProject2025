using Backend.Controllers;
using Backend.Domains.BoardDomain;
using Backend.Domains.PieceDomain;
using Backend.Domains.PlayerDomain;
using Backend.Services.DiceServices.Interfaces;
using Backend.Services.GameManagerService;
using Common.DTOs;
using Common.DTOs.ResponseDTOs;
using Common.Enums;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.ControllerTest
{
	public class LudoControllerTest
	{
		private readonly Mock<ILogger<LudoController>> _loggerMock;
		private readonly Mock<IGameManagerService> _GameManagerMock;
		private readonly Mock<IDiceService> _DiceServiceMock;
		private readonly LudoController _sut; // SUT = System Under Test

		public LudoControllerTest()
		{
			_loggerMock = new Mock<ILogger<LudoController>>();
			_GameManagerMock = new Mock<IGameManagerService>();
			_DiceServiceMock = new Mock<IDiceService>();
			_sut = new LudoController(_GameManagerMock.Object, _DiceServiceMock.Object, _loggerMock.Object);
		}

		[Theory]
		[MemberData(nameof(PossibleDiceValues.RangeNumbers), MemberType = typeof(PossibleDiceValues))]
		public void RollDieAndFindValidMoves_ValidInput_ReturnListOfPlayerIDs(int diceValue)
		{
			// arrange
			var validPieces = new List<Guid> { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
			_GameManagerMock.Setup(gm => gm.GetMovablePieces()).Returns(validPieces);
			_DiceServiceMock.Setup(d => d.Roll()).Returns(diceValue);

			// act
			var result = _sut.RollDieAndFindValidMoves().Result;

			// assert
			result.Should().BeOfType<OkObjectResult>()
				.And.BeEquivalentTo(new
				{
					StatusCode = 200,
					Value = new RollDieAndFindValidMovesResponseDTO
					{
						diceroll = diceValue,
						validPieces = validPieces
					}
				}, options => options.ExcludingMissingMembers());
			_GameManagerMock.Verify(s => s.Roll(It.IsAny<int>()), Times.Once);
			_GameManagerMock.Verify(s => s.GetMovablePieces(), Times.Once);
		}

		[Fact]
		public void RollDieAndFindValidMoves_GameManagerReturnsException_ErrorMessageReturned()
		{
			// arrange
			_GameManagerMock.Setup(gm => gm.GetMovablePieces()).Throws(new Exception());
			_DiceServiceMock.Setup(d => d.Roll()).Returns(6);

			// act
			var result = _sut.RollDieAndFindValidMoves().Result;

			// assert
			result.Should().BeOfType<BadRequestObjectResult>()
				.Which.StatusCode.Should().Be(400);
		}

		[Theory]
		[MemberData(nameof(PieceAndPosIndexTestCases))]
		public void MoveSelectedPieceAndNextTurn_VariousPiecesAndPosIndexes_ReturnListOfPiecesNewLocations(Piece piece, PosIndex posIndex)
		{
			// arrange
			var playerPieces = GetTestPieces(piece.Colour);
			playerPieces[new Random().Next(4)] = piece;

			_GameManagerMock.Setup(gm => gm.MovePiece(piece.ID)).Returns(() =>
			{
				var updatedPieces = playerPieces.Select(p =>
				{
					if (p.ID == piece.ID)
					{
						p.PosIndex = posIndex;
					}
					return p;
				}).ToList();

				return updatedPieces;
			});

			// act
			var result = _sut.MoveSelectedPieceAndNextTurn(piece.ID).Result;

			// assert
			result.Should().BeOfType<OkObjectResult>()
				.And.BeEquivalentTo(new
				{
					StatusCode = 200
				}, options => options.ExcludingMissingMembers());
			_GameManagerMock.Verify(s => s.MovePiece(It.IsAny<Guid>()), Times.Once);
			_GameManagerMock.Verify(s => s.NextTurn(), Times.Once);
		}

		[Fact]
		public void moveSelectedPieceAndNextTurn_GameManagerReturnsException_ErrorMessageReturned()
		{
			// arrange
			_GameManagerMock.Setup(gm => gm.MovePiece(It.IsAny<Guid>())).Throws(new Exception());

			// act
			var result = _sut.MoveSelectedPieceAndNextTurn(Guid.NewGuid()).Result;

			// assert
			result.Should().BeOfType<BadRequestObjectResult>()
				.Which.StatusCode.Should().Be(400);
		}

		[Fact]
		public void StartGame_ValidInput_ListOfCreatedPlayersReturned()
		{
			// arrange
			var testPlayers = new List<Player>();
			testPlayers.Add(CreateTestPlayer(ColourEnum.Green));
			testPlayers.Add(CreateTestPlayer(ColourEnum.Yellow));
			_GameManagerMock.Setup(gm => gm.CreateNewGame(2, It.IsAny<int>(), It.IsAny<int>()))
				.Returns((new Board(42, 6, new List<ColourEnum> { ColourEnum.Green, ColourEnum.Yellow }, new List<Piece>()), testPlayers));
			_GameManagerMock.Setup(gm => gm.RollForPlayerOrder()).Returns(testPlayers);

			// act
			var result = _sut.StartGame(2, 4).Result;

			// assert
			result.Should().BeOfType<OkObjectResult>()
				.Which.StatusCode.Should().Be(200);
			_GameManagerMock.Verify(s => s.CreateNewGame(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
		}

		[Fact]
		public void StartGame_GameManagerReturnsException_ErrorMessageReturned()
		{
			// arrange
			_GameManagerMock.Setup(gm => gm.CreateNewGame(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
				.Throws(new Exception());

			// act
			var result = _sut.StartGame(2, 4).Result;

			// assert
			result.Should().BeOfType<BadRequestObjectResult>()
				.Which.StatusCode.Should().Be(400);
		}

		private List<Piece> GetTestPieces(ColourEnum colour)
		{
			var pieces = new List<Piece>();

			// Add 4 Pieces to the player's list
			for (int i = 0; i < 4; i++)
			{
				pieces.Add(new Piece(colour));
			}

			return pieces;
		}

		private Player CreateTestPlayer(ColourEnum colour)
		{
			var player = new Player(colour);

			return player;
		}

		public class PossibleDiceValues
		{
			public static IEnumerable<object[]> RangeNumbers =>
				Enumerable.Range(1, 6)
						  .Select(n => new object[] { n });
		}

		public static IEnumerable<object[]> PieceAndPosIndexTestCases =>
		new List<object[]>
		{
			new object[] { new Piece(ColourEnum.Yellow), new PosIndex(1, ColourEnum.Yellow)},
			new object[] { new Piece(ColourEnum.Green), new PosIndex(2214, ColourEnum.Green)},
			new object[] { new Piece(ColourEnum.Yellow), new PosIndex(120, ColourEnum.None) },
		};
	}
}
