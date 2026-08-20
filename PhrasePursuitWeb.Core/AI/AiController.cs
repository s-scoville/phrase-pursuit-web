using PhrasePursuitWeb.Core.Enums;
using PhrasePursuitWeb.Core.Models;

namespace PhrasePursuitWeb.Core.AI
{
    /// <summary>
    /// Provides artificial intelligence logic for automated players in the word puzzle game.
    /// </summary>
    public class AiController
    {
        /// <summary>
        /// Represents the configured difficulty level of the AI.
        /// </summary>
        private readonly AiDifficulty _difficulty;

        /// <summary>
        /// Represents the random number generator used for AI decision-making.
        /// </summary>
        private readonly Random _random;

        /// <summary>
        /// Represents the collection of uppercase English consonants available for guessing.
        /// </summary>
        private static readonly char[] _consonants = "BCDFGHJKLMNPQRSTVWXYZ".ToCharArray();

        /// <summary>
        /// Represents the prioritized consonant order used by Normal difficulty when selecting guesses.
        /// </summary>
        private static readonly char[] _commonConsonants = "RSTLNDCMPHGBFYWKVXZQJ".ToCharArray();

        /// <summary>
        /// Represents the collection of uppercase English vowels available for guessing.
        /// </summary>
        private static readonly char[] _vowels = "AEIOU".ToCharArray();


        /// <summary>
        /// Represents the prioritized vowel order used by Normal difficulty when selecting guesses.
        /// </summary>
        private static readonly char[] _commonVowels = "EAOUI".ToCharArray();

        /// <summary>
        /// Initializes a new instance of the <see cref="AiController"/> class
        /// with the specified difficulty level.
        /// </summary>
        /// <param name="difficulty">The difficulty level used to control AI decision-making behavior.</param>
        public AiController(AiDifficulty difficulty)
        {
            _difficulty = difficulty;
            _random = new Random();
        }

        /// <summary>
        /// Determines the AI's next action by evaluating whether to solve the puzzle,
        /// buy a vowel, or spin the spinner.
        /// </summary>
        /// <param name="currentGame">
        /// The current game state containing puzzle information, player data, and guessed letters.
        /// </param>
        /// <returns>The action selected by the AI.</returns>
        public AiAction ChooseAction(GameState currentGame)
        {
            double puzzleCompletion = GetPuzzleCompletion(currentGame);
            Player currentPlayer = currentGame.Players[currentGame.CurrentPlayerIndex];

            if (ShouldSolvePuzzle(puzzleCompletion))
            {
                return AiAction.SolvePuzzle;
            }

            if (currentPlayer.CurrentWinnings >= 50 &&
                HasRemainingVowels(currentGame.GuessedLetters) &&
                ShouldBuyVowel(puzzleCompletion))
            {
                return AiAction.BuyVowel;
            }

            return AiAction.Spin;
        }

        /// <summary>
        /// Selects an unguessed consonant based on the configured AI difficulty.
        /// Easy difficulty selects randomly, while Normal difficulty prioritizes common consonants.
        /// </summary>
        /// <param name="currentGame">The current game state containing previously guessed letters.</param>
        /// <returns>An unguessed consonant selected according to the configured AI difficulty.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no unguessed consonants remain.
        /// </exception>
        public char ChooseConsonant(GameState currentGame)
        {
            List<char> availableConsonants = _consonants
            .Where(c => !currentGame.GuessedLetters.Contains(c))
            .ToList();

            if (availableConsonants.Count == 0)
            {
                throw new InvalidOperationException("No unguessed consonants remain.");
            }

            if (_difficulty == AiDifficulty.Easy)
            {
                return availableConsonants[_random.Next(availableConsonants.Count)];
            }
            else
            {
                return _commonConsonants
                    .First(c => availableConsonants.Contains(c));
            }
        }

        /// <summary>
        /// Selects an unguessed vowel based on the configured AI difficulty.
        /// Easy difficulty selects randomly, while Normal difficulty prioritizes common vowels.
        /// </summary>
        /// <param name="currentGame">The current game state containing previously guessed letters.</param>
        /// <returns>An unguessed vowel selected according to the configured AI difficulty.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no unguessed vowels remain.
        /// </exception>
        public char ChooseVowel(GameState currentGame)
        {
            List<char> availableVowels = _vowels
                .Where(c => !currentGame.GuessedLetters.Contains(c))
                .ToList();

            if (availableVowels.Count == 0)
            {
                throw new InvalidOperationException("No unguessed vowels remain.");
            }

            if (_difficulty == AiDifficulty.Easy)
            {
                return availableVowels[_random.Next(availableVowels.Count)];
            }
            else
            {
                return _commonVowels
                    .First(v => availableVowels.Contains(v));
            }
        }

        /// <summary>
        /// Attempts to solve the current puzzle based on puzzle completion
        /// and the configured AI difficulty.
        /// </summary>
        /// <param name="currentGame">The current game state containing the puzzle to solve.</param>
        /// <returns>
        /// The puzzle phrase if the solve attempt succeeds; otherwise, an empty string.
        /// </returns>
        public string AttemptSolve(GameState currentGame)
        {
            double puzzleCompletion = GetPuzzleCompletion(currentGame);
            int correctChance = GetSolveAccuracy(puzzleCompletion);

            if (_random.Next(100) < correctChance)
            {
                return currentGame.CurrentPuzzle.Phrase;
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets a random simulated thinking time for the AI.
        /// </summary>
        /// <returns>A random number of seconds between 1 and 6 inclusive.</returns>
        public int GetThinkingTime()
        {
            return _random.Next(1, 7);
        }

        /// <summary>
        /// Determines whether the AI should attempt to solve the puzzle based on
        /// puzzle completion, configured difficulty, and random chance.
        /// </summary>
        /// <param name="puzzleCompletion">
        /// The completion ratio of the puzzle as a value between 0.0 and 1.0.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the AI should attempt to solve the puzzle;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        private bool ShouldSolvePuzzle(double puzzleCompletion)
        {
            int chance;

            if (_difficulty == AiDifficulty.Easy)
            {
                if (puzzleCompletion >= 0.95)
                {
                    chance = 60;
                }
                else if (puzzleCompletion >= 0.85)
                {
                    chance = 35;
                }
                else if (puzzleCompletion >= 0.70)
                {
                    chance = 15;
                }
                else if (puzzleCompletion >= 0.50)
                {
                    chance = 5;
                }
                else
                {
                    chance = 0;
                }
            }
            else
            {
                if (puzzleCompletion >= 0.95)
                {
                    chance = 80;
                }
                else if (puzzleCompletion >= 0.85)
                {
                    chance = 55;
                }
                else if (puzzleCompletion >= 0.70)
                {
                    chance = 30;
                }
                else if (puzzleCompletion >= 0.50)
                {
                    chance = 10;
                }
                else
                {
                    chance = 0;
                }
            }

            return _random.Next(100) < chance;
        }

        /// <summary>
        /// Determines the percentage chance that the AI's solve attempt will be correct
        /// based on puzzle completion and configured difficulty.
        /// </summary>
        /// <param name="puzzleCompletion">
        /// The completion ratio of the puzzle as a value between 0.0 and 1.0.
        /// </param>
        /// <returns>A percentage chance from 0 to 100 that the solve attempt will be correct.</returns>
        private int GetSolveAccuracy(double puzzleCompletion)
        {
            if (_difficulty == AiDifficulty.Easy)
            {
                if (puzzleCompletion >= 0.95)
                {
                    return 70;
                }

                if (puzzleCompletion >= 0.85)
                {
                    return 45;
                }

                if (puzzleCompletion >= 0.70)
                {
                    return 20;
                }

                if (puzzleCompletion >= 0.50)
                {
                    return 5;
                } 
            }
            else
            {
                if (puzzleCompletion >= 0.95)
                {
                    return 90;
                }

                if (puzzleCompletion >= 0.85)
                {
                    return 70;
                }

                if (puzzleCompletion >= 0.70)
                {
                    return 40;
                }

                if (puzzleCompletion >= 0.50)
                {
                    return 10;
                }
            }

            return 0;
        }

        /// <summary>
        /// Determines whether any vowels remain that have not already been guessed.
        /// </summary>
        /// <param name="guessedLetters">The collection of letters that have already been guessed.</param>
        /// <returns>
        /// <see langword="true"/> if at least one unguessed vowel remains;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        private bool HasRemainingVowels(HashSet<char> guessedLetters)
        {
            foreach (char vowel in _vowels)
            {
                if (!guessedLetters.Contains(vowel))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether the AI should buy a vowel based on puzzle completion,
        /// configured difficulty, and random chance.
        /// </summary>
        /// <param name="puzzleCompletion">
        /// The completion ratio of the puzzle as a value between 0.0 and 1.0.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the AI should buy a vowel; otherwise,
        /// <see langword="false"/>. Always returns <see langword="false"/>
        /// when puzzle completion is 90% or higher.
        /// </returns>
        private bool ShouldBuyVowel(double puzzleCompletion)
        {
            if (puzzleCompletion >= 0.90)
            {
                return false;
            }

            if (_difficulty == AiDifficulty.Easy)
            {
                return _random.Next(100) < 35; 
            }
            else
            {
                return _random.Next(100) < 60;
            }
        }

        /// <summary>
        /// Calculates the completion ratio of the current puzzle based on revealed letters.
        /// </summary>
        /// <param name="currentGame">
        /// The game state containing the current puzzle and guessed letters.
        /// </param>
        /// <returns>
        /// A value between 0.0 and 1.0 representing the ratio of revealed letters
        /// to total letters in the puzzle.
        /// </returns>
        private double GetPuzzleCompletion(GameState currentGame)
        {
            Puzzle puzzle = currentGame.CurrentPuzzle;
            HashSet<char> guessedLetters = currentGame.GuessedLetters;

            int totalLetters = puzzle.Phrase.Count(c => char.IsLetter(c));
            int revealedLettersCount = puzzle.Phrase.Count(
                c => char.IsLetter(c) && 
                guessedLetters.Contains(char.ToUpper(c)));

            if (totalLetters == 0)
            {
                return 0;
            }

            return (double)revealedLettersCount / totalLetters;
        }
    }
}
