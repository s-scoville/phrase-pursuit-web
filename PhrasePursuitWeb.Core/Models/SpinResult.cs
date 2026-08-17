using PhrasePursuitWeb.Core.Enums;

namespace PhrasePursuitWeb.Core.Models
{
    /// <summary>
    /// Represents the result of a spin operation.
    /// </summary>
    public record SpinResult
    {
        /// <summary>
        /// Gets the outcome of the spin.
        /// </summary>
        public SpinOutcome Outcome { get; init; }

        /// <summary>
        /// Gets the monetary value awarded by the spin, if applicable.
        /// </summary>
        public int? MoneyValue { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpinResult"/> record with the specified outcome and optional
        /// value.
        /// </summary>
        /// <param name="outcome">The outcome of the spin.</param>
        /// <param name="value">The monetary value awarded by the spin, if applicable.</param>
        public SpinResult(SpinOutcome outcome, int? value = null)
        {
            Outcome = outcome;
            MoneyValue = value;
        }
    }
}
