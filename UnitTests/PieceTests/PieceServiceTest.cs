using Backend.Domains.PieceDomain;
using Backend.Services.PieceService;
using Backend.Services.PieceService.Interfaces;
using Common.DTOs;
using Common.Enums;
using FluentAssertions;
using System.Collections;

namespace UnitTests.PieceTests
{
	public class PieceServiceTest
	{
		[Theory]
		[InlineData(ColourEnum.None, 42)]
		[InlineData(ColourEnum.Yellow, 12)]
		[InlineData(ColourEnum.None, 99999)]
		public void TestSuccessfulMovePieceMethod(ColourEnum posIndexColour, int posIndexIndex)
		{
			// Arrange
			var piece = new Piece(1, ColourEnum.Yellow);
			piece.IsInPlay = true;
			var posIndex = new PosIndex
			{
				Colour = posIndexColour,
				Index = posIndexIndex
			};

			IPieceService pieceService = new PieceService();

			// Act
			piece = pieceService.MovePiece(piece, posIndex);

			// Assert
			piece.PosIndex.Should().NotBeNull();
			piece.PosIndex.Index.Should().Be(posIndexIndex);
			piece.PosIndex.Colour.Should().Be(posIndexColour);
		}

		[Theory]
		[ClassData(typeof(InvalidMovePieceTestData))]
		public void TestInvalidMovePieceMethod(
			string parameterCausingInvalidOperation,
			bool invalidValue,
			string expectedMessage)
		{
			// Arrange
			var piece = new Piece(1, ColourEnum.Yellow);
			piece.IsInPlay = true;
			var posIndex = new PosIndex
			{
				Colour = ColourEnum.None,
				Index = 1
			};

			IPieceService pieceService = new PieceService();

			if (parameterCausingInvalidOperation == "IsInPlay")
			{
				piece.IsInPlay = invalidValue;
			}
			else if (parameterCausingInvalidOperation == "IsFinished")
			{
				piece.IsFinished = invalidValue;
			}

			// Act
			Action act = () => pieceService.MovePiece(piece, posIndex);

			// Assert
			act.Should().Throw<Exception>().WithMessage(expectedMessage);
		}

		[Theory]
		[MemberData(nameof(SuccessfulMovePieceOutTestData), parameters: 4)]
		public void TestSuccessfulMovePieceOut(ColourEnum posIndexColour, int posIndexIndex)
		{
			// Arrange
			var piece = new Piece(1, ColourEnum.Yellow);
			var posIndex = new PosIndex
			{
				Colour = posIndexColour,
				Index = posIndexIndex
			};

			IPieceService pieceService = new PieceService();

			// Act
			piece = pieceService.MovePieceOut(piece, posIndex);

			// Assert
			piece.PosIndex.Should().BeEquivalentTo(posIndex);
			piece.IsInPlay.Should().BeTrue();
		}

		[Fact]
		public void MovePieceOut_PieceAlreadyOnBoard_ExceptionThrown()
		{
			// Arrange
			var piece = new Piece(1, ColourEnum.Yellow);
			piece.IsInPlay = true;
			var posIndex = new PosIndex
			{
				Colour = ColourEnum.None,
				Index = 4
			};

			IPieceService pieceService = new PieceService();

			// Act
			Action act = () => piece = pieceService.MovePieceOut(piece, posIndex);

			// Assert
			act.Should().Throw<Exception>().WithMessage("*Piece is already in play*");
			piece.PosIndex.Should().BeNull();
		}

		[Fact]
		public void ReturnPieceToBase_PieceIsOnBoard_PieceNoLongerInPlay()
		{
			// Arrange
			var piece = new Piece(1, ColourEnum.Yellow);
			piece.IsInPlay = true;

			IPieceService pieceService = new PieceService();

			// Act
			piece = pieceService.ReturnPieceToBase(piece);

			// Assert
			piece.IsInPlay.Should().BeFalse();
			piece.PosIndex.Should().BeNull();
		}

		[Fact]
		public void ReturnPieceToBase_PieceIsNotOnBoard_ExceptionThrown()
		{
			// Arrange
			var piece = new Piece(1, ColourEnum.Yellow);
			piece.IsInPlay = false;

			IPieceService pieceService = new PieceService();

			// Act
			Action act = () => piece = pieceService.ReturnPieceToBase(piece);

			// Assert
			act.Should().Throw<Exception>().WithMessage("*Piece is already in base*");
			piece.IsInPlay.Should().BeFalse();
			piece.PosIndex.Should().BeNull();
		}

		public static IEnumerable<object[]> SuccessfulMovePieceOutTestData(int numTests)
		{
			var allData = new List<object[]>
			{
				new object[] { ColourEnum.Yellow, 1 },
				new object[] { ColourEnum.None, 52 },
				new object[] { ColourEnum.None, 891283 },
				new object[] { ColourEnum.Blue, int.MinValue },
				new object[] { ColourEnum.Red, int.MaxValue },
				new object[] { ColourEnum.None, int.MaxValue-400 },
			};

			return allData.Take(numTests);
		}
	}
}

public class InvalidMovePieceTestData : IEnumerable<object[]>
{
	public IEnumerator<object[]> GetEnumerator()
	{
		yield return new object[] { "IsInPlay", false, "Piece is in base, not on board" };
		yield return new object[] { "IsFinished", true, "Piece is in goal, and can't be moved anymore" };
	}

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
