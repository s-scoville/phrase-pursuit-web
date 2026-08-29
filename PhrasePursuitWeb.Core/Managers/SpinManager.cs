using PhrasePursuitWeb.Core.Enums;
using PhrasePursuitWeb.Core.Models;

namespace PhrasePursuitWeb.Core.Managers
{
    /// <summary>
    /// Builds and manages the game spinner by creating weighted monetary and special outcome segments.
    /// </summary>
    public class SpinManager
    {
        /// <summary>
        /// Represents the segments of the spinner, each containing a monetary value or a special outcome.
        /// </summary>
        private readonly List<SpinResult> _wheel;

        /// <summary>
        /// Represents a random number generator used to select segments from the spinner.
        /// </summary>
        private readonly Random _random;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpinManager"/> class.
        /// </summary>
        public SpinManager()
        {
            _wheel = new List<SpinResult>();
            _random = new Random();

            InitializeWheel();
        }

        /// <summary>
        /// Initializes the spinner with money segments and special outcome segments.
        /// </summary>
        private void InitializeWheel()
        {
            AddMoneySegments(100, 2);
            AddMoneySegments(150, 2);
            AddMoneySegments(200, 3);
            AddMoneySegments(250, 3);
            AddMoneySegments(300, 3);
            AddMoneySegments(350, 3);
            AddMoneySegments(400, 4);
            AddMoneySegments(500, 2);
            AddMoneySegments(600, 2);
            AddMoneySegments(700, 1);
            AddMoneySegments(800, 1);

            AddSpecialSegments(SpinOutcome.Bankrupt, 2);
            AddSpecialSegments(SpinOutcome.LoseTurn, 2);
        }

        /// <summary>
        /// Adds the specified number of money segments to the spinner.
        /// </summary>
        /// <param name="amount">The monetary value for each segment.</param>
        /// <param name="count">The number of segments to add.</param>
        private void AddMoneySegments(int amount, int count)
        {
            for (int i = 0; i < count; i++)
            {
                _wheel.Add(new SpinResult(SpinOutcome.Money, amount));
            }
        }

        /// <summary>
        /// Adds the specified number of special outcome segments to the spinner.
        /// </summary>
        /// <param name="outcome">The spin outcome to assign to each segment.</param>
        /// <param name="count">The number of special segments to add.</param>
        private void AddSpecialSegments(SpinOutcome outcome, int count)
        {
            for (int i = 0; i < count; i++)
            {
                _wheel.Add(new SpinResult(outcome));
            }
        }

        /// <summary>
        /// Performs a weighted random spin of the spinner.
        /// </summary>
        /// <returns>A randomly selected <see cref="SpinResult"/> from the spinner.</returns>
        public SpinResult Spin()
        {
            return _wheel[_random.Next(_wheel.Count)];
        }

    }
}
