namespace PhrasePursuitWeb.Core.Models
{
    /// <summary>
    /// Represents a puzzle with a unique identifier, category, and phrase.
    /// </summary>
    public class Puzzle
    {
        /// <summary>
        /// Gets the unique identifier for the puzzle.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Gets the category of the puzzle.
        /// </summary>
        public string Category { get; private set; }

        /// <summary>
        /// Gets the phrase to be solved.
        /// </summary>
        public string Phrase { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Puzzle"/> class with the specified identifier, category, and phrase.
        /// </summary>
        /// <param name="id">The unique identifier for the puzzle.</param>
        /// <param name="category">The category of the puzzle.</param>
        /// <param name="phrase">The phrase to be solved.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="id"/> is less than or equal to zero.</exception>
        /// <exception cref="ArgumentException"><paramref name="category"/> is null, empty, or consists only of white-space characters.</exception>
        /// <exception cref="ArgumentException"><paramref name="phrase"/> is null, empty, or consists only of white-space characters.</exception>
        public Puzzle(int id, string category, string phrase)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(id),
                    "Puzzle ID must be greater than zero.");
            }

            if (string.IsNullOrWhiteSpace(category))
            {
                throw new ArgumentException(
                    "A puzzle category must be provided.",
                    nameof(category));
            }

            if (string.IsNullOrWhiteSpace(phrase))
            {
                throw new ArgumentException(
                    "A puzzle phrase must be provided.",
                    nameof(phrase));
            }

            Id = id;
            Category = category;
            Phrase = phrase;
        }
    }
}
