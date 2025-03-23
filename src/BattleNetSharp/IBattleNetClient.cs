namespace BattleNetSharp;

public interface IBattleNetClient
{
    public ValueTask AuthenticateAsync(bool force = false, CancellationToken token = default);
}
