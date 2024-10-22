using MonoTanksClientLogic.Networking;
using MonoTanksClientLogic.Networking.Payloads;
using Newtonsoft.Json.Linq;

namespace MonoTanksClientLogic;

public class AgentResponse() : Packet
{
    public static AgentResponse Pass()
    {
        PassPayload passPayload = new();
        return new AgentResponse()
        {
            Type = passPayload.Type,
            Payload = JObject.FromObject(passPayload),
        };
    }

    public static AgentResponse Move(MovementDirection tankMovement)
    {
        MovementPayload movementPayload = new(tankMovement);
        return new AgentResponse()
        {
            Type = movementPayload.Type,
            Payload = JObject.FromObject(movementPayload),
        };
    }

    public static AgentResponse Rotate(Rotation? tankRotation, Rotation? turretRotation)
    {
        RotationPayload rotationPayload = new()
        {
            TankRotation = tankRotation,
            TurretRotation = turretRotation,
        };
        return new AgentResponse()
        {
            Type = rotationPayload.Type,
            Payload = JObject.FromObject(rotationPayload),
        };
    }

    public static AgentResponse UseAbility(AbilityType abilityType)
    {
        UseAbilityPayload abilityUsePayload = new(abilityType);
        return new AgentResponse()
        {
            Type = abilityUsePayload.Type,
            Payload = JObject.FromObject(abilityUsePayload),
        };
    }
}
