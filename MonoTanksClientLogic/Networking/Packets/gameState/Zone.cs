using MonoTanksClientLogic.JsonConverters;
using Newtonsoft.Json;

namespace MonoTanksClientLogic.Models;

[JsonConverter(typeof(ZoneJsonConverter))]
public record class Zone(int index, long X, long Y, long Width, long Height, ZoneStatus status);