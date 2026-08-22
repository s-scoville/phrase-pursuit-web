using PhrasePursuitWeb.Core.Managers;

namespace PhrasePursuitWeb.Web.Services
{
    /// <summary>
    /// Maintains the active game session for the web application across page navigation.
    /// </summary>
    public class GameSessionService
    {
        /// <summary>
        /// Gets or sets the game manager for the active game session.
        /// </summary>
        public GameManager? CurrentGame { get; set; }
    }
}
