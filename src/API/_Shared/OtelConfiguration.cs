using Npgsql;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace API._Shared;

internal static class OtelConfiguration
{
    internal static WebApplicationBuilder ConfigureOpenTelemetry(this WebApplicationBuilder builder)
    {
        builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.IncludeFormattedMessage = true;
            logging.IncludeScopes = true;
        });

        var otel = builder.Services.AddOpenTelemetry();
        otel.WithMetrics(metrics =>
        {
            metrics.AddAspNetCoreInstrumentation();
            metrics.AddHttpClientInstrumentation();
            metrics.AddProcessInstrumentation();
        });

        otel.WithTracing(tracing =>
        {
            tracing.AddAspNetCoreInstrumentation();
            tracing.AddHttpClientInstrumentation();
            tracing.AddNpgsql();
            tracing.AddEntityFrameworkCoreInstrumentation();
        });

        var otlpEndpoint = builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"];
        if (otlpEndpoint != null)
        {
            otel.UseOtlpExporter();
        }

        return builder;
    }
}