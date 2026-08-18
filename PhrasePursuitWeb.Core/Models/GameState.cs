using PhrasePursuitWeb.Core.Enums;

namespace PhrasePursuitWeb.Core.Models
{
    /// <summary>
    /// Represents the current state of a game, including the puzzle, players, and turn progression.
    /// </summary>
    public class GameState
    {
        /// <summary>
        /// Gets the puzzle currently being played.
        /// </summary>
        public Puzzle CurrentPuzzle { get; private set; }

        /// <summary>
        /// Gets the list of players participating in the game.
        /// </summary>
        public List<Player> Players { get; private set; }

        /// <summary>
        /// Gets the index of the player whose turn is currently active.
        /// </summary>
        public int CurrentPlayerIndex { get; private set; } = 0;

        /// <summary>
        /// Gets the player whose turn is currently active.
        /// </summary>
        public Player CurrentPlayer => Players[CurrentPlayerIndex];

        /// <summary>
        /// Gets the collection of letters that have already been guessed during the game.
        /// </summary>
        public HashSet<char> GuessedLetters { get; private set; } = new HashSet<char>();

        /// <summary>
        /// Gets the monetary value of the current spin.
        /// </summary>
        public int CurrentSpinValue { get; private set; } = 0;

        /// <summary>
        /// Gets the current phase of the active turn.
        /// </summary>
        public TurnPhase CurrentPhase { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the game has ended.
        /// </summary>
        public bool IsGameOver { get; private set; } = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameState"/> class with the specified puzzle and players.
        /// </summary>
        /// <param name="puzzle">The puzzle to be solved.</param>
        /// <param name="players">The players participating in the game.</param>
        /// <exception cref="ArgumentNullException"><paramref name="puzzle"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="players"/> is null or contains no players.</exception>
        public GameState(Puzzle puzzle, List<Player> players)
        {
            CurrentPuzzle = puzzle ?? throw new ArgumentNullException(nameof(puzzle));

            Players = players is { Count: > 0 }
                ? players
                : throw new ArgumentException("At least one player is required.", nameof(players));

            CurrentPhase = TurnPhase.WaitingForAction;
        }

        /// <summary>
        /// Advances to the next player and resets the turn state to the beginning of a new turn.
        /// </summary>
        public void AdvanceTurn()
        {
            CurrentPlayerIndex = (CurrentPlayerIndex + 1) % Players.Count;
            CurrentPhase = TurnPhase.WaitingForAction;
            CurrentSpinValue = 0;
        }

        /// <summary>
        /// Adds a letter to the collection of letters that have already been guessed.
        /// </summary>
        /// <param name="letter">The letter to add.</param>
        public void AddGuessedLetter(char letter)
        {
            GuessedLetters.Add(letter);
        }

        /// <summary>
        /// Sets the monetary value of the current spin.
        /// </summary>
        /// <param name="value">The spin value to set.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is negative.</exception>
        public void SetSpinValue(int value)
        {
            if (value >= 0)
            {
                CurrentSpinValue = value;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Spin value cannot be negative.");
            }
        }

        /// <summary>
        /// Sets the current phase of the active turn.
        /// </summary>
        /// <param name="phase">The turn phase to set.</param>
        public void SetPhase(TurnPhase phase)
        {
            CurrentPhase = phase;
        }

        /// <summary>
        /// Ends the game and sets the current turn phase to game over.
        /// </summary>
        public void EndGame()
        {
            IsGameOver = true;
            CurrentPhase = TurnPhase.GameOver;
        }

        /// <summary>
        /// Resets the current spin value to zero.
        /// </summary>
        public void ResetSpinValue()
        {
            CurrentSpinValue = 0;
        }
    }
}
