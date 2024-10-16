using MonoTanksClientLogic.Networking;

namespace MonoTanksClientLogic
{
    /// <summary>
    /// Represents GameEndPayload alias class.
    /// </summary>
    /// <remarks>
    /// This is an alias class that is created in order to
    /// simplify working with framework for the end user.
    /// </remarks>
    /// <param name="payload">game end payload.</param>
    public class LobbyData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LobbyData"/> class.
        /// </summary>
        /// <param name="payload">game end payload.</param>
        public LobbyData(LobbyDataPayload payload)
        {
            this.PlayerId = payload.PlayerId;
            this.Players = payload.Players;
            this.ServerSettings = payload.ServerSettings;
        }

        /// <summary>
        /// Gets id of a player.
        /// </summary>
        public string? PlayerId { get; }

        /// <summary>
        /// Gets id of a player.
        /// </summary>
        public List<Player> Players { get; }

        /// <summary>
        /// Gets server settings.
        /// </summary>
        public ServerSettings ServerSettings { get; }
    }
}
