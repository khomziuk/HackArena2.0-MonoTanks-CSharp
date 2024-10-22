using MonoTanksClientLogic.Networking;

namespace MonoTanksClientLogic;

public record class LobbyData(string PlayerId, LobbyPlayer[] Players, ServerSettings ServerSettings);