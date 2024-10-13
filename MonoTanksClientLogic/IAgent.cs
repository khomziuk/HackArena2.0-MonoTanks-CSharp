using MonoTanksClientLogic.Networking;

namespace MonoTanksClientLogic;

/// <summary>
/// Represents required methods for agent.
/// </summary>
public interface IAgent
{
    public void OnSubsequentLobbyData(LobbyDataPayload lobbyData);

    public AgentResponse NextMove(GameStatePayload gameState);

    public void onGameEnd(GameEndPayload gameEnd);
}
