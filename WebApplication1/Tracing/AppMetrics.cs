using System.Diagnostics.Metrics;

namespace WebApplication1.Tracing;

public class AppMetrics
{
    public AppMetrics(string meterName, string meterVersion)
    {
        // Creates a new meter
        var meter = new Meter(meterName, meterVersion);
        // Creates a new instrument from the meter
        VoteCounter = meter.CreateCounter<long>("vote", "count", "Number of votes cast");
    }

    public Counter<long> VoteCounter { get; }
}