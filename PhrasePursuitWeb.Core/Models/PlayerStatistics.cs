using System.Text.Json.Serialization;

namespace PhrasePursuitWeb.Core.Models
{
    /// <summary>
    /// Represents a player's lifetime statistics across all completed games.
    /// </summary>
    public class PlayerStatistics
    {
        /// <summary>
        /// Gets the player's average winnings per game.
        /// </summary>
        [JsonIgnore]
        public double AverageWinnings =>
            GamesPlayed > 0
                ? (double)LifetimeWinnings / GamesPlayed
                : 0;

        /// <summary>
        /// Gets the total number of games played.
        /// </summary>
        [JsonInclude]
        public int GamesPlayed { get; private set; }

        /// <summary>
        /// Gets the total number of games won.
        /// </summary>
        [JsonInclude]
        public int GamesWon { get; private set; }

        /// <summary>
        /// Gets the total number of games lost.
        /// </summary>
        [JsonInclude]
        public int GamesLost { get; private set; }

        /// <summary>
        /// Gets the highest amount of winnings earned in a single game.
        /// </summary>
        [JsonInclude]
        public int HighestWinnings { get; private set; }

        /// <summary>
        /// Gets the player's lifetime winnings across all completed games.
        /// </summary>
        [JsonInclude]
        public int LifetimeWinnings { get; private set; }

        /// <summary>
        /// Gets the player's win percentage based on the number of games played.
        /// </summary>
        [JsonIgnore]
        public double WinPercentage =>
            GamesPlayed > 0
                ? (double)GamesWon / GamesPlayed * 100
                : 0;

        /// <summary>
        /// Records a win and increments the total number of games played.
        /// </summary>
        public void RecordWin()
        {
            GamesWon++;
            GamesPlayed++;
        }

        /// <summary>
        /// Records a loss and increments the total number of games played.
        /// </summary>
        public void RecordLoss()
        {
            GamesLost++;
            GamesPlayed++;
        }

        /// <summary>
        /// Records game winnings by adding them to lifetime winnings and updating the highest winnings if applicable.
        /// </summary>
        /// <param name="currentWinnings">The player's winnings from the completed game.</param>
        /// <exception cref="ArgumentException"><paramref name="currentWinnings"/> is negative.</exception>
        public void RecordGameWinnings(int currentWinnings)
        {
            if (currentWinnings < 0)
            {
                throw new ArgumentException(
                    "Game winnings cannot be negative.",
                    nameof(currentWinnings));
            }

            LifetimeWinnings += currentWinnings;

            if (currentWinnings > HighestWinnings)
            {
                HighestWinnings = currentWinnings;
            }
        }

        /// <summary>
        /// Resets all game statistics to zero.
        /// </summary>
        public void ResetStatistics()
        {
            GamesPlayed = 0;
            GamesWon = 0;
            GamesLost = 0;
            LifetimeWinnings = 0;
            HighestWinnings = 0;
        }
    }
}
