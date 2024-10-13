using Newtonsoft.Json;

namespace MonoTanksClientLogic.Networking;

/// <summary>
/// Represents a packet payload.
/// </summary>
public interface IPacketPayload
{
    /// <summary>
    /// Gets the packet type.
    /// </summary>
    [JsonIgnore]
    PacketType Type { get; }
}
