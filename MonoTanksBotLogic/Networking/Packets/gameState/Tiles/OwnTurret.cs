using MonoTanksBotLogic.Enums;

namespace MonoTanksBotLogic.Models;

/// <summary>
/// Represents turret of an player own tank.
/// </summary>
/// <param name="Direction">Represents turret direction.</param>
/// <param name="BulletCount">Represents number of available bullets.</param>
/// /// <param name="TicksToRegenBullet">Represents time in ticks to regenerate bullet.</param>
public record class OwnTurret(Direction Direction, int BulletCount, double? TicksToRegenBullet);
