using PhrasePursuitWeb.Core.AI;
using PhrasePursuitWeb.Core.Enums;
using PhrasePursuitWeb.Core.Models;

namespace PhrasePursuitWeb.Core.Managers
{
    /// <summary>
    /// Manages the core game logic and state for Phrase Pursuit, including player turns,
    /// spinner outcomes, letter guesses, vowel purchases, puzzle solving, statistics,
    /// and AI opponent behavior.
    /// </summary>
    public class GameManager
    {
        /// <summary>
        /// Represents the puzzle manager used for puzzle selection, rendering, and validation.
        /// </summary>
        private readonly PuzzleManager _puzzleManager;

        /// <summary>
        /// Represents the spinner manager used to generate randomized spinner outcomes.
        /// </summary>
        private readonly SpinManager _spinManager;

        /// <summary>
        /// Represents the statistics manager used to load, update, and persist player statistics.
        /// </summary>
        private readonly StatisticsManager _statisticsManager;

        /// <summary>
        /// Represents the AI controller used to determine computer-player actions and guesses.
        /// </summary>
        private readonly AiController _aiController;

        /// <summary>
        /// Represents the random number generator used when selecting AI player names.
        /// </summary>
        private readonly Random _random;

        /// <summary>
        /// Represents the predefined names available for randomly generated AI players.
        /// </summary>
        private static readonly string[] _aiNames =
        {
            "Aaron",
            "Alex",
            "Amanda",
            "Andrew",
            "Ashley",
            "Ben",
            "Bob",
            "Brandon",
            "Brittany",
            "Chris",
            "Courtney",
            "Daniel",
            "David",
            "Derek",
            "Emily",
            "Eric",
            "Erica",
            "Ethan",
            "Gabriel",
            "Grace",
            "Hannah",
            "Heather",
            "Jacob",
            "James",
            "Jason",
            "Jennifer",
            "Jessica",
            "Jordan",
            "Josh",
            "Justin",
            "Katie",
            "Kevin",
            "Kyle",
            "Lauren",
            "Lisa",
            "Madison",
            "Mark",
            "Matt",
            "Megan",
            "Michael",
            "Michelle",
            "Nathan",
            "Nicole",
            "Olivia",
            "Patrick",
            "Rachel",
            "Rebecca",
            "Ryan",
            "Samantha",
            "Sarah",
            "Scott",
            "Stephanie",
            "Taylor",
            "Thomas",
            "Tim",
            "Tyler",
            "Victoria",
            "William",
            "Zach",
            "Zoe"
        };

        /// <summary>
        /// Gets the current game state.
        /// </summary>
        public GameState CurrentGame { get; private set; } = null!;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameManager"/> class
        /// with the required gameplay managers and AI controller.
        /// </summary>
        /// <param name="puzzleManager">The puzzle manager used for puzzle-related operations.</param>
        /// <param name="spinManager">The spinner manager used to generate spinner outcomes.</param>
        /// <param name="statisticsManager">The statistics manager used to manage player statistics.</param>
        /// <param name="aiController">The AI controller used to determine computer-player behavior.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any required dependency is null.
        /// </exception>
        public GameManager(PuzzleManager puzzleManager, SpinManager spinManager, StatisticsManager statisticsManager, AiController aiController)
        {
            
            _puzzleManager = puzzleManager
                ?? throw new ArgumentNullException(nameof(puzzleManager));
            
            _spinManager = spinManager 
                ?? throw new ArgumentNullException(nameof(spinManager));
            
            _statisticsManager = statisticsManager 
                ?? throw new ArgumentNullException(nameof(statisticsManager));
            
            _aiController = aiController
                ?? throw new ArgumentNullException(nameof(aiController));
            
            _random = new Random();
        }

        /// <summary>
        /// Starts a new game with a randomly selected unplayed puzzle, one human player,
        /// and two randomly named AI opponents.
        /// </summary>
        /// <param name="playerName">The name of the human player.</param>
        /// <returns>A task representing the asynchronous game initialization operation.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="playerName"/> is null, empty, or whitespace.
        /// </exception>
        public async Task StartNewGameAsync(string playerName)
        {
            if (string.IsNullOrWhiteSpace(playerName))
            {
                throw new ArgumentException("Player name cannot be empty.", nameof(playerName));
            }

            Puzzle currentPuzzle = await _puzzleManager.GetRandomPuzzleAsync();

            string[] aiNames = GetRandomAiNames();

            List<Player> players = new List<Player>
            {
                new Player($"{playerName} (Player)", PlayerType.Player),
                new Player($"{aiNames[0]} (AI)", PlayerType.Computer),
                new Player($"{aiNames[1]} (AI)", PlayerType.Computer)
            };

            CurrentGame = new GameState(currentPuzzle, players);
        }

        /// <summary>
        /// Gets the current puzzle formatted for display based on the letters that have been guessed.
        /// </summary>
        /// <returns>
        /// A formatted string containing revealed letters, hidden letters, and preserved non-letter characters.
        /// </returns>
        public string GetPuzzleDisplay()
        {
            return _puzzleManager.RenderPuzzle(
                CurrentGame.CurrentPuzzle,
                CurrentGame.GuessedLetters);
        }

        /// <summary>
        /// Generates a spinner result for the current player's turn.
        /// </summary>
        /// <returns>A <see cref="SpinResult"/> containing the generated spinner outcome.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the current game phase is not <see cref="TurnPhase.WaitingForAction"/>.
        /// </exception>
        public SpinResult Spin()
        {
            if (CurrentGame.CurrentPhase != TurnPhase.WaitingForAction)
            {
                throw new InvalidOperationException("Cannot spin the spinner at this time.");
            }

            return _spinManager.Spin();
        }

        /// <summary>
        /// Processes a consonant guess for the current player and updates the game state.
        /// </summary>
        /// <param name="letter">The consonant to guess.</param>
        /// <returns>
        /// A task whose result contains the outcome of the consonant guess.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the game is not waiting for a consonant or the letter has already been guessed.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="letter"/> is not a valid consonant.
        /// </exception>
        public async Task<GuessResult> GuessConsonantAsync(char letter)
        {
            if (CurrentGame.CurrentPhase != TurnPhase.WaitingForConsonant)
            {
                throw new InvalidOperationException("Cannot guess a consonant until a spin is conducted.");
            }

            letter = char.ToUpper(letter);

            if (!char.IsLetter(letter) || "AEIOU".Contains(letter))
            {
                throw new ArgumentException("Invalid consonant guess.");
            }

            if (CurrentGame.GuessedLetters.Contains(letter))
            {
                throw new InvalidOperationException("This letter has already been guessed.");
            }

            CurrentGame.AddGuessedLetter(letter);

            return await ProcessLetterGuessAsync(letter, isVowel: false);
        }

        /// <summary>
        /// Begins the vowel-purchase process for the current player by placing the game
        /// into the vowel-selection phase.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the current phase does not allow a vowel purchase,
        /// the current player has less than $50, or no unguessed vowels remain.
        /// </exception>
        public void BuyVowel()
        {
            if (CurrentGame.CurrentPhase != TurnPhase.WaitingForAction)
            {
                throw new InvalidOperationException("Cannot buy a vowel at this time.");
            }

            Player currentPlayer =
                CurrentGame.CurrentPlayer;

            bool hasRemainingVowel = false;

            foreach (char vowel in "AEIOU")
            {
                if (!CurrentGame.GuessedLetters.Contains(vowel))
                {
                    hasRemainingVowel = true;
                    break;
                }
            }

            if (currentPlayer.CurrentWinnings < 50)
            {
                throw new InvalidOperationException("Not enough money to buy a vowel.");
            }

            if (!hasRemainingVowel)
            {
                throw new InvalidOperationException("No vowels remaining to guess.");
            }

            CurrentGame.SetPhase(TurnPhase.WaitingForVowel);
        }

        /// <summary>
        /// Processes a vowel guess for the current player after deducting the $50 vowel cost.
        /// </summary>
        /// <param name="letter">The vowel to guess.</param>
        /// <returns>
        /// A task whose result contains the outcome of the vowel guess.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the game is not waiting for a vowel or the letter has already been guessed.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="letter"/> is not a valid vowel.
        /// </exception>
        public async Task<GuessResult> GuessVowelAsync(char letter)
        {
            if (CurrentGame.CurrentPhase != TurnPhase.WaitingForVowel)
            {
                throw new InvalidOperationException("Cannot guess a vowel at this time.");
            }

            letter = char.ToUpper(letter);

            if (!"AEIOU".Contains(letter))
            {
                throw new ArgumentException("Invalid vowel guess.");
            }

            if (CurrentGame.GuessedLetters.Contains(letter))
            {
                throw new InvalidOperationException("This letter has already been guessed.");
            }

            Player currentPlayer =
                CurrentGame.CurrentPlayer;

            currentPlayer.DeductWinnings(50);

            CurrentGame.AddGuessedLetter(letter);

            return await ProcessLetterGuessAsync(letter, isVowel: true);
        }

        /// <summary>
        /// Processes an attempt to solve the current puzzle.
        /// A correct solution ends the game, while an incorrect solution resets the
        /// current player's winnings and ends their turn.
        /// </summary>
        /// <param name="answer">The submitted puzzle solution.</param>
        /// <returns>
        /// A task whose result indicates whether the puzzle was solved successfully.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the current game phase is not <see cref="TurnPhase.WaitingForAction"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="answer"/> is null, empty, or whitespace.
        /// </exception>
        public async Task<SolveResult> SolvePuzzleAsync(string answer)
        {
            if (CurrentGame.CurrentPhase != TurnPhase.WaitingForAction)
            {
                throw new InvalidOperationException("Cannot solve the puzzle at this time.");
            }

            if (string.IsNullOrWhiteSpace(answer))
            {
                throw new ArgumentException("Answer cannot be empty.", nameof(answer));
            }

            string normalizedAnswer = answer.ToUpper().Trim();

            if (normalizedAnswer == CurrentGame.CurrentPuzzle.Phrase)
            {
                await EndGameAsync();
                return new SolveResult(true);
            }

            HandleBankrupt();
            return new SolveResult(false);
        }

        /// <summary>
        /// Executes a complete action for the current AI player by selecting an action,
        /// applying the appropriate game logic, and returning the resulting action details.
        /// </summary>
        /// <returns>
        /// A task whose result contains the AI action and any associated spinner,
        /// letter-guess, or solve result.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the current player is not a computer player or the AI selects
        /// an unsupported action.
        /// </exception>
        public async Task<AiActionResult> RunAiTurnAsync()
        {
            Player currentPlayer =
                CurrentGame.CurrentPlayer;

            if (currentPlayer.PlayerType != PlayerType.Computer)
            {
                throw new InvalidOperationException("It's not the AI's turn.");
            }

            AiAction action = _aiController.ChooseAction(CurrentGame);

            switch (action)
            {
                case AiAction.Spin:
                    {
                        SpinResult spinResult = Spin();

                        ApplySpinResult(spinResult);

                        if (spinResult.Outcome == SpinOutcome.Money)
                        {
                            char aiConsonantGuess =
                                _aiController.ChooseConsonant(CurrentGame);

                            GuessResult guessResult =
                                await GuessConsonantAsync(aiConsonantGuess);

                            return new AiActionResult(
                                action: AiAction.Spin,
                                spinResult: spinResult,
                                guessResult: guessResult
                            );
                        }

                        return new AiActionResult(
                            action: AiAction.Spin,
                            spinResult: spinResult
                        );
                    }
                case AiAction.BuyVowel:
                    {
                        BuyVowel();

                        char aiVowelGuess =
                            _aiController.ChooseVowel(CurrentGame);

                        GuessResult guessResult =
                            await GuessVowelAsync(aiVowelGuess);

                        return new AiActionResult(
                            action: AiAction.BuyVowel,
                            guessResult: guessResult
                        );
                    }
                case AiAction.SolvePuzzle:
                    {
                        string solveAttempt = _aiController.AttemptSolve(CurrentGame);

                        SolveResult solveResult = await SolvePuzzleAsync(solveAttempt);

                        return new AiActionResult(
                            action: AiAction.SolvePuzzle,
                            solveResult: solveResult
                        );
                    }
                default:
                    throw new InvalidOperationException(
                        "The AI selected an unsupported action.");
            }
        }

        /// <summary>
        /// Applies a spinner result to the current game by processing bankruptcy,
        /// ending the turn, or preparing the current player to guess a consonant.
        /// </summary>
        /// <param name="spinResult">The spinner result to apply to the current turn.</param>
        public void ApplySpinResult(SpinResult spinResult)
        {
            switch (spinResult.Outcome)
            {
                case SpinOutcome.Bankrupt:
                    HandleBankrupt();
                    break;
                case SpinOutcome.LoseTurn:
                    HandleLoseTurn();
                    break;
                case SpinOutcome.Money:
                    CurrentGame.SetSpinValue(spinResult.MoneyValue ?? 0);
                    CurrentGame.SetPhase(TurnPhase.WaitingForConsonant);
                    break;
            }
        }

        /// <summary>
        /// Processes a consonant or vowel guess, updates the current game state,
        /// awards any applicable winnings, and handles turn or game completion.
        /// </summary>
        /// <param name="letter">The letter being guessed.</param>
        /// <param name="isVowel">
        /// <see langword="true"/> when the guess is a vowel; otherwise, <see langword="false"/>.
        /// </param>
        /// <returns>
        /// A task whose result contains the correctness of the guess, number of occurrences,
        /// money earned, and whether the turn or puzzle ended.
        /// </returns>
        private async Task<GuessResult> ProcessLetterGuessAsync(char letter, bool isVowel)
        {
            int numOccurrences = _puzzleManager.CountOccurrences(CurrentGame.CurrentPuzzle, letter);

            if (numOccurrences == 0)
            {
                EndTurn();

                return new GuessResult(
                    guessedLetter: letter,
                    wasCorrect: false,
                    occurrences: 0,
                    moneyEarned: 0,
                    turnEnded: true,
                    puzzleCompleted: false
                );
            }

            bool puzzleCompleted = _puzzleManager.IsPuzzleCompleted(CurrentGame.CurrentPuzzle, CurrentGame.GuessedLetters);
            bool turnEnded = isVowel || puzzleCompleted;

            Player currentPlayer =
                CurrentGame.CurrentPlayer;

            int moneyEarned = 0;

            if (!isVowel)
            {
                moneyEarned = numOccurrences * CurrentGame.CurrentSpinValue;
                currentPlayer.AddWinnings(moneyEarned);
            }

            if (puzzleCompleted)
            {
                await EndGameAsync();
            }
            else if (isVowel)
            {
                EndTurn();
            }
            else
            {
                CurrentGame.SetPhase(TurnPhase.WaitingForAction);
            }

            return new GuessResult(
                guessedLetter: letter,
                wasCorrect: true,
                occurrences: numOccurrences,
                moneyEarned: moneyEarned,
                turnEnded: turnEnded,
                puzzleCompleted: puzzleCompleted
            );
        }

        /// <summary>
        /// Ends the current player's turn and advances the game to the next player.
        /// </summary>
        private void EndTurn()
        {
            CurrentGame.AdvanceTurn();
        }

        /// <summary>
        /// Handles a bankrupt outcome by resetting the current player's winnings
        /// and ending their turn.
        /// </summary>
        private void HandleBankrupt()
        {
            Player currentPlayer =
                CurrentGame.CurrentPlayer;

            currentPlayer.ResetWinnings();

            EndTurn();
        }

        /// <summary>
        /// Handles a lose-turn outcome by ending the current player's turn
        /// without changing their winnings.
        /// </summary>
        private void HandleLoseTurn()
        {
            EndTurn();
        }

        /// <summary>
        /// Ends the current game, records the appropriate win or loss statistics,
        /// reveals the completed puzzle, and persists the updated statistics.
        /// </summary>
        /// <returns>A task representing the asynchronous statistics persistence operation.</returns>
        private async Task EndGameAsync()
        {
            Player currentPlayer =
                CurrentGame.CurrentPlayer;

            if (currentPlayer.PlayerType == PlayerType.Player)
            {
                _statisticsManager.Statistics.RecordWin();
                _statisticsManager.Statistics.RecordGameWinnings(
                    currentPlayer.CurrentWinnings);
            }
            else
            {
                _statisticsManager.Statistics.RecordLoss();
            }

            CurrentGame.EndGame();

            RevealPuzzle();

            await _statisticsManager.SaveStatisticsAsync();
        }

        /// <summary>
        /// Reveals all letters in the current puzzle by adding every alphabetic character
        /// to the collection of guessed letters.
        /// </summary>
        private void RevealPuzzle()
        {
            foreach (char letter in "ABCDEFGHIJKLMNOPQRSTUVWXYZ")
            {
                CurrentGame.GuessedLetters.Add(letter);
            }
        }

        /// <summary>
        /// Selects two distinct names randomly from the available AI player names.
        /// </summary>
        /// <returns>An array containing two different randomly selected AI names.</returns>
        private string[] GetRandomAiNames()
        {
            int firstIndex = _random.Next(_aiNames.Length);
            int secondIndex = _random.Next(_aiNames.Length);

            while (secondIndex == firstIndex)
            {
                secondIndex = _random.Next(_aiNames.Length);
            }

            return new string[]
            {
                _aiNames[firstIndex],
                _aiNames[secondIndex]
            };
        }
    }
}
