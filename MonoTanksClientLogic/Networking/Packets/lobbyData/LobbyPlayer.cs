namespace MonoTanksClientLogic.Models;

/// <summary>
/// Represents lobby player.
/// </summary>
/// <param name="Id">Represents lobby player id.</param>
/// <param name="Nickname">Represents lobby player nickname.</param>
/// <param name="Color">Represents lobby player color.</param>
public record class LobbyPlayer(string Id, string Nickname, long Color);
