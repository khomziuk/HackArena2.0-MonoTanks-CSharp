using MonoTanksClientLogic.Networking;

namespace MonoTanksClientLogic;

/// <summary>
/// Represents required methods for agent.
/// </summary>
public interface IAgent
{
    public AgentResponse NextMove(GameState gameState);

    public void onGameEnd(GameEnd gameEnd);

    public void onSubsequentLobbyData(LobbyData lobbyData);
}
