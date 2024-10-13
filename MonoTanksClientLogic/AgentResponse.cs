using MonoTanksClientLogic.Networking;
using Newtonsoft.Json.Linq;

namespace MonoTanksClientLogic;

public class AgentResponse : Packet
{
    public static AgentResponse Pass()
    {
        ResponsePassPayload responsePassPayload = new();
        return new AgentResponse()
        {
            Type = responsePassPayload.Type,
            Payload = JObject.FromObject(responsePassPayload),
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
        AbilityUsePayload abilityUsePayload = new(abilityType);
        return new AgentResponse()
        {
            Type = abilityUsePayload.Type,
            Payload = JObject.FromObject(abilityUsePayload),
        };
    }
}
