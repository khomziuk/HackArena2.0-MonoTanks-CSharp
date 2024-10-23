namespace MonoTanksClientLogic.Models;

public record class OwnPlayer(
    string Id,
    string Nickname,
    long Color,
    long Ping,
    long Score,
    long? TicksToRegen,
    bool IsUsingRadar)
    : GamePlayer(
        Id,
        Nickname,
        Color,
        Ping);