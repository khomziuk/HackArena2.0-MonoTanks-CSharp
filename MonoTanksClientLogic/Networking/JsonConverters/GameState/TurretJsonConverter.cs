using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MonoTanksClientLogic.Networking.GameState;

/// <summary>
/// Represents a turret json converter.
/// </summary>
/// <param name="context">The serializaton context.</param>
internal class TurretJsonConverter(GameSerializationContext context) : JsonConverter<Turret>
{
    /// <inheritdoc/>
    public override Turret? ReadJson(JsonReader reader, Type objectType, Turret? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var jsonObject = JObject.Load(reader);
        var direction = JsonConverterUtils.ReadEnum<Direction>(jsonObject["direction"]!);
        var bulletCount = jsonObject["bulletCount"]?.Value<int>();
        var remainingTicksToRegenBullet = jsonObject["ticksToRegenBullet"]?.Value<int?>();

        if (bulletCount is null)
        {
            // Player perspective for other players
            return new Turret(direction);
        }

        return new Turret(direction, bulletCount.Value, remainingTicksToRegenBullet);
    }

    /// <inheritdoc/>
    public override void WriteJson(JsonWriter writer, Turret? value, JsonSerializer serializer)
    {
        var jObject = new JObject
        {
            ["direction"] = JsonConverterUtils.WriteEnum(value!.Direction, context.EnumSerialization),
        };

        jObject.WriteTo(writer);
    }
}
