# Phrase Pursuit Web

Phrase Pursuit Web is a browser-based word puzzle game built with C# and Blazor WebAssembly. Players compete against two computer-controlled opponents by earning money, guessing letters, buying vowels, and attempting to solve word puzzles.

This project is a web-based redevelopment and expansion of my original Phrase Pursuit Windows Forms application. Rather than simply porting the original project to the web, I am using this version as an opportunity to improve the game's structure, add new features, and create a more polished and responsive user experience.

## Project Status

**In Development**

Core gameplay and game-flow functionality are complete. The game can now be played from setup through completion, including computer-controlled opponents, puzzle solving, persistent statistics, replay functionality, and game-over navigation.

Development is now moving into the final presentation phase, which will focus on responsive layouts, visual styling, animations, and spinner presentation.

## Features

- Single-player gameplay against two computer-controlled opponents
- Easy and Normal opponent difficulty levels
- Difficulty-based letter selection and decision-making
- Simulated spinner with weighted money and special outcomes
- Consonant guessing and vowel purchasing
- Full-puzzle solving
- 400 puzzles across 10 categories
- Persistent puzzle history to reduce repeated puzzles
- Persistent player statistics
- Game record statistics including wins, losses, and win percentage
- Winnings statistics including lifetime, highest, and average winnings
- Browser-based local storage
- Play Again functionality
- Automated testing of core game logic

## In Progress

- Responsive desktop and mobile layouts
- Final visual theme and styling
- Game animations and transitions
- Animated cylindrical spinner presentation
- Branding and final interface polish

## Technology

- C#
- .NET 10
- Blazor WebAssembly
- Razor
- HTML
- CSS
- JSON
- JavaScript interoperability
- xUnit
- Git and GitHub
- GitHub Pages

## Design

One of my main goals with this version is to improve the separation between the game logic and the user interface. In the original Windows Forms version, the main game form ended up handling more of the gameplay logic than I wanted it to. This version separates the core game logic from the Blazor interface so that the UI is primarily responsible for displaying the current game state and collecting player input.

The solution separates the application into Core, Web, and Tests projects. Core contains the game models, managers, computer-player behavior, and other gameplay logic. Web contains the Blazor interface and browser-specific functionality, while Tests provides automated testing of the core game logic independently of the browser interface.

Browser storage is accessed through a storage abstraction, allowing persistent statistics and puzzle history to remain separate from the core gameplay models.

## Testing

Automated testing with xUnit is used to test core game behavior independently of the browser interface.

Manual gameplay testing is also used throughout development to verify complete game flow, computer-player behavior, puzzle solving, statistics persistence, replay functionality, and synchronization between the core game state and its presentation in the user interface.

Responsive layouts, animations, browser behavior, and final presentation will receive additional testing during the final development phase.

## Deployment

Phrase Pursuit Web is deployed through GitHub Pages at:

**https://phrasepursuit.stevenscoville.dev**

## Development Progress

### Week 8

- Implemented persistent player statistics and browser local storage
- Added the Statistics page with game record and winnings information
- Added statistics navigation and reset functionality
- Continued development of computer-controlled opponent behavior
- Improved puzzle handling and game-state integration
- Corrected gameplay presentation issues discovered during testing

### Week 9

- Added the SolveBoard interface for submitting and canceling puzzle solutions
- Added the GameOverPanel with Play Again, Game Setup, and Main Menu options
- Implemented replay functionality while preserving the current player name and difficulty
- Improved synchronization between computer turns and the displayed game state
- Prevented player controls from becoming available before computer turn presentation completes
- Corrected the timing of computer winnings updates and game-over presentation
- Corrected statistics loading to preserve data between application sessions
- Expanded the puzzle library to 400 puzzles across 10 categories
- Adjusted spinner balance to reduce the frequency of Bankrupt outcomes
- Performed complete-game and regression testing of the updated game flow
- Deployed the application through GitHub Pages
- Configured the custom Phrase Pursuit subdomain

## Next Steps

The final development phase will focus on responsive design, visual styling, branding, animations, the cylindrical spinner presentation, and final testing.