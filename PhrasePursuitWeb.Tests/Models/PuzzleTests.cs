using PhrasePursuitWeb.Core.Models;

namespace PhrasePursuitWeb.Tests.Models
{
    public class PuzzleTests
    {
        [Fact]
        public void Constructor_ValidInputs_CreatesPuzzle()
        {
            // Arrange
            int id = 1;
            string category = "MOVIES";
            string phrase = "THE GODFATHER";
            
            // Act
            Puzzle puzzle = new Puzzle(id, category, phrase);
            
            // Assert
            Assert.Equal(id, puzzle.Id);
            Assert.Equal(category, puzzle.Category);
            Assert.Equal(phrase, puzzle.Phrase);
        }


        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public void Constructor_InvalidId_ThrowsArgumentOutOfRangeException(int invalidId)
        {
            // Arrange
            string category = "MOVIES";
            string phrase = "THE GODFATHER";
            
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new Puzzle(invalidId, category, phrase));
        }


        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Constructor_InvalidCategory_ThrowsArgumentException(string? invalidCategory)
        {
            // Arrange
            int id = 1;
            string phrase = "THE GODFATHER";

            // Act & Assert
            Assert.Throws<ArgumentException>(
                () => new Puzzle(id, invalidCategory!, phrase));
        }


        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Constructor_InvalidPhrase_ThrowsArgumentException(string? invalidPhrase)
        {
            // Arrange
            int id = 1;
            string category = "MOVIES";
            
            // Act & Assert
            Assert.Throws<ArgumentException>(
                () => new Puzzle(id, category, invalidPhrase!));
        }
    }
}
