namespace MonoTanksClientLogic.Networking;

public record class EnemyPlayer(
string Id,
string Nickname,
long Color,
long Ping)
: GamePlayer(
    Id,
    Nickname,
    Color,
    Ping);
