namespace MonoTanksClientLogic;

/// <summary>
/// Represents a mine.
/// </summary>
public class Mine : IStunEffect
{
    /// <summary>
    /// The number of ticks that the explosion lasts.
    /// </summary>
    public const int ExplosionTicks = 10;

    private static int idCounter = 0;

    /// <summary>
    /// Initializes a new instance of the <see cref="Mine"/> class.
    /// </summary>
    /// <param name="x">The x coordinate of the mine.</param>
    /// <param name="y">The y coordinate of the mine.</param>
    /// <param name="damage">The damage dealt by the mine.</param>
    /// <param name="layer">The player that deployed the mine.</param>
    /// <remarks>
    /// <para>This constructor should be used when a tank deploys a mine.</para>
    /// <para>The <see cref="Id"/> property is set automatically.</para>
    /// </remarks>
    internal Mine(int x, int y, int damage, Player layer)
        : this(idCounter++, x, y)
    {
        this.Damage = damage;
        this.Layer = layer;
        this.LayerId = layer.Id;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Mine"/> class.
    /// </summary>
    /// <param name="id">The id of the mine.</param>
    /// <param name="x">The x coordinate of the mine.</param>
    /// <param name="y">The y coordinate of the mine.</param>
    /// <remarks>
    /// This constructor should be used when creating a mine
    /// from player perspective, because they shouldn't know
    /// the <see cref="LayerId"/>, <see cref="Layer"/>
    /// and <see cref="Damage"/> (these will be set to <see langword="null"/>).
    /// </remarks>
    internal Mine(int id, int x, int y)
    {
        this.Id = id;
        this.X = x;
        this.Y = y;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Mine"/> class.
    /// </summary>
    /// <param name="id">The id of the mine.</param>
    /// <param name="x">The x coordinate of the mine.</param>
    /// <param name="y">The y coordinate of the mine.</param>
    /// <param name="damage">The damage dealt by the mine.</param>
    /// <param name="layerId">The id of the player that dropped the mine.</param>
    /// <remarks>
    /// <para>
    /// This constructor should be used when creating a mine
    /// from the server or spectator perspective, because they know
    /// all the properties of the mine.
    /// </para>
    /// <para>
    /// This constructor does not set the <see cref="Layer"/> property.
    /// See its documentation for more information.
    /// </para>
    /// </remarks>
    internal Mine(int id, int x, int y, int damage, string layerId)
        : this(id, x, y)
    {
        this.Damage = damage;
        this.LayerId = layerId;
    }

    /// <inheritdoc/>
    int IStunEffect.StunTicks => 10;

    /// <inheritdoc/>
    StunBlockEffect IStunEffect.StunBlockEffect
        => StunBlockEffect.Movement | StunBlockEffect.TankRotation;

    /// <summary>
    /// Gets the id of the mine.
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// Gets the x coordinate of the mine.
    /// </summary>
    public int X { get; }

    /// <summary>
    /// Gets the y coordinate of the mine.
    /// </summary>
    public int Y { get; }

    /// <summary>
    /// Gets the damage dealt by the mine.
    /// </summary>
    public int? Damage { get; }

    /// <summary>
    /// Gets the id of the player that deployed the mine.
    /// </summary>
    public string? LayerId { get; }

    /// <summary>
    /// Gets the player that deployed the mine.
    /// </summary>
    /// <remarks>
    /// This value is internal and should be set in the
    /// <see cref="Grid.UpdateFromGameStatePayload"/>
    /// method, if the <see cref="LayerId"/> is known.
    /// </remarks>
    public Player? Layer { get; internal set; }

    /// <summary>
    /// Gets the remaining ticks of the mine's explosion.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The value is <see langword="null"/>
    /// if the mine hasn't exploded yet.
    /// </para>
    /// </remarks>
    public int? ExplosionRemainingTicks { get; internal set; }

    /// <summary>
    /// Gets a value indicating whether the mine is exploded.
    /// </summary>
    public bool IsExploded => this.ExplosionRemainingTicks is not null;

    /// <summary>
    /// Gets a value indicating whether the mine is fully exploded.
    /// </summary>
    /// <remarks>
    /// The mine is fully exploded if it has exploded
    /// and the explosion has finished.
    /// </remarks>
    public bool IsFullyExploded => this.ExplosionRemainingTicks <= 0;
}
