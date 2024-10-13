namespace MonoTanksClientLogic.Networking;
public record class GameEndPayload(List<Player> Players) : IPacketPayload
{
    /// <inheritdoc/>
    public PacketType Type => PacketType.GameEnd;
}
