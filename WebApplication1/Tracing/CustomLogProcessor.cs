using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using OpenTelemetry;
using OpenTelemetry.Logs;

namespace WebApplication1.Tracing;

public class CustomLogProcessor : BaseProcessor<LogRecord>
{
    public override void OnEnd(LogRecord data)
    {
        var logState = new List<KeyValuePair<string, object?>>
        {
            new("ProcessID", Environment.ProcessId),
            new("DotnetFramework", RuntimeInformation.FrameworkDescription),
            new("Runtime", RuntimeInformation.RuntimeIdentifier),
        };
        if (data.StateValues != null)
        {
            data.StateValues =
                new ReadOnlyCollectionBuilder<KeyValuePair<string, object?>>(data.StateValues.Concat(logState))
                    .ToReadOnlyCollection();
        }

        base.OnEnd(data);
    }
}