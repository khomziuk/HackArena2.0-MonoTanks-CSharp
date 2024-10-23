using MonoTanksClientLogic.Enums;

namespace MonoTanksClientLogic.Networking.Payloads;

/// <summary>
/// Represents an ability use payload.
/// </summary>
/// <param name="AbilityType">The ability type.</param>
public class UseAbilityPayload(AbilityType AbilityType)
{
    public PacketType Type => PacketType.AbilityUse;

    public string? GameStateId { get; init; }
}
