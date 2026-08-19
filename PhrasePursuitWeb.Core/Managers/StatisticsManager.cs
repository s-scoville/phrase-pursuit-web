using PhrasePursuitWeb.Core.Interfaces;
using PhrasePursuitWeb.Core.Models;

namespace PhrasePursuitWeb.Core.Managers
{
    /// <summary>
    /// Manages loading and saving player statistics using the configured storage service.
    /// </summary>
    public class StatisticsManager
    {
        /// <summary>
        /// Gets the current player statistics.
        /// </summary>
        public PlayerStatistics Statistics { get; private set; }

        /// <summary>
        /// Represents the storage service used to persist and retrieve player statistics.
        /// </summary>
        private readonly IStorageService _storageService;

        /// <summary>
        /// Represents the key used to store and retrieve player statistics from persistent storage.
        /// </summary>
        private const string StatisticsKey = "playerStatistics";

        /// <summary>
        /// Initializes a new instance of the <see cref="StatisticsManager"/> class
        /// with the specified storage service.
        /// </summary>
        /// <param name="storageService">
        /// The storage service used to persist and retrieve player statistics.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="storageService"/> is null.
        /// </exception>
        public StatisticsManager(IStorageService storageService)
        {
            _storageService = storageService
                ?? throw new ArgumentNullException(nameof(storageService));

            Statistics = new PlayerStatistics();
        }

        /// <summary>
        /// Loads player statistics from persistent storage.
        /// If no saved statistics are found, initializes a new set of player statistics.
        /// </summary>
        /// <returns>A task representing the asynchronous load operation.</returns>
        public async Task LoadStatisticsAsync()
        {
            Statistics =
                await _storageService.LoadAsync<PlayerStatistics>(StatisticsKey)
                ?? new PlayerStatistics();
        }

        /// <summary>
        /// Saves the current player statistics to persistent storage.
        /// </summary>
        /// <returns>A task representing the asynchronous save operation.</returns>
        public async Task SaveStatisticsAsync()
        {
            await _storageService.SaveAsync(
                StatisticsKey,
                Statistics);
        }
    }
}
