namespace MonoTanksClientLogic;

/// <summary>
/// Represents required methods for agent.
/// </summary>
public interface IAgent
{
    public void OnGameStarting();

    public AgentResponse NextMove(GameState gameState);

    public void OnGameEnd(GameEnd gameEnd);

    public void OnSubsequentLobbyData(LobbyData lobbyData);

    public void OnWarningReceived(Warning warning, string? message);
}
