using PhrasePursuitWeb.Core.Managers;
using PhrasePursuitWeb.Core.Models;
using PhrasePursuitWeb.Tests.TestHelpers;

namespace PhrasePursuitWeb.Tests.Managers
{
    public class StatisticsManagerTests
    {
        [Fact]
        public void Constructor_ValidStorageService_CreatesStatisticsManager()
        {
            // Arrange
            FakeStorageService storageService = new FakeStorageService();

            // Act
            StatisticsManager statisticsManager = new StatisticsManager(storageService);

            // Assert
            Assert.NotNull(statisticsManager);
            Assert.NotNull(statisticsManager.Statistics);
            Assert.Equal(0, statisticsManager.Statistics.AverageWinnings);
            Assert.Equal(0, statisticsManager.Statistics.GamesPlayed);
            Assert.Equal(0, statisticsManager.Statistics.GamesWon);
            Assert.Equal(0, statisticsManager.Statistics.GamesLost);
            Assert.Equal(0, statisticsManager.Statistics.LifetimeWinnings);
            Assert.Equal(0, statisticsManager.Statistics.HighestWinnings);
            Assert.Equal(0, statisticsManager.Statistics.WinPercentage);
        }


        [Fact]
        public void Constructor_NullStorageService_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => new StatisticsManager(null!));
        }


        [Fact]
        public async Task LoadStatisticsAsync_NoSavedStatistics_UsesNewStatistics()
        {
            // Arrange
            FakeStorageService storageService = new FakeStorageService();
            StatisticsManager statisticsManager = new StatisticsManager(storageService);
            
            // Act
            await statisticsManager.LoadStatisticsAsync();
            
            // Assert
            Assert.NotNull(statisticsManager.Statistics);
            Assert.Equal(0, statisticsManager.Statistics.AverageWinnings);
            Assert.Equal(0, statisticsManager.Statistics.GamesPlayed);
            Assert.Equal(0, statisticsManager.Statistics.GamesWon);
            Assert.Equal(0, statisticsManager.Statistics.GamesLost);
            Assert.Equal(0, statisticsManager.Statistics.LifetimeWinnings);
            Assert.Equal(0, statisticsManager.Statistics.HighestWinnings);
            Assert.Equal(0, statisticsManager.Statistics.WinPercentage);
        }


        [Fact]
        public async Task LoadStatisticsAsync_SavedStatistics_LoadsStoredValues()
        {
            // Arrange
            PlayerStatistics savedStatistics = new PlayerStatistics();
            savedStatistics.RecordWin();
            savedStatistics.RecordGameWinnings(100);
            savedStatistics.RecordLoss();
            savedStatistics.RecordWin();
            savedStatistics.RecordGameWinnings(200);

            FakeStorageService storageService = new FakeStorageService();
            await storageService.SaveAsync("playerStatistics", savedStatistics);
            StatisticsManager statisticsManager = new StatisticsManager(storageService);

            // Act
            await statisticsManager.LoadStatisticsAsync();

            // Assert
            Assert.NotNull(statisticsManager.Statistics);
            Assert.Equal(3, statisticsManager.Statistics.GamesPlayed);
            Assert.Equal(2, statisticsManager.Statistics.GamesWon);
            Assert.Equal(1, statisticsManager.Statistics.GamesLost);
            Assert.Equal(300, statisticsManager.Statistics.LifetimeWinnings);
            Assert.Equal(200, statisticsManager.Statistics.HighestWinnings);
            Assert.Equal(66.67, statisticsManager.Statistics.WinPercentage, 2);
            Assert.Equal(100, statisticsManager.Statistics.AverageWinnings);
        }


        [Fact]
        public async Task SaveStatisticsAsync_CurrentStatistics_SavesStatistics()
        {
            // Arrange
            FakeStorageService storageService = new FakeStorageService();
            StatisticsManager statisticsManager = new StatisticsManager(storageService);
            statisticsManager.Statistics.RecordLoss();
            statisticsManager.Statistics.RecordWin();
            statisticsManager.Statistics.RecordGameWinnings(1000);

            // Act
            await statisticsManager.SaveStatisticsAsync();
            PlayerStatistics? savedStatistics =
                await storageService.LoadAsync<PlayerStatistics>("playerStatistics");

            // Assert
            Assert.NotNull(savedStatistics);
            Assert.Equal(2, savedStatistics.GamesPlayed);
            Assert.Equal(1, savedStatistics.GamesWon);
            Assert.Equal(1, savedStatistics.GamesLost);
            Assert.Equal(1000, savedStatistics.LifetimeWinnings);
            Assert.Equal(1000, savedStatistics.HighestWinnings);
            Assert.Equal(50, savedStatistics.WinPercentage);
            Assert.Equal(500, savedStatistics.AverageWinnings);
        }
    }
}
