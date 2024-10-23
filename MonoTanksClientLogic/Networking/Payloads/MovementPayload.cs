namespace MonoTanksClientLogic.Networking.Payloads
{
    using MonoTanksClientLogic.Enums;

    public class MovementPayload(MovementDirection direction)
    {
        public PacketType Type => PacketType.Movement;

        public string? GameStateId { get; init; }

        public MovementDirection Direction { get; } = direction;
    }
}
