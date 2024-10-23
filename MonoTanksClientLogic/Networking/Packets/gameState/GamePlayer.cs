using MonoTanksClientLogic.JsonConverters;
using Newtonsoft.Json;

namespace MonoTanksClientLogic.Models;

[JsonConverter(typeof(GamePlayerJsonConverter))]
public abstract record class GamePlayer(
    string Id,
    string Nickname,
    long Color,
    long Ping);