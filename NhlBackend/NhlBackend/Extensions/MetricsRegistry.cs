using Prometheus;

namespace NhlBackend.Extensions;

public static class MetricsRegistry
{
    public static readonly Histogram DbDuration =
        Metrics.CreateHistogram(
            "nhl_db_query_duration_seconds",
            "Duration of DB queries",
            new HistogramConfiguration
            {
                Buckets = Histogram.ExponentialBuckets(0.001, 2, 15),
                LabelNames = ["query"]
            });
}