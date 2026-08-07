# ADR-002 : Solution Structure

Status

Accepted

---

## Projects

Gateway.OpenTelemetry.Core

Shared contracts and constants.

No ASP.NET Core dependency.

---

Gateway.OpenTelemetry.AspNetCore

ASP.NET Core integration.

Contains ASP.NET specific enrichers.

---

Gateway.OpenTelemetry.Yarp

YARP specific enrichers.

Depends on AspNetCore.

---

## Dependency Rules

Core

↑

AspNetCore

↑

Yarp

No reverse dependency is allowed.