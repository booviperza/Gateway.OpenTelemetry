using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Xunit;

namespace Gateway.OpenTelemetry.IntegrationTests.Infrastructure;

/// <summary>
/// Shared fixture for Gateway integration tests.
/// </summary>
public sealed class GatewayFixture
    : WebApplicationFactory<Program>,
      IAsyncLifetime
{
    private BackendHost? _backendHost;

    /// <summary>
    /// Gets the shared HTTP client.
    /// </summary>
    public HttpClient Client { get; private set; } = default!;

    /// <summary>
    /// Gets collected activities.
    /// </summary>
    internal ActivityCollector Collector { get; } = new();

    /// <summary>
    /// Gets collected HTTP metric tags.
    /// </summary>
    internal MetricTagsCollector MetricTags { get; } = new();

    /// <summary>
    /// Initializes the test gateway.
    /// </summary>
    public async Task InitializeAsync()
    {
        _backendHost = await BackendHost.StartAsync();

        Client = CreateClient();
    }

    /// <summary>
    /// Disposes test resources.
    /// </summary>
    public new async Task DisposeAsync()
    {
        Client?.Dispose();

        if (_backendHost is not null)
        {
            await _backendHost.DisposeAsync();
            _backendHost = null;
        }

        await base.DisposeAsync();
    }

    /// <inheritdoc />
    protected override void ConfigureWebHost(
        IWebHostBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.UseEnvironment("Development");

        builder.ConfigureAppConfiguration((_, configuration) =>
        {
            if (_backendHost is null)
            {
                return;
            }

            Dictionary<string, string?> settings = new()
            {
                ["ReverseProxy:Clusters:cluster1:Destinations:backend1:Address"]
                    = _backendHost.BaseAddress.ToString()
            };

            configuration.AddInMemoryCollection(settings);
        });

        builder.ConfigureServices(services =>
        {
            services
                .AddOpenTelemetry()
                .WithTracing(tracing =>
                {
                    tracing.AddTestExporter(Collector);
                });

            ServiceDescriptor? metricFilter =
                services.FirstOrDefault(
                    descriptor =>
                        descriptor.ServiceType == typeof(IStartupFilter) &&
                        descriptor.ImplementationType?.Name ==
                            "MetricEnrichmentStartupFilter");

            if (metricFilter is not null)
            {
                services.Remove(metricFilter);
            }

            services.AddSingleton<IStartupFilter>(
                new MetricTagsCaptureStartupFilter(MetricTags));

            if (metricFilter is not null)
            {
                services.Add(metricFilter);
            }
        });
    }
}
