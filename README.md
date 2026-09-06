# Phrase Pursuit Web

Phrase Pursuit Web is a browser-based word puzzle game built with C# and Blazor WebAssembly. Players compete against two computer-controlled opponents by spinning for potential winnings, guessing consonants, purchasing vowels, and attempting to solve word puzzles.

This project is a complete web-based redevelopment of my original Phrase Pursuit Windows Forms application. Rather than directly porting the desktop application, I redesigned the project around a web architecture that separates core gameplay logic from the user interface, supports persistent browser data, adapts across desktop and mobile devices, and provides a more polished gameplay experience.

## Live Application

Phrase Pursuit Web is deployed through GitHub Pages and can be played at:

**https://phrasepursuit.stevenscoville.dev**

## Features

### Gameplay

- Single-player gameplay against two computer-controlled opponents
- Simulated spinner with weighted money values, Bankrupt, and Lose Turn outcomes
- Consonant guessing and vowel purchasing
- Full-puzzle solving
- Current-player and winnings tracking
- Automatic turn progression between the player and computer opponents
- Play Again functionality without returning to game setup
- Game Setup and Main Menu navigation after game completion

### Computer Opponents

- Easy and Normal difficulty levels
- Difficulty-based letter selection
- Different decision-making behavior based on selected difficulty
- Computer-controlled spinning, vowel purchasing, letter guessing, and puzzle-solving attempts
- Gameplay presentation synchronized with computer decision-making and turn progression

### Puzzles

- 400 puzzles across 10 categories
- Persistent puzzle history
- Previously played puzzles are avoided until the available puzzle collection has been exhausted
- Puzzle state updates as correct letters are discovered
- Non-letter characters are automatically revealed

### Player Statistics

Phrase Pursuit maintains statistics between browser sessions using browser-based storage.

Tracked statistics include:

- Games Played
- Wins
- Losses
- Win Percentage
- Lifetime Winnings
- Highest Winnings
- Average Winnings

Statistics can also be reset from within the application.

### Responsive Interface

The interface is designed to adapt across desktop and mobile screen sizes while maintaining the same gameplay hierarchy.

Responsive behavior includes:

- Fluid gameplay layouts
- Responsive player panels
- Flexible Letter Board controls
- Responsive spinner and status displays
- Mobile-friendly game controls
- Responsive Game Setup and Statistics pages
- Layout changes when additional reorganization is necessary on smaller screens

### Gameplay Presentation

The application includes several animations and transitions designed to communicate gameplay state without interfering with interaction:

- Animated cylindrical spinner reel
- Progressive spinner deceleration and final result settling
- Active-player expansion and elevation during turn changes
- Sliding transition between the Letter Board and Solve interface
- Animated Game Over overlay
- Synchronized presentation during computer-controlled turns

Gameplay actions are temporarily unavailable when necessary so that the visual presentation remains synchronized with the underlying game state.

## Technology Stack

| Area | Technology |
| --- | --- |
| Language | C# |
| Framework | .NET 10 |
| Web Framework | Blazor WebAssembly |
| UI | Razor, HTML, CSS |
| Puzzle Data | JSON |
| Browser Persistence | JavaScript Interoperability / Local Storage |
| Automated Testing | xUnit |
| Version Control | Git / GitHub |
| CI/CD | GitHub Actions |
| Hosting | GitHub Pages |
| Production Domain | phrasepursuit.stevenscoville.dev |

## Architecture

One of the primary goals of Phrase Pursuit Web was to improve the separation between gameplay logic and presentation.

The original Windows Forms version placed more gameplay responsibility in the main game form than I wanted. The web version restructures the application so that core game rules and state management remain independent of the Blazor interface.

The solution is divided into three projects:

### `PhrasePursuitWeb.Core`

Contains application logic that is independent of the browser interface, including:

- Models
- Game state
- Game management
- Puzzle management
- Spinner logic
- Statistics management
- Computer-player behavior
- Enumerations
- Storage abstractions

### `PhrasePursuitWeb.Web`

Contains the Blazor WebAssembly application and presentation layer, including:

- Razor pages
- Reusable gameplay components
- Responsive styling
- Browser-specific storage implementation
- Gameplay presentation and animations
- Navigation
- JavaScript interoperability

### `PhrasePursuitWeb.Tests`

Contains xUnit tests for core application behavior independently of the browser interface.

This separation allows the underlying gameplay logic to be tested without requiring the Blazor UI or browser environment.

## Game Management

`GameManager` coordinates the primary game flow and acts as the central connection between the user interface and the underlying gameplay systems.

Responsibilities include:

- Starting games
- Managing turns
- Processing spins
- Processing consonant and vowel guesses
- Handling puzzle-solving attempts
- Coordinating computer-controlled turns
- Updating game state
- Detecting puzzle and game completion
- Recording completed-game statistics

The Blazor interface reads the current game state and invokes the appropriate game operations rather than implementing the core rules itself.

## Computer-Player Behavior

Computer-controlled opponents are managed separately from the user interface.

Difficulty affects how computer players select letters and make gameplay decisions. Easy opponents use less strategic selection, while Normal opponents use prioritized letter choices and more deliberate decision-making.

Computer turns are coordinated through the game-management layer so that the UI does not need to implement computer-player logic directly.

The browser presentation adds deliberate timing between computer actions so players can follow decisions, spins, guesses, winnings changes, and turn transitions rather than having an entire computer turn complete instantaneously.

## Spinner

The spinner uses weighted outcomes to determine gameplay results, including money values, Bankrupt, and Lose Turn.

The visual spinner is implemented as a cylindrical reel rather than a traditional circular wheel. It displays multiple outcomes simultaneously using perspective, scaling, opacity, and vertical movement to create the appearance of values rotating around a cylinder.

During a spin:

1. The gameplay layer determines the spin result.
2. The visual reel begins moving through its fixed sequence of values.
3. The reel progressively slows as it approaches the selected outcome.
4. The final value settles inside the selection area.
5. Gameplay continues only after the spinner animation reports that it has completed.

This keeps the animated presentation synchronized with the actual game result.

## Browser Persistence

Phrase Pursuit uses browser-based local storage to preserve data between sessions.

Persistent data includes:

- Player statistics
- Previously played puzzle history

The Core project does not directly depend on browser storage. Instead, persistence is accessed through an `IStorageService` abstraction.

The Web project provides the browser-specific implementation, while automated tests can substitute a fake storage implementation. This keeps browser-specific functionality separate from the core application logic and improves testability.

## Responsive Design

Phrase Pursuit uses a fluid responsive design rather than relying exclusively on fixed desktop and mobile layouts.

The interface uses techniques including:

- CSS Flexbox
- CSS Grid
- `minmax()`
- `clamp()`
- Percentage-based sizing
- `aspect-ratio`
- Flexible growth and shrinking
- Responsive breakpoints where structural reorganization is necessary

For example, the Letter Board uses a fluid seven-column grid so the letter controls grow and shrink with the available gameplay area while maintaining consistent proportions.

On smaller screens, the overall gameplay hierarchy is preserved while the interaction area reorganizes vertically to provide additional space for player input, the spinner, status information, and controls.

## Testing

Phrase Pursuit includes automated xUnit testing of the core application independently of the Blazor interface.

Automated tests cover areas including:

- Models
- Game initialization
- Game management
- Puzzle loading and rendering
- Puzzle history
- Spinner outcomes
- Consonant guesses
- Vowel purchases and guesses
- Puzzle solving
- Turn progression
- Computer-player behavior
- Statistics
- Persistent-storage interactions

Manual gameplay testing was also performed throughout development to verify behavior that crosses the boundary between the core game logic and browser presentation.

Manual testing included:

- Complete gameplay sessions
- Easy and Normal computer opponents
- Spinner animation and result synchronization
- Human and computer turn transitions
- Puzzle completion
- Solve and Cancel behavior
- Game Over behavior
- Play Again functionality
- Statistics persistence and reset behavior
- Browser navigation
- Desktop and mobile layouts
- Multiple viewport sizes
- Production deployment behavior

## Deployment

Phrase Pursuit Web is hosted using GitHub Pages.

Deployment is automated through GitHub Actions. Changes integrated into the production branch are built and published through the deployment workflow.

The deployment configuration also includes support for Blazor client-side routing so that directly navigating to or refreshing application routes continues to load the application correctly.

The production site uses a custom domain with HTTPS:

**https://phrasepursuit.stevenscoville.dev**

## Project Structure

```text
PhrasePursuitWeb
├── PhrasePursuitWeb.Core
│   ├── AI
│   ├── Enums
│   ├── Interfaces
│   ├── Managers
│   └── Models
│
├── PhrasePursuitWeb.Web
│   ├── Components
│   ├── Pages
│   └── wwwroot
│
└── PhrasePursuitWeb.Tests
    ├── AI
    ├── Managers
    └── Models
```

## Running Locally

### Prerequisites

- .NET 10 SDK
- A modern web browser

### Clone the Repository

```bash
git clone <repository-url>
cd PhrasePursuitWeb
```

### Restore Dependencies

```bash
dotnet restore
```

### Build the Solution

```bash
dotnet build
```

### Run the Web Application

```bash
dotnet run --project PhrasePursuitWeb.Web
```

Open the local address displayed by .NET in your browser.

### Run Automated Tests

```bash
dotnet test
```

## Future Enhancements

Phrase Pursuit Web is functionally complete, but the project may continue to receive optional improvements over time.

Possible future enhancements include:

- Additional computer-opponent difficulty levels
- Additional puzzle categories and puzzles
- Further animation and presentation refinements
- Additional automated test coverage
- Accessibility improvements
- Additional gameplay statistics

## Background

Phrase Pursuit Web began as a redevelopment of an earlier Windows Forms project. Rebuilding the application for the browser provided an opportunity to revisit architectural decisions from the original version rather than simply recreating its interface.

The project became an exercise in separating application logic from presentation, designing reusable Razor components, working with dependency injection and browser persistence, implementing responsive layouts, automating deployment, and coordinating asynchronous gameplay behavior with an animated user interface.

The result is a standalone web application that retains the gameplay of the original Phrase Pursuit while substantially expanding its architecture, portability, persistence, presentation, and overall user experience.