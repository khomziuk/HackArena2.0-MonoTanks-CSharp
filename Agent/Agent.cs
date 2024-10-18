using MonoTanksClientLogic;
using MonoTanksClientLogic.Networking;

namespace Agent
{
    public class Agent : IAgent
    {
        public Agent(LobbyData lobbyData)
        {
            // Do your agent initialization here.
        }

        public AgentResponse NextMove(GameState gameState)
        {
            // Define behaviour of your bot in this method. 
            // This method will be called every game tick with most recent game state.
            // Use AgentResponse to return your action for each game tick.

            //Bot that randomly choses one of all possible agent responses.
            var rand = new Random();
            return rand.Next(0, 18) switch
            {
                0 => AgentResponse.Pass(),
                1 => AgentResponse.Move(MovementDirection.Backward),
                2 => AgentResponse.Move(MovementDirection.Forward),
                3 => AgentResponse.Rotate(null, null),
                4 => AgentResponse.Rotate(null, Rotation.Left),
                5 => AgentResponse.Rotate(null, Rotation.Right),
                6 => AgentResponse.Rotate(Rotation.Left, null),
                7 => AgentResponse.Rotate(Rotation.Left, Rotation.Left),
                8 => AgentResponse.Rotate(Rotation.Left, Rotation.Right),
                9 => AgentResponse.Rotate(Rotation.Right, null),
                10 => AgentResponse.Rotate(Rotation.Right, null),
                11 => AgentResponse.Rotate(Rotation.Right, Rotation.Left),
                12 => AgentResponse.Rotate(Rotation.Right, Rotation.Right),
                13 => AgentResponse.UseAbility(AbilityType.DropMine),
                14 => AgentResponse.UseAbility(AbilityType.FireBullet),
                15 => AgentResponse.UseAbility(AbilityType.FireDoubleBullet),
                16 => AgentResponse.UseAbility(AbilityType.UseLaser),
                17 => AgentResponse.UseAbility(AbilityType.UseRadar),
                _ => throw new NotSupportedException(),
            };
        }

        public void OnGameEnd(GameEnd gameEnd)
        {
            // Define what your program should do when game is finished.
        }

        public void OnGameStarting()
        {
            // Define what your program should do when game is starting.
        }

        public void OnSubsequentLobbyData(LobbyData lobbyData)
        {
            // Define what should happen when lobby is changed.
            // For example when somebody new joins or disconnects
            // new LobbyData is sent and this method is run.
        }

        public void OnWarningReceived(Warning warning, string? message)
        {
            // Define what your program should do when game is warning is recieved.

            switch (warning)
            {
                case Warning.PlayerAlreadyMadeActionWarning:
                    {
                        Console.WriteLine("Player already made action warning");
                        break;
                    }
                case Warning.SlowResponseWarning:
                    {
                        Console.WriteLine("Slow response warning");
                        break;
                    }
                case Warning.ActionIgnoredDueToDeadWarning:
                    {
                        Console.WriteLine("Action ignored due to dead warning");
                        break;
                    }
                case Warning.CustomWarning:
                    {
                        Console.WriteLine($"Custom warning: {message ?? "no message"}");
                        break;
                    }
            }
        }
    }
}
