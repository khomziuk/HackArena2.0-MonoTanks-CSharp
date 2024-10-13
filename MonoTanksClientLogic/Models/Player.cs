using Newtonsoft.Json;

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
    [JsonConstructor]
    public Player(string id, string nickname, uint color)
    {
        this.Id = id;
        this.Nickname = nickname;
        this.Color = color;
        this.tank = null!;
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
}
