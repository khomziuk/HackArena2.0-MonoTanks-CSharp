using MonoTanksClientLogic;
using MonoTanksClientLogic.Enums;
using MonoTanksClientLogic.JsonConverters;
using MonoTanksClientLogic.Networking;
using Newtonsoft.Json;

namespace MonoTanksClientLogic.Models;

[JsonConverter(typeof(GameStateJsonConverter))]
public record class GameState(
    string Id,
    float Tick,
    GamePlayer[] Players,
    Tile[,] Map,
    Zone[] Zones);