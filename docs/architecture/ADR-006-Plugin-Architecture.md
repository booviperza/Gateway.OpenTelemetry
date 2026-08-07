# ADR-006 : Plugin Architecture

Status

Accepted

---

## Context

Gateway.OpenTelemetry is designed to support multiple
Gateway technologies.

Examples

- YARP

- OpenIddict

- gRPC

- SignalR

Each integration is implemented as an independent
package.

---

## Architecture

Gateway.OpenTelemetry.Core

↓

Gateway.OpenTelemetry.AspNetCore

↓

Gateway.OpenTelemetry.Yarp

Future packages

↓

Gateway.OpenTelemetry.OpenIddict

Gateway.OpenTelemetry.Grpc

Gateway.OpenTelemetry.SignalR

No reverse dependency is allowed.

---

## Responsibilities

Core

Provides

- Constants

- Options

- Shared utilities

Core has no dependency on

- ASP.NET Core

- YARP

- Hosting

---

AspNetCore

Provides

ASP.NET Core integration with
OpenTelemetry.

Responsible for

- Trace callback integration

- Metric callback integration

- Registration

AspNetCore does not contain
Gateway-specific business logic.

---

Yarp

Provides YARP-specific enrichment.

Examples

RouteId

ClusterId

DestinationId

YARP enrichers execute after
route selection,

when

IReverseProxyFeature

is available.

Validated during

Spike #2.

---

## Plugin Rules

Each plugin

- owns its own enrichment logic

- owns its own package

- does not modify other plugins

Plugins communicate only through
OpenTelemetry extension points.

---

## Dependency Rules

Allowed

Core

↑

AspNetCore

↑

Yarp

Forbidden

Core

↓

AspNetCore

AspNetCore

↓

Yarp

Reverse dependencies

---

## Design Goals

Independent packages

Minimal dependencies

OpenTelemetry-native

Easy to extend

No duplicated telemetry generation