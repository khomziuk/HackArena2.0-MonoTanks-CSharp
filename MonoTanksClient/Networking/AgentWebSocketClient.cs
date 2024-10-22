using System.Net.WebSockets;
using System.Text;
using MonoTanksClientLogic;
using MonoTanksClientLogic.Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MonoTanksClient.Networking;

/// <summary>
/// Represents custom web socket client that comunicates agent with game server.
/// </summary>
internal class AgentWebSocketClient : IDisposable
{
    private readonly IContractResolver ContractResolver = new CamelCasePropertyNamesContractResolver();
    private readonly uint incomingMessageBufferSize = 32 * 1024;
    private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1);

    private readonly ClientWebSocket clientWebSocket;
    private readonly Uri serverURI;
    private Task? currentProcess;

#if DEBUG
    private bool isFirstRecieved = false;
#endif

    //private Player? player;

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
                _ = this.HandleBuffer(buffer, result.Count);
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

    private async Task HandleBuffer(byte[] buffer, int bytesRecieved)
    {
        Packet packet;

        try
        {
            var settings = new JsonSerializerSettings()
            {
                ContractResolver = ContractResolver,
            };

            packet = JsonConvert.DeserializeObject<Packet>(Encoding.UTF8.GetString(buffer, 0, bytesRecieved), settings)!;

            if (packet.Type == PacketType.GameState)
            {
                if (this.isFirstRecieved == false)
                {
                    Console.WriteLine(Encoding.UTF8.GetString(buffer, 0, bytesRecieved));
                    this.isFirstRecieved = true;
                }

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
            case PacketType.LobbyData:
            case PacketType.GameStarting:
            case PacketType.GameState:
            case PacketType.GameEnded:
            case PacketType.PlayerAlreadyMadeActionWarning:
            case PacketType.SlowResponseWarning:
            case PacketType.ActionIgnoredDueToDeadWarning:
            case PacketType.CustomWarning:
                {
                    if (this.semaphore.Wait(new TimeSpan(0, 0, 1)))
                    {
                        try
                        {
                            await this.ProcessPacket(packet);
                        }
                        finally
                        {
                            _ = this.semaphore.Release();
                        }
                    }
                    else
                    {
                        Console.WriteLine("[System] Skipping packet due to previous packet not being processed in time.");
                        return;
                    }

                    break;
                }

            default:
                {
                    _ = this.ProcessPacket(packet);
                    break;
                }
        }
    }

    private async Task ProcessPacket(Packet packet)
    {
        switch (packet.Type)
        {
            case PacketType.Ping:
                {
                    Packet pongPacket = new Packet() { Type = PacketType.Pong, Payload = [] };
                    await this.SendMessageAsync(JsonConvert.SerializeObject(pongPacket));
                    break;
                }

            case PacketType.ConnectionAccepted:
                {
                    Console.WriteLine("[System] Connection accepted");
                    Packet lobbyDataRequest = new Packet() { Type = PacketType.LobbyDataRequest, Payload = [] };
                    await this.SendMessageAsync(JsonConvert.SerializeObject(lobbyDataRequest));
                    break;
                }

            case PacketType.ConnectionRejected:
                {
                    var connectionRejected = packet.GetPayload<ConnectionRejected>();
                    Console.WriteLine($"[System] Connection rejected -> {connectionRejected.Reason}");
                    break;
                }

            case PacketType.LobbyData:
                {
                    Console.WriteLine("[System] Lobby data received");
                    var lobbyData = packet.GetPayload<LobbyData>();

                    if (this.agent == null)
                    {
                        this.agent = new Agent.Agent(lobbyData);
                        //this.player = lobbyDataPayload.Players.Find((player) => player.Id == lobbyDataPayload.PlayerId);

                        if (lobbyData.ServerSettings.SandboxMode)
                        {
                            Console.WriteLine("[SYSTEM] Sandbox mode enabled");

                            Packet readyToReceivePacket = new Packet() { Type = PacketType.ReadyToReceiveGameState, Payload = [] };
                            await this.SendMessageAsync(JsonConvert.SerializeObject(readyToReceivePacket));
                            Packet gameStatusRequestPacket = new Packet() { Type = PacketType.GameStatusRequest, Payload = [] };
                            await this.SendMessageAsync(JsonConvert.SerializeObject(gameStatusRequestPacket));
                        }
                    }
                    else
                    {
                        this.agent.OnSubsequentLobbyData(lobbyData);
                    }

                    break;
                }


            case PacketType.GameStarting:
                {
                    Console.WriteLine("[System] Game starting");

                    if (this.agent == null)
                    {
                        while (this.agent == null)
                        {
                            try
                            {
                                Thread.Sleep(100);
                            }
                            catch (ThreadInterruptedException e)
                            {
                                Thread.CurrentThread.Interrupt();
                                Console.WriteLine("[ERROR] Interrupted while waiting for agent initialization.");
                                Console.WriteLine($"[^^^^^] {e.Message}");
                            }
                        }

                        try
                        {
                            Packet readyToReceivePacket = new Packet() { Type = PacketType.ReadyToReceiveGameState, Payload = [] };
                            await this.SendMessageAsync(JsonConvert.SerializeObject(readyToReceivePacket));
                            Console.WriteLine("[SYSTEM] Sent ReadyToReceiveGameState after agent initialization.");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("[ERROR] Error while sending ReadyToReceiveGameState");
                            Console.WriteLine($"[^^^^^] {e.Message}");
                        }
                    }
                    else
                    {
                        Packet readyToReceivePacket = new Packet() { Type = PacketType.ReadyToReceiveGameState, Payload = [] };
                        await this.SendMessageAsync(JsonConvert.SerializeObject(readyToReceivePacket));
                    }

                    break;
                }

            case PacketType.GameState:
                {
                    try
                    {
                        var gameState = packet.GetPayload<GameState>();

                        AgentResponse agentResponse = this.agent!.NextMove(gameState);
                        agentResponse.Payload["gameStateId"] = gameState.Id;
                        await this.SendMessageAsync(JsonConvert.SerializeObject(agentResponse));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("[ERROR] Error while processing game state: ");
                        Console.WriteLine($"[^^^^^] {e.Message}");
                        throw;
                    }

                    break;
                }

            case PacketType.GameEnded:
                {
                    Console.WriteLine("[SYSTEM] Game ended!");
                    var gameEnd = packet.GetPayload<GameEnd>();
                    this.agent!.OnGameEnd(gameEnd);
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
                    var customWarning = packet.GetPayload<CustomWarning>();
                    this.agent!.OnWarningReceived(Warning.CustomWarning, customWarning.Message);
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
    }
}