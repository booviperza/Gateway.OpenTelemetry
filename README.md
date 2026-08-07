# Gateway.OpenTelemetry

Lightweight OpenTelemetry extensions for ASP.NET Core and YARP.

---

## Features

- Automatic ASP.NET Core Activity enrichment
- YARP route / cluster / destination tags
- Small dependency footprint
- Production-ready
- Unit tested
- Integration tested

---

## Packages

| Package | Description |
|---------|-------------|
| Gateway.OpenTelemetry.Core | Shared abstractions and constants |
| Gateway.OpenTelemetry.AspNetCore | ASP.NET Core enrichment |
| Gateway.OpenTelemetry.Yarp | YARP enrichment |

---

## Installation

```bash
dotnet add package Gateway.OpenTelemetry.AspNetCore
dotnet add package Gateway.OpenTelemetry.Yarp


##. Quick Start
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