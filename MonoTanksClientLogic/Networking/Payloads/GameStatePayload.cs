using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace MonoTanksClientLogic.Networking;

/// <summary>
/// Represents a grid state payload.
/// </summary>
public class GameStatePayload : IPacketPayload
{
    [JsonConstructor]
    private GameStatePayload(int tick, List<Player> players, Grid.MapPayload map)
    {
        this.Tick = tick;
        this.Players = players;
        this.Map = map;
    }

    /// <inheritdoc/>
    public PacketType Type => PacketType.GameState;

    /// <summary>
    /// Gets the number of ticks that
    /// have passed in the game.
    /// </summary>
    public int Tick { get; }

    /// <summary>
    /// Gets the players.
    /// </summary>
    public List<Player> Players { get; }

    /// <summary>
    /// Gets the map state.
    /// </summary>
    [JsonProperty]
    internal Grid.MapPayload Map { get; }

    /// <summary>
    /// Represents a grid state payload for a specific player.
    /// </summary>
    public class ForPlayer : GameStatePayload
    {
        [JsonConstructor]
        [SuppressMessage("CodeQuality", "IDE0051", Justification = "Used by Newtonsoft.Json.")]
        private ForPlayer(string id, int tick, List<Player> players, Grid.MapPayload map)
            : base(tick, players, map)
        {
            this.Id = id;
        }

        /// <summary>
        /// Gets the packet id.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Gets the visibility grid of the player.
        /// </summary>
        [JsonIgnore]
        public bool[,] VisibilityGrid => this.Map.Visibility!.VisibilityGrid;
    }
}
