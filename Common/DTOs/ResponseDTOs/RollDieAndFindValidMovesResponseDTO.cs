using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTOs.ResponseDTOs
{
	public class RollDieAndFindValidMovesResponseDTO
	{
		public int diceroll { get; init; }
		public List<Guid>? validPieces { get; init; }
		public bool canReroll { get; init; }
	}
}
