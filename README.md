# Phrase Pursuit Web

Phrase Pursuit Web is a browser-based word puzzle game built with C# and Blazor WebAssembly. Players compete against two computer-controlled opponents by earning money, guessing letters, buying vowels, and attempting to solve word puzzles.

This project is a web-based redevelopment and expansion of my original Phrase Pursuit Windows Forms application. Rather than simply porting the original project to the web, I am using this version as an opportunity to improve the game's structure, add new features, and create a more polished and responsive user experience.

## Project Status

**In Development**

The project is currently in the design and scaffolding stage. The initial solution structure has been created, and gameplay development will begin during the next phase of the project.

## Planned Features

- Single-player gameplay against two AI opponents
- Easy and Normal AI difficulty levels
- More intelligent letter selection for Normal AI opponents
- Simulated digital spinner with weighted outcomes
- Consonant guessing and vowel purchasing
- Puzzle solving
- Expanded puzzle library
- Reduced repetition of previously played puzzles
- Persistent player statistics
- Additional statistics, including win percentage
- Browser-based local storage
- Responsive desktop and mobile layouts
- Automated testing of core game logic
- Deployment through GitHub Pages

## Technology

- C#
- .NET 10
- Blazor WebAssembly
- Razor
- HTML
- CSS
- JSON
- xUnit
- Git and GitHub
- GitHub Pages

## Design

One of my main goals with this version is to improve the separation between the game logic and the user interface. In the original Windows Forms version, the main game form ended up handling more of the gameplay logic than I wanted it to. This version separates the core game logic from the Blazor interface so that the UI is primarily responsible for displaying the current game state and collecting player input.

The project also includes a separate testing project so that the game logic can be tested independently of the browser interface.

## Testing

Automated testing will be done with xUnit and will focus primarily on the core game logic, including game state, turn progression, puzzle handling, spinner behavior, statistics, and AI behavior.

Manual testing will also be used to test gameplay, the Blazor user interface, responsive layouts, and browser-specific features.

## Deployment

The completed application is planned to be hosted through GitHub Pages at:

**phrasepursuit.stevenscoville.dev**

## Development

Phrase Pursuit Web is currently under active development. The README will be updated with additional information, screenshots, and gameplay instructions as the project progresses.