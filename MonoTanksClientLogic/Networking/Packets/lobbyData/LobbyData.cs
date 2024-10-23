using MonoTanksClientLogic.Networking;

namespace MonoTanksClientLogic.Models;

public record class LobbyData(string PlayerId, LobbyPlayer[] Players, ServerSettings ServerSettings);