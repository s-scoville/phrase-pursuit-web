using PhrasePursuitWeb.Core.Enums;
using PhrasePursuitWeb.Core.Models;
using PhrasePursuitWeb.Core.AI;

namespace PhrasePursuitWeb.Tests.AI
{
    public class AiControllerTests
    {
        [Fact]
        public void ChooseConsonant_NormalDifficulty_ReturnsHighestPriorityAvailableConsonant()
        {
            // Arrange
            Puzzle puzzle = new Puzzle(1, "MUSIC", "HEY THERE DELILAH");

            List<Player> playerList = new List<Player>
            {
                new Player("Steve", PlayerType.Player),
                new Player("Al", PlayerType.Computer),
                new Player("Bob", PlayerType.Computer)
            };

            GameState newGame = new GameState(puzzle, playerList);

            newGame.AddGuessedLetter('R');
            newGame.AddGuessedLetter('S');
            newGame.AddGuessedLetter('T');

            AiController aiController = new AiController(AiDifficulty.Normal);

            // Act
            char chosenConsonant = aiController.ChooseConsonant(newGame);

            // Assert
            Assert.Equal('L', chosenConsonant);
        }


        [Fact]
        public void ChooseConsonant_EasyDifficulty_ReturnsAvailableConsonant()
        {
            // Arrange
            Puzzle puzzle = new Puzzle(1, "MUSIC", "HEY THERE DELILAH");
            List<Player> playerList = new List<Player>
            {
                new Player("Steve", PlayerType.Player),
                new Player("Al", PlayerType.Computer),
                new Player("Bob", PlayerType.Computer)
            };
            GameState newGame = new GameState(puzzle, playerList);
            HashSet<char> guessedLetters = "FGHJKLMNPQRSTVWXYZ".ToHashSet();
            foreach (char letter in guessedLetters)
            {
                newGame.AddGuessedLetter(letter);
            }
            AiController aiController = new AiController(AiDifficulty.Easy);

            // Act
            char chosenConsonant = aiController.ChooseConsonant(newGame);

            // Assert
            Assert.Contains(chosenConsonant, "BCD".ToHashSet());
            Assert.DoesNotContain(chosenConsonant, newGame.GuessedLetters);
        }


        [Fact]
        public void ChooseConsonant_AllConsonantsGuessed_ThrowsInvalidOperationException()
        {
            // Arrange
            Puzzle puzzle = new Puzzle(1, "VIDEO GAME", "SONIC THE HEDGEHOG");
            List<Player> playerList = new List<Player>
            {
                new Player("Steve", PlayerType.Player),
                new Player("Al", PlayerType.Computer),
                new Player("Bob", PlayerType.Computer)
            };
            GameState newGame = new GameState(puzzle, playerList);
            HashSet<char> guessedLetters = "BCDFGHJKLMNPQRSTVWXYZ".ToHashSet();
            foreach (char letter in guessedLetters)
            {
                newGame.AddGuessedLetter(letter);
            }
            AiController aiController = new AiController(AiDifficulty.Normal);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(
                () => aiController.ChooseConsonant(newGame));
        }


        [Fact]
        public void ChooseVowel_NormalDifficulty_ReturnsHighestPriorityAvailableVowel()
        {
            // Arrange
            Puzzle puzzle = new Puzzle(1, "MUSIC", "HEY THERE DELILAH");
            List<Player> playerList = new List<Player>
            {
                new Player("Steve", PlayerType.Player),
                new Player("Al", PlayerType.Computer),
                new Player("Bob", PlayerType.Computer)
            };
            GameState newGame = new GameState(puzzle, playerList);
            AiController aiController = new AiController(AiDifficulty.Normal);
            newGame.AddGuessedLetter('E');
            newGame.AddGuessedLetter('A');

            // Act
            char chosenVowel = aiController.ChooseVowel(newGame);

            // Assert
            Assert.Equal('O', chosenVowel);
        }


        [Fact]
        public void ChooseVowel_EasyDifficulty_ReturnsAvailableVowel()
        {
            // Arrange
            Puzzle puzzle = new Puzzle(1, "MUSIC", "HEY THERE DELILAH");
            List<Player> playerList = new List<Player>
            {
                new Player("Steve", PlayerType.Player),
                new Player("Al", PlayerType.Computer),
                new Player("Bob", PlayerType.Computer)
            };
            GameState newGame = new GameState(puzzle, playerList);
            AiController aiController = new AiController(AiDifficulty.Easy);
            HashSet<char> guessedLetters = "IOU".ToHashSet();
            foreach (char letter in guessedLetters)
            {
                newGame.AddGuessedLetter(letter);
            }

            // Act
            char chosenVowel = aiController.ChooseVowel(newGame);

            // Assert
            Assert.Contains(chosenVowel, "AE".ToHashSet());
            Assert.DoesNotContain(chosenVowel, newGame.GuessedLetters);
        }


        [Fact]
        public void ChooseVowel_AllVowelsGuessed_ThrowsInvalidOperationException()
        {
            // Arrange
            Puzzle puzzle = new Puzzle(1, "VIDEO GAME", "SONIC THE HEDGEHOG");
            List<Player> playerList = new List<Player>
            {
                new Player("Steve", PlayerType.Player),
                new Player("Al", PlayerType.Computer),
                new Player("Bob", PlayerType.Computer)
            };
            GameState newGame = new GameState(puzzle, playerList);
            HashSet<char> guessedLetters = "AEIOU".ToHashSet();
            foreach (char letter in guessedLetters)
            {
                newGame.AddGuessedLetter(letter);
            }
            AiController aiController = new AiController(AiDifficulty.Normal);
            
            // Act & Assert
            Assert.Throws<InvalidOperationException>(
                () => aiController.ChooseVowel(newGame));
        }


        [Theory]
        [InlineData(AiDifficulty.Easy)]
        [InlineData(AiDifficulty.Normal)]
        public void AttemptSolve_LowPuzzleCompletion_ReturnsEmptyString(AiDifficulty difficulty)
        {
            // Arrange
            Puzzle currentPuzzle = new Puzzle(1, "RANDOM", "EEEAAAA");
            List<Player> playerList = new List<Player>
            {
                new Player("Steve", PlayerType.Player),
                new Player("Al", PlayerType.Computer),
                new Player("Bob", PlayerType.Computer)
            };
            GameState newGame = new GameState(currentPuzzle, playerList);
            AiController aiController = new AiController(difficulty);
            newGame.AddGuessedLetter('E');

            // Act
            string result = aiController.AttemptSolve(newGame);

            // Assert
            Assert.Equal(string.Empty, result);
        }


        [Fact]
        public void GetThinkingTime_MultipleCalls_ReturnsValueWithinExpectedRange()
        {
            // Arrange
            AiController aiController = new AiController(AiDifficulty.Normal);
            int numberOfCalls = 1000;

            // Act
            for (int i = 0; i < numberOfCalls; i++)
            {
                int thinkingTime = aiController.GetThinkingTime();
                
                // Assert
                Assert.InRange(thinkingTime, 1, 6);
            }
        }


        [Theory]
        [InlineData(AiDifficulty.Easy)]
        [InlineData(AiDifficulty.Normal)]
        public void ChooseAction_LowPuzzleCompletionAndNoWinnings_ReturnsSpin(AiDifficulty difficulty)
        {
            // Arrange
            Puzzle currentPuzzle = new Puzzle(1, "RANDOM", "EEEAAAA");
            List<Player> playerList = new List<Player>
            {
                new Player("Steve", PlayerType.Player),
                new Player("Al", PlayerType.Computer),
                new Player("Bob", PlayerType.Computer)
            };
            GameState newGame = new GameState(currentPuzzle, playerList);
            AiController aiController = new AiController(difficulty);
            newGame.AddGuessedLetter('E');
            
            // Act
            AiAction chosenAction = aiController.ChooseAction(newGame);
            
            // Assert
            Assert.Equal(AiAction.Spin, chosenAction);
        }
    }
}
