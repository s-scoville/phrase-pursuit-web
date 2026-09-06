using PhrasePursuitWeb.Core.AI;
using PhrasePursuitWeb.Core.Enums;
using PhrasePursuitWeb.Core.Managers;
using PhrasePursuitWeb.Core.Models;
using PhrasePursuitWeb.Tests.TestHelpers;
using System.Runtime.CompilerServices;

namespace PhrasePursuitWeb.Tests.Managers
{
    public class GameManagerTests
    {
        [Fact]
        public async Task StartNewGameAsync_ValidPlayerName_CreatesGameState()
        {
            // Arrange
            List<Puzzle> puzzles = new List<Puzzle>
            {
                new Puzzle ( 1, "MUSIC", "FAITHFULLY" ),
                new Puzzle ( 2, "MOVIES", "THE GODFATHER" ),
                new Puzzle ( 3, "TV SHOWS", "BREAKING BAD" ),
                new Puzzle ( 4, "BOOKS", "TO KILL A MOCKINGBIRD" )
            };
            FakeStorageService storageService = new FakeStorageService();
            PuzzleManager puzzleManager = new PuzzleManager(puzzles, storageService);
            SpinManager spinManager = new SpinManager();
            StatisticsManager statisticsManager = new StatisticsManager(storageService);
            AiController aiController = new AiController(AiDifficulty.Normal);
            GameManager gameManager = new GameManager(puzzleManager, spinManager, statisticsManager, aiController);

            // Act
            await gameManager.StartNewGameAsync("TestPlayer");

            // Assert
            Assert.NotNull(gameManager.CurrentGame);
            Assert.Contains(gameManager.CurrentGame.CurrentPuzzle, puzzles);
            Assert.Equal(3, gameManager.CurrentGame.Players.Count);
            Assert.Equal(PlayerType.Player, gameManager.CurrentGame.Players[0].PlayerType);
            Assert.Equal(PlayerType.Computer, gameManager.CurrentGame.Players[1].PlayerType);
            Assert.Equal(PlayerType.Computer, gameManager.CurrentGame.Players[2].PlayerType);
            Assert.Contains("TestPlayer", gameManager.CurrentGame.Players[0].Name);
            Assert.Equal(0, gameManager.CurrentGame.CurrentPlayerIndex);
        }


        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public async Task StartNewGameAsync_InvalidPlayerName_ThrowsArgumentException(string? playerName)
        {
            // Arrange
            List<Puzzle> puzzles = new List<Puzzle>
            {
                new Puzzle ( 1, "MUSIC", "FAITHFULLY" ),
                new Puzzle ( 2, "MOVIES", "THE GODFATHER" ),
                new Puzzle ( 3, "TV SHOWS", "BREAKING BAD" ),
                new Puzzle ( 4, "BOOKS", "TO KILL A MOCKINGBIRD" )
            };
            FakeStorageService storageService = new FakeStorageService();
            PuzzleManager puzzleManager = new PuzzleManager(puzzles, storageService);
            SpinManager spinManager = new SpinManager();
            StatisticsManager statisticsManager = new StatisticsManager(storageService);
            AiController aiController = new AiController(AiDifficulty.Normal);
            GameManager gameManager = new GameManager(puzzleManager, spinManager, statisticsManager, aiController);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(
                () => gameManager.StartNewGameAsync(playerName!));
        }


        [Fact]
        public async Task GetPuzzleDisplay_CurrentGame_ReturnsRenderedPuzzle()
        {
            // Arrange
            List<Puzzle> puzzles = new List<Puzzle>
            {
                new Puzzle ( 1, "MUSIC", "FAITHFULLY" )
            };
            FakeStorageService storageService = new FakeStorageService();
            PuzzleManager puzzleManager = new PuzzleManager(puzzles, storageService);
            SpinManager spinManager = new SpinManager();
            StatisticsManager statisticsManager = new StatisticsManager(storageService);
            AiController aiController = new AiController(AiDifficulty.Normal);
            GameManager gameManager = new GameManager(puzzleManager, spinManager, statisticsManager, aiController);
            await gameManager.StartNewGameAsync("TestPlayer");
            gameManager.CurrentGame.AddGuessedLetter('L');
            gameManager.CurrentGame.AddGuessedLetter('A');

            // Act
            string puzzleDisplay = gameManager.GetPuzzleDisplay();

            // Assert
            Assert.Equal("_A_____LL_", puzzleDisplay);
        }


        [Fact]
        public async Task Spin_InvalidPhase_ThrowsInvalidOperationException()
        {
            // Arrange
            List<Puzzle> puzzles = new List<Puzzle>
            {
                new Puzzle ( 1, "MUSIC", "FAITHFULLY" )
            };
            FakeStorageService storageService = new FakeStorageService();
            PuzzleManager puzzleManager = new PuzzleManager(puzzles, storageService);
            SpinManager spinManager = new SpinManager();
            StatisticsManager statisticsManager = new StatisticsManager(storageService);
            AiController aiController = new AiController(AiDifficulty.Normal);
            GameManager gameManager = new GameManager(puzzleManager, spinManager, statisticsManager, aiController);
            await gameManager.StartNewGameAsync("TestPlayer");
            gameManager.CurrentGame.SetPhase(TurnPhase.WaitingForVowel);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(
                () => gameManager.Spin());
        }


        [Fact]
        public async Task ApplySpinResult_Money_SetsSpinValueAndWaitsForConsonant()
        {
            // Arrange
            List<Puzzle> puzzles = new List<Puzzle>
            {
                new Puzzle ( 1, "MUSIC", "FAITHFULLY" )
            };
            FakeStorageService storageService = new FakeStorageService();
            PuzzleManager puzzleManager = new PuzzleManager(puzzles, storageService);
            SpinManager spinManager = new SpinManager();
            StatisticsManager statisticsManager = new StatisticsManager(storageService);
            AiController aiController = new AiController(AiDifficulty.Normal);
            GameManager gameManager = new GameManager(puzzleManager, spinManager, statisticsManager, aiController);
            await gameManager.StartNewGameAsync("TestPlayer");
            SpinResult result = new SpinResult(SpinOutcome.Money, 500);

            // Act
            gameManager.ApplySpinResult(result);

            // Assert
            Assert.Equal(500, gameManager.CurrentGame.CurrentSpinValue);
            Assert.Equal(TurnPhase.WaitingForConsonant, gameManager.CurrentGame.CurrentPhase);
        }


        [Fact]
        public async Task ApplySpinResult_Bankrupt_ResetsWinningsAndAdvancesTurn()
        {
            // Arrange
            List<Puzzle> puzzles = new List<Puzzle>
            {
                new Puzzle ( 1, "MUSIC", "FAITHFULLY" )
            };
            FakeStorageService storageService = new FakeStorageService();
            PuzzleManager puzzleManager = new PuzzleManager(puzzles, storageService);
            SpinManager spinManager = new SpinManager();
            StatisticsManager statisticsManager = new StatisticsManager(storageService);
            AiController aiController = new AiController(AiDifficulty.Normal);
            GameManager gameManager = new GameManager(puzzleManager, spinManager, statisticsManager, aiController);
            await gameManager.StartNewGameAsync("TestPlayer");
            gameManager.CurrentGame.Players[0].AddWinnings(1000);
            SpinResult result = new SpinResult(SpinOutcome.Bankrupt);

            // Act
            gameManager.ApplySpinResult(result);

            // Assert
            Assert.Equal(0, gameManager.CurrentGame.Players[0].CurrentWinnings);
            Assert.Equal(1, gameManager.CurrentGame.CurrentPlayerIndex);
        }


        [Fact]
        public async Task ApplySpinResult_LoseTurn_AdvancesTurnWithoutResettingWinnings()
        {
            // Arrange
            List<Puzzle> puzzles = new List<Puzzle>
            {
                new Puzzle ( 1, "MUSIC", "FAITHFULLY" )
            };
            FakeStorageService storageService = new FakeStorageService();
            PuzzleManager puzzleManager = new PuzzleManager(puzzles, storageService);
            SpinManager spinManager = new SpinManager();
            StatisticsManager statisticsManager = new StatisticsManager(storageService);
            AiController aiController = new AiController(AiDifficulty.Normal);
            GameManager gameManager = new GameManager(puzzleManager, spinManager, statisticsManager, aiController);
            await gameManager.StartNewGameAsync("TestPlayer");
            gameManager.CurrentGame.Players[0].AddWinnings(1000);
            SpinResult result = new SpinResult(SpinOutcome.LoseTurn);

            // Act
            gameManager.ApplySpinResult(result);

            // Assert
            Assert.Equal(1000, gameManager.CurrentGame.Players[0].CurrentWinnings);
            Assert.Equal(1, gameManager.CurrentGame.CurrentPlayerIndex);
        }


        [Fact]
        public async Task GuessConsonantAsync_CorrectGuess_AwardsWinningsAndKeepsTurn()
        {
            // Arrange
            List<Puzzle> puzzles = new List<Puzzle>
            {
                new Puzzle ( 1, "MUSIC", "FAITHFULLY" )
            };
            FakeStorageService storageService = new FakeStorageService();
            PuzzleManager puzzleManager = new PuzzleManager(puzzles, storageService);
            SpinManager spinManager = new SpinManager();
            StatisticsManager statisticsManager = new StatisticsManager(storageService);
            AiController aiController = new AiController(AiDifficulty.Normal);
            GameManager gameManager = new GameManager(puzzleManager, spinManager, statisticsManager, aiController);
            await gameManager.StartNewGameAsync("TestPlayer");
            gameManager.CurrentGame.Players[0].AddWinnings(5000);
            SpinResult result = new SpinResult(SpinOutcome.Money, 500);
            gameManager.ApplySpinResult(result);

            // Act
            GuessResult guessResult = await gameManager.GuessConsonantAsync('F');

            // Assert
            Assert.True(guessResult.WasCorrect);
            Assert.Equal(2, guessResult.Occurrences);
            Assert.Equal(1000, guessResult.MoneyEarned);
            Assert.Equal(6000, gameManager.CurrentGame.Players[0].CurrentWinnings);
            Assert.False(guessResult.TurnEnded);
            Assert.False(guessResult.PuzzleCompleted);
            Assert.Equal(0, gameManager.CurrentGame.CurrentPlayerIndex);
            Assert.Equal(TurnPhase.WaitingForAction, gameManager.CurrentGame.CurrentPhase);
        }


        [Fact]
        public async Task GuessConsonantAsync_IncorrectGuess_EndsTurnWithoutAwardingWinnings()
        {
            // Arrange
            List<Puzzle> puzzles = new List<Puzzle>
            {
                new Puzzle ( 1, "MUSIC", "FAITHFULLY" )
            };
            FakeStorageService storageService = new FakeStorageService();
            PuzzleManager puzzleManager = new PuzzleManager(puzzles, storageService);
            SpinManager spinManager = new SpinManager();
            StatisticsManager statisticsManager = new StatisticsManager(storageService);
            AiController aiController = new AiController(AiDifficulty.Normal);
            GameManager gameManager = new GameManager(puzzleManager, spinManager, statisticsManager, aiController);
            await gameManager.StartNewGameAsync("TestPlayer");
            gameManager.CurrentGame.Players[0].AddWinnings(5000);
            SpinResult result = new SpinResult(SpinOutcome.Money, 500);
            gameManager.ApplySpinResult(result);

            // Act
            GuessResult guessResult = await gameManager.GuessConsonantAsync('Z');

            // Assert
            Assert.False(guessResult.WasCorrect);
            Assert.Equal(0, guessResult.Occurrences);
            Assert.Equal(0, guessResult.MoneyEarned);
            Assert.Equal(5000, gameManager.CurrentGame.Players[0].CurrentWinnings);
            Assert.True(guessResult.TurnEnded);
            Assert.False(guessResult.PuzzleCompleted);
            Assert.Equal(1, gameManager.CurrentGame.CurrentPlayerIndex);
        }


        [Fact]
        public async Task BuyVowel_ValidPurchase_SetsWaitingForVowelPhase()
        {
            // Arrange
            List<Puzzle> puzzles = new List<Puzzle>
            {
                new Puzzle ( 1, "MUSIC", "FAITHFULLY" )
            };
            FakeStorageService storageService = new FakeStorageService();
            PuzzleManager puzzleManager = new PuzzleManager(puzzles, storageService);
            SpinManager spinManager = new SpinManager();
            StatisticsManager statisticsManager = new StatisticsManager(storageService);
            AiController aiController = new AiController(AiDifficulty.Normal);
            GameManager gameManager = new GameManager(puzzleManager, spinManager, statisticsManager, aiController);
            await gameManager.StartNewGameAsync("TestPlayer");
            gameManager.CurrentGame.Players[0].AddWinnings(500);

            // Act
            gameManager.BuyVowel();

            // Assert
            Assert.Equal(TurnPhase.WaitingForVowel, gameManager.CurrentGame.CurrentPhase);
            Assert.Equal(500, gameManager.CurrentGame.CurrentPlayer.CurrentWinnings);
        }


        [Fact]
        public async Task GuessVowelAsync_CorrectGuess_DeductsCostAndEndsTurn()
        {
            // Arrange
            List<Puzzle> puzzles = new List<Puzzle>
            {
                new Puzzle ( 1, "MUSIC", "FAITHFULLY" )
            };
            FakeStorageService storageService = new FakeStorageService();
            PuzzleManager puzzleManager = new PuzzleManager(puzzles, storageService);
            SpinManager spinManager = new SpinManager();
            StatisticsManager statisticsManager = new StatisticsManager(storageService);
            AiController aiController = new AiController(AiDifficulty.Normal);
            GameManager gameManager = new GameManager(puzzleManager, spinManager, statisticsManager, aiController);
            await gameManager.StartNewGameAsync("TestPlayer");
            gameManager.CurrentGame.Players[0].AddWinnings(500);
            gameManager.BuyVowel();

            // Act
            GuessResult guessResult = await gameManager.GuessVowelAsync('A');

            // Assert
            Assert.True(guessResult.WasCorrect);
            Assert.Equal(1, guessResult.Occurrences);
            Assert.Equal(0, guessResult.MoneyEarned);
            Assert.Equal(450, gameManager.CurrentGame.Players[0].CurrentWinnings);
            Assert.True(guessResult.TurnEnded);
            Assert.False(guessResult.PuzzleCompleted);
            Assert.Equal(1, gameManager.CurrentGame.CurrentPlayerIndex);
        }


        [Fact]
        public async Task SolvePuzzleAsync_CorrectAnswer_EndsGameAndRecordsWin()
        {
            // Arrange
            List<Puzzle> puzzles = new List<Puzzle>
            {
                new Puzzle ( 1, "MUSIC", "FAITHFULLY" )
            };
            FakeStorageService storageService = new FakeStorageService();
            PuzzleManager puzzleManager = new PuzzleManager(puzzles, storageService);
            SpinManager spinManager = new SpinManager();
            StatisticsManager statisticsManager = new StatisticsManager(storageService);
            AiController aiController = new AiController(AiDifficulty.Normal);
            GameManager gameManager = new GameManager(puzzleManager, spinManager, statisticsManager, aiController);
            await gameManager.StartNewGameAsync("TestPlayer");
            gameManager.CurrentGame.Players[0].AddWinnings(5000);


            // Act
            SolveResult solveResult = await gameManager.SolvePuzzleAsync("FAITHFULLY");

            // Assert
            Assert.True(solveResult.WasCorrect);
            Assert.True(gameManager.CurrentGame.IsGameOver);
            Assert.Equal("FAITHFULLY", gameManager.GetPuzzleDisplay());
            Assert.Equal(1, statisticsManager.Statistics.GamesPlayed);
            Assert.Equal(1, statisticsManager.Statistics.GamesWon);
            Assert.Equal(0, statisticsManager.Statistics.GamesLost);
            Assert.Equal(5000, statisticsManager.Statistics.LifetimeWinnings);
            Assert.Equal(5000, statisticsManager.Statistics.HighestWinnings);
        }


        [Fact]
        public async Task SolvePuzzleAsync_IncorrectAnswer_ResetsWinningsAndAdvancesTurn()
        {
            // Arrange
            List<Puzzle> puzzles = new List<Puzzle>
            {
                new Puzzle ( 1, "MUSIC", "FAITHFULLY" )
            };
            FakeStorageService storageService = new FakeStorageService();
            PuzzleManager puzzleManager = new PuzzleManager(puzzles, storageService);
            SpinManager spinManager = new SpinManager();
            StatisticsManager statisticsManager = new StatisticsManager(storageService);
            AiController aiController = new AiController(AiDifficulty.Normal);
            GameManager gameManager = new GameManager(puzzleManager, spinManager, statisticsManager, aiController);
            await gameManager.StartNewGameAsync("TestPlayer");
            gameManager.CurrentGame.Players[0].AddWinnings(5000);

            // Act
            SolveResult solveResult = await gameManager.SolvePuzzleAsync("WRONGANSWER");

            // Assert
            Assert.False(solveResult.WasCorrect);
            Assert.False(gameManager.CurrentGame.IsGameOver);
            Assert.Equal(0, gameManager.CurrentGame.Players[0].CurrentWinnings);
            Assert.Equal(1, gameManager.CurrentGame.CurrentPlayerIndex);
            Assert.Equal(0, statisticsManager.Statistics.GamesPlayed);
            Assert.Equal(0, statisticsManager.Statistics.GamesWon);
            Assert.Equal(0, statisticsManager.Statistics.GamesLost);
            Assert.Equal(0, statisticsManager.Statistics.LifetimeWinnings);
            Assert.Equal(0, statisticsManager.Statistics.HighestWinnings);
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task SolvePuzzleAsync_InvalidAnswer_ThrowsArgumentException(string? playerAnswer)
        {
            // Arrange
            List<Puzzle> puzzles = new List<Puzzle>
            {
                new Puzzle ( 1, "MUSIC", "FAITHFULLY" )
            };
            FakeStorageService storageService = new FakeStorageService();
            PuzzleManager puzzleManager = new PuzzleManager(puzzles, storageService);
            SpinManager spinManager = new SpinManager();
            StatisticsManager statisticsManager = new StatisticsManager(storageService);
            AiController aiController = new AiController(AiDifficulty.Normal);
            GameManager gameManager = new GameManager(puzzleManager, spinManager, statisticsManager, aiController);
            await gameManager.StartNewGameAsync("TestPlayer");

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(
                () => gameManager.SolvePuzzleAsync(playerAnswer!));
        }


        [Fact]
        public async Task BuyVowel_InsufficientWinnings_ThrowsInvalidOperationException()
        {
            // Arrange
            List<Puzzle> puzzles = new List<Puzzle>
            {
                new Puzzle ( 1, "MUSIC", "FAITHFULLY" )
            };
            FakeStorageService storageService = new FakeStorageService();
            PuzzleManager puzzleManager = new PuzzleManager(puzzles, storageService);
            SpinManager spinManager = new SpinManager();
            StatisticsManager statisticsManager = new StatisticsManager(storageService);
            AiController aiController = new AiController(AiDifficulty.Normal);
            GameManager gameManager = new GameManager(puzzleManager, spinManager, statisticsManager, aiController);
            await gameManager.StartNewGameAsync("TestPlayer");

            // Act & Assert
            Assert.Throws<InvalidOperationException>(
                () => gameManager.BuyVowel());
        }


        [Fact]
        public async Task GuessVowelAsync_IncorrectGuess_DeductsCostAndEndsTurn()
        {
            // Arrange
            List<Puzzle> puzzles = new List<Puzzle>
            {
                new Puzzle ( 1, "MUSIC", "FAITHFULLY" )
            };
            FakeStorageService storageService = new FakeStorageService();
            PuzzleManager puzzleManager = new PuzzleManager(puzzles, storageService);
            SpinManager spinManager = new SpinManager();
            StatisticsManager statisticsManager = new StatisticsManager(storageService);
            AiController aiController = new AiController(AiDifficulty.Normal);
            GameManager gameManager = new GameManager(puzzleManager, spinManager, statisticsManager, aiController);
            await gameManager.StartNewGameAsync("TestPlayer");
            gameManager.CurrentGame.Players[0].AddWinnings(500);
            gameManager.BuyVowel();

            // Act
            GuessResult guessResult = await gameManager.GuessVowelAsync('E');

            // Assert
            Assert.False(guessResult.WasCorrect);
            Assert.Equal(0, guessResult.Occurrences);
            Assert.Equal(0, guessResult.MoneyEarned);
            Assert.Equal(450, gameManager.CurrentGame.Players[0].CurrentWinnings);
            Assert.True(guessResult.TurnEnded);
            Assert.False(guessResult.PuzzleCompleted);
            Assert.Equal(1, gameManager.CurrentGame.CurrentPlayerIndex);
        }


        [Theory]
        [InlineData('A')]
        [InlineData('1')]
        [InlineData('#')]
        public async Task GuessConsonantAsync_InvalidLetter_ThrowsArgumentException(char letter)
        {
            // Arrange
            List<Puzzle> puzzles = new List<Puzzle>
            {
                new Puzzle ( 1, "MUSIC", "FAITHFULLY" )
            };
            FakeStorageService storageService = new FakeStorageService();
            PuzzleManager puzzleManager = new PuzzleManager(puzzles, storageService);
            SpinManager spinManager = new SpinManager();
            StatisticsManager statisticsManager = new StatisticsManager(storageService);
            AiController aiController = new AiController(AiDifficulty.Normal);
            GameManager gameManager = new GameManager(puzzleManager, spinManager, statisticsManager, aiController);
            await gameManager.StartNewGameAsync("TestPlayer");
            SpinResult result = new SpinResult(SpinOutcome.Money, 500);
            gameManager.ApplySpinResult(result);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(
                () => gameManager.GuessConsonantAsync(letter));
        }


        [Theory]
        [InlineData('P')]
        [InlineData('9')]
        [InlineData('#')]
        public async Task GuessVowelAsync_InvalidLetter_ThrowsArgumentException(char letter)
        {
            // Arrange
            List<Puzzle> puzzles = new List<Puzzle>
            {
                new Puzzle ( 1, "MUSIC", "FAITHFULLY" )
            };
            FakeStorageService storageService = new FakeStorageService();
            PuzzleManager puzzleManager = new PuzzleManager(puzzles, storageService);
            SpinManager spinManager = new SpinManager();
            StatisticsManager statisticsManager = new StatisticsManager(storageService);
            AiController aiController = new AiController(AiDifficulty.Normal);
            GameManager gameManager = new GameManager(puzzleManager, spinManager, statisticsManager, aiController);
            await gameManager.StartNewGameAsync("TestPlayer");
            gameManager.CurrentGame.Players[0].AddWinnings(500);
            gameManager.BuyVowel();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(
                () => gameManager.GuessVowelAsync(letter));
        }
    }
}
