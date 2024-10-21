using MonoTanksClientLogic.Networking.GameEnd;
using Newtonsoft.Json;

namespace MonoTanksClientLogic.Networking;
public record class GameEndPayload(List<Player> Players) : IPacketPayload
{
    /// <inheritdoc/>
    public PacketType Type => PacketType.GameEnded;

    /// <summary>
    /// Gets the converters to use during
    /// serialization and deserialization.
    /// </summary>
    /// <returns>
    /// The list of converters to use during
    /// serialization and deserialization.
    /// </returns>
    public static List<JsonConverter> GetConverters()
    {
        return [new PlayerJsonConverter()];
    }
}
