using MonoTanksClientLogic.JsonConverters;
using Newtonsoft.Json;

namespace MonoTanksClientLogic.Networking;

[JsonConverter(typeof(GamePlayerJsonConverter))]
public abstract record class GamePlayer(
    string Id,
    string Nickname,
    long Color,
    long Ping);