using MonoTanksClientLogic.Enums;
using MonoTanksClientLogic.Networking;
using MonoTanksClientLogic.Networking.Payloads;
using Newtonsoft.Json.Linq;

namespace MonoTanksClientLogic;

/// <summary>
/// Represents agent response.
/// </summary>
public class AgentResponse() : Packet
{
    /// <summary>
    /// Represents pass response.
    /// </summary>
    /// <remarks>
    /// Use this method to return pass action for your agent.
    /// Pass means that bot will do nothing for one game tick.
    /// </remarks>
    /// <returns>
    /// AgentResponse representing pass action.
    /// </returns>
    public static AgentResponse Pass()
    {
        PassPayload passPayload = new();
        return new AgentResponse()
        {
            Type = passPayload.Type,
            Payload = JObject.FromObject(passPayload),
        };
    }

    /// <summary>
    /// Represents move response.
    /// </summary>
    /// <param name="tankMovement">
    /// Represents tank movement direction.
    /// </param>
    /// <remarks>
    /// Use this method to return move action for your agent.
    /// Move will change position of your tank one tile in specified direction.
    /// </remarks>
    /// <returns>
    /// AgentResponse representing move action.
    /// </returns>
    public static AgentResponse Move(MovementDirection tankMovement)
    {
        MovementPayload movementPayload = new(tankMovement);
        return new AgentResponse()
        {
            Type = movementPayload.Type,
            Payload = JObject.FromObject(movementPayload),
        };
    }

    /// <summary>
    /// Represents rotate response.
    /// </summary>
    /// <param name="tankRotation">
    /// Represents tank rotation direction.
    /// </param>
    /// <param name="turretRotation">
    /// Represents turret rotation direction.
    /// </param>
    /// <remarks>
    /// Use this method to return rotate action for your agent.
    /// Rotate will move both tank and turret in the same tick.
    /// Passing null as rotation will result in no rotation of tank or turret.
    /// </remarks>
    /// <returns>
    /// AgentResponse representing Rotate action.
    /// </returns>
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

    /// <summary>
    /// Represents use ability response.
    /// </summary>
    /// <param name="abilityType">
    /// Represents ability type.
    /// </param>
    /// <remarks>
    /// Use this method to return use ability action for your agent.
    /// Use ability will use one of available abilities.
    /// </remarks>
    /// <returns>
    /// AgentResponse representing UseAbility action.
    /// </returns>
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
