namespace MonoTanksBotLogic.Models;

/// <summary>
/// Represents game end.
/// </summary>
/// <param name="Players">
/// Represents list of player stats at the end of the game.
/// </param>
public record class GameEnd(GameEndPlayer[] Players);
