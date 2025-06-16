using Backend.Domains.PieceDomain;
using Common.DTOs;
using Common.Enums;

namespace Backend.Domains.PlayerDomain
{

    public class Player
    {
        public Guid Id { get; init; }
        public ColourEnum Colour { get; init; }
        public List<Piece> Pieces { get; init; } = new();
        public PosIndex? StartTile { get; set; }
        public int LastRoll { get; set; } = 0;

        public Player(ColourEnum colour)
        {
			if (colour == ColourEnum.None)
			{
				throw new Exception("A player must have a valid colour");
			}
			for (int i = 0; i < 4; i++)
            {
                Pieces.Add(new Piece(colour));
            }
            

            Id = Guid.NewGuid();
            Colour = colour;
            StartTile = null;
        }
    }
}
