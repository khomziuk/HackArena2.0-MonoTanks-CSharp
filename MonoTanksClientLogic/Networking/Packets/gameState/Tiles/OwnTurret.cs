namespace MonoTanksClientLogic.Networking;

public record class OwnTurret(Direction Direction, long bulletCount, double? ticksToRegenBullet);
