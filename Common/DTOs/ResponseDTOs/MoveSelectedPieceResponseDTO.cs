using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTOs.ResponseDTOs
{
	public class MoveSelectedPieceResponseDTO
	{
		public List<PieceDTO> affectedPieces { get; init; }
		public Guid nextPlayerID { get; init; }
	}
}
