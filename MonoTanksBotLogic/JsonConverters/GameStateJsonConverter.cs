using MonoTanksClientLogic.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MonoTanksClientLogic.JsonConverters;

/// <summary>
/// Represents game state json converter.
/// </summary>
internal class GameStateJsonConverter : JsonConverter<GameState>
{
    /// <inheritdoc/>
    public override GameState? ReadJson(JsonReader reader, Type objectType, GameState? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var jsonObject = JObject.Load(reader);

        var id = jsonObject["id"]!.ToObject<string>()!;
        var tick = jsonObject["tick"]!.ToObject<float>();
        var rawMap = jsonObject["map"]!;

        List<GamePlayer> players = new();
        foreach (var player in (JArray)jsonObject["players"]!)
        {
            players.Add(player.ToObject<GamePlayer>()!);
        }

        List<Zone> zones = new();
        foreach (var zone in (JArray)jsonObject["map"]!["zones"]!)
        {
            zones.Add(zone.ToObject<Zone>()!);
        }

        var rawTiles = (JArray)rawMap["tiles"]!;
        int columns = rawTiles.Count;
        int rows = rawTiles[0].Count();
        var map = new Tile[rows, rows];
        for (int x = 0; x < columns; x++)
        {
            var columnArray = (JArray)rawTiles[x];

            for (int y = 0; y < columnArray.Count; y++)
            {
                var isVisible = this.IsVisible((JArray)rawMap["visibility"]!, x, y);
                var zoneIndex = this.ComputeZoneIndex(zones, x, y);
                var tile = columnArray[y].ToObject<Tile>()!;
                map[y, x] = new(isVisible, zoneIndex, tile.Entities);
            }
        }

        return new GameState(id, tick, players.ToArray(), map, zones.ToArray());
    }

    /// <inheritdoc/>
    public override void WriteJson(JsonWriter writer, GameState? value, JsonSerializer serializer)
    {
        throw new NotSupportedException();
    }

    private bool IsVisible(JArray visibility, int x, int y)
    {
        string row = visibility[y]!.ToObject<string>()!;
        return row[x] == '1';
    }

    private int? ComputeZoneIndex(List<Zone> zones, int x, int y)
    {
        foreach (var zone in zones)
        {
            if ((zone.X <= x) && (x < zone.X + zone.Width))
            {
                if ((zone.Y <= y) && (y < zone.Y + zone.Height))
                {
                    return zone.Index;
                }
            }
        }

        return null;
    }
}
