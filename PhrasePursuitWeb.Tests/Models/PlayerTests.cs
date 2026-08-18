using PhrasePursuitWeb.Core.Models;
using PhrasePursuitWeb.Core.Enums;

namespace PhrasePursuitWeb.Tests.Models
{
    public class PlayerTests
    {

        [Fact]
        public void Constructor_ValidInputs_CreatesPlayerWithZeroWinnings()
        {
            // Arrange
            string name = "Steven";
            PlayerType playerType = PlayerType.Player;

            // Act
            Player player = new Player(name, playerType);

            // Assert
            Assert.Equal(name, player.Name);
            Assert.Equal(playerType, player.PlayerType);
            Assert.Equal(0, player.CurrentWinnings);
        }


        [Fact]
        public void AddWinnings_ValidAmount_IncreasesCurrentWinnings()
        {
            // Arrange
            Player player = new Player("Steven", PlayerType.Player);

            // Act
            player.AddWinnings(5000);

            // Assert
            Assert.Equal(5000, player.CurrentWinnings);
        }


        [Fact]
        public void DeductWinnings_ValidAmount_DecreasesCurrentWinnings()
        {
            // Arrange
            Player player = new Player("Steven", PlayerType.Player);
            player.AddWinnings(5000);

            // Act
            player.DeductWinnings(1000);

            // Assert
            Assert.Equal(4000, player.CurrentWinnings);
        }


        [Fact]
        public void ResetWinnings_ExistingWinnings_ResetsCurrentWinnings()
        {
            // Arrange
            Player player = new Player("Steven", PlayerType.Player);
            player.AddWinnings(1000);

            // Act
            player.ResetWinnings();

            // Assert
            Assert.Equal(0, player.CurrentWinnings);
        }


        [Fact]
        public void AddWinnings_NegativeAmount_ThrowsArgumentException()
        {
            // Arrange
            Player player = new Player("Steven", PlayerType.Player);

            // Act and Assert
            Assert.Throws<ArgumentException>(
                () => player.AddWinnings(-100));
        }


        [Fact]
        public void DeductWinnings_NegativeAmount_ThrowsArgumentException()
        {
            // Arrange
            Player player = new Player("Steven", PlayerType.Player);

            // Act and Assert
            Assert.Throws<ArgumentException>(
                () => player.DeductWinnings(-100));
        }


        [Fact]
        public void DeductWinnings_AmountGreaterThanCurrentWinnings_ThrowsInvalidOperationException()
        {
            // Arrange
            Player player = new Player("Steven", PlayerType.Player);
            player.AddWinnings(500);

            // Act and Assert
            Assert.Throws<InvalidOperationException>(
                () => player.DeductWinnings(1000));
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_InvalidName_ThrowsArgumentException(string? invalidName)
        {
            // Act and Assert
            Assert.Throws<ArgumentException>(
                () => new Player(invalidName!, PlayerType.Player));
        }


        [Fact]
        public void DeductWinnings_AmountEqualToCurrentWinnings_SetsCurrentWinningsToZero()
        {
            // Arrange
            Player player = new Player("Steven", PlayerType.Player);
            player.AddWinnings(1000);

            // Act
            player.DeductWinnings(1000);

            // Assert
            Assert.Equal(0, player.CurrentWinnings);
        }
    }
}
