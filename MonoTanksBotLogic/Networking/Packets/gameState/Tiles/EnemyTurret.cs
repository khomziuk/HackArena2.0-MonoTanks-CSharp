using MonoTanksBotLogic.Enums;

namespace MonoTanksBotLogic.Models;

/// <summary>
/// Represents turret of an enemy tank.
/// </summary>
/// <param name="Direction">Represents turret direction.</param>
public record class EnemyTurret(Direction Direction);
