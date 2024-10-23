using MonoTanksClientLogic.Enums;

namespace MonoTanksClientLogic.Networking.Payloads;

public class RotationPayload
{
    public PacketType Type => PacketType.Rotation;

    public string? GameStateId { get; init; }

    /// <summary>
    /// Gets the tank rotation.
    /// </summary>
    /// <remarks>
    /// If the value is <see langword="null"/>,
    /// the tank rotation is not changed.
    /// </remarks>
    public Rotation? TankRotation { get; init; }

    /// <summary>
    /// Gets the turret rotation.
    /// </summary>
    /// <remarks>
    /// If the value is <see langword="null"/>,
    /// the turret rotation is not changed.
    /// </remarks>
    public Rotation? TurretRotation { get; init; }
}
