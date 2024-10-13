namespace MonoTanksClientLogic.Networking;
public record class LobbyDataPayload(
    string? PlayerId,
    List<Player> Players,
    ServerSettings ServerSettings) : IPacketPayload
{
    /// <inheritdoc/>
    public PacketType Type => PacketType.LobbyData;
}
