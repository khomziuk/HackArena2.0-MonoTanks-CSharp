using static MonoTanksClientLogic.ZoneStatus;

namespace MonoTanksClientLogic;

/// <summary>
/// Represents a zone.
/// </summary>
public class Zone
{
    /// <summary>
    /// The number of ticks required to capture the zone.
    /// </summary>
    public const int TicksToCapture = 100;

    private readonly Dictionary<Player, int> remainingTicksToCapture = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="Zone"/> class.
    /// </summary>
    /// <param name="x">The x-coordinate of the zone.</param>
    /// <param name="y">The y-coordinate of the zone.</param>
    /// <param name="width">The width of the zone.</param>
    /// <param name="height">The height of the zone.</param>
    /// <param name="index">The index of the zone.</param>
    /// <remarks>
    /// The <see cref="Status"/> property is set to <see cref="Neutral"/>.
    /// </remarks>
    internal Zone(int x, int y, int width, int height, char index)
    {
        this.X = x;
        this.Y = y;
        this.Width = width;
        this.Height = height;
        this.Index = index;
        this.Status = new Neutral();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Zone"/> class.
    /// </summary>
    /// <param name="x">The x-coordinate of the zone.</param>
    /// <param name="y">The y-coordinate of the zone.</param>
    /// <param name="width">The width of the zone.</param>
    /// <param name="height">The height of the zone.</param>
    /// <param name="index">The index of the zone.</param>
    /// <param name="status">The status of the zone.</param>
    internal Zone(int x, int y, int width, int height, char index, ZoneStatus status)
        : this(x, y, width, height, index)
    {
        this.Status = status;
    }

    /// <summary>
    /// Gets the x-coordinate of the zone.
    /// </summary>
    public int X { get; }

    /// <summary>
    /// Gets the y-coordinate of the zone.
    /// </summary>
    public int Y { get; }

    /// <summary>
    /// Gets the width of the zone.
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// Gets the height of the zone.
    /// </summary>
    public int Height { get; }

    /// <summary>
    /// Gets the index of the zone.
    /// </summary>
    public char Index { get; }

    /// <summary>
    /// Gets the status of the zone.
    /// </summary>
    public ZoneStatus Status { get; internal set; }
}