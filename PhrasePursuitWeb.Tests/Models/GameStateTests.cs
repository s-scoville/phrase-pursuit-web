using PhrasePursuitWeb.Core.Models;
using PhrasePursuitWeb.Core.Enums;

namespace PhrasePursuitWeb.Tests.Models
{
    public class GameStateTests
    {
        [Fact]
        public void Constructor_ValidInputs_CreatesInitialGameState()
        {
            // Arrange
            Puzzle puzzle = new Puzzle(1, "MOVIE", "SUPERMAN RETURNS");
            List<Player> players = new List<Player>
            {
                new Player("Alice", PlayerType.Player),
                new Player("Bob", PlayerType.Computer),
                new Player("Charlie", PlayerType.Computer)
            };

            // Act
            GameState gameState = new GameState(puzzle, players);

            // Assert
            Assert.Equal(puzzle, gameState.CurrentPuzzle);
            Assert.Equal(players, gameState.Players);
            Assert.Equal(0, gameState.CurrentPlayerIndex);
            Assert.Equal(gameState.Players[0], gameState.CurrentPlayer);
            Assert.Empty(gameState.GuessedLetters);
            Assert.Equal(0, gameState.CurrentSpinValue);
            Assert.Equal(TurnPhase.WaitingForAction, gameState.CurrentPhase);
            Assert.False(gameState.IsGameOver);
        }


        [Fact]
        public void Constructor_NullPuzzle_ThrowsArgumentNullException()
        {
            // Arrange
            List<Player> players = new List<Player>
            {
                new Player("Alice", PlayerType.Player),
                new Player("Bob", PlayerType.Computer)
            };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => new GameState(null!, players));
        }


        [Fact]
        public void Constructor_NullPlayers_ThrowsArgumentException()
        {
            // Arrange
            Puzzle puzzle = new Puzzle(1, "MOVIE", "SUPERMAN RETURNS");

            // Act & Assert
            Assert.Throws<ArgumentException>(
                () => new GameState(puzzle, null!));
        }


        [Fact]
        public void Constructor_EmptyPlayers_ThrowsArgumentException()
        {
            // Arrange
            Puzzle puzzle = new Puzzle(1, "MOVIE", "SUPERMAN RETURNS");
            List<Player> players = new List<Player>();

            // Act & Assert
            Assert.Throws<ArgumentException>(
                () => new GameState(puzzle, players));
        }


        [Fact]
        public void AdvanceTurn_CurrentPlayer_AdvancesToNextPlayer()
        {
            // Arrange
            Puzzle puzzle = new Puzzle(1, "MOVIE", "SUPERMAN RETURNS");
            List<Player> players = new List<Player>
            {
                new Player("Alice", PlayerType.Player),
                new Player("Bob", PlayerType.Computer),
                new Player("Charlie", PlayerType.Computer)
            };
            GameState gameState = new GameState(puzzle, players);

            // Act
            gameState.AdvanceTurn();

            // Assert
            Assert.Equal(1, gameState.CurrentPlayerIndex);
            Assert.Equal(gameState.Players[1], gameState.CurrentPlayer);
            Assert.Equal("Bob", gameState.CurrentPlayer.Name);
        }


        [Fact]
        public void AdvanceTurn_LastPlayer_WrapsToFirstPlayer()
        {
            // Arrange
            Puzzle puzzle = new Puzzle(1, "MOVIE", "SUPERMAN RETURNS");
            List<Player> players = new List<Player>
            {
                new Player("Alice", PlayerType.Player),
                new Player("Bob", PlayerType.Computer),
                new Player("Charlie", PlayerType.Computer)
            };
            GameState gameState = new GameState(puzzle, players);

            // Act
            gameState.AdvanceTurn();
            gameState.AdvanceTurn();
            gameState.AdvanceTurn();

            // Assert
            Assert.Equal(0, gameState.CurrentPlayerIndex);
            Assert.Equal(gameState.Players[0], gameState.CurrentPlayer);
            Assert.Equal("Alice", gameState.CurrentPlayer.Name);
        }


        [Fact]
        public void AdvanceTurn_ActiveTurn_ResetsTurnState()
        {
            // Arrange
            Puzzle puzzle = new Puzzle(1, "MOVIE", "SUPERMAN RETURNS");
            List<Player> players = new List<Player>
            {
                new Player("Alice", PlayerType.Player),
                new Player("Bob", PlayerType.Computer)
            };
            GameState gameState = new GameState(puzzle, players);
            gameState.SetPhase(TurnPhase.WaitingForConsonant);
            gameState.SetSpinValue(500);

            // Act
            gameState.AdvanceTurn();

            // Assert
            Assert.Equal(TurnPhase.WaitingForAction, gameState.CurrentPhase);
            Assert.Equal(0, gameState.CurrentSpinValue);
        }


        [Fact]
        public void AddGuessedLetter_ValidLetter_AddsToGuessedLetters()
        {
            // Arrange
            Puzzle puzzle = new Puzzle(1, "MOVIE", "SUPERMAN RETURNS");
            List<Player> players = new List<Player>
            {
                new Player("Alice", PlayerType.Player),
                new Player("Bob", PlayerType.Computer),
                new Player("Charlie", PlayerType.Computer)
            };
            GameState gameState = new GameState(puzzle, players);

            // Act
            gameState.AddGuessedLetter('S');

            // Assert
            Assert.Contains('S', gameState.GuessedLetters);
        }


        [Fact]
        public void SetSpinValue_ValidValue_SetsCurrentSpinValue()
        {
            // Arrange
            Puzzle puzzle = new Puzzle(1, "MOVIE", "SUPERMAN RETURNS");
            List<Player> players = new List<Player>
            {
                new Player("Alice", PlayerType.Player),
                new Player("Bob", PlayerType.Computer),
                new Player("Charlie", PlayerType.Computer)
            };
            GameState gameState = new GameState(puzzle, players);

            // Act
            gameState.SetSpinValue(500);

            // Assert
            Assert.Equal(500, gameState.CurrentSpinValue);
        }


        [Fact]
        public void SetSpinValue_NegativeValue_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            Puzzle puzzle = new Puzzle(1, "MOVIE", "SUPERMAN RETURNS");
            List<Player> players = new List<Player>
            {
                new Player("Alice", PlayerType.Player),
                new Player("Bob", PlayerType.Computer),
                new Player("Charlie", PlayerType.Computer)
            };
            GameState gameState = new GameState(puzzle, players);

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => gameState.SetSpinValue(-500));
        }


        [Fact]
        public void SetPhase_ValidPhase_SetsCurrentPhase()
        {
            // Arrange
            Puzzle puzzle = new Puzzle(1, "MOVIE", "SUPERMAN RETURNS");
            List<Player> players = new List<Player>
            {
                new Player("Alice", PlayerType.Player),
                new Player("Bob", PlayerType.Computer),
                new Player("Charlie", PlayerType.Computer)
            };
            GameState gameState = new GameState(puzzle, players);

            // Act
            gameState.SetPhase(TurnPhase.WaitingForConsonant);

            // Assert
            Assert.Equal(TurnPhase.WaitingForConsonant, gameState.CurrentPhase);
        }


        [Fact]
        public void ResetSpinValue_ResetsCurrentSpinValueToZero()
        {
            // Arrange
            Puzzle puzzle = new Puzzle(1, "MOVIE", "SUPERMAN RETURNS");
            List<Player> players = new List<Player>
            {
                new Player("Alice", PlayerType.Player),
                new Player("Bob", PlayerType.Computer),
                new Player("Charlie", PlayerType.Computer)
            };
            GameState gameState = new GameState(puzzle, players);
            gameState.SetSpinValue(500);

            // Act
            gameState.ResetSpinValue();

            // Assert
            Assert.Equal(0, gameState.CurrentSpinValue);
        }


        [Fact]
        public void EndGame_ActiveGame_EndsGameAndSetsPhaseToGameOver()
        {
            // Arrange
            Puzzle puzzle = new Puzzle(1, "MOVIE", "SUPERMAN RETURNS");
            List<Player> players = new List<Player>
            {
                new Player("Alice", PlayerType.Player),
                new Player("Bob", PlayerType.Computer),
                new Player("Charlie", PlayerType.Computer)
            };
            GameState gameState = new GameState(puzzle, players);
            
            // Act
            gameState.EndGame();
            
            // Assert
            Assert.True(gameState.IsGameOver);
            Assert.Equal(TurnPhase.GameOver, gameState.CurrentPhase);
        }
    }
}
