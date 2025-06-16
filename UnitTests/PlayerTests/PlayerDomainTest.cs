using Backend.Domains.PlayerDomain;
using Common.Enums;
using Backend.Domains.PieceDomain;
using Common.DTOs;
using System.Security.Cryptography;
using FluentAssertions;

namespace UnitTests.PlayerTests
{

    public class PlayerDomainTest
    {
        public static IEnumerable<object[]> PlayerConstructorTestData()
        {
            yield return new object[] { ColourEnum.Red, false, null, true };
            yield return new object[] { ColourEnum.Blue, false, null, true };
            yield return new object[] { ColourEnum.Green, false, null, true };
            yield return new object[] { ColourEnum.Yellow, false, null, true };
        }

        [Theory]
        [MemberData(nameof(PlayerConstructorTestData))]
        public void TestPlayerConstructor(
            ColourEnum colour,
            bool expectedIsTurn,
            PosIndex? expectedStartTile,
            bool shouldPass)
        {
            // Assert
            if (shouldPass)
            {
                var player = new Player(colour);

                player.Should().NotBeNull();
                player.Colour.Should().Be(colour);
                player.Pieces.Should().HaveCount(4);
                player.Pieces.All(p => p.Colour == colour).Should().BeTrue();
                player.StartTile.Should().Be(expectedStartTile);
            }
            else
            {
                Action act = () => new Player(colour);
                act.Should().Throw<ArgumentException>()
                    .WithMessage("A player must have a valid colour");
            }
        }
        [Fact]
        public void TestPlayer_InvalidColour()
        {
            Action act = () => new Player(ColourEnum.None);

            act.Should().Throw<Exception>()
                .WithMessage("A player must have a valid colour");
        }

        // Test om brikker kan flyttes ud
        [Fact]
        public void TestPlayerCanMovePieceOutFromStart()
        {
            var player = new Player(ColourEnum.Red);
            // Sæt tre brikker til ikke at være i spil manuelt
            player.Pieces[0].IsInPlay = false;
            player.Pieces[1].IsInPlay = false;
            player.Pieces[2].IsInPlay = false;
            player.Pieces[3].IsInPlay = true;

            var moveablePieces = player.Pieces.Where(p => !p.IsInPlay).ToList();

            moveablePieces.Should().HaveCount(3);
        }

        // Test at ingen brikker kan flyttes ud, hvis de er i spil.
        [Fact]
        public void TestPlayerCannotMoveAnyPieceOutFromStart()
        {
            var player = new Player(ColourEnum.Blue);
            player.Pieces.ForEach(p => p.IsInPlay = true);

            var moveablePieces = player.Pieces.Where(p => !p.IsInPlay).ToList();

            moveablePieces.Should().BeEmpty();
        }

        // Test for at sikre at kun en brik kan flyttes ud, hvis kun en brik ikke er i spil
        [Fact]
        public void TestPlayerHasOnePieceThatCanMoveOutFromStart()
        {
            var player = new Player(ColourEnum.Green);
            player.Pieces.ForEach(p => p.IsInPlay = true);
            player.Pieces[3].IsInPlay = false;

            var moveablePieces = player.Pieces.Where(p => !p.IsInPlay).ToList();

            moveablePieces.Should().ContainSingle()
                .Which.IsInPlay.Should().BeFalse();
        }
    }
}
