using GamesEngine.Service.GameLoop;

namespace GamesEngine.Tests.Fakes;

public class MockInterval : IInterval
{
    private long interval;

    public MockInterval(long interval)
    {
        this.interval = interval;
    }

    public long GetInterval()
    {
        return interval;
    }
}