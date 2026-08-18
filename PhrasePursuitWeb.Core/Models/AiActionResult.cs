using PhrasePursuitWeb.Core.Enums;

namespace PhrasePursuitWeb.Core.Models
{
    /// <summary>
    /// Represents the result of an action performed by an AI player during their turn, including any associated results.
    /// </summary>
    public record AiActionResult
    {
        /// <summary>
        /// Gets the action performed by the AI player.
        /// </summary>
        public AiAction Action { get; init; }

        /// <summary>
        /// Gets the result of the spin, if applicable. This property is null if the action did not include a spin.
        /// </summary>
        public SpinResult? SpinResult { get; init; }

        /// <summary>
        /// Gets the result of the letter guess, if applicable. This property is null if the action did not include a letter guess.
        /// </summary>
        public GuessResult? GuessResult { get; init; }

        /// <summary>
        /// Gets the result of the solve attempt, if applicable. This property is null if the action was not a solve attempt.
        /// </summary>
        public SolveResult? SolveResult { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AiActionResult"/> record with the specified action and any associated results.
        /// </summary>
        /// <param name="action">The action performed by the AI player.</param>
        /// <param name="spinResult">The result of the spin, if applicable.</param>
        /// <param name="guessResult">The result of the letter guess, if applicable.</param>
        /// <param name="solveResult">The result of the solve attempt, if applicable.</param>
        public AiActionResult(
            AiAction action,
            SpinResult? spinResult = null,
            GuessResult? guessResult = null,
            SolveResult? solveResult = null)
        {
            Action = action;
            SpinResult = spinResult;
            GuessResult = guessResult;
            SolveResult = solveResult;
        }
    }
}
