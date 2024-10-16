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
    public class GameEnd
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameEnd"/> class.
        /// </summary>
        /// <param name="payload">game end payload.</param>
        public GameEnd(GameEndPayload payload)
        {
            this.Players = payload.Players;
        }

        /// <summary>
        /// Gets list of players.
        /// </summary>
        public List<Player> Players { get; init; }
    }
}
