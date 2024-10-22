using MonoTanksClientLogic.Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MonoTanksClientLogic.JsonConverters;

internal class GameStateJsonConverter : JsonConverter<GameState>
{
    public override GameState? ReadJson(JsonReader reader, Type objectType, GameState? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var jsonObject = JObject.Load(reader);

        var id = jsonObject["id"]!.ToObject<string>()!;
        var tick = jsonObject["tick"]!.ToObject<float>();

        List<GamePlayer> players = new();
        foreach (var player in (JArray)jsonObject["players"]!)
        {
            players.Add(player.ToObject<GamePlayer>()!);
        }

        var rawMap = (JArray)jsonObject["map"]!["tiles"]!;
        int rows = rawMap.Count;
        int cols = rawMap[0].Count();
        var map = new Tile[rows, cols];
        for (int i = 0; i < rows; i++)
        {
            var rowArray = (JArray)rawMap[i];

            for (int j = 0; j < rowArray.Count; j++)
            {
                map[j, i] = rowArray[j].ToObject<Tile>()!;
            }
        }

        List<Zone> zones = new();
        foreach (var zone in (JArray)jsonObject["map"]!["zones"]!)
        {
            zones.Add(zone.ToObject<Zone>()!);
        }

        return new GameState(id, tick, players.ToArray(), map, zones.ToArray());
    }

    public override void WriteJson(JsonWriter writer, GameState? value, JsonSerializer serializer)
    {
        throw new NotSupportedException();
    }
}
