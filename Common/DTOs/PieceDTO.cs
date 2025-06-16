using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTOs
{
	public class PieceDTO
	{
		public Guid ID { get; init; }
        public ColourEnum Colour { get; init; }
        public PosIndex? PosIndex { get; init; }
		public bool IsInPlay { get; init; }
		public bool IsFinished { get; init; }
	}
}
