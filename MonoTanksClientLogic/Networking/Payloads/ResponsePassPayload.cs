namespace MonoTanksClientLogic.Networking;

/// <summary>
/// Represents a response pass payload.
/// </summary>
internal class ResponsePassPayload : IPacketPayload, IActionPayload
{
    /// <inheritdoc/>
    public PacketType Type => PacketType.Pass;

    /// <inheritdoc/>
    public string? GameStateId { get; init; }
}
