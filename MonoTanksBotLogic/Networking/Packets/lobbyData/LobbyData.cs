namespace MonoTanksBotLogic.Models;

/// <summary>
/// Represents lobby data.
/// </summary>
/// <param name="PlayerId">Represents your own player id.</param>
/// <param name="Players">Represents players list.</param>
/// <param name="ServerSettings">Represents current game server settings.</param>
public record class LobbyData(string PlayerId, LobbyPlayer[] Players, ServerSettings ServerSettings);
