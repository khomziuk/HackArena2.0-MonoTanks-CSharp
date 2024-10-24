using MonoTanksBotLogic.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MonoTanksBotLogic.JsonConverters;

/// <summary>
/// Represents game player json converter.
/// </summary>
internal class GamePlayerJsonConverter : JsonConverter<GamePlayer>
{
    /// <inheritdoc/>
    public override GamePlayer? ReadJson(JsonReader reader, Type objectType, GamePlayer? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JObject jsonObject = JObject.Load(reader);

        if (jsonObject.ContainsKey("score") && jsonObject.ContainsKey("ticksToRegen") && jsonObject.ContainsKey("isUsingRadar"))
        {
            return new OwnPlayer(
                jsonObject["id"]!.ToObject<string>()!,
                jsonObject["nickname"]!.ToObject<string>()!,
                jsonObject["color"]!.ToObject<uint>()!,
                jsonObject["ping"]!.ToObject<int>()!,
                jsonObject["score"]!.ToObject<int>()!,
                jsonObject["ticksToRegen"]!.ToObject<int?>()!,
                jsonObject["isUsingRadar"]!.ToObject<bool>()!);
        }
        else
        {
            return new EnemyPlayer(
                jsonObject["id"]!.ToObject<string>()!,
                jsonObject["nickname"]!.ToObject<string>()!,
                jsonObject["color"]!.ToObject<uint>()!,
                jsonObject["ping"]!.ToObject<int>()!);
        }
    }

    /// <inheritdoc/>
    public override void WriteJson(JsonWriter writer, GamePlayer? value, JsonSerializer serializer)
    {
        throw new NotSupportedException();
    }
}
