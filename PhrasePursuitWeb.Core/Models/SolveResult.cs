namespace PhrasePursuitWeb.Core.Models
{
    /// <summary>
    /// Represents the result of a puzzle solve operation.
    /// </summary>
    public record SolveResult
    {
        /// <summary>
        /// Gets a value indicating whether the puzzle solve attempt was correct.
        /// </summary>
        public bool WasCorrect { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SolveResult"/> record.
        /// </summary>
        /// <param name="wasCorrect">Indicates whether the puzzle solve attempt was correct.</param>
        public SolveResult(bool wasCorrect)
        {
            WasCorrect = wasCorrect;
        }
    }
}
