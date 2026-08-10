# Gateway.OpenTelemetry

Lightweight OpenTelemetry extensions for ASP.NET Core, YARP, and gateway/proxy applications.

Gateway.OpenTelemetry enriches telemetry already produced by OpenTelemetry with gateway-specific metadata. It supports both tracing and ASP.NET Core HTTP metrics.

## Features

- ASP.NET Core trace enrichment
- YARP trace enrichment
- ASP.NET Core HTTP metric enrichment
- YARP metric enrichment
- Gateway route, cluster, and destination metadata
- Dependency Injection integration
- No reflection or runtime code generation
- Stateless built-in enrichers
- Unit, integration, and package smoke tests

## Packages

| Package | Purpose |
|---|---|
| `Gateway.OpenTelemetry.Core` | Shared abstractions, constants, and common infrastructure. |
| `Gateway.OpenTelemetry.AspNetCore` | ASP.NET Core tracing and HTTP metrics integration. |
| `Gateway.OpenTelemetry.Yarp` | YARP-specific tracing and metric enrichment. |
| `Gateway.OpenTelemetry.Proxy` | Proxy telemetry abstractions and proxy-facing telemetry features. |

For a YARP gateway, install `Gateway.OpenTelemetry.Yarp`; its NuGet dependencies bring the required Gateway.OpenTelemetry ASP.NET Core/Core packages.

## Requirements

| Component | Version |
|---|---|
| .NET | 10.0 |
| OpenTelemetry | 1.17+ |
| YARP | 2.3+ |

Current packages target `net10.0`.

## Installation

### YARP

```bash
dotnet add package Gateway.OpenTelemetry.Yarp --version 1.0.0
```

### ASP.NET Core only

```bash
dotnet add package Gateway.OpenTelemetry.AspNetCore --version 1.0.0
```

### Proxy

```bash
dotnet add package Gateway.OpenTelemetry.Proxy --version 1.0.0
```

## Quick Start

```csharp
using Gateway.OpenTelemetry.AspNetCore.DependencyInjection;
using Gateway.OpenTelemetry.Yarp;
using Gateway.OpenTelemetry.Yarp.DependencyInjection;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddGatewayOpenTelemetry();
builder.Services.AddGatewayYarpOpenTelemetry();

builder.Services
    .AddOpenTelemetry()
    .WithMetrics(metrics =>
    {
        metrics.AddAspNetCoreInstrumentation();
    })
    .WithTracing(tracing =>
    {
        tracing.AddAspNetCoreInstrumentation();
    });

var app = builder.Build();

app.MapGatewayReverseProxy();

app.Run();
```

The application remains responsible for configuring OpenTelemetry exporters.

## Architecture

### Tracing

```text
HTTP Request
    |
    v
ASP.NET Core OpenTelemetry Instrumentation
    |
    v
Gateway.OpenTelemetry.AspNetCore
    |
    v
CompositeTraceEnricher
    |
    v
Gateway.OpenTelemetry.Yarp / YarpTraceEnricher
    |
    v
Activity.SetTag(...)
    |
    v
OpenTelemetry Exporter
```

Gateway.OpenTelemetry enriches the existing request `Activity`; it does not create a second server Activity solely for enrichment.

### Metrics

```text
HTTP Request
    |
    v
ASP.NET Core HTTP Metrics
    |
    v
IHttpMetricsTagsFeature
    |
    v
MetricEnrichmentMiddleware
    |
    v
MetricEnrichmentDispatcher
    |
    v
CompositeMetricEnricher
    |
    v
YarpMetricEnricher
    |
    v
ASP.NET Core metric tags
    |
    v
OpenTelemetry Metrics / Exporter
```

The YARP metric enricher adds gateway metadata to the ASP.NET Core HTTP metrics feature.

## OpenTelemetry Configuration

### Tracing

```csharp
builder.Services
    .AddOpenTelemetry()
    .WithTracing(tracing =>
    {
        tracing
            .AddAspNetCoreInstrumentation()
            .AddOtlpExporter();
    });
```

Use the exporter appropriate for your environment.

### Metrics

```csharp
builder.Services
    .AddOpenTelemetry()
    .WithMetrics(metrics =>
    {
        metrics.AddAspNetCoreInstrumentation();
    });
```

### Prometheus

With the OpenTelemetry Prometheus exporter configured in the application:

```csharp
builder.Services
    .AddOpenTelemetry()
    .WithMetrics(metrics =>
    {
        metrics
            .AddAspNetCoreInstrumentation()
            .AddPrometheusExporter();
    });

app.MapPrometheusScrapingEndpoint();
```

## Gateway Trace Tags

### ASP.NET Core

| Tag | Description |
|---|---|
| `gateway.host` | Request host information. |
| `gateway.endpoint.display_name` | ASP.NET Core endpoint display name. |
| `gateway.exception.type` | Exception type when available. |

### YARP

| Tag | Description |
|---|---|
| `gateway.yarp.route_id` | Matched YARP route ID. |
| `gateway.yarp.cluster_id` | Selected YARP cluster ID. |
| `gateway.yarp.destination_id` | Selected YARP destination ID. |

## Gateway Metric Tags

YARP metric enrichment adds the following attributes to ASP.NET Core HTTP metrics such as `http.server.request.duration`:

| Attribute | Description |
|---|---|
| `gateway.yarp.route_id` | Matched YARP route ID. |
| `gateway.yarp.cluster_id` | Selected YARP cluster ID. |
| `gateway.yarp.destination_id` | Selected YARP destination ID. |

Example exported metric:

```text
http.server.request.duration

http.request.method = GET
http.response.status_code = 200
http.route = /proxy/{**catch-all}
gateway.yarp.route_id = smoke-route
gateway.yarp.cluster_id = smoke-cluster
gateway.yarp.destination_id = smoke-backend
```

These are attributes on the existing HTTP metric, not a separate custom metric.

## YARP Integration

Register YARP normally:

```csharp
builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
```

Register Gateway.OpenTelemetry:

```csharp
builder.Services.AddGatewayOpenTelemetry();
builder.Services.AddGatewayYarpOpenTelemetry();
```

Map the gateway endpoint:

```csharp
app.MapGatewayReverseProxy();
```

The application should not directly instantiate internal enrichers such as `YarpMetricEnricher`, `YarpTraceEnricher`, `MetricEnrichmentDispatcher`, or `CompositeMetricEnricher`.

## Example YARP Configuration

```json
{
  "ReverseProxy": {
    "Routes": {
      "api": {
        "ClusterId": "backend",
        "Match": {
          "Path": "/api/{**catch-all}"
        }
      }
    },
    "Clusters": {
      "backend": {
        "Destinations": {
          "node1": {
            "Address": "http://localhost:5001/"
          }
        }
      }
    }
  }
}
```

A matching request can produce:

```text
gateway.yarp.route_id = api
gateway.yarp.cluster_id = backend
gateway.yarp.destination_id = node1
```

## Proxy Telemetry

`Gateway.OpenTelemetry.Proxy` exposes the public proxy telemetry feature:

```csharp
Gateway.OpenTelemetry.Proxy.Features.IProxyTelemetryFeature
```

The feature provides proxy telemetry information for the current request, including:

```text
RouteId
RoutePattern
Upstream
ClientId
CorrelationId
CertificateId
Timeout
ProxyName
```

Application code should consume the public feature rather than depend on internal proxy implementation types.

## Dependency Injection

ASP.NET Core integration:

```csharp
builder.Services.AddGatewayOpenTelemetry();
```

YARP integration:

```csharp
builder.Services.AddGatewayYarpOpenTelemetry();
```

Built-in enrichers are registered by the integration packages.

## Custom Trace Enrichers

Applications can register additional implementations of `ITraceEnricher`:

```csharp
public sealed class UserTraceEnricher : ITraceEnricher
{
    public void Enrich(
        HttpContext context,
        Activity activity)
    {
        activity.SetTag(
            "user.id",
            context.User.Identity?.Name);
    }
}
```

Register it through dependency injection:

```csharp
builder.Services.AddSingleton<ITraceEnricher, UserTraceEnricher>();
```

Keep custom enrichers lightweight because they execute during request telemetry processing.

## Public API

The primary public integration surface is:

```text
AddGatewayOpenTelemetry()
AddGatewayYarpOpenTelemetry()
MapGatewayReverseProxy()
IProxyTelemetryFeature
```

Shared trace enrichment abstractions and constants are provided by the Core package.

## Package Dependencies

The current YARP package has these package dependencies:

```text
Gateway.OpenTelemetry.AspNetCore  1.0.0
Gateway.OpenTelemetry.Core        1.0.0
Yarp.ReverseProxy                 2.3.0
```

Conceptually:

```text
Gateway.OpenTelemetry.Yarp
    |
    +--> Gateway.OpenTelemetry.AspNetCore
    |         |
    |         +--> Gateway.OpenTelemetry.Core
    |
    +--> Yarp.ReverseProxy
```

## Performance

The implementation is designed to keep the enrichment path lightweight. It currently:

- Does not use reflection.
- Does not use runtime code generation.
- Uses dependency injection.
- Uses stateless built-in enrichers.
- Enriches the existing `Activity` for tracing.
- Adds YARP attributes to the existing ASP.NET Core HTTP metrics feature.
- Does not create a separate custom metric for each YARP request.

Actual performance depends on request volume, telemetry configuration, sampling, and exporter behavior.

## Thread Safety

Built-in enrichers are designed to be stateless and safe for concurrent request processing. Custom enrichers should avoid shared mutable state or protect it appropriately.

## Best Practices

1. Configure ASP.NET Core OpenTelemetry instrumentation.
2. Register `AddGatewayOpenTelemetry()`.
3. Register `AddGatewayYarpOpenTelemetry()` for YARP.
4. Configure exporters in the host application.
5. Use `MapGatewayReverseProxy()` for the Gateway YARP endpoint.
6. Keep custom enrichers lightweight.
7. Avoid blocking I/O, database calls, and network calls inside enrichers.
8. Avoid creating an additional server Activity solely for gateway enrichment.

## Troubleshooting

### No Gateway Trace Tags

Check:

- `AddGatewayOpenTelemetry()` is registered.
- `AddAspNetCoreInstrumentation()` is registered for tracing.
- The request reaches the ASP.NET Core application.
- The request is not excluded by application telemetry configuration.

### No YARP Trace Tags

Check:

- `AddGatewayYarpOpenTelemetry()` is registered.
- The request is handled by YARP.
- A YARP route matches the request.
- A cluster/destination is selected.

### No Gateway Metric Tags

Check:

- `AddGatewayOpenTelemetry()` is registered.
- `AddGatewayYarpOpenTelemetry()` is registered.
- `AddAspNetCoreInstrumentation()` is registered for metrics.
- HTTP metrics are enabled.
- The request is handled by YARP.

Expected YARP metric attributes:

```text
gateway.yarp.route_id
gateway.yarp.cluster_id
gateway.yarp.destination_id
```

### No Exported Metrics

Check that a metrics exporter is registered and its endpoint is reachable. For Prometheus, verify that the scraping endpoint is mapped and reachable.

### No Exported Traces

Check that a tracing exporter is registered, its endpoint is reachable, and sampling allows the trace to be exported.

### `MapGatewayReverseProxy()` Not Found

Ensure the YARP package is referenced and import:

```csharp
using Gateway.OpenTelemetry.Yarp;
```

## Version Compatibility

| Gateway.OpenTelemetry | .NET | OpenTelemetry | YARP |
|---|---|---|---|
| 1.x | .NET 10 | 1.17+ | 2.3+ |

## Sample Project

The repository contains:

```text
samples/OpenTelemetry.Spike
```

The sample is intended for OpenTelemetry experimentation. Integration infrastructure is provided separately under `tests`.

## Building from Source

```bash
git clone https://github.com/booviperza/Gateway.OpenTelemetry.git
cd Gateway.OpenTelemetry
dotnet build -warnaserror
```

## Running Tests

Run the complete test suite:

```bash
dotnet test
```

Individual test projects:

```bash
dotnet test tests/Gateway.OpenTelemetry.Core.UnitTests
dotnet test tests/Gateway.OpenTelemetry.AspNetCore.UnitTests
dotnet test tests/Gateway.OpenTelemetry.Yarp.UnitTests
dotnet test tests/Gateway.OpenTelemetry.Proxy.UnitTests
dotnet test tests/Gateway.OpenTelemetry.IntegrationTests
```

## Package Smoke Test

The repository also validates the generated NuGet artifacts through a package consumer. The smoke-test flow verifies that:

1. The NuGet package can be restored from a local package source.
2. A consuming YARP application builds successfully.
3. YARP routes a request to a backend.
4. Gateway metric enrichment is present in the resulting HTTP metrics.

A successful result contains:

```text
gateway.yarp.route_id
gateway.yarp.cluster_id
gateway.yarp.destination_id
```

This verifies the packaged artifact rather than only the source project.

## Repository Structure

```text
Gateway.OpenTelemetry
|
+-- src
|   +-- Gateway.OpenTelemetry.Core
|   +-- Gateway.OpenTelemetry.AspNetCore
|   +-- Gateway.OpenTelemetry.Yarp
|   +-- Gateway.OpenTelemetry.Proxy
|
+-- tests
|   +-- Gateway.OpenTelemetry.Core.UnitTests
|   +-- Gateway.OpenTelemetry.AspNetCore.UnitTests
|   +-- Gateway.OpenTelemetry.Yarp.UnitTests
|   +-- Gateway.OpenTelemetry.Proxy.UnitTests
|   +-- Gateway.OpenTelemetry.IntegrationTests
|   +-- Gateway.OpenTelemetry.IntegrationHost
|
+-- samples
    +-- OpenTelemetry.Spike
```

## Repository Standards

- Semantic Versioning
- XML documentation for public APIs
- Nullable reference types
- SourceLink
- Central Package Management
- Unit tests
- Integration tests
- Package smoke testing
- MIT License

## Roadmap

Potential future improvements include:

- Additional built-in trace enrichers
- Additional built-in metric enrichers
- Logging enrichment
- Health Checks integration
- Benchmark project
- Roslyn analyzers
- Additional gateway metadata
- Additional exporter/configuration examples

Metrics enrichment and the current YARP metric tags are implemented functionality, not roadmap items.

## Contributing

Before submitting a change:

1. Build the solution successfully.
2. Run the unit tests.
3. Run integration tests when runtime behavior changes.
4. Add tests for new functionality.
5. Update public API documentation.
6. Update the README when public usage changes.

## License

This project is licensed under the MIT License. See `LICENSE` for details.

## Support

For questions, bug reports, and feature requests, use GitHub Issues:

https://github.com/booviperza/Gateway.OpenTelemetry

## Final Notes

Gateway.OpenTelemetry is an enrichment layer for gateway applications. It does not replace OpenTelemetry instrumentation, exporters, ASP.NET Core HTTP telemetry, or YARP.

For tracing, gateway information is added to the existing `Activity`. For metrics, YARP information is added to ASP.NET Core HTTP metric tags. This keeps gateway telemetry aligned with the OpenTelemetry ecosystem while making route, cluster, destination, endpoint, and proxy context available to observability backends.
