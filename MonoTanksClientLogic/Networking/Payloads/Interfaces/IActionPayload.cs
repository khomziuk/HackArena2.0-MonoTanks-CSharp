namespace MonoTanksClientLogic.Networking;

/// <summary>
/// Represents an action payload.
/// </summary>
internal interface IActionPayload
{
    /// <summary>
    /// Gets the game state packet id,
    /// that this action is associated with.
    /// </summary>
    public string? GameStateId { get; init; }
}
