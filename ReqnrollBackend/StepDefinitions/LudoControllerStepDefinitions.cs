using Backend.Controllers;
using Backend.Services.DiceServices;
using Backend.Services.DiceServices.Interfaces;
using Backend.Services.GameManagerService;
using Common.DTOs;
using Common.DTOs.ResponseDTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Reqnroll;
using ReqnrollBackend.TestInfrastructure;
using ReqnrollBackend.TestInfrastructure.DiceFakes;
using System;
using System.Linq;

namespace ReqnrollBackend.StepDefinitions
{
    [Binding]
    public class LudoControllerStepDefinitions
    {
        private LudoBackendFactory _factory = new();
        private LudoController _controller;
        private List<IActionResult> _responses = new();
		private IDiceService? _diceService = null;
		private IServiceProvider _services;
		private IGameManagerService _gameManager;

		private int _playerNumber;
        private int _boardSize;

        [Given("I have {int} players")]
        public void GivenIHavePlayers(int numberOfPlayers)
        {
            _playerNumber = numberOfPlayers;
        }

        [Given("The board size is {int}")]
        public void GivenTheBoardSizeIs(int boardSize)
        {
            _boardSize = boardSize;
        }

		[Given("The game backend is initialized")]
		public void GivenTheGameBackendIsInitialized()
		{
			_factory = new LudoBackendFactory(_diceService);
			var scope = _factory.Services.CreateScope();
			_services = scope.ServiceProvider;

			_gameManager = _services.GetRequiredService<IGameManagerService>();
			var logger = _services.GetRequiredService<ILogger<LudoController>>();

			_controller = new LudoController(_gameManager, _diceService, logger);
		}


		[When("I call the StartGame endpoint")]
        public async Task WhenICallTheStartGameEndpoint()
        {
			_responses.Add(await _controller.StartGame(_playerNumber, _boardSize));
		}

        [Then("I should recieve a list of {int} players")]
        public void ThenIShouldRecieveAListOfPlayers(int expectedNumberOfPlayers)
        {
			var okResult = Assert.IsType<OkObjectResult>(_responses.FirstOrDefault());
			var players = Assert.IsAssignableFrom<IEnumerable<PlayerDTO>>(okResult.Value);

			Assert.Equal(expectedNumberOfPlayers, players.Count());
		}

        [Then("Each player should have pieces assigned")]
        public void ThenEachPlayerShouldHavePiecesAssigned()
        {
			var okResult = (OkObjectResult)_responses.FirstOrDefault();
			var players = (IEnumerable<PlayerDTO>)okResult.Value;

			Assert.All(players, p => Assert.NotEmpty(p.pieces));
		}

        [Then("Each player should have a unique color and start tile")]
        public void ThenEachPlayerShouldHaveAUniqueColorAndStartTile()
        {
			var okResult = (OkObjectResult)_responses.FirstOrDefault();
			var players = (IEnumerable<PlayerDTO>)okResult.Value;

			var colours = players.Select(p => p.colour).ToList();
			var startTiles = players.Select(p => p.startTile).ToList();

			Assert.Equal(colours.Count, colours.Distinct().Count());
			Assert.Equal(startTiles.Count, startTiles.Distinct().Count());
		}

		[Given("the dice should always roll {int}")]
		public void GivenTheDiceShouldAlwaysRoll(int roll)
		{
			IDiceService diceService = new SpecificValueDiceService(roll);
			_diceService = diceService;
		}

		[Given("The dice should first roll for player order")]
		public void GivenTheDiceShouldFirstRollForPlayerOrder()
		{
			int maxRoll = 6;
			var rollsForPlayerOrder = Enumerable.Range(maxRoll - _playerNumber + 1, _playerNumber)
												.Reverse();

			QueueBasedDiceService diceServiceWithOrderRolls;

			if (_diceService != null)
			{
				diceServiceWithOrderRolls = new QueueBasedDiceService(_diceService);
			}
			else
			{
				diceServiceWithOrderRolls = new QueueBasedDiceService();
			}

			diceServiceWithOrderRolls.EnqueueRolls(rollsForPlayerOrder);
			_diceService = diceServiceWithOrderRolls;
		}


		[Given("The game has started")]
		public void GivenTheGameHasStarted()
		{
			_controller.StartGame(_playerNumber, _boardSize);
		}

		[Given("First player has at least one piece that can be moved")]
		public void GivenFirstPlayerHasAtLeastOnePieceThatCanBeMoved()
		{
			var player = _gameManager.GetCurrentPlayer();
			var piece = player.Pieces.First();
			piece.PosIndex = new(0, Common.Enums.ColourEnum.None); // or any index on the board that makes it movable
			piece.IsInPlay = true;
			piece.IsFinished = false;
		}

		[When("the player rolls the die")]
		public async Task WhenThePlayerRollsTheDie()
		{
			_responses.Add(await _controller.RollDieAndFindValidMoves());
		}

		[Then("the response should contain a dice roll of {int}")]
		public void ThenTheResponseShouldContainADiceRollOf(int expected)
		{
			var dto = (_responses.FirstOrDefault() as OkObjectResult)?.Value as RollDieAndFindValidMovesResponseDTO;
			dto.Should().NotBeNull();
			dto.diceroll.Should().Be(expected);
		}

		[Then("the response should contain a non-empty list of valid pieces")]
		public void ThenTheResponseShouldContainANon_EmptyListOfValidPieces()
		{
			var dto = (_responses.FirstOrDefault() as OkObjectResult)?.Value as RollDieAndFindValidMovesResponseDTO;
			dto.validPieces.Should().NotBeNullOrEmpty();
		}

		[Then("canReroll should be {string}")]
		public void ThenCanRerollShouldBe(string expected)
		{
			bool expectedBool = bool.Parse(expected);
			var dto = (_responses.FirstOrDefault() as OkObjectResult)?.Value as RollDieAndFindValidMovesResponseDTO;
			dto.canReroll.Should().Be(expectedBool);
		}

		[Given("The dice rolls are (.*)")]
		public void GivenTheDiceRollsAre(string commaSeparatedRolls)
		{
			var rolls = commaSeparatedRolls
						.Split(',')
						.Select(s => int.Parse(s.Trim()))
						.ToList();

			var diceService = new QueueBasedDiceService();

			diceService.EnqueueRolls(rolls);
			_diceService = diceService;
		}

		[When("the player rolls the die {int} times")]
		public async Task WhenThePlayerRollsTheDieTimes(int rollTimes)
		{
			for (int i = 0; i < rollTimes; i++)
			{
				var result = await _controller.RollDieAndFindValidMoves();
				_responses.Add(result);
			}
		}

		[Then("the responses should be:")]
		public void ThenTheResponsesShouldBe(DataTable dataTable)
		{
			var expected = dataTable.Rows.Select(r => new
			{
				roll = int.Parse(r["roll"]),
				canReroll = bool.Parse(r["canReroll"])
			}).ToList();

			var rollResponses = _responses
				.OfType<OkObjectResult>()
				.Where(r => r.Value is RollDieAndFindValidMovesResponseDTO)
				.Select(r => r.Value as RollDieAndFindValidMovesResponseDTO)
				.ToList();

			rollResponses.Count.Should().Be(expected.Count, "the number of RollDie responses should match the expected table");

			for (int i = 0; i < expected.Count; i++)
			{
				var dto = rollResponses[i]!;

				dto.diceroll.Should().Be(expected[i].roll, $"Roll {i + 1} should match expected value");
				dto.canReroll.Should().Be(expected[i].canReroll, $"canReroll for roll {i + 1} should match expected value");
			}
		}

		[Given("Current player has a piece on the board at position {int}")]
		public void GivenCurrentPlayerHasAPieceOnTheBoardAtPosition(int index)
		{
			var player = _gameManager.GetCurrentPlayer();
			var piece = player.Pieces.Where(p => p.IsInPlay == false).First();
			piece.PosIndex = new(index, Common.Enums.ColourEnum.None); // or any index on the board that makes it movable
			piece.IsInPlay = true;
			piece.IsFinished = false;
		}

		[When("the player moves the first valid piece")]
		public async Task WhenThePlayerMovesTheFirstValidPiece()
		{
			var lastRollResponse = _responses
				.OfType<OkObjectResult>()
				.Select(r => r.Value)
				.OfType<RollDieAndFindValidMovesResponseDTO>()
				.LastOrDefault();

			lastRollResponse.Should().NotBeNull("there should be a previous roll response");

			var pieceID = lastRollResponse!.validPieces.First();

			var moveResult = await _controller.MoveSelectedPieceAndNextTurn(pieceID);
			_responses.Add(moveResult);
		}

		[Then("the response should contain updated piece states")]
		public void ThenTheResponseShouldContainUpdatedPieceStates()
		{
			var response = _responses
				.OfType<OkObjectResult>()
				.Select(r => r.Value)
				.OfType<MoveSelectedPieceResponseDTO>()
				.LastOrDefault();

			response.Should().NotBeNull("last response should be from MoveSelectedPiece endpoint");
			response!.affectedPieces.Should().NotBeEmpty("it should include affected pieces");
		}

		[Then("the response should include the next player's ID")]
		public void ThenTheResponseShouldIncludeTheNextPlayersID()
		{
			var response = _responses
				.OfType<OkObjectResult>()
				.Select(r => r.Value)
				.OfType<MoveSelectedPieceResponseDTO>()
				.LastOrDefault();

			response.Should().NotBeNull("expected a MoveSelectedPieceResponseDTO");
			response!.nextPlayerID.Should().NotBe(Guid.Empty, "nextPlayerID should be set");
		}

	}
}
