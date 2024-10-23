# C# WebSocket Client for Hackathon 2024

This C#-based WebSocket client was developed for the Hackathon 2024, organized
by WULS-SGGW. It serves as a framework for participants to create AI agents that
can play the game.

To fully test and run the game, you will also need the game server and GUI
client, as the GUI provides a visual representation of gameplay. You can find
more information about the server and GUI client in the following repository:

- [Server and GUI Client Repository](https://github.com/INIT-SGGW/HackArena2024H2-Game)

## Development

The agent logic you are going to implement is located in
`Agent/Agent.cs`:

```C#
using MonoTanksClientLogic;
using MonoTanksClientLogic.Enums;
using MonoTanksClientLogic.Models;

namespace Agent;

public class Agent : IAgent
{
    public Agent(LobbyData lobbyData)
    {
        // Initialize your agent. 
    }

    public void OnSubsequentLobbyData(LobbyData lobbyData)
    {
        // Define what should happen when lobby is changed.
        // For example when somebody new joins or disconnects
        // new LobbyData is sent and this method is run.
    }

    public AgentResponse NextMove(GameState gameState)
    {
        // Define behaviour of your bot in this method. 
        // This method will be called every game tick with most recent game state.
        // Use AgentResponse to return your action for each game tick.

        return AgentResponse.Pass();
    }

    public void OnGameEnd(GameEnd gameEnd)
    {
        // Define what your program should do when game is finished.
    }

    public void OnGameStarting()
    {
        // Define what your program should do when game is starting.
    }

    public void OnWarningReceived(Warning warning, string? message)
    {
        // Define what your program should do when game is warning is recieved.
    }
}
```


The `Agent` class implements the `IAgent` interface and defines the behavior of the agent.
 - The constructor `Agent(LobbyData lobbyData)` is called when the agent is created to initialize its state based on the initial lobby data.
 - The `OnSubsequentLobbyData(LobbyData lobbyData)` method is triggered whenever there is an update to the lobby data, such as when players join or leave the game.
 - The `NextMove(GameState gameState)` method is called every game tick to determine the agent's next action. This is where the agent’s behavior logic is implemented. It returns an AgentResponse that defines what action the agent will take during the game tick.
 - The `OnGameEnd(GameEnd gameEnd)` method is called when the game finishes, allowing the agent to handle any final actions or cleanup.
 - The `OnGameStarting()` method is called when the game is about to start, letting the agent perform any preparations.
 - The `OnWarningReceived(Warning warning, string? message)` method is invoked when the game issues a warning, allowing the agent to handle warnings appropriately.

The `NextMove` method returns an `AgentResponse` object, which can be one of the following:
 - `MoveResponse`: Move the tank forward or backward. The `MoveDirection` enum defines the options: `FORWARD` or `BACKWARD`.
 - `RotationResponse`: Rotate the tank or turret. The `RotationDirection` enum specifies the direction: `LEFT` or `RIGHT`.
 - `AbilityUseResponse`: Use an ability such as firing a bullet or using a radar. The `AbilityType` enum includes various options, such as `FIRE_BULLET`, `FIRE_DOUBLE_BULLET`, `USE_LASER`, `USE_RADAR`, and `DROP_MINE`.
 - `PassResponse`: Do nothing for the current game tick.

You can create these responses using static factory methods in the `AgentResponse` class:
 - `AgentResponse.Move()`
 - `AgentResponse.Rotate()`
 - `AgentResponse.UseAbility()`
 - `AgentResponse.Pass()`

You can modify the mentioned file and create more files in the
`Agent` directory. Do not
modify any other files, as this may prevent us from running your agent during
the competition.<br><br>
**Agent example** is available in `Agent/Agent.cs` file.
### Including Static Files

If you need to include static files that your program should access during
testing or execution, place them in the `data` folder. This folder is copied
into the Docker image and will be accessible to your application at runtime.
For example, you could include configuration files, pre-trained models, or any
other data your agent might need.

## Running the Client

You can run this client in three different ways: locally, within a VS Code
development container, or manually using Docker.

### 1. Running locally (on Windows using Visual Studio).
Simply open project in Visual Studio and use IDE interface to run and debug your agent!

### 2. Running locally (on Linux or in other IDEs)

To run the client locally, you must have dotnet SDK 8.0 or later installed and dotnet runtime 8.0 or later. <br><br>
Verify your dotnet SDK version by running:
```sh
dotnet --list-sdks
```
Verify your dotnet SDK runtime by running:
```sh
dotnet --list-runtimes
```

To build your solution use in `Debug` configuration use:
```sh
dotnet build HackArena2024H2-CSharp.sln
```
**Remember** we will test your agent in optimized `Release` configuration so make sure everything is correct. You can build `Release` version using command below:
```sh
dotnet build HackArena2024H2-CSharp.sln -c Release
```

Assuming the game server is running on `localhost:5000` (refer to the server
repository's README for setup instructions), start the client by running in build directory `MonoTanksClient/bin/Debug/net8.0`:

```sh
./MonoTanksClient --nickname TEAM_NAME
```

The `--nickname` argument is required and must be unique. For additional
configuration options, run:

```sh
./MonoTanksClient --help
```

### 3. Running in a Docker Container (Manual Setup)

To run the client manually in a Docker container, ensure Docker is installed on
your system.

Steps:

1. Build the Docker image:
   ```sh
   docker build -t client .
   ```
2. Run the Docker container:
   ```sh
   docker run --rm client --host host.docker.internal --nickname TEAM_NAME
   ```

If the server is running on your local machine, use the
`--host host.docker.internal` flag to connect the Docker container to your local
host.
