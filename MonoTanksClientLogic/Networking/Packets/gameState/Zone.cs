using MonoTanksClientLogic.JsonConverters;
using Newtonsoft.Json;

namespace MonoTanksClientLogic.Networking;

[JsonConverter(typeof(ZoneJsonConverter))]
public record class Zone(int index, long X, long Y, long Width, long Height, ZoneStatus status);