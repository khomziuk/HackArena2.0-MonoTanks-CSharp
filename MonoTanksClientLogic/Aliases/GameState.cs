using MonoTanksClientLogic.Networking;

namespace MonoTanksClientLogic
{
    /// <summary>
    /// Represents a game state.
    /// </summary>
    public class GameState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameState"/> class.
        /// </summary>
        /// <param name="payload">game end payload.</param>
        public GameState(GameStatePayload.ForPlayer payload)
        {
            this.Map = payload.Map;
            this.Tick = payload.Tick;
            this.Players = payload.Players;
        }

        /// <summary>
        /// Gets the number of ticks that
        /// have passed in the game.
        /// </summary>
        public int Tick { get; }

        /// <summary>
        /// Gets the players.
        /// </summary>
        public List<Player> Players { get; }

        /// <summary>
        /// Gets the map state.
        /// </summary>
        public Grid.Map Map { get; }

        /// <summary>
        /// Gets the visibility grid of the player.
        /// </summary>
        public bool[,] VisibilityGrid => this.Map.Visibility!.VisibilityGrid;
    }
}
