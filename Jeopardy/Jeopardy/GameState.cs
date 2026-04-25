using System.Collections.Generic;

namespace Jeopardy
{
    /// <summary>
    /// Represents the game state and configuration
    /// </summary>
    public class GameState
    {
        public GameMode GameMode { get; set; }
        public GameType GameType { get; set; }
        public List<string> PlayerNames { get; set; }
        public int TimerDuration { get; set; } // in seconds (60, 90, or 120)
        public bool IsHost { get; set; }
        public string LobbyCode { get; set; }
        
        public GameState()
        {
            PlayerNames = new List<string>();
            TimerDuration = 60;
            IsHost = false;
            LobbyCode = "";
        }
    }
}
