using MonoTanksClientLogic.JsonConverters;
using Newtonsoft.Json;

namespace MonoTanksClientLogic.Networking;

[JsonConverter(typeof(GameStateJsonConverter))]
public record class GameState(
    string Id,
    float Tick,
    GamePlayer[] Players,
    Tile[,] Map,
    Zone[] Zones);