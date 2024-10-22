namespace MonoTanksClientLogic.Networking.Payloads
{
    public class MovementPayload(MovementDirection direction)
    {
        public PacketType Type => PacketType.Movement;

        public string? GameStateId { get; init; }

        public MovementDirection Direction { get; } = direction;
    }
}
