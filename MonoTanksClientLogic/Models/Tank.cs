using System.Diagnostics;

namespace MonoTanksClientLogic;

/// <summary>
/// Represents a tank.
/// </summary>
public class Tank
{
    private const int MineDamage = 50;
    private readonly Dictionary<IStunEffect, int> remainingStuns = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="Tank"/> class.
    /// </summary>
    /// <param name="x">The x coordinate of the tank.</param>
    /// <param name="y">The y coordinate of the tank.</param>
    /// <param name="direction">The direction of the tank.</param>
    /// <param name="turretDirection">The direction of the turret.</param>
    /// <param name="owner">The owner of the tank.</param>
    internal Tank(int x, int y, Direction direction, Direction turretDirection, Player owner)
        : this(x, y, owner.Id, direction)
    {
        this.Owner = owner;
        this.Health = 100;
        this.Turret = new Turret(this, turretDirection);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Tank"/> class.
    /// </summary>
    /// <param name="x">The x coordinate of the tank.</param>
    /// <param name="y">The y coordinate of the tank.</param>
    /// <param name="ownerId">The owner ID of the tank.</param>
    /// <param name="direction">The direction of the tank.</param>
    /// <param name="turret">The turret of the tank.</param>
    /// <remarks>
    /// <para>
    /// This constructor should be used when creating a tank
    /// from player perspective, because they shouldn't know
    /// the <see cref="Health"/> and <see cref="SecondaryItem"/>
    /// (these will be set to <see langword="null"/>).
    /// </para>
    /// <para>
    /// The <see cref="Owner"/> property is set to <see langword="null"/>.
    /// See its documentation for more information.
    /// </para>
    /// </remarks>
    internal Tank(int x, int y, string ownerId, Direction direction, Turret turret)
        : this(x, y, ownerId, direction)
    {
        this.Turret = turret;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Tank"/> class.
    /// </summary>
    /// <param name="x">The x coordinate of the tank.</param>
    /// <param name="y">The y coordinate of the tank.</param>
    /// <param name="ownerId">The owner ID of the tank.</param>
    /// <param name="health">The health of the tank.</param>
    /// <param name="direction">The direction of the tank.</param>
    /// <param name="turret">The turret of the tank.</param>
    /// <param name="secondaryItemType">The secondary item type of the tank.</param>
    /// <remarks>
    /// <para>
    /// This constructor should be used when creating a tank
    /// from the server or spectator perspective, because they know
    /// all the properties of the tank.
    /// </para>
    /// <para>
    /// The <see cref="Owner"/> property is set to <see langword="null"/>.
    /// See its documentation for more information.
    /// </para>
    /// </remarks>
    internal Tank(
        int x,
        int y,
        string ownerId,
        int health,
        Direction direction,
        Turret turret,
        SecondaryItemType? secondaryItemType)
        : this(x, y, ownerId, direction, turret)
    {
        this.Health = health;
        this.SecondaryItemType = secondaryItemType;
    }

    private Tank(int x, int y, string ownerId, Direction direction)
    {
        this.X = x;
        this.Y = y;
        this.Owner = null!;
        this.OwnerId = ownerId;
        this.Direction = direction;
        this.Turret = null!;  // Set in the other constructors
    }

    /// <summary>
    /// Occurs when the tank dies.
    /// </summary>
    internal event EventHandler? Died;

    /// <summary>
    /// Occurs when the mine has been dropped;
    /// </summary>
    internal event EventHandler<Mine>? MineDropped;

    /// <summary>
    /// Gets the x coordinate of the tank.
    /// </summary>
    public int X { get; private set; }

    /// <summary>
    /// Gets the y coordinate of the tank.
    /// </summary>
    public int Y { get; private set; }

    /// <summary>
    /// Gets the health of the tank.
    /// </summary>
    public int? Health { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the tank is dead.
    /// </summary>
    public bool IsDead => this.Health <= 0;

    /// <summary>
    /// Gets the owner of the tank.
    /// </summary>
    /// <remarks>
    /// The setter is internal because the owner is set
    /// in the <see cref="Grid.UpdateFromGameStatePayload"/> method.
    /// </remarks>
    public Player Owner { get; internal set; }

    /// <summary>
    /// Gets the direction of the tank.
    /// </summary>
    public Direction Direction { get; private set; }

    /// <summary>
    /// Gets the turret of the tank.
    /// </summary>
    public Turret Turret { get; private set; }

#if DEBUG
    /// <summary>
    /// Gets or sets the secondary item of the tank.
    /// </summary>
    public SecondaryItemType? SecondaryItemType { get; set; }
#else
    /// <summary>
    /// Gets the secondary item of the tank.
    /// </summary>
    public SecondaryItemType? SecondaryItemType { get; internal set; }
#endif

    /// <summary>
    /// Gets the owner ID of the tank.
    /// </summary>
    internal string OwnerId { get; private set; }

    /// <summary>
    /// Sets the position of the tank.
    /// </summary>
    /// <param name="x">The x coordinate of the tank.</param>
    /// <param name="y">The y coordinate of the tank.</param>
    internal void SetPosition(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }
}
