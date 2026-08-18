using PhrasePursuitWeb.Core.Models;

namespace PhrasePursuitWeb.Tests.Models
{
    public class PlayerStatisticsTests
    {
        [Fact]
        public void Constructor_ValidInputs_CreatesPlayerStatisticsWithZeroWinnings()
        {
            // Act
            PlayerStatistics playerStats = new PlayerStatistics();

            // Assert
            Assert.Equal(0, playerStats.AverageWinnings);
            Assert.Equal(0, playerStats.GamesPlayed);
            Assert.Equal(0, playerStats.GamesWon);
            Assert.Equal(0, playerStats.GamesLost);
            Assert.Equal(0, playerStats.HighestWinnings);
            Assert.Equal(0, playerStats.LifetimeWinnings);
            Assert.Equal(0, playerStats.WinPercentage);
        }


        [Fact]
        public void RecordWin_ValidInputs_UpdatesPlayerStatistics()
        {
            // Arrange
            PlayerStatistics playerStats = new PlayerStatistics();

            // Act
            playerStats.RecordWin();

            // Assert
            Assert.Equal(1, playerStats.GamesPlayed);
            Assert.Equal(1, playerStats.GamesWon);
            Assert.Equal(0, playerStats.GamesLost);
        }


        [Fact]
        public void RecordLoss_ValidInputs_UpdatesPlayerStatistics()
        {
            // Arrange
            PlayerStatistics playerStats = new PlayerStatistics();

            // Act
            playerStats.RecordLoss();

            // Assert
            Assert.Equal(1, playerStats.GamesPlayed);
            Assert.Equal(1, playerStats.GamesLost);
            Assert.Equal(0, playerStats.GamesWon);
        }


        [Theory]
        [InlineData(2000, 1000, 3000, 2000)]
        [InlineData(2000, 2000, 4000, 2000)]
        [InlineData(3000, 4000, 7000, 4000)]
        public void RecordGameWinnings_ValidInputs_UpdatesPlayerStatistics(int previousWinnings, int currentWinnings, int expectedLifetimeWinnings, int expectedHighestWinnings)
        {
            // Arrange
            PlayerStatistics playerStats = new PlayerStatistics();
            playerStats.RecordGameWinnings(previousWinnings);

            // Act
            playerStats.RecordGameWinnings(currentWinnings);

            // Assert
            Assert.Equal(expectedLifetimeWinnings, playerStats.LifetimeWinnings);
            Assert.Equal(expectedHighestWinnings, playerStats.HighestWinnings);
        }


        [Fact]
        public void RecordGameWinnings_NegativeCurrentWinnings_ThrowsArgumentException()
        {
            // Arrange
            PlayerStatistics playerStats = new PlayerStatistics();

            // Act and Assert
            Assert.Throws<ArgumentException>(() => playerStats.RecordGameWinnings(-1000));
        }


        [Fact]
        public void AverageWinnings_CalculatedCorrectly()
        {
            // Arrange
            PlayerStatistics playerStats = new PlayerStatistics();
            
            playerStats.RecordLoss();
            playerStats.RecordWin();
            playerStats.RecordGameWinnings(2000);

            playerStats.RecordWin();
            playerStats.RecordGameWinnings(4000);

            // Act
            double actualAverageWinnings = playerStats.AverageWinnings;
            
            // Assert
            Assert.Equal(2000.00, actualAverageWinnings, 2);
        }


        [Fact]
        public void WinPercentage_CalculatedCorrectly()
        {
            // Arrange
            PlayerStatistics playerStats = new PlayerStatistics();

            playerStats.RecordLoss();
            playerStats.RecordWin();
            playerStats.RecordWin();
            playerStats.RecordWin();

            // Act
            double actualWinPercentage = playerStats.WinPercentage;

            // Assert
            Assert.Equal(75, actualWinPercentage);
        }


        [Fact]
        public void ResetStatistics_ValidInputs_ResetsPlayerStatistics()
        {
            // Arrange
            PlayerStatistics playerStats = new PlayerStatistics();
            playerStats.RecordWin();
            playerStats.RecordLoss();
            playerStats.RecordGameWinnings(2000);

            // Act
            playerStats.ResetStatistics();

            // Assert
            Assert.Equal(0, playerStats.GamesPlayed);
            Assert.Equal(0, playerStats.GamesWon);
            Assert.Equal(0, playerStats.GamesLost);
            Assert.Equal(0, playerStats.LifetimeWinnings);
            Assert.Equal(0, playerStats.HighestWinnings);
            Assert.Equal(0, playerStats.AverageWinnings);
            Assert.Equal(0, playerStats.WinPercentage);
        }
    }
}
