using MonoTanksClientLogic.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static MonoTanksClientLogic.Models.ZoneStatus;

namespace MonoTanksClientLogic.JsonConverters;

public class ZoneJsonConverter : JsonConverter<Zone>
{
    public override Zone? ReadJson(JsonReader reader, Type objectType, Zone? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JObject jsonObject = JObject.Load(reader);

        var x = jsonObject["x"]!.ToObject<long>()!;
        var y = jsonObject["y"]!.ToObject<long>()!;
        var width = jsonObject["width"]!.ToObject<long>()!;
        var height = jsonObject["height"]!.ToObject<long>()!;
        var index = jsonObject["index"]!.ToObject<int>()!;
        var status = jsonObject["status"]!["type"]!.ToObject<string>()!;
        ZoneStatus zoneStatus = status switch
        {
            "neutral" => new Neutral()!,
            "beingCaptured" => jsonObject["status"]!.ToObject<BeingCaptured>()!,
            "captured" => jsonObject["status"]!.ToObject<Captured>()!,
            "beingContested" => jsonObject["status"]!.ToObject<BeingContested>()!,
            "beingRetaken" => jsonObject["status"]!.ToObject<BeingRetaken>()!,
            _ => throw new NotSupportedException()
        };

        return new Zone(index, x, y, width, height, zoneStatus);
    }

    public override void WriteJson(JsonWriter writer, Zone? value, JsonSerializer serializer)
    {
        throw new NotSupportedException();
    }
}
