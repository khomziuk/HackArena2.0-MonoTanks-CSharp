using System.Net.WebSockets;
using System.Text;
using GameLogic.Networking;
using MonoTanksClientLogic;
using MonoTanksClientLogic.Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MonoTanksClient.Networking;

/// <summary>
/// Represents custom web socket client that comunicates agent with game server.
/// </summary>
internal class AgentWebSocketClient : IDisposable
{
    private readonly uint incomingMessageBufferSize = 32 * 1024;

    private readonly ClientWebSocket clientWebSocket;
    private readonly Uri serverURI;
    private Task? currentProcess;

    private Player? player;

    private IAgent? agent;

    /// <summary>
    /// Initializes a new instance of the <see cref="AgentWebSocketClient"/> class.
    /// </summary>
    /// <param name="host">Represents host of a WebSocket server.</param>
    /// <param name="port">Represents port of a WebSocket server.</param>
    /// <param name="nickname">Represents nickname of agent.</param>
    /// <param name="code">Represents join code.</param>
    public AgentWebSocketClient(string host = "localhost", string port = "5000", string nickname = "", string code = "")
    {
        this.clientWebSocket = new ClientWebSocket();

        StringBuilder link = new($"ws://{host}:{port}/?nickname={nickname}&playerType=hackathonBot");

        if (code.Length != 0)
        {
            _ = link.Append($"&joinCode={code}");
        }

        this.serverURI = new(link.ToString());
    }

    /// <summary>
    ///  Connects to a WebSocket server as an asynchronous operation.
    /// </summary>
    /// <param name="uri">Represents uri of a WebSocket server.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public async Task ConnectAsync()
    {
        try
        {
            await this.clientWebSocket.ConnectAsync(this.serverURI, CancellationToken.None);
            Console.WriteLine("[System] Successfully connected to the server");

            // TODO: Run this in a new thread.
            await this.ListenForMessagesAsync();
        }
        catch (Exception)
        {
            Console.WriteLine("[System] Connection Failed.");
        }
    }

    /// <summary>
    ///  Close the <see cref="AgentWebSocketClient" /> instance as an asynchronous operation.
    /// </summary>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public async Task CloseAsync()
    {
        await this.clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client closing", CancellationToken.None);
        Console.WriteLine("[System] WebSocket connection closed.");
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        this.clientWebSocket.Dispose();
    }

    /// <summary>
    /// Send message using clientWebSocket.
    /// </summary>
    /// <param name="message">Message to send. </param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    private async Task SendMessageAsync(string message)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(message);
        await this.clientWebSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
    }

    private async Task ListenForMessagesAsync()
    {
        while (this.clientWebSocket.State == WebSocketState.Open)
        {
            byte[] buffer = new byte[this.incomingMessageBufferSize];
            WebSocketReceiveResult result;
            try
            {
                result = await this.clientWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            catch (WebSocketException ex)
            {
                Console.WriteLine("[ERROR] Receiving a message failed: ");
                Console.WriteLine("[^^^^^] {0}", ex.Message);
                Console.WriteLine("[^^^^^] Closing the connection with InternalServerError.");

                await this.clientWebSocket.CloseAsync(
                    WebSocketCloseStatus.InternalServerError,
                    "Internal server error",
                    CancellationToken.None);

                break;
            }

            if (!result.EndOfMessage)
            {
                Console.WriteLine("[WARN] Received message is too big");
                Console.WriteLine("[^^^^] Closing the connection with MessageTooBig.");

                await this.clientWebSocket.CloseAsync(
                    WebSocketCloseStatus.MessageTooBig,
                    "Message too big",
                    CancellationToken.None);

                break;
            }

            if (result.MessageType == WebSocketMessageType.Text)
            {
                this.HandleBuffer(buffer, result.Count);
            }
            else if (result.MessageType == WebSocketMessageType.Close)
            {
                await this.clientWebSocket.CloseAsync(
                    WebSocketCloseStatus.NormalClosure,
                    "Closing",
                    CancellationToken.None);

                Console.WriteLine("[System] WebSocket connection closed.");
            }
        }
    }

    private void HandleBuffer(byte[] buffer, int bytesRecieved)
    {
        Packet packet;

        try
        {
            packet = PacketSerializer.Deserialize(Encoding.UTF8.GetString(buffer, 0, bytesRecieved));

            if (packet.Type != PacketType.Ping)
            {
                Console.WriteLine(Encoding.UTF8.GetString(buffer, 0, bytesRecieved));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("[ERROR] Packet deserialization failed: ");
            Console.WriteLine("[^^^^^] Message: {0}", ex.Message);

            throw;
        }

        switch (packet.Type)
        {
            case PacketType.GameStarting:
            case PacketType.GameState:
            case PacketType.PlayerAlreadyMadeActionWarning:
            case PacketType.SlowResponseWarning:
            case PacketType.ActionIgnoredDueToDeadWarning:
            case PacketType.CustomWarning:
                {
                    if (this.currentProcess == null || this.currentProcess.IsCompleted)
                    {
                        this.currentProcess = Task.Run(() => this.ProcessPacket(packet));
                    }
                    else
                    {
                        Console.WriteLine("[System] Skipping packet due to previous packet not being processed yet");
                        return;
                    }

                    break;
                }

            default:
                {
                    this.currentProcess = Task.Run(() => this.ProcessPacket(packet));
                    break;
                }
        }
    }

    private async Task ProcessPacket(Packet packet)
    {
        SerializationContext serializationContext = new(EnumSerializationFormat.Int);

        string? response = null;
        switch (packet.Type)
        {
            case PacketType.Ping:
                {
                    Packet pongPacket = new Packet() { Type = PacketType.Pong, Payload = new JObject() };
                    response = JsonConvert.SerializeObject(pongPacket);
                    break;
                }

            case PacketType.ConnectionAccepted:
                {
                    Console.WriteLine("[System] Connection accepted");
                    break;
                }

            case PacketType.ConnectionRejected:
                {
                    var connectionRejectedPayload = packet.GetPayload<ConnectionRejectedPayload>();
                    Console.WriteLine($"[System] Connection rejected -> {connectionRejectedPayload.Reason}");
                    break;
                }

            case PacketType.LobbyData:
                {
                    Console.WriteLine("[System] Lobby data received");
                    var serializer = PacketSerializer.GetSerializer([new MonoTanksClientLogic.Networking.LobbyData.PlayerJsonConverter()]);
                    var lobbyDataPayload = packet.GetPayload<LobbyDataPayload>(serializer);

                    if (this.agent == null)
                    {
                        this.agent = new Agent.Agent(new LobbyData(lobbyDataPayload));
                        this.player = lobbyDataPayload.Players.Find((player) => player.Id == lobbyDataPayload.PlayerId);
                    }
                    else
                    {
                        this.agent.OnSubsequentLobbyData(new LobbyData(lobbyDataPayload));
                    }

                    break;
                }

            case PacketType.GameStarting:
                {
                    Console.WriteLine("[System] Game starting");
                    Packet readyToReceiveGameStatePayload = new() { Type = PacketType.ReadyToReceiveGameState, Payload = new JObject() };
                    this.agent!.OnGameStarting();
                    response = JsonConvert.SerializeObject(readyToReceiveGameStatePayload);
                    break;
                }

            case PacketType.GameState:
                {
                    try
                    {
                        GameSerializationContext.Player gameSerializationContext = new(this.player!, EnumSerializationFormat.Int);
                        var serializer = PacketSerializer.GetSerializer(GameStatePayload.GetConverters(gameSerializationContext));
                        var gameStatePayload = packet.GetPayload<GameStatePayload.ForPlayer>(serializer);
                        AgentResponse agentResponse = this.agent!.NextMove(new GameState(gameStatePayload));

                        // there is already field "GameStateId" in JObject but
                        // when i do agentResponse.Payload["GameStateId"] it
                        // trggers no game state id warnings.
                        // Due to that there are two game state id fields
                        // in the serialized responce.
                        agentResponse.Payload["gameStateId"] = gameStatePayload.Id;
                        response = JsonConvert.SerializeObject(agentResponse);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("[ERROR] Error while processing game state: ");
                        Console.WriteLine($"[^^^^^] {e.Message}");
                        throw;
                    }

                    break;
                }

            case PacketType.GameEnd:
                {
                    Console.WriteLine("[SYSTEM] Game ended!");
                    var serializer = PacketSerializer.GetSerializer([new MonoTanksClientLogic.Networking.GameEnd.PlayerJsonConverter()]);
                    GameEndPayload gameEndPayload = packet.GetPayload<GameEndPayload>(serializer);
                    this.agent!.OnGameEnd(new GameEnd(gameEndPayload));
                    break;
                }

            case PacketType.PlayerAlreadyMadeActionWarning:
                {
                    this.agent!.OnWarningReceived(Warning.PlayerAlreadyMadeActionWarning, null);
                    break;
                }

            case PacketType.SlowResponseWarning:
                {
                    this.agent!.OnWarningReceived(Warning.SlowResponseWarning, null);
                    break;
                }

            case PacketType.ActionIgnoredDueToDeadWarning:
                {
                    this.agent!.OnWarningReceived(Warning.ActionIgnoredDueToDeadWarning, null);
                    break;
                }

            case PacketType.InvalidPacketTypeError:
                {
                    Console.WriteLine("[ERROR] Invalid packet type error");
                    break;
                }

            case PacketType.InvalidPacketUsageError:
                {
                    Console.WriteLine("[ERROR] Invalid packet usage error");
                    break;
                }

            case PacketType.CustomWarning:
                {
                    var customWarningPayload = packet.GetPayload<CustomWarningPayload>();
                    this.agent!.OnWarningReceived(Warning.CustomWarning, customWarningPayload.Message);
                    break;
                }

            // Should never happen
            case PacketType.Pong: break;
            case PacketType.Movement: break;
            case PacketType.Rotation: break;
            case PacketType.AbilityUse: break;
            case PacketType.Pass: break;
            case PacketType.ReadyToReceiveGameState: break;
            default: throw new NotSupportedException();
        }

        if (response != null)
        {
            _ = this.SendMessageAsync(response);
        }
    }
}