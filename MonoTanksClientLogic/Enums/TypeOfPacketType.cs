namespace MonoTanksClientLogic.Networking;

/// <summary>
/// Represents a type of packet type.
/// </summary>
internal enum TypeOfPacketType
{
    /// <summary>
    /// The packet type is serialized as an integer.
    /// </summary>
    Int,

    /// <summary>
    /// The packet type is serialized as a string.
    /// </summary>
    String,
}
