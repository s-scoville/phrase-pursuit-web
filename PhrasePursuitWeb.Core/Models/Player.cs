using PhrasePursuitWeb.Core.Enums;

namespace PhrasePursuitWeb.Core.Models
{
    /// <summary>
    /// Represents a player in the current game with a name, player type, and current winnings.
    /// </summary>
    public class Player
    {
        /// <summary>
        /// Gets the name of the player.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the type of the player.
        /// </summary>
        public PlayerType PlayerType { get; private set; }

        /// <summary>
        /// Gets the player's current winnings.
        /// </summary>
        public int CurrentWinnings { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class with the specified name and player type.
        /// </summary>
        /// <param name="name">The name of the player.</param>
        /// <param name="playerType">The type of the player.</param>
        /// <exception cref="ArgumentException"><paramref name="name"/> is null, empty, or consists only of white-space characters.</exception>
        public Player(string name, PlayerType playerType)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("A name must be entered.", nameof(name));
            }
            Name = name;
            PlayerType = playerType;
            CurrentWinnings = 0;
        }

        /// <summary>
        /// Adds the specified amount to the player's current winnings.
        /// </summary>
        /// <param name="amount">The amount to add to the current winnings.</param>
        /// <exception cref="ArgumentException"><paramref name="amount"/> is negative.</exception>
        public void AddWinnings(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException("Amount to add must not be negative.", nameof(amount));
            }
            CurrentWinnings += amount;
        }

        /// <summary>
        /// Deducts the specified amount from the player's current winnings.
        /// </summary>
        /// <param name="amount">The amount to deduct from the current winnings.</param>
        /// <exception cref="ArgumentException"><paramref name="amount"/> is negative.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="amount"/> exceeds the player's current winnings.</exception>
        public void DeductWinnings(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException("Amount to deduct must not be negative.", nameof(amount));
            }
            if (amount > CurrentWinnings)
            {
                throw new InvalidOperationException("Cannot deduct more than the current winnings.");
            }
            CurrentWinnings -= amount;
        }

        /// <summary>
        /// Resets the player's current winnings to zero.
        /// </summary>
        public void ResetWinnings()
        {
            CurrentWinnings = 0;
        }
    }
}
