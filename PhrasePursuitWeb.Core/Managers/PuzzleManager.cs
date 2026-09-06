using PhrasePursuitWeb.Core.Interfaces;
using PhrasePursuitWeb.Core.Models;
using System.Text;

namespace PhrasePursuitWeb.Core.Managers
{
    /// <summary>
    /// Manages puzzle data and provides functionality for puzzle selection, rendering, and validation.
    /// </summary>
    public class PuzzleManager
    {
        /// <summary>
        /// Represents the collection of available puzzles.
        /// </summary>
        private readonly List<Puzzle> _puzzles;

        /// <summary>
        /// Represents the storage service used to persist and retrieve puzzle data.
        /// </summary>
        private readonly IStorageService _storageService;

        /// <summary>
        /// Represents a random number generator used to select puzzles randomly.
        /// </summary>
        private readonly Random _random;

        /// <summary>
        /// Represents the key used to store and retrieve the list of played puzzle IDs from persistent storage.
        /// </summary>
        private const string PlayedPuzzleIdsKey = "playedPuzzleIds";

        /// <summary>
        /// Initializes a new instance of the <see cref="PuzzleManager"/> class with the specified puzzles and storage service.
        /// </summary>
        /// <param name="puzzles">The collection of puzzles available for selection.</param>
        /// <param name="storageService">The storage service used to persist and retrieve played puzzle IDs.</param>
        /// <exception cref="ArgumentException"><paramref name="puzzles"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="storageService"/> is null.</exception>
        public PuzzleManager(List<Puzzle> puzzles, IStorageService storageService)
        {
            _puzzles = puzzles is { Count: > 0  }
                ? puzzles
                : throw new ArgumentException(
                    "Puzzles list cannot be null or empty.",
                    nameof(puzzles));

            _storageService = storageService
                ?? throw new ArgumentNullException(nameof(storageService));

            _random = new Random();
        }

        /// <summary>
        /// Gets a random puzzle that has not already been played during the current puzzle cycle.
        /// When all puzzles have been played, the played puzzle history is reset and a new cycle begins.
        /// </summary>
        /// <returns>A randomly selected available puzzle.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no puzzles are available.</exception>
        public async Task<Puzzle> GetRandomPuzzleAsync()
        {
            if (_puzzles.Count == 0)
            {
                throw new InvalidOperationException("No puzzles available.");
            }

            HashSet<int> playedPuzzleIds =
                await _storageService.LoadAsync<HashSet<int>>(PlayedPuzzleIdsKey)
                ?? new HashSet<int>();

            List<Puzzle> availablePuzzles = _puzzles
                .Where(puzzle => !playedPuzzleIds.Contains(puzzle.Id))
                .ToList();

            if (availablePuzzles.Count == 0)
            {
                playedPuzzleIds.Clear();
                availablePuzzles = _puzzles.ToList();
            }

            Puzzle selectedPuzzle = availablePuzzles[_random.Next(availablePuzzles.Count)];

            playedPuzzleIds.Add(selectedPuzzle.Id);

            await _storageService.SaveAsync(
                PlayedPuzzleIdsKey,
                playedPuzzleIds);

            return selectedPuzzle;
        }

        /// <summary>
        /// Renders the puzzle phrase with guessed letters revealed and unguessed letters hidden as underscores.
        /// </summary>
        /// <param name="puzzle">The puzzle containing the phrase to render.</param>
        /// <param name="guessedLetters">The set of letters that have been guessed.</param>
        /// <returns>
        /// A formatted string representing the puzzle with guessed letters revealed, unguessed letters replaced by 
        /// underscores, and non-letter characters preserved.
        /// </returns>
        public string RenderPuzzle(Puzzle puzzle, HashSet<char> guessedLetters)
        {
            var rendered = new StringBuilder();

            foreach (char c in puzzle.Phrase)
            {
                if (!char.IsLetter(c))
                {
                    rendered.Append(c);
                }
                else if (guessedLetters.Contains(char.ToUpper(c)))
                {
                    rendered.Append(c);
                }
                else
                {
                    rendered.Append("_");
                }
            }

            return rendered.ToString().Trim();
        }

        /// <summary>
        /// Determines whether the puzzle's phrase contains the specified letter, ignoring case.
        /// </summary>
        /// <param name="puzzle">The puzzle to search.</param>
        /// <param name="letter">The letter to search for.</param>
        /// <returns><see langword="true"/> if the letter is found in the puzzle's phrase; otherwise, <see langword="false"/>.</returns>
        public bool ContainsLetter(Puzzle puzzle, char letter)
        {
            return puzzle.Phrase.IndexOf(letter, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        /// <summary>
        /// Counts the occurrences of a specified letter in the puzzle's phrase using case-insensitive comparison.
        /// </summary>
        /// <param name="puzzle">The puzzle containing the phrase to search.</param>
        /// <param name="letter">The letter to count.</param>
        /// <returns>The number of times the letter appears in the phrase.</returns>
        public int CountOccurrences(Puzzle puzzle, char letter)
        {
            return puzzle.Phrase.Count(c => char.ToUpper(c) == char.ToUpper(letter));
        }

        /// <summary>
        /// Determines whether all letters in the puzzle phrase have been guessed.
        /// </summary>
        /// <param name="puzzle">The puzzle to check for completion.</param>
        /// <param name="guessedLetters">The set of letters that have been guessed.</param>
        /// <returns><see langword="true"/> if all letters in the puzzle phrase have been guessed; otherwise, <see
        /// langword="false"/>.</returns>
        public bool IsPuzzleCompleted(Puzzle puzzle, HashSet<char> guessedLetters)
        {
            return puzzle.Phrase.All(c =>
                !char.IsLetter(c) ||
                guessedLetters.Contains(char.ToUpper(c)));
        }
    }
}
