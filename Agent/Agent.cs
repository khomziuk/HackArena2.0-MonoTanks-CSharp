using MonoTanksClientLogic;
using MonoTanksClientLogic.Networking;

namespace Agent
{
    public class Agent : IAgent
    {
        public Agent(LobbyDataPayload lobbyData)
        {
            Console.WriteLine($"AGENT CREATED!!! {lobbyData.PlayerId}");
        }

        public AgentResponse NextMove(GameStatePayload.ForPlayer gameState)
        {
            return AgentResponse.Pass();
        }

        public void onGameEnd(GameEndPayload gameEnd)
        {
            throw new NotImplementedException();
        }

        public void OnSubsequentLobbyData(LobbyDataPayload lobbyData)
        {
            throw new NotImplementedException();
        }
    }
}
