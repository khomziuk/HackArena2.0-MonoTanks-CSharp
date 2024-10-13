namespace MonoTanksClientLogic;

/// <summary>
/// Represents a bullet.
/// </summary>
public class Bullet
{
    private static int idCounter = 0;

    private float x;
    private float y;

    /// <summary>
    /// Initializes a new instance of the <see cref="Bullet"/> class.
    /// </summary>
    /// <param name="x">The x coordinate of the bullet.</param>
    /// <param name="y">The y coordinate of the bullet.</param>
    /// <param name="direction">The direction of the bullet.</param>
    /// <param name="speed">The speed of the bullet per second.</param>
    /// <param name="damage">The damage dealt by the bullet.</param>
    /// <param name="shooter">The tank that shot the bullet.</param>
    /// <remarks>
    /// <para>This constructor should be used when a tank shoots a bullet.</para>
    /// <para>The <see cref="Id"/> property is set automatically.</para>
    /// </remarks>
    internal Bullet(int x, int y, Direction direction, float speed, int damage, Player shooter)
        : this(idCounter++, x, y, direction, speed)
    {
        this.Damage = damage;
        this.Shooter = shooter;
        this.ShooterId = shooter.Id;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Bullet"/> class.
    /// </summary>
    /// <param name="id">The id of the bullet.</param>
    /// <param name="x">The x coordinate of the bullet.</param>
    /// <param name="y">The y coordinate of the bullet.</param>
    /// <param name="direction">The direction of the bullet.</param>
    /// <param name="speed">The speed of the bullet per second.</param>
    /// <remarks>
    /// This constructor should be used when creating a bullet
    /// from player perspective, because they shouldn't know
    /// the <see cref="ShooterId"/>, <see cref="Shooter"/>
    /// and <see cref="Damage"/> (these will be set to <see langword="null"/>).
    /// </remarks>
    internal Bullet(int id, int x, int y, Direction direction, float speed)
    {
        this.Id = id;
        this.x = x;
        this.y = y;
        this.Direction = direction;
        this.Speed = speed;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Bullet"/> class.
    /// </summary>
    /// <param name="id">The id of the bullet.</param>
    /// <param name="direction">The direction of the bullet.</param>
    /// <param name="speed">The speed of the bullet per second.</param>
    /// <param name="x">The x coordinate of the bullet.</param>
    /// <param name="y">The y coordinate of the bullet.</param>
    /// <param name="damage">The damage dealt by the bullet.</param>
    /// <param name="shooterId">The id of the tank that shot the bullet.</param>
    /// <remarks>
    /// <para>
    /// This constructor should be used when creating a bullet
    /// from the server or spectator perspective, because they know
    /// all the properties of the bullet.
    /// </para>
    /// <para>
    /// This constructor does not set the <see cref="Shooter"/> property.
    /// See its documentation for more information.
    /// </para>
    /// </remarks>
    internal Bullet(int id, int x, int y, Direction direction, float speed, int damage, string shooterId)
        : this(id, x, y, direction, speed)
    {
        this.Damage = damage;
        this.ShooterId = shooterId;
    }

    /// <summary>
    /// Gets the id of the bullet.
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// Gets the x coordinate of the bullet.
    /// </summary>
    public int X => (int)this.x;

    /// <summary>
    /// Gets the y coordinate of the bullet.
    /// </summary>
    public int Y => (int)this.y;

    /// <summary>
    /// Gets the direction of the bullet.
    /// </summary>
    public Direction Direction { get; }

    /// <summary>
    /// Gets the speed of the bullet.
    /// </summary>
    /// <value>
    /// The speed of the bullet per second.
    /// </value>
    public float Speed { get; }

    /// <summary>
    /// Gets the damage dealt by the bullet.
    /// </summary>
    public int? Damage { get; }

    /// <summary>
    /// Gets the id of the owner of the bullet.
    /// </summary>
    public string? ShooterId { get; }

    /// <summary>
    /// Gets the tank that shot the bullet.
    /// </summary>
    /// <remarks>
    /// The setter is internal because the owner is set
    /// in the <see cref="Grid.UpdateFromGameStatePayload"/> method,
    /// if the <see cref="ShooterId"/> is known.
    /// </remarks>
    public Player? Shooter { get; internal set; }
}
