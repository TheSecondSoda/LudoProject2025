Feature: LudoController

The controller for ludo functionality

Scenario: The game is started with 4 players on a standard board
	Given I have 4 players
	And The board size is 52
	And The game backend is initialized
	When I call the StartGame endpoint
	Then I should recieve a list of 4 players
	And Each player should have pieces assigned
	And Each player should have a unique color and start tile

Scenario: The game is stared with 2 players on a small board
	Given I have 2 players
	And The board size is 36
	And The game backend is initialized
	When I call the StartGame endpoint
	Then I should recieve a list of 2 players
	And Each player should have pieces assigned
	And Each player should have a unique color and start tile

Scenario: Player rolls the die and gets valid moves
	Given I have 4 players
	And The board size is 52
    And the dice should always roll 4
	And The dice should first roll for player order
	And The game backend is initialized
	And The game has started
	And First player has at least one piece that can be moved
    When the player rolls the die
    Then the response should contain a dice roll of 4
    And the response should contain a non-empty list of valid pieces
    And canReroll should be "false"

Scenario: Player rolls a 6 and is allowed to reroll
  Given I have 4 players
  And The board size is 52
  And the dice should always roll 6
  And The dice should first roll for player order
  And The game backend is initialized
  And The game has started
  When the player rolls the die
  Then the response should contain a dice roll of 6
  And the response should contain a non-empty list of valid pieces
  And canReroll should be "true"

Scenario: Player is allowed multiple attempts to roll a 6 to enter a piece
  Given I have 4 players
  And The board size is 52
  And The dice rolls are 2, 5, 3
  And The dice should first roll for player order
  And The game backend is initialized
  And The game has started
  When the player rolls the die 3 times
  Then the responses should be:
    | roll | canReroll |
    | 2    | true      |
    | 5    | true      |
    | 3    | false     |

Scenario: Player moves a valid piece and turn passes to the next player
  Given I have 4 players
  And The board size is 52
  And The dice rolls are 3
  And The dice should first roll for player order
  And The game backend is initialized
  And The game has started
  And Current player has a piece on the board at position 5
  When the player rolls the die
  And the player moves the first valid piece
  Then the response should contain updated piece states
  And the response should include the next player's ID
