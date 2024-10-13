namespace MonoTanksClientLogic;

public class Turret
{
    /// <summary>
    /// The maximum number of bullets a tank can have.
    /// </summary>
    public const int MaxBulletCount = 3;

    private const int BulletRegenTicks = 10;

    /// <summary>
    /// Initializes a new instance of the <see cref="Turret"/> class.
    /// </summary>
    /// <param name="tank">The tank that owns the turret.</param>
    /// <param name="direction">The direction of the turret.</param>
    internal Turret(Tank tank, Direction direction)
    {
        this.Tank = tank;
        this.Direction = direction;
        this.BulletCount = MaxBulletCount;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Turret"/> class.
    /// </summary>
    /// <param name="direction">The direction of the turret.</param>
    /// <remarks>
    /// <para>
    /// This constructor should be used when creating a turret
    /// from player perspective, because they shouldn't know
    /// the <see cref="BulletCount"/>, <see cref="BulletRegenProgress"/>
    /// (these will be set to <see langword="null"/>).
    /// </para>
    /// <para>
    /// The <see cref="Tank"/> property is set to <see langword="null"/>.
    /// See its documentation for more information.
    /// </para>
    /// </remarks>
    internal Turret(Direction direction)
    {
        this.Direction = direction;
        this.Tank = null!;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Turret"/> class.
    /// </summary>
    /// <param name="direction">The direction of the turret.</param>
    /// <param name="bulletCount">The number of bullets the tank has.</param>
    /// <param name="remainingTicksToRegenBullet">
    /// The remaining ticks to regenerate the bullet.
    /// </param>
    /// <remarks>
    /// <para>
    /// This constructor should be used when creating a turret
    /// from the server or spectator perspective, because they know
    /// all the properties of the turret.
    /// </para>
    /// <para>
    /// The <see cref="Tank"/> property is set to <see langword="null"/>.
    /// See its documentation for more information.
    /// </para>
    /// </remarks>
    internal Turret(Direction direction, int bulletCount, int? remainingTicksToRegenBullet)
    {
        this.Direction = direction;
        this.BulletCount = bulletCount;
        this.RemainingTicksToRegenBullet = remainingTicksToRegenBullet;
        this.Tank = null!;
    }

    /// <summary>
    /// Occurs when the tank shoots a bullet.
    /// </summary>
    public event Action<Bullet>? Shot;

    /// <summary>
    /// Gets the direction of the turret.
    /// </summary>
    public Direction Direction { get; private set; }

    /// <summary>
    /// Gets the number of bullets the tank has.
    /// </summary>
    public int? BulletCount { get; private set; }

    /// <summary>
    /// Gets the tank that owns the turret.
    /// </summary>
    /// <remarks>
    /// The setter is internal because the owner is set in the
    /// <see cref="Grid.UpdateFromGameStatePayload"/> method.
    /// </remarks>
    public Tank Tank { get; internal set; }

    /// <summary>
    /// Gets the bullet regeneration progress.
    /// </summary>
    /// <value>
    /// The regeneration progress of the bullet as a value between 0 and 1.
    /// </value>
    /// <remarks>
    /// The value is <see langword="null"/> if the tank is dead or has full bullets.
    /// </remarks>
    public float? BulletRegenProgress => this.RemainingTicksToRegenBullet is not null
        ? 1f - (this.RemainingTicksToRegenBullet / (float)BulletRegenTicks)
        : null;

    /// <summary>
    /// Gets the remaining ticks to regenerate the bullet.
    /// </summary>
    /// <remarks>
    /// The value is <see langword="null"/> if the tank is dead or has full bullets.
    /// </remarks>
    public int? RemainingTicksToRegenBullet { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the tank has full bullets.
    /// </summary>
    public bool HasFullBullets => this.BulletCount >= MaxBulletCount;

    /// <summary>
    /// Gets a value indicating whether the tank has bullets.
    /// </summary>
    public bool HasBullets => this.BulletCount > 0;
}
