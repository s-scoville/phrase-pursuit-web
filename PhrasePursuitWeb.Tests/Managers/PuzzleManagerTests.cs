using PhrasePursuitWeb.Core.Managers;
using PhrasePursuitWeb.Core.Models;
using PhrasePursuitWeb.Tests.TestHelpers;

namespace PhrasePursuitWeb.Tests.Managers
{
    public class PuzzleManagerTests
    {
        [Fact]
        public void Constructor_ValidInputs_CreatesPuzzleManager()
        {
            // Arrange
            var puzzles = new List<Puzzle>
            {
                new Puzzle(1, "MOVIE", "SUPERMAN RETURNS"),
                new Puzzle(2, "MUSIC", "SIMPLE MAN"),
                new Puzzle(3, "BOOK", "TO KILL A MOCKINGBIRD")
            };

            // Act
            var puzzleManager = new PuzzleManager(puzzles, new FakeStorageService());

            // Assert
            Assert.NotNull(puzzleManager);
        }


        [Fact]
        public void Constructor_NullPuzzles_ThrowsArgumentException()
        {
            // Arrange
            List<Puzzle> puzzles = null!;

            // Act & Assert
            Assert.Throws<ArgumentException>(
                () => new PuzzleManager(puzzles, new FakeStorageService()));
        }


        [Fact]
        public void Constructor_EmptyPuzzles_ThrowsArgumentException()
        {
            // Arrange
            var puzzles = new List<Puzzle>();

            // Act & Assert
            Assert.Throws<ArgumentException>(
                () => new PuzzleManager(puzzles, new FakeStorageService()));
        }


        [Fact]
        public void Constructor_NullStorageService_ThrowsArgumentNullException()
        {
            // Arrange
            var puzzles = new List<Puzzle>
            {
                new Puzzle(1, "MOVIE", "SUPERMAN RETURNS")
            };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => new PuzzleManager(puzzles, null!));
        }


        [Fact]
        public async Task GetRandomPuzzleAsync_ReturnsPuzzleFromList()
        {
            // Arrange
            var puzzles = new List<Puzzle>
            {
                new Puzzle(1, "MOVIE", "SUPERMAN RETURNS"),
                new Puzzle(2, "MUSIC", "SIMPLE MAN"),
                new Puzzle(3, "BOOK", "TO KILL A MOCKINGBIRD")
            };

            // Act
            var puzzleManager = new PuzzleManager(puzzles, new FakeStorageService());
            var randomPuzzle = await puzzleManager.GetRandomPuzzleAsync();

            // Assert
            Assert.NotNull(randomPuzzle);
            Assert.Contains(randomPuzzle, puzzles);
        }


        [Fact]
        public async Task GetRandomPuzzleAsync_SelectedPuzzle_SavesPuzzleIdAsPlayed()
        {
            // Arrange
            List<Puzzle> puzzles = new List<Puzzle>
            {
                new Puzzle(1, "MOVIES", "SUPERMAN"),
                new Puzzle(2, "MOVIES", "BATMAN"),
                new Puzzle(3, "MOVIES", "THE GODFATHER")
            };

            FakeStorageService storageService = new FakeStorageService();
            PuzzleManager puzzleManager = new PuzzleManager(puzzles, storageService);

            // Act
            Puzzle selectedPuzzle = await puzzleManager.GetRandomPuzzleAsync();

            HashSet<int>? playedPuzzleIds =
                await storageService.LoadAsync<HashSet<int>>("playedPuzzleIds");

            // Assert
            Assert.NotNull(playedPuzzleIds);
            Assert.Contains(selectedPuzzle.Id, playedPuzzleIds);
        }


        [Fact]
        public async Task GetRandomPuzzleAsync_PreviouslyPlayedPuzzle_ExcludesPuzzleFromSelection()
        {
            // Arrange
            var puzzles = new List<Puzzle>
            {
                new Puzzle(1, "MOVIE", "SUPERMAN RETURNS"),
                new Puzzle(2, "MUSIC", "SIMPLE MAN"),
                new Puzzle(3, "BOOK", "TO KILL A MOCKINGBIRD")
            };

            FakeStorageService storageService = new FakeStorageService();

            await storageService.SaveAsync(
                "playedPuzzleIds",
                new HashSet<int> { 1 });

            var puzzleManager = new PuzzleManager(puzzles, storageService);

            // Act
            Puzzle randomPuzzle = await puzzleManager.GetRandomPuzzleAsync();

            // Assert
            Assert.NotEqual(1, randomPuzzle.Id);
        }


        [Fact]
        public async Task GetRandomPuzzleAsync_AllPuzzlesPlayed_ResetsHistoryAndReturnsNewPuzzle()
        {
            // Arrange
            var puzzles = new List<Puzzle>
            {
                new Puzzle(1, "MOVIE", "SUPERMAN RETURNS"),
                new Puzzle(2, "MUSIC", "SIMPLE MAN"),
                new Puzzle(3, "BOOK", "TO KILL A MOCKINGBIRD")
            };
            var storageService = new FakeStorageService();
            var puzzleManager = new PuzzleManager(puzzles, storageService);

            for (int i = 0; i < puzzles.Count; i++)
            {
                await puzzleManager.GetRandomPuzzleAsync();
            }

            // Act
            var newPuzzle = await puzzleManager.GetRandomPuzzleAsync();
            HashSet<int>? playedPuzzleIds =
                await storageService.LoadAsync<HashSet<int>>("playedPuzzleIds");

            // Assert
            Assert.NotNull(playedPuzzleIds);
            Assert.Single(playedPuzzleIds);
            Assert.Contains(newPuzzle.Id, playedPuzzleIds);
        }


        [Fact]
        public void RenderPuzzle_GuessedLetters_RevealsGuessedLetters()
        {
            // Arrange
            var puzzle = new Puzzle(1, "MOVIE", "SUPERMAN RETURNS");
            var guessedLetters = new HashSet<char> { 'S', 'U', 'P', 'E', 'R' };
            var puzzleManager = new PuzzleManager(new List<Puzzle> { puzzle }, new FakeStorageService());

            // Act
            string renderedPuzzle = puzzleManager.RenderPuzzle(puzzle, guessedLetters);

            // Assert
            Assert.Contains("SUPER", renderedPuzzle);
            Assert.DoesNotContain("MAN", renderedPuzzle);
        }


        [Fact]
        public void RenderPuzzle_UnguessedLetters_HidesUnguessedLetters()
        {
            // Arrange
            var puzzle = new Puzzle(1, "ANIMAL", "CAT");
            var guessedLetters = new HashSet<char> { 'C' };
            var puzzleManager = new PuzzleManager(new List<Puzzle> { puzzle }, new FakeStorageService());

            // Act
            string renderedPuzzle = puzzleManager.RenderPuzzle(puzzle, guessedLetters);

            // Assert
            Assert.Contains("C__", renderedPuzzle);
        }


        [Fact]
        public void RenderPuzzle_NonLetterCharacters_PreservesNonLetterCharacters()
        {
            // Arrange
            var puzzle = new Puzzle(1, "MUSIC", "I'M YOURS");
            var guessedLetters = new HashSet<char>();
            var puzzleManager = new PuzzleManager(new List<Puzzle> { puzzle }, new FakeStorageService());

            // Act
            string renderedPuzzle = puzzleManager.RenderPuzzle(puzzle, guessedLetters);

            // Assert
            Assert.Contains("'", renderedPuzzle);
        }


        [Fact]
        public void ContainsLetter_LetterInPhrase_ReturnsTrue()
        {
            // Arrange
            var puzzle = new Puzzle(1, "MOVIE", "SUPERMAN RETURNS");
            var puzzleManager = new PuzzleManager(new List<Puzzle> { puzzle }, new FakeStorageService());
            char letterToCheck = 's';
            
            // Act
            bool containsLetter = puzzleManager.ContainsLetter(puzzle, letterToCheck);
            
            // Assert
            Assert.True(containsLetter);
        }


        [Fact]
        public void CountOccurrences_LetterInPhrase_ReturnsCorrectCount()
        {
            // Arrange
            var puzzle = new Puzzle(1, "MOVIE", "SUPERMAN RETURNS");
            var puzzleManager = new PuzzleManager(new List<Puzzle> { puzzle }, new FakeStorageService());
            char letterToCount = 'R';

            // Act
            int count = puzzleManager.CountOccurrences(puzzle, letterToCount);

            // Assert
            Assert.Equal(3, count);
        }


        [Fact]
        public void IsPuzzleCompleted_PuzzleCompleted_ReturnsTrue()
        {
            // Arrange
            var puzzle = new Puzzle(1, "MOVIE", "SUPERMAN RETURNS");
            var guessedLetters = new HashSet<char> { 'S', 'U', 'P', 'E', 'R', 'M', 'A', 'N', 'T' };
            var puzzleManager = new PuzzleManager(new List<Puzzle> { puzzle }, new FakeStorageService());

            // Act
            bool isCompleted = puzzleManager.IsPuzzleCompleted(puzzle, guessedLetters);

            // Assert
            Assert.True(isCompleted);
        }


        [Fact]
        public void IsPuzzleCompleted_PuzzleIncomplete_ReturnsFalse()
        {
            // Arrange
            var puzzle = new Puzzle(1, "MOVIE", "SUPERMAN RETURNS");
            var guessedLetters = new HashSet<char> { 'S', 'U', 'P', 'E', 'R' };
            var puzzleManager = new PuzzleManager(new List<Puzzle> { puzzle }, new FakeStorageService());

            // Act
            bool isCompleted = puzzleManager.IsPuzzleCompleted(puzzle, guessedLetters);

            // Assert
            Assert.False(isCompleted);
        }
    }
}
