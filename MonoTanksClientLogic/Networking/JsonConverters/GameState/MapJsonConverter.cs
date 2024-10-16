using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MonoTanksClientLogic.Networking.GameState;

/// <summary>
/// Represents a map JSON converter.
/// </summary>
/// <param name="context">The serialization context.</param>
internal class MapJsonConverter(GameSerializationContext context) : JsonConverter<Grid.Map>
{
    /// <inheritdoc/>
    public override Grid.Map? ReadJson(JsonReader reader, Type objectType, Grid.Map? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var jObject = JObject.Load(reader);

        var tiles = jObject["tiles"]!.ToObject<Grid.Tiles>(serializer)!;
        var zones = jObject["zones"]!.ToObject<List<Zone>>(serializer)!;

        var visibility = context is GameSerializationContext.Player
            ? jObject["visibility"]!.ToObject<Grid.Visibility>(serializer)!
            : null;

        return new Grid.Map(visibility, tiles, zones);
    }

    /// <inheritdoc/>
    public override void WriteJson(JsonWriter writer, Grid.Map? value, JsonSerializer serializer)
    {
        var jObject = new JObject()
        {
            ["tiles"] = JToken.FromObject(value!.Tiles, serializer),
            ["zones"] = JToken.FromObject(value!.Zones, serializer),
        };

        if (context is GameSerializationContext.Player)
        {
            jObject["visibility"] = JToken.FromObject(value!.Visibility!, serializer);
        }

        jObject.WriteTo(writer);
    }
}
