using MonoTanksClientLogic.Enums;

namespace MonoTanksClientLogic.Models;

public record class OwnTurret(Direction Direction, long bulletCount, double? ticksToRegenBullet);
