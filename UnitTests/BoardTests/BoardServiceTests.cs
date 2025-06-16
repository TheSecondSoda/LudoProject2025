using Backend.Domains.BoardDomain;
using Backend.Domains.PieceDomain;
using Backend.Services.BoardServices;
using Backend.Services.BoardServices.Interfaces;
using Common.DTOs;
using Common.Enums;
using System.Drawing;

namespace UnitTests.BoardTests
{
    public class BoardServiceTests
    {
        private readonly IBoardService _boardService;
        public BoardServiceTests(IBoardService boardService)
        {
            _boardService = boardService;
        }

        [Theory]
        [InlineData(ColourEnum.Red)]
        [InlineData(ColourEnum.Green)]
        [InlineData(ColourEnum.Blue)]
        [InlineData(ColourEnum.Yellow)]
        public void Schould_get_one_posIndex_for_the_colour_start_tile(ColourEnum colour)
        {
            //Arange
            var board = GetBoardForTest();

            //Act
            var result = _boardService.GetStartTilePos(board.Tiles, colour);

            //Assert
            Assert.True(result == board.Tiles.Find(x => x.Colour == colour)!.PosIndex);
        }

        public void Should_move_given_piece_x_tiles_given_by_diceroll(ColourEnum colour, int diceRoll)
        {
            //Arange
            var board = GetBoardForTest();
            var randomPieceIndex = new Random().Next(3);
            var piece = board.Pieces.Where(x => x.Colour == colour).ElementAt(randomPieceIndex);
            piece.PosIndex = _boardService.GetStartTilePos(board.Tiles, colour);
            piece.IsInPlay = true;

            //Act
            var result = _boardService.MovePiece(board.Pieces, piece, colour, diceRoll, board.Tiles, board.PlayerZones.GetValueOrDefault(colour));

            //Assert
            
        }

        private Board GetBoardForTest()
        {
            var playerColours = new List<ColourEnum>() { ColourEnum.Red, ColourEnum.Blue, ColourEnum.Green, ColourEnum.Yellow };
            var pieces = new List<Piece>();
            foreach (var colour in playerColours)
            {
                for (int i = 0; i < 4; i++)
                    pieces.Add(new Piece(colour));
            }
            var board = new Board(60, 6, playerColours, pieces);
            return board;
        }
    }
}
