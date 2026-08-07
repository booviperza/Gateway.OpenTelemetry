# Gateway.OpenTelemetry

Lightweight OpenTelemetry extensions for ASP.NET Core and YARP.

Designed for modern API Gateways and reverse proxy applications.

> Extend OpenTelemetry with gateway-aware Activity enrichment while keeping your application lightweight, maintainable, and production-ready.

---

## Table of Contents

- [Why Gateway.OpenTelemetry?](#why-gatewayopentelemetry)
- [Features](#features)
- [Packages](#packages)
- [Package Selection Guide](#package-selection-guide)
- [Architecture](#architecture)
- [Installation](#installation)
- [Requirements](#requirements)
- [Quick Start](#quick-start)
- [Exporting Traces](#exporting-traces)
- [What Gets Added?](#what-gets-added)
- [Built-in Activity Tags](#built-in-activity-tags)
- [ASP.NET Core Integration](#aspnet-core-integration)
- [YARP Integration](#yarp-integration)
- [Example Configuration](#example-configuration)
- [Design Philosophy](#design-philosophy)
- [Dependency Injection](#dependency-injection)
- [Custom Trace Enrichers](#custom-trace-enrichers)
- [Public API Reference](#public-api-reference)
- [Package Dependencies](#package-dependencies)
- [Performance](#performance)
- [Thread Safety](#thread-safety)
- [Best Practices](#best-practices)
- [Troubleshooting](#troubleshooting)
- [FAQ](#faq)
- [Version Compatibility](#version-compatibility)
- [Sample Project](#sample-project)
- [Building from Source](#building-from-source)
- [Running Tests](#running-tests)
- [Repository Structure](#repository-structure)
- [Repository Standards](#repository-standards)
- [Roadmap](#roadmap)
- [Contributing](#contributing)
- [License](#license)
- [Support](#support)
- [Acknowledgements](#acknowledgements)
---

## Why Gateway.OpenTelemetry?

OpenTelemetry provides excellent instrumentation for ASP.NET Core, but gateway applications often require additional context that is not available out of the box.

For example:

- Which YARP Route handled the request?
- Which Cluster processed the request?
- Which Destination was selected?
- Which ASP.NET Core Endpoint handled the request?
- Which Exception Type caused the failure?

Gateway.OpenTelemetry automatically enriches tracing Activities with this information.

---

## Features

- Lightweight and dependency-friendly
- Built on top of OpenTelemetry
- ASP.NET Core Activity enrichment
- YARP Activity enrichment
- Dependency Injection friendly
- No reflection
- No runtime code generation
- Minimal allocations
- Production-ready architecture
- Unit tested
- Integration tested

---

## Packages

Gateway.OpenTelemetry consists of three packages.

| Package | Description |
|----------|-------------|
| **Gateway.OpenTelemetry.Core** | Shared abstractions, constants and common infrastructure. |
| **Gateway.OpenTelemetry.AspNetCore** | ASP.NET Core Activity enrichment. |
| **Gateway.OpenTelemetry.Yarp** | YARP-specific Activity enrichment. |

Dependency hierarchy:

```
Gateway.OpenTelemetry.Yarp
            │
            ▼
Gateway.OpenTelemetry.AspNetCore
            │
            ▼
Gateway.OpenTelemetry.Core
```
---

## Package Selection Guide

Choose the package that matches your application's requirements.

| Scenario | Recommended Package |
|----------|---------------------|
| ASP.NET Core application | Gateway.OpenTelemetry.AspNetCore |
| YARP reverse proxy | Gateway.OpenTelemetry.Yarp |
| Shared library | Gateway.OpenTelemetry.Core |

Because the packages are layered, installing `Gateway.OpenTelemetry.Yarp` automatically includes the ASP.NET Core integration.
---

## Architecture

```
                ASP.NET Core Request
                        │
                        ▼
        AddAspNetCoreInstrumentation()
                        │
                        ▼
       Gateway.OpenTelemetry.AspNetCore
        ├───────────────────────────────┐
        │                               │
        ▼                               ▼
EndpointTraceEnricher        ExceptionTraceEnricher
        │                               │
        └───────────────┬───────────────┘
                        ▼
               ITraceEnricher Pipeline
                        │
                        ▼
          Gateway.OpenTelemetry.Yarp
                        │
                        ▼
               YarpTraceEnricher
                        │
                        ▼
                 Activity.SetTag(...)
                        │
                        ▼
             OpenTelemetry Exporters
```


```
                  +-------------------------+
                  |     Client Request      |
                  +------------+------------+
                               |
                               v
                    ASP.NET Core Pipeline
                               |
                               v
             OpenTelemetry ASP.NET Core Instrumentation
                               |
                               v
        +-----------------------------------------------+
        |        Gateway.OpenTelemetry.AspNetCore        |
        |-----------------------------------------------|
        | EndpointTraceEnricher                         |
        | ExceptionTraceEnricher                        |
        +----------------------+------------------------+
                               |
                               v
                  Gateway.OpenTelemetry.Yarp
                               |
                               v
                      Activity Enrichment
                               |
                               v
                     OpenTelemetry Exporter
                               |
      +-----------+------------+-------------+
      |           |                          |
      v           v                          v
     OTLP      Jaeger                    Zipkin
```

The architecture intentionally separates responsibilities into independent libraries.

- **Core** contains reusable abstractions.
- **AspNetCore** contains HTTP-specific enrichers.
- **Yarp** adds reverse proxy metadata.

This layered design keeps dependencies clean and avoids circular references.

---

## Installation

Install the required packages.

### ASP.NET Core

```bash
dotnet add package Gateway.OpenTelemetry.AspNetCore
```

### YARP

```bash
dotnet add package Gateway.OpenTelemetry.Yarp
```

---

## Requirements

| Component | Version |
|-----------|---------|
| .NET | 10.0 |
| OpenTelemetry | 1.17+ |
| YARP | 2.3+ |

---

## Quick Start

Register OpenTelemetry.

```csharp
builder.Services
    .AddOpenTelemetry()
    .WithTracing(tracing =>
    {
        tracing.AddAspNetCoreInstrumentation();
    });
```

Register Gateway.OpenTelemetry.

```csharp
builder.Services
    .AddGatewayOpenTelemetry();

builder.Services
    .AddGatewayYarpOpenTelemetry();
```

Register YARP.

```csharp
builder.Services
    .AddReverseProxy()
    .LoadFromConfig(
        builder.Configuration.GetSection("ReverseProxy"));
```

Build the application.

```csharp
WebApplication app = builder.Build();

app.MapReverseProxy();

app.Run();
```

That's it.

Every request will automatically be enriched with additional gateway-related tracing information.

## Exporting Traces

Gateway.OpenTelemetry enriches Activities but does not include an exporter.

Choose the exporter that best fits your environment.

### OTLP Exporter

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

### Console Exporter

```csharp
builder.Services
    .AddOpenTelemetry()
    .WithTracing(tracing =>
    {
        tracing
            .AddAspNetCoreInstrumentation()
            .AddConsoleExporter();
    });
```

Any OpenTelemetry-compatible exporter can be used, including:

- OTLP
- Jaeger
- Zipkin
- Azure Monitor
- Grafana Tempo
- Elastic APM
---

## What Gets Added?

Gateway.OpenTelemetry enriches the current Activity with additional tags.

For ASP.NET Core:

- Endpoint display name
- Host information
- Exception type

For YARP:

- Route ID
- Cluster ID
- Destination ID

These tags become available to any OpenTelemetry exporter such as:

- OTLP
- Jaeger
- Zipkin
- Azure Monitor
- Grafana Tempo
- Elastic APM

without requiring additional configuration.

## Built-in Activity Tags

Gateway.OpenTelemetry automatically enriches the current `System.Diagnostics.Activity` with additional metadata.

These tags are added during request processing and become available to any OpenTelemetry exporter.

## ASP.NET Core Tags

| Tag | Description | Example |
|-----|-------------|---------|
| `gateway.host` | Request host name. | `api.example.com` |
| `gateway.endpoint.display_name` | ASP.NET Core endpoint display name. | `GET /orders/{id}` |
| `gateway.exception.type` | Exception type when a request fails. | `System.InvalidOperationException` |

---

## YARP Tags

| Tag | Description | Example |
|-----|-------------|---------|
| `gateway.yarp.route_id` | Matched YARP Route ID. | `api-route` |
| `gateway.yarp.cluster_id` | Selected Cluster ID. | `backend-cluster` |
| `gateway.yarp.destination_id` | Selected Destination ID. | `destination-01` |

---

## Example Activity

```
Activity

Name:
HTTP GET

Tags

http.method = GET

http.route = /api/orders/{id}

gateway.host = api.company.com

gateway.endpoint.display_name = GET /api/orders/{id}

gateway.yarp.route_id = orders

gateway.yarp.cluster_id = orders-cluster

gateway.yarp.destination_id = orders-node-02
```

---
## Example Trace

The following example shows how an enriched Activity may appear after passing through the gateway.

```text
Activity

DisplayName
HTTP GET

Duration
34 ms

Tags

http.method = GET

http.route = /api/orders/{id}

server.address = api.company.com

gateway.host = api.company.com

gateway.endpoint.display_name = GET /api/orders/{id}

gateway.yarp.route_id = orders

gateway.yarp.cluster_id = orders-cluster

gateway.yarp.destination_id = node-02
```

The exact output depends on the configured OpenTelemetry exporter.

---

## ASP.NET Core Integration

Gateway.OpenTelemetry extends the existing ASP.NET Core OpenTelemetry instrumentation.

It does **not** replace the built-in instrumentation.

Instead, it enriches the current Activity with additional gateway-specific metadata.

```
ASP.NET Core Request

        │

        ▼

OpenTelemetry ASP.NET Core Instrumentation

        │

        ▼

Gateway.OpenTelemetry.AspNetCore

        │

        ▼

Additional Activity Tags
```

Because Gateway.OpenTelemetry works on top of the existing instrumentation pipeline, it remains fully compatible with any OpenTelemetry exporter.

No exporter-specific configuration is required.

---

## YARP Integration

Gateway.OpenTelemetry.Yarp enriches requests handled by YARP.

When a request is successfully matched by the reverse proxy, the library automatically records:

- Route ID
- Cluster ID
- Destination ID

Example

```
Incoming Request

        │

        ▼

YARP Route

        │

        ▼

Cluster

        │

        ▼

Destination

        │

        ▼

Activity Tags
```

If the request is **not** processed by YARP, no YARP tags are added.

This behavior avoids polluting Activities with empty or meaningless values.

## Example Configuration

The following example demonstrates a minimal YARP configuration.

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
            "Address": "https://localhost:5001"
          }
        }
      }
    }
  }
}
```

When a request matches this route, Gateway.OpenTelemetry automatically enriches the current Activity with:

- `gateway.yarp.route_id`
- `gateway.yarp.cluster_id`
- `gateway.yarp.destination_id`
---

## Dependency Injection

Gateway.OpenTelemetry integrates with the standard Microsoft dependency injection container.

Registration is straightforward.

```csharp
builder.Services
    .AddGatewayOpenTelemetry();

builder.Services
    .AddGatewayYarpOpenTelemetry();
```

No manual service registration is required.

---

## Design Philosophy

Gateway.OpenTelemetry follows several design principles.

## Small Public API

Only a minimal set of public APIs is exposed.

This keeps the library easy to understand and minimizes future breaking changes.

---

## Layered Architecture

```
Core

▲

AspNetCore

▲

Yarp
```

Each package depends only on the layer below it.

No circular dependencies exist.

---

## OpenTelemetry First

Gateway.OpenTelemetry is **not** an alternative tracing framework.

Instead, it extends the official OpenTelemetry SDK.

This means:

- Existing exporters continue to work.
- Existing instrumentation continues to work.
- Existing Activity pipelines continue to work.

---

## Zero Reflection

The library does not use reflection.

Benefits include:

- Faster startup
- Better Native AOT compatibility
- Lower memory usage
- Easier debugging

---

## Dependency Injection Friendly

All enrichers are resolved through dependency injection.

This allows applications to:

- replace implementations
- register additional enrichers
- customize behavior

without modifying the library itself.

---

## Custom Trace Enrichers

Gateway.OpenTelemetry is designed to be extensible.

Applications can provide their own implementations of `ITraceEnricher`.

Example:

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

Register your enricher.

```csharp
builder.Services.AddSingleton<ITraceEnricher, UserTraceEnricher>();
```

All registered enrichers are executed automatically.

---

## Public API Reference

## Extension Methods

### AddGatewayOpenTelemetry()

Registers ASP.NET Core Activity enrichers.

### AddGatewayYarpOpenTelemetry()

Registers YARP Activity enrichers.

---

## Interfaces

### ITraceEnricher

Represents a component capable of enriching the current Activity.

---

## Constants

### GatewayTagNames

Provides the built-in Activity tag names used throughout the library.
---

## Package Dependencies

```
Gateway.OpenTelemetry.Yarp

        │

        ▼

Gateway.OpenTelemetry.AspNetCore

        │

        ▼

Gateway.OpenTelemetry.Core

        │

        ▼

OpenTelemetry
```

Keeping packages separated allows applications to reference only what they need.

For example:

- ASP.NET Core applications only require `Gateway.OpenTelemetry.AspNetCore`.
- Gateway applications can additionally reference `Gateway.OpenTelemetry.Yarp`.

This reduces unnecessary dependencies.

## Performance

Gateway.OpenTelemetry is designed to add gateway-specific tracing information with minimal overhead.

## Design Goals

- Minimal memory allocations
- No reflection
- No runtime code generation
- Dependency Injection friendly
- Compatible with OpenTelemetry SDK
- Suitable for high-throughput gateway applications

The library enriches the current `Activity` only and does not create additional Activities.

---

## Thread Safety

All built-in enrichers are stateless.

This means:

- No shared mutable state
- Safe for concurrent requests
- Suitable for singleton registration

Applications should follow the same guideline when implementing custom enrichers.

---

## Best Practices

## Register OpenTelemetry First

Always configure OpenTelemetry before registering Gateway.OpenTelemetry.

```csharp
builder.Services
    .AddOpenTelemetry()
    .WithTracing(tracing =>
    {
        tracing.AddAspNetCoreInstrumentation();
    });

builder.Services
    .AddGatewayOpenTelemetry();

builder.Services
    .AddGatewayYarpOpenTelemetry();
```

---

## Register YARP Before Building the Application

```csharp
builder.Services
    .AddReverseProxy()
    .LoadFromConfig(
        builder.Configuration.GetSection("ReverseProxy"));
```

---

## Avoid Manual Activity Creation

Gateway.OpenTelemetry enriches the Activity created by ASP.NET Core instrumentation.

Creating additional Activities for incoming HTTP requests is generally unnecessary.

---

## Keep Custom Enrichers Lightweight

Custom enrichers execute for every request.

Avoid:

- Blocking I/O
- Database queries
- Network calls
- Long-running computations

Instead, only enrich the Activity with information already available in the current request.

---

## Troubleshooting

## No Gateway Tags

Possible causes:

- `AddGatewayOpenTelemetry()` was not registered.
- `AddAspNetCoreInstrumentation()` is missing.
- The request did not reach ASP.NET Core.

---

## No YARP Tags

Possible causes:

- The request was not handled by YARP.
- `AddGatewayYarpOpenTelemetry()` was not registered.
- No matching route was found.

---

## No Exported Traces

Verify that:

- OpenTelemetry is configured correctly.
- An exporter has been registered.
- Sampling is enabled.
- The exporter endpoint is reachable.

---

## Version Compatibility

| Gateway.OpenTelemetry | .NET | OpenTelemetry | YARP |
|-----------------------|------|---------------|------|
| 1.x | .NET 10 | 1.17+ | 2.3+ |

---

## Sample Project

A sample application is included in the repository.

```
samples/
└── OpenTelemetry.Spike
```

The sample demonstrates:

- ASP.NET Core integration
- YARP integration
- OpenTelemetry configuration
- End-to-end request tracing

---

## Building from Source

Clone the repository.

```bash
git clone https://github.com/booviperza/Gateway.OpenTelemetry.git

cd Gateway.OpenTelemetry
```

Build the solution.

```bash
dotnet build
```

---

## Running Tests

Run all tests.

```bash
dotnet test
```

Run a specific project.

```bash
dotnet test tests/Gateway.OpenTelemetry.Core.UnitTests

dotnet test tests/Gateway.OpenTelemetry.AspNetCore.UnitTests

dotnet test tests/Gateway.OpenTelemetry.Yarp.UnitTests

dotnet test tests/Gateway.OpenTelemetry.IntegrationTests
```

---

## Repository Structure

```
Gateway.OpenTelemetry

├── src
│   ├── Gateway.OpenTelemetry.Core
│   ├── Gateway.OpenTelemetry.AspNetCore
│   └── Gateway.OpenTelemetry.Yarp
│
├── tests
│   ├── Gateway.OpenTelemetry.Core.UnitTests
│   ├── Gateway.OpenTelemetry.AspNetCore.UnitTests
│   ├── Gateway.OpenTelemetry.Yarp.UnitTests
│   ├── Gateway.OpenTelemetry.IntegrationTests
│   └── Gateway.OpenTelemetry.IntegrationHost
│
└── samples
    └── OpenTelemetry.Spike
```

---

## Repository Standards

Gateway.OpenTelemetry follows several engineering practices.

- Semantic Versioning
- XML documentation
- SourceLink enabled
- Nullable reference types enabled
- Unit tests
- Integration tests
- Central Package Management
- MIT License
---

## Roadmap

The following improvements are planned for future releases.

## Planned

- Metrics enrichment
- Logging enrichment
- Health Checks integration
- Prometheus helpers
- OTLP configuration helpers
- Benchmark project
- Roslyn analyzers
- Additional built-in enrichers

The roadmap may evolve based on community feedback.

## Version 1.x

- Improved documentation
- Additional built-in enrichers
- More integration samples

## Version 2.x

- Metrics support
- Additional gateway metadata
- Expanded exporter examples
- Optional analyzers

The roadmap may evolve based on community feedback and project priorities.

---

## Contributing

Contributions are welcome.

If you would like to report a bug, request a feature, or submit improvements:

1. Open an issue.
2. Discuss the proposed change.
3. Submit a pull request.

Please ensure:

- The solution builds successfully.
- All tests pass.
- New functionality includes appropriate unit tests.
- Public API changes are documented.

---

## License

This project is licensed under the MIT License.

See the LICENSE file for details.

---

## Support

For questions, bug reports, or feature requests, please use GitHub Issues.

Repository:

https://github.com/booviperza/Gateway.OpenTelemetry

---

## Acknowledgements

Gateway.OpenTelemetry is built on top of the excellent .NET ecosystem, including:

- .NET
- ASP.NET Core
- OpenTelemetry
- YARP (Yet Another Reverse Proxy)

Special thanks to the maintainers and contributors of these open-source projects.

---

## Final Notes

Gateway.OpenTelemetry focuses on one goal:

> Enrich OpenTelemetry Activities with meaningful gateway metadata while keeping the implementation simple, lightweight, and production-ready.

Rather than replacing the OpenTelemetry SDK, Gateway.OpenTelemetry complements it by providing gateway-specific tracing information that is commonly required in API gateway and reverse proxy environments.

---

If Gateway.OpenTelemetry helps your project, consider giving the repository a ⭐ on GitHub.

Feedback, bug reports, feature requests, and pull requests are always welcome.

Thank you for using Gateway.OpenTelemetry.