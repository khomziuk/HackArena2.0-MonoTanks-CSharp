namespace MonoTanksClientLogic.Models;

/// <summary>
/// Represents game end player.
/// </summary>
/// <param name="Id">
/// Represents id of a player.
/// </param>
/// <param name="Nickname">
/// Represents nickname of a player.
/// </param>
/// <param name="Color">
/// Represents color of a player.
/// </param>
/// <param name="Score">
/// Represents score of a player.
/// </param>
/// <param name="Kills">
/// Represents kills of a player.
/// </param>
public record class GameEndPlayer(string Id, string Nickname, long Color, long Score, long Kills);
