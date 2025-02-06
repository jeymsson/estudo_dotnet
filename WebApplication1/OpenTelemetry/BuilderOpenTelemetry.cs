using System.Runtime.InteropServices;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Extensions.Docker.Resources;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using WebApplication1.Telemetry.Tracing;

namespace WebApplication1.Telemetry
{
    public class BuilderOpenTelemetry
    {
        private static ResourceBuilder getResourceBuilder(WebApplicationBuilder builder) {
            // Shared resources for OTEL signals
            var resourceBuilder = ResourceBuilder.CreateDefault()
                // add attributes for the name and version of the service
                .AddService(GlobalData.ApplicationName, serviceVersion: GlobalData.ApplicationVersion)
                // add attributes for the OpenTelemetry SDK version
                .AddTelemetrySdk()
                // Populate platform details
                .AddDetector(new DockerResourceDetector())
                // add custom attributes
                .AddAttributes(new Dictionary<string, object>
                {
                    [ResourceSemanticConventions.AttributeHostName] = Environment.MachineName,
                    [ResourceSemanticConventions.AttributeOsDescription] = RuntimeInformation.OSDescription,
                    [ResourceSemanticConventions.AttributeDeploymentEnvironment] =
                        builder.Environment.EnvironmentName.ToLowerInvariant(),
                });
            return resourceBuilder;
        }

        private static bool isConsoleExporterEnabled(WebApplicationBuilder builder) {
            // Check if the console exporter is enabled
            return bool.Parse(
                builder.Configuration["OTLP:ConsoleExporter"] ?? "false"
            );
        }

        internal static void buildLogging(WebApplicationBuilder builder) {
            var resourceBuilder = getResourceBuilder(builder);
            var consoleExporterEnabled = isConsoleExporterEnabled(builder);
            // Set up logging pipeline
            builder.Logging.AddOpenTelemetry(loggerOptions => {
                loggerOptions.IncludeFormattedMessage = true;
                loggerOptions.IncludeScopes = true;
                loggerOptions.ParseStateValues = true;
                loggerOptions
                    // define the resource
                    .SetResourceBuilder(resourceBuilder)
                    // add custom processor
                    .AddProcessor(new CustomLogProcessor())
                    // send logs to the console using exporter
                    .AddConsoleExporter();
                    // send logs to collector if configured
                    if (consoleExporterEnabled) {
                        loggerOptions.AddOtlpExporter(options =>
                            options.Endpoint = new($"http://{builder.Configuration["Hosts:OTLP"]!}:4317"));
                    }
            });
        }

        internal static void buildTracingAndMetrics(WebApplicationBuilder builder) {
            builder.Services.AddSingleton(new AppMetrics(GlobalData.SourceName, GlobalData.ApplicationVersion));
            var resourceBuilder = getResourceBuilder(builder);
            var consoleExporterEnabled = isConsoleExporterEnabled(builder);
            // Configure tracing and metrics
            builder.Services
                .AddOpenTelemetry()
                .WithTracing(tracerProviderBuilder => {
                    tracerProviderBuilder
                        // Sets span status to ERROR on exception
                        .SetErrorStatusOnException()
                        // define the resource
                        .SetResourceBuilder(resourceBuilder)
                        // receive traces from our own custom sources
                        .AddSource(GlobalData.SourceName)
                        // receive traces from built-in sources
                        .AddAspNetCoreInstrumentation(options => {
                            options.Filter = (httpContext) => true;
                            options.EnrichWithException = (activity, exception) => {
                                activity.SetTag("otel.status_code", "ERROR");
                                activity.SetTag("otel.status_description", exception.Message);
                                activity.SetTag("exception.type", exception.GetType().Name);
                                activity.SetTag("exception.message", exception.Message);
                                activity.SetTag("exception.stacktrace", exception.StackTrace);
                            };
                        })
                        .AddHttpClientInstrumentation()
                        // receive traces from Entity Framework Core
                        .AddEntityFrameworkCoreInstrumentation(options => {
                            options.SetDbStatementForText = true;
                        })
                        // ensures that all spans are recorded and sent to exporter
                        .SetSampler(new AlwaysOnSampler())
                        // stream traces to the SpanExporter
                        // BatchActivityExportProcessor processes spans on a separate thread unlike the SimpleActivityExportProcessor
                        .AddProcessor(new BatchActivityExportProcessor(new OtlpTraceExporter (
                            new OtlpExporterOptions {
                                Endpoint = new Uri($"http://{builder.Configuration["Hosts:OTLP"]}:4317")
                            }
                        )));

                    // stream traces to the console
                    if(consoleExporterEnabled) {
                        tracerProviderBuilder.AddConsoleExporter();
                    }
                })
                .WithMetrics(meterProviderBuilder => {
                    meterProviderBuilder
                        // add rich tags to our metrics
                        .SetResourceBuilder(resourceBuilder)
                        // receive metrics from built-in sources
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        // receive metrics from custom sources
                        .AddMeter(GlobalData.SourceName)
                        // expose metrics in Prometheus exposition format
                        .AddPrometheusExporter();

                    // stream metrics to the console
                    if(consoleExporterEnabled) {
                        meterProviderBuilder.AddConsoleExporter();
                    }
                });
        }

        internal static void mapException(WebApplication app) {
            app.MapGet("/exception/", (TracerProvider tracerProvider
            , ILogger<Program> logger) => {
                var tracer = tracerProvider.GetTracer(GlobalData.SourceName);
                using var span = tracer.StartActiveSpan("Exception span");
                var simulatedException = new ApplicationException("Error processing the request");
                span.RecordException(simulatedException);
                span.SetStatus(Status.Error);
                logger.LogError(simulatedException, "Error logged");
                return Results.Ok();
            })
                .WithName("Exception")
                .Produces(StatusCodes.Status200OK);
        }

        internal static void usePrometheusScraping(WebApplication app) {
            // Enable the /metrics endpoint which will be scraped by Prometheus
            app.UseOpenTelemetryPrometheusScrapingEndpoint();
        }
    }
}
