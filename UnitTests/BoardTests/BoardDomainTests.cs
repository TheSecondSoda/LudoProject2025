using Backend.Domains.BoardDomain;
using Backend.Domains.PieceDomain;
using Common.Enums;
using FluentAssertions;

namespace UnitTests.BoardTests
{
    public class BoardDomainTests
    {
        [Theory]
        [MemberData(nameof(Should_get_data))]
        public void Schould_get_a_board_with_all_the_things(int length, int zoneLength, List<ColourEnum> playerColours)
        {
            //Act
            var result = new Board(length, zoneLength, playerColours, new List<Piece>());

            //Assert
            result.Tiles.Should().HaveCount(length);
            result.PlayerZones.Should().HaveCount(playerColours.Count);
            result.PlayerZones.Values.Should().AllSatisfy(x => x.Should().HaveCount(zoneLength));
            result.PlayerZones.Values.Should().AllSatisfy(x => x.Last().IsGoalTile.Should().BeTrue());
        }

        public static IEnumerable<object[]> Should_get_data()
        {
            yield return new object[] { 40, 6, new List<ColourEnum> { ColourEnum.Red, ColourEnum.Blue } };
            yield return new object[] { 60, 8, new List<ColourEnum> { ColourEnum.Red, ColourEnum.Blue, ColourEnum.Green } };
            yield return new object[] { 80, 11, new List<ColourEnum> { ColourEnum.Red, ColourEnum.Blue, ColourEnum.Green, ColourEnum.Yellow } };
        }

    }
}
