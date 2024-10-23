using MonoTanksClientLogic.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static MonoTanksClientLogic.Models.Tile;

namespace MonoTanksClientLogic.JsonConverters;

/// <summary>
/// Represents tile json converter.
/// </summary>
internal class TileJsonConverter : JsonConverter<Tile>
{
    /// <inheritdoc/>
    public override Tile? ReadJson(JsonReader reader, Type objectType, Tile? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var jsonArray = JArray.Load(reader);

        List<TileEntity> entities = new();
        foreach (var tileEntity in jsonArray)
        {
            switch (tileEntity["type"]!.ToObject<string>()!)
            {
                case "wall":
                    {
                        entities.Add(new Wall());
                        break;
                    }

                case "tank":
                    {
                        var rawPayload = (JObject)tileEntity["payload"]!;
                        if (rawPayload.ContainsKey("health"))
                        {
                            entities.Add(rawPayload.ToObject<OwnTank>()!);
                        }
                        else
                        {
                            entities.Add(rawPayload.ToObject<EnemyTank>()!);
                        }

                        break;
                    }

                case "bullet":
                    {
                        var rawPayload = (JObject)tileEntity["payload"]!;
                        entities.Add(rawPayload.ToObject<Bullet>()!);
                        break;
                    }

                case "laser":
                    {
                        var rawPayload = (JObject)tileEntity["payload"]!;
                        entities.Add(rawPayload.ToObject<Laser>()!);
                        break;
                    }

                case "mine":
                    {
                        var rawPayload = (JObject)tileEntity["payload"]!;
                        entities.Add(rawPayload.ToObject<Mine>()!);
                        break;
                    }

                case "item":
                    {
                        var rawPayload = (JObject)tileEntity["payload"]!;
                        entities.Add(rawPayload.ToObject<Item>()!);
                        break;
                    }
            }
        }

        return new Tile(entities.ToArray());
    }

    /// <inheritdoc/>
    public override void WriteJson(JsonWriter writer, Tile? value, JsonSerializer serializer)
    {
        throw new NotSupportedException();
    }
}
