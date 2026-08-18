using PhrasePursuitWeb.Core.Enums;
using PhrasePursuitWeb.Core.Managers;
using PhrasePursuitWeb.Core.Models;

namespace PhrasePursuitWeb.Tests.Managers
{
    public class SpinManagerTests
    {
        [Fact]
        public void Constructor_ValidInitialization_CreatesSpinManager()
        {
            // Arrange
            SpinManager spinManager = new SpinManager();

            // Act
            SpinResult result = spinManager.Spin();

            // Assert
            Assert.NotNull(result);
        }


        [Fact]
        public void Spin_MultipleSpins_ReturnsOnlyValidResults()
        {
            // Arrange
            SpinManager spinManager = new SpinManager();
            HashSet<int> validMoneyValues = new()
            {
                100, 150, 200, 250, 300, 350, 400, 500, 600, 700, 800
            };

            // Act & Assert
            for (int i = 0; i < 1000; i++)
            {
                SpinResult result = spinManager.Spin();

                if (result.Outcome == SpinOutcome.Money)
                {
                    Assert.NotNull(result.MoneyValue);
                    Assert.Contains(result.MoneyValue.Value, validMoneyValues);
                }
                else
                {
                    Assert.True(
                        result.Outcome == SpinOutcome.Bankrupt ||
                        result.Outcome == SpinOutcome.LoseTurn);

                    Assert.Null(result.MoneyValue);
                }
            }
        }
    }
}
