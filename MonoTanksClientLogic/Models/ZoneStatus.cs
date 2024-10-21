using Newtonsoft.Json;

namespace MonoTanksClientLogic;

/// <summary>
/// Represents the status of a zone.
/// </summary>
public class ZoneStatus
{
    /// <summary>
    /// Represents a neutral zone.
    /// </summary>
    public class Neutral : ZoneStatus
    {
    }

    /// <summary>
    /// Represents a zone that is being captured.
    /// </summary>
    /// <param name="player">The player that is capturing the zone.</param>
    /// <param name="remainingTicks">The number of ticks remaining until the zone is captured.</param>
    public class BeingCaptured(string playerId, int remainingTicks) : ZoneStatus
    {
        /// <summary>
        /// Gets the number of ticks remaining until the zone is captured.
        /// </summary>
        public int RemainingTicks { get; internal set; } = remainingTicks;

        /// <summary>
        /// Gets the ID of the player that is being capturing the zone.
        /// </summary>
        public string PlayerId { get; internal set; } = playerId;
    }

    /// <summary>
    /// Represents a zone that is captured.
    /// </summary>
    /// <param name="player">The player that captured the zone.</param>
    public class Captured(string playerId) : ZoneStatus
    {
        /// <summary>
        /// Gets the ID of the player that is capturing the zone.
        /// </summary>
        public string PlayerId { get; internal set; } = playerId;
    }

    /// <summary>
    /// Represents a zone that is being contested.
    /// </summary>
    /// <param name="capturedBy">The player that captured the zone, if the zone was captured.</param>
    public class BeingContested(string? capturedById) : ZoneStatus
    {
        /// <summary>
        /// Gets the ID of the player that is capturing the zone.
        /// </summary>
        public string? CapturedById { get; private set; } = capturedById;
    }

    /// <summary>
    /// Represents a zone that is being retaken.
    /// </summary>
    /// <param name="capturedBy">The player that captured the zone.</param>
    /// <param name="retakenBy">The player that is retaking the zone.</param>
    /// <param name="remainingTicks">The number of ticks remaining until the zone is retaken.</param>
    public class BeingRetaken(string capturedById, string retakenById, int remainingTicks) : ZoneStatus
    {
        /// <summary>
        /// Gets the number of ticks remaining until the zone is retaken.
        /// </summary>
        public int RemainingTicks { get; internal set; } = remainingTicks;

        /// <summary>
        /// Gets the ID of the player that is capturing the zone.
        /// </summary>
        public string CapturedById { get; private set; } = capturedById;

        /// <summary>
        /// Gets the ID of the player that is being retaking the zone.
        /// </summary>
        public string RetakenById { get; private set; } = retakenById;
    }
}
