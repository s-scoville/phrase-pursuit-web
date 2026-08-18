namespace PhrasePursuitWeb.Core.Enums
{
    /// <summary>
    /// Represents the different phases of a turn in the game.
    /// </summary>
    public enum TurnPhase
    {
        /// <summary>
        /// The game is waiting for the player to take an action, such as spinning the wheel, buying a vowel, or solving the puzzle.
        /// </summary>
        WaitingForAction,

        /// <summary>
        /// The game is waiting for the player to choose a consonant letter.
        /// </summary>
        WaitingForConsonant,

        /// <summary>
        /// The game is waiting for the player to choose a vowel letter.
        /// </summary>
        WaitingForVowel,

        /// <summary>
        /// The current turn has ended, and the game is transitioning to the next player's turn.
        /// </summary>
        TurnEnded,

        /// <summary>
        /// The game has ended, and the final results are being displayed.
        /// </summary>
        GameOver
    }
}
