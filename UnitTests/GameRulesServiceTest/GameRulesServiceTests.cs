using Backend.Domains.BoardDomain;
using Backend.Domains.PlayerDomain;
using Backend.Services.GameRulesService;
using Backend.Services.PlayerServices.Interfaces;
using Common.Enums;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.GameRulesTest
{
    public class GameRulesServiceTests
    {
        private readonly Mock<IPlayerService> playerServiceMock;
        private readonly GameRulesService gameRulesService;
        private readonly Player player;
        private readonly Board dummyBoard;

        public GameRulesServiceTests()
        {
            playerServiceMock = new Mock<IPlayerService>();
            gameRulesService = new GameRulesService(playerServiceMock.Object);
            player = new Player(ColourEnum.Red);
            dummyBoard = new Board(10, 2, new List<ColourEnum> { ColourEnum.Red }, player.Pieces);
        }

        [Fact]
        public void CanRollAgain_ReturnsTrue_WhenRollIsSix()
        {
            player.LastRoll = 6;

            var result = gameRulesService.CanRollAgain(player, 1, dummyBoard, movedPiece: false);

            Assert.True(result);
        }

        [Fact]
        public void CanRollAgain_ReturnsTrue_WhenNoPiecesInPlay_AndRollsTakenLessThan3_AndDidNotMovePiece()
        {
            player.LastRoll = 4;
            playerServiceMock.Setup(s => s.AnyPiecesInPlay(player)).Returns(false);

            var result = gameRulesService.CanRollAgain(player, 2, dummyBoard, movedPiece: false);

            Assert.True(result);
        }

        [Fact]
        public void CanRollAgain_ReturnsFalse_WhenNoPiecesInPlay_AndRollsTakenLessThan3_AndMovedPiece()
        {
            player.LastRoll = 4;
            playerServiceMock.Setup(s => s.AnyPiecesInPlay(player)).Returns(false);

            var result = gameRulesService.CanRollAgain(player, 2, dummyBoard, movedPiece: true);

            Assert.False(result);
        }

        [Fact]
        public void CanRollAgain_ReturnsFalse_WhenRollIsNotSix_AndPiecesInPlay()
        {
            player.LastRoll = 2;
            playerServiceMock.Setup(s => s.AnyPiecesInPlay(player)).Returns(true);

            var result = gameRulesService.CanRollAgain(player, 1, dummyBoard, movedPiece: false);

            Assert.False(result);
        }

        [Fact]
        public void CanRollAgain_ReturnsFalse_WhenRollIsNotSix_AndRollsTakenEqualTo3()
        {
            player.LastRoll = 3;
            playerServiceMock.Setup(s => s.AnyPiecesInPlay(player)).Returns(false);

            var result = gameRulesService.CanRollAgain(player, 3, dummyBoard, movedPiece: false);

            Assert.False(result);
        }
    }

}
