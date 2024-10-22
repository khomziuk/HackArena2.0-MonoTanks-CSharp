namespace MonoTanksClientLogic.Networking.Payloads;

public class PassPayload
{
    public PacketType Type => PacketType.Pass;

    public string? GameStateId { get; init; }
}
