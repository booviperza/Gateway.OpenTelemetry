# ADR-003 : Dependency Rules

Status

Accepted

---

Gateway.OpenTelemetry.Core

Must never reference

- ASP.NET Core

- YARP

- IServiceCollection

- Microsoft.Extensions.Hosting

---

Gateway.OpenTelemetry.AspNetCore

May reference

- ASP.NET Core

- OpenTelemetry Instrumentation

Must not reference YARP.

---

Gateway.OpenTelemetry.Yarp

May reference

- Yarp.ReverseProxy

Must not reference Hosting.