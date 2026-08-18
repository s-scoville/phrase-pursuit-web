namespace PhrasePursuitWeb.Core.Models
{
    /// <summary>
    /// Represents the result of a letter guess.
    /// </summary>
    public record GuessResult
    {
        /// <summary>
        /// Gets the letter that was guessed.
        /// </summary>
        public char GuessedLetter { get; init; }

        /// <summary>
        /// Gets a value indicating whether the guessed letter was correct.
        /// </summary>
        public bool WasCorrect { get; init; }

        /// <summary>
        /// Gets the number of occurrences of the guessed letter in the puzzle.
        /// </summary>
        public int Occurrences { get; init; }

        /// <summary>
        /// Gets the amount of money earned from the letter guess.
        /// </summary>
        public int MoneyEarned { get; init; }

        /// <summary>
        /// Gets a value indicating whether the guess caused the current turn to end.
        /// </summary>
        public bool TurnEnded { get; init; }

        /// <summary>
        /// Gets a value indicating whether the guess completed the puzzle.
        /// </summary>
        public bool PuzzleCompleted { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GuessResult"/> record.
        /// </summary>
        /// <param name="guessedLetter">The letter that was guessed.</param>
        /// <param name="wasCorrect">Indicates whether the guess was correct.</param>
        /// <param name="occurrences">The number of occurrences of the guessed letter in the puzzle.</param>
        /// <param name="moneyEarned">The amount of money earned from the current letter guess.</param>
        /// <param name="turnEnded">Indicates whether the turn has ended.</param>
        /// <param name="puzzleCompleted">Indicates whether the puzzle has been completed.</param>
        public GuessResult(char guessedLetter, bool wasCorrect, int occurrences, int moneyEarned, bool turnEnded = false, bool puzzleCompleted = false)
        {
            GuessedLetter = guessedLetter;
            WasCorrect = wasCorrect;
            Occurrences = occurrences;
            MoneyEarned = moneyEarned;
            TurnEnded = turnEnded;
            PuzzleCompleted = puzzleCompleted;
        }
    }
}
