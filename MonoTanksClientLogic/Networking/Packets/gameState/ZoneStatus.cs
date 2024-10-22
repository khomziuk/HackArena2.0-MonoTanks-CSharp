namespace MonoTanksClientLogic.Networking;

public abstract record class ZoneStatus
{
    public record class Neutral : ZoneStatus;

    public record class BeingCaptured(long RemainingTicks, string PlayerId) : ZoneStatus;

    public record class Captured(string PlayerId) : ZoneStatus;

    public record class BeingContested(string? CapturedById) : ZoneStatus;

    public record class BeingRetaken(long RemainingTicks, string CapturedById, string RetakenById) : ZoneStatus;
}
