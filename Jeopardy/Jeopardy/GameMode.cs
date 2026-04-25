namespace Jeopardy
{
    /// <summary>
    /// Enum to define the game mode
    /// </summary>
    public enum GameMode
    {
        Buzzer,     // Players must buzz in to answer
        Timer       // All players answer within a time limit
    }

    /// <summary>
    /// Enum to define if game is local or online
    /// </summary>
    public enum GameType
    {
        LocalGame,  // Single machine, pass-and-play
        OnlineGame  // Multiple machines via Azure
    }
}
