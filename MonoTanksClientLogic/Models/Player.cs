namespace MonoTanksClientLogic;

/// <summary>
/// Represents a player.
/// </summary>
public class Player
{
    private const int RegenTicks = 50;
    private Tank tank;

    /// <summary>
    /// Initializes a new instance of the <see cref="Player"/> class.
    /// </summary>
    /// <param name="id">The id of the player.</param>
    /// <param name="nickname">The nickname of the player.</param>
    /// <param name="color">The color of the player.</param>
    /// <remarks>
    /// <para>
    /// The <see cref="Tank"/> property is set to <see langword="null"/>.
    /// See its documentation for more information.
    /// </para>
    /// </remarks>
    public Player(string id, string nickname, uint color)
    {
        this.Id = id;
        this.Nickname = nickname;
        this.Color = color;
        this.tank = null!;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Player"/> class.
    /// </summary>
    /// <param name="id">The id of the player.</param>
    /// <param name="nickname">The nickname of the player.</param>
    /// <param name="color">The color of the player.</param>
    /// <param name="remainingTicksToRegen">The remaining ticks to regenerate the tank.</param>
    /// <param name="visibilityGrid">The visibility grid of the player.</param>
    /// <remarks>
    /// The <see cref="Tank"/> property is set to <see langword="null"/>.
    /// See its documentation for more information.
    /// </remarks>
    public Player(string id, string nickname, uint color, int? remainingTicksToRegen, bool[,]? visibilityGrid)
        : this(id, nickname, color)
    {
        this.RemainingTicksToRegen = remainingTicksToRegen;
        this.VisibilityGrid = visibilityGrid;
    }

    /// <summary>
    /// Gets the id of the player.
    /// </summary>
    public string Id { get; private set; }

    /// <summary>
    /// Gets the nickname of the player.
    /// </summary>
    public string Nickname { get; private set; }

    /// <summary>
    /// Gets the score of the player.
    /// </summary>
    public int Score { get; internal set; } = 0;

    /// <summary>
    /// Gets the color of the player.
    /// </summary>
    public uint Color { get; internal set; }

    /// <summary>
    /// Gets the number of players killed by this player.
    /// </summary>
    public int Kills { get; internal set; } = 0;

    /// <summary>
    /// Gets or sets the ping of the player.
    /// </summary>
    public int Ping { get; set; }

    /// <summary>
    /// Gets a value indicating whether the player is using radar.
    /// </summary>
    public bool IsUsingRadar { get; internal set; }

    /// <summary>
    /// Gets the remaining ticks to regenerate the tank.
    /// </summary>
    /// <remarks>
    /// The value is <see langword="null"/> if the tank is not dead.
    /// </remarks>
    public int? RemainingTicksToRegen { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the player is dead.
    /// </summary>
    public bool IsDead => this.Tank is null || this.Tank.IsDead;

    /// <summary>
    /// Gets the tank of the player.
    /// </summary>
    /// <remarks>
    /// The setter is internal because the owner is set
    /// in the <see cref="Grid.UpdateFromGameStatePayload"/> method.
    /// </remarks>
    public Tank Tank
    {
        get => this.tank;
        internal set
        {
            this.tank = value;
        }
    }

    /// <summary>
    /// Gets the visibility grid of the player.
    /// </summary>
    public bool[,]? VisibilityGrid { get; private set; }
}
