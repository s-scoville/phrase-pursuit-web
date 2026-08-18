namespace PhrasePursuitWeb.Core.Enums
{
    /// <summary>
    /// Represents the possible outcomes of a spin in the game.
    /// </summary>
    public enum SpinOutcome
    {
        /// <summary>
        /// Represents a spin that results in a monetary value.
        /// </summary>
        Money,

        /// <summary>
        /// Represents a spin that results in a bankrupt outcome, causing the player to lose all their money.
        /// </summary>
        Bankrupt,

        /// <summary>
        /// Represents a spin that results in the player losing their turn, with no money lost.
        /// </summary>
        LoseTurn
    }
}
