using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using MonoTanksClientLogic;
using MonoTanksClientLogic.Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

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

    private IAgent? agent;

    /// <summary>
    /// Initializes a new instance of the <see cref="AgentWebSocketClient"/> class.
    /// </summary>
    /// <param name="host">Represents host of a WebSocket server.</param>
    /// <param name="port">Represents port of a WebSocket server.</param>
    /// <param name="nickname">Represents nickname of agent.</param>
    /// <param name="code">Represents join code.</param>
    public AgentWebSocketClient(string host = "localhost", string port = "5000", string nickname = "empty", string code = "")
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
        await this.clientWebSocket.ConnectAsync(this.serverURI, CancellationToken.None);
        Console.WriteLine("[System] Successfully connected to the server");

        // TODO: Run this in a new thread.
        await this.ListenForMessagesAsync();
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
        Console.WriteLine($"Sent: {message}");
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
            var settings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
            };

            packet = JsonConvert.DeserializeObject<Packet>(Encoding.UTF8.GetString(buffer, 0, bytesRecieved), settings)!;
        }
        catch (Exception ex)
        {
            Console.WriteLine("[ERROR] Packet deserialization failed: ");

            Console.WriteLine("[^^^^^] Message: {0}", ex.Message);
            return;
        }

        if (this.currentProcess == null || this.currentProcess.IsCompleted)
        {
            this.currentProcess = Task.Run(() => this.ProcessPacket(packet));
        }
        else
        {
            Console.WriteLine("[System] Skipping packet due to previous packet not being processed yet");
            return;
        }
    }

    private async Task ProcessPacket(Packet packet)
    {
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
                    var lobbyDataPayload = packet.GetPayload<LobbyDataPayload>();

                    if (this.agent == null)
                    {
                        this.agent = new Agent.Agent(lobbyDataPayload);
                    }
                    else
                    {
                        this.agent.OnSubsequentLobbyData(lobbyDataPayload);
                    }

                    break;
                }

            case PacketType.LobbyDeleted:
                {
                    Console.WriteLine("[System] Lobby deleted");
                    break;
                }

            case PacketType.GameStart:
                {
                    Console.WriteLine("[System] Game started");
                    break;
                }

            case PacketType.GameState:
                {
                    try
                    {
                        var gameStatePayload = packet.GetPayload<GameStatePayload>();
                        AgentResponse agentResponse = this.agent!.NextMove(gameStatePayload);
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
                    GameEndPayload gameEndPayload = packet.GetPayload<GameEndPayload>();
                    this.agent!.onGameEnd(gameEndPayload);
                    break;
                }

            case PacketType.PlayerAlreadyMadeActionWarning:
                {
                    Console.WriteLine("[System] Player already made action warning");
                    break;
                }

            case PacketType.SlowResponseWarning:
                {
                    Console.WriteLine("[System] Slow response warning");
                    break;
                }

            case PacketType.ActionIgnoredDueToDeadWarning:
                {
                    Console.WriteLine("[System] Action ignored due to dead warning");
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
                    Console.WriteLine("[WARN] Warning: ");
                    Console.WriteLine($"[^^^^] {customWarningPayload.Message}");
                    break;
                }

            // Should never happen
            case PacketType.Pong: break;
            case PacketType.Movement: break;
            case PacketType.Rotation: break;
            case PacketType.AbilityUse: break;
            case PacketType.Pass: break;
            default: throw new NotSupportedException();
        }

        if (response != null)
        {
            _ = this.SendMessageAsync(response);
        }
    }
}