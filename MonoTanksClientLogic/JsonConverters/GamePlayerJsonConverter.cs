﻿using MonoTanksClientLogic.Networking;
using MonoTanksClientLogic.Networking.Packets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MonoTanksClientLogic.JsonConverters;

internal class GamePlayerJsonConverter : JsonConverter<GamePlayer>
{
    public override GamePlayer? ReadJson(JsonReader reader, Type objectType, GamePlayer? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JObject jsonObject = JObject.Load(reader);

        if (jsonObject.ContainsKey("score") && jsonObject.ContainsKey("ticksToRegen") && jsonObject.ContainsKey("isUsingRadar"))
        {
            return new OwnPlayer(
                jsonObject["id"]!.ToObject<string>()!,
                jsonObject["nickname"]!.ToObject<string>()!,
                jsonObject["color"]!.ToObject<long>()!,
                jsonObject["ping"]!.ToObject<long>()!,
                jsonObject["score"]!.ToObject<long>()!,
                jsonObject["ticksToRegen"]!.ToObject<long?>()!,
                jsonObject["isUsingRadar"]!.ToObject<bool>()!
            );
        }
        else
        {
            return new EnemyPlayer(
                jsonObject["id"]!.ToObject<string>()!,
                jsonObject["nickname"]!.ToObject<string>()!,
                jsonObject["color"]!.ToObject<long>()!,
                jsonObject["ping"]!.ToObject<long>()!
            );
        }
    }

    public override void WriteJson(JsonWriter writer, GamePlayer? value, JsonSerializer serializer)
    {
        throw new NotSupportedException();
    }
}