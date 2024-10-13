namespace MonoTanksClientLogic;

/// <summary>
/// Represents a grid.
/// </summary>
/// <param name="dimension">The dimension of the grid.</param>
/// <param name="seed">The seed of the grid.</param>
public class Grid(int dimension, int seed)
{
    private readonly Queue<Bullet> queuedBullets = new();
    private readonly Random random = new(seed);

    private List<Zone> zones = [];
    private List<Tank> tanks = [];
    private List<Bullet> bullets = [];

    /// <summary>
    /// Occurs when the state is updating.
    /// </summary>
    public event EventHandler? StateUpdating;

    /// <summary>
    /// Occurs when the state has updated.
    /// </summary>
    public event EventHandler? StateUpdated;

    /// <summary>
    /// Occurs when the dimensions are changing.
    /// </summary>
    public event EventHandler? DimensionsChanging;

    /// <summary>
    /// Occurs when the dimensions have changed.
    /// </summary>
    public event EventHandler? DimensionsChanged;

    /// <summary>
    /// Gets an empty grid.
    /// </summary>
    public static Grid Empty => new(0, 0);

    /// <summary>
    /// Gets the dimension of the grid.
    /// </summary>
    public int Dim { get; private set; } = dimension;

    /// <summary>
    /// Gets the seed of the grid.
    /// </summary>
    public int Seed { get; private init; } = seed;

    /// <summary>
    /// Gets the wall grid.
    /// </summary>
    public Wall?[,] WallGrid { get; private set; } = new Wall?[dimension, dimension];

    /// <summary>
    /// Gets the zones.
    /// </summary>
    public IEnumerable<Zone> Zones => this.zones;

    /// <summary>
    /// Gets the tanks.
    /// </summary>
    public IEnumerable<Tank> Tanks => this.tanks;

    /// <summary>
    /// Gets the bullets.
    /// </summary>
    public IEnumerable<Bullet> Bullets => this.bullets;

    /// <summary>
    /// Represents a map payload for the grid.
    /// </summary>
    /// <param name="Visibility">The visibility payload for the grid.</param>
    /// <param name="Tiles">The tiles payload for the grid.</param>
    /// <param name="Zones">The zones of the grid.</param>
    internal record class MapPayload(VisibilityPayload? Visibility, TilesPayload Tiles, List<Zone> Zones);

    /// <summary>
    /// Represents a tiles payload for the grid.
    /// </summary>
    /// <param name="WallGrid">The wall grid of the grid.</param>
    /// <param name="Tanks">The tanks of the grid.</param>
    /// <param name="Bullets">The bullets of the grid.</param>
    internal record class TilesPayload(Wall?[,] WallGrid, List<Tank> Tanks, List<Bullet> Bullets);

    /// <summary>
    /// Represents a visibility payload for the grid.
    /// </summary>
    /// <param name="VisibilityGrid">The visibility grid of the grid.</param>
    internal record class VisibilityPayload(bool[,] VisibilityGrid);
}