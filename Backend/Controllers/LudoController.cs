using Backend.Domains.PieceDomain;
using Backend.Services.GameManagerService;
using Common.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LudoController : ControllerBase
	{
		private IGameManagerService GameManager { get; }
		private ILogger<LudoController> Logger { get; }

		public LudoController(IGameManagerService gameManager, ILogger<LudoController> logger)
		{
			GameManager = gameManager;
			Logger = logger;
		}

		[HttpGet("FindValidMoves")]
		public async Task<IActionResult> FindValidMoves(int diceroll)
		{
			try
			{
				GameManager.Roll(diceroll);
				var resultValue = GameManager.GetMovablePieces();

				return new OkObjectResult(resultValue);
			}
			catch (Exception ex)
			{
				Logger.LogError(ex, "Error");
				return new BadRequestObjectResult("An exception occurred, please check logs");
			}
		}

		[HttpGet("MoveSelectedPiece")]
		public async Task<IActionResult> MoveSelectedPiece(Guid pieceID)
		{
			try
			{
				var affectedPieces = GameManager.MovePiece(pieceID);

				var resultValue = from p in affectedPieces
								  select new PieceDTO()
								  {
									  ID = p.ID,
									  PosIndex = p.PosIndex,
									  IsInPlay = p.IsInPlay,
									  IsFinished = p.IsFinished
								  };

				return new OkObjectResult(affectedPieces);
			}
			catch (Exception ex)
			{
				Logger.LogError(ex, "Error");
				return new BadRequestObjectResult("An exception occurred, please check logs");
			}
		}

		[HttpGet("StartGame")]
		public async Task<IActionResult> StartGame(int PlayerNumber, int BoardSize)
		{
			try
			{
				GameManager.CreateNewGame(PlayerNumber, BoardSize, 6);
				var playersInOrder = GameManager.RollForPlayerOrder();

				var resultValue = from p in playersInOrder
								  select new PlayerDTO()
								  {
									  id = p.Id,
									  colour = p.Colour,
									  startTile = p.StartTile,
									  pieces = (List<PieceDTO>)(from piece in p.Pieces
											   select new PieceDTO()
											   {
												   ID = piece.ID,
												   PosIndex = piece.PosIndex,
												   IsInPlay = piece.IsInPlay,
												   IsFinished = piece.IsFinished
											   }).ToList()
								  };

				return new OkObjectResult(resultValue);
			}
			catch (Exception ex)
			{
				Logger.LogError(ex, "Error");
				return new BadRequestObjectResult("An exception occurred, please check logs");
			}
		}

		[HttpGet("NextTurn")]
		public async Task<IActionResult> NextTurn()
		{
			try
			{
				var nextPlayer = GameManager.NextTurn();
				return new OkObjectResult(nextPlayer);
			}
			catch (Exception ex)
			{
				Logger.LogError(ex, "Error");
				return new BadRequestObjectResult("An exception occurred, please check logs");
			}
		}
	}
}
