using MonoTanksClientLogic.JsonConverters;
using Newtonsoft.Json;

namespace MonoTanksClientLogic.Networking;

[JsonConverter(typeof(TileJsonConverter))]
public record class Tile(Tile.TileEntity[] Entities)
{
    public abstract record class TileEntity;

    public record class Wall : TileEntity;

    public record class EnemyTank(Direction Direction, string OwnerId, EnemyTurret Turret) : TileEntity;

    public record class OwnTank(Direction Direction, long Health, string OwnerId, OwnTurret Turret, SecondaryItemType? SecondaryItem) : TileEntity;

    public record class Bullet(Direction Direction, long Id, double Speed, BulletType type) : TileEntity;

    public record class Item(SecondaryItemType ItemType) : TileEntity;

    public record class Laser(long Id, LaserDirection orientation) : TileEntity;

    public record class Mine(long Id, int ExplosionRemainingTicks) : TileEntity;
}