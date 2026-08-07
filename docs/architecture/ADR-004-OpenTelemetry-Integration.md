# ADR-004 : OpenTelemetry Integration

Status

Accepted

---

## Context

Gateway.OpenTelemetry extends the telemetry produced by
OpenTelemetry.

It does not replace OpenTelemetry SDK,
Instrumentation, or Exporters.

Gateway.OpenTelemetry integrates only through official
OpenTelemetry extension points.

---

## Principles

Gateway.OpenTelemetry never creates Activities.

Gateway.OpenTelemetry never creates Meters.

Gateway.OpenTelemetry never creates Exporters.

Gateway.OpenTelemetry enriches telemetry that already
exists.

---

## Tracing Integration

Tracing integrates with

OpenTelemetry.Instrumentation.AspNetCore

using

AspNetCoreTraceInstrumentationOptions

The following callbacks are available.

- EnrichWithHttpRequest

- EnrichWithHttpResponse

- EnrichWithException

---

## Request Callback

EnrichWithHttpRequest executes before endpoint routing
has completed.

Validated during Spike #1.

At this stage

HttpContext.GetEndpoint()

returns null.

RouteValues are also unavailable.

Suitable for

- Host

- Request headers

- URL

- Query string

Not suitable for

- Endpoint metadata

- Route metadata

- YARP metadata

---

## Response Callback

EnrichWithHttpResponse executes after endpoint routing.

Validated during Spike #1.

At this stage

HttpContext.GetEndpoint()

returns the selected endpoint.

For YARP requests,

IReverseProxyFeature

is available.

Suitable for

- Endpoint metadata

- Route metadata

- Response information

- YARP metadata

---

## Exception Callback

EnrichWithException executes when an exception is
recorded by OpenTelemetry.

Suitable for

- Exception enrichment

- Custom exception tags

---

## Metrics Integration

Metrics integrate using the ASP.NET Core metrics
pipeline.

Gateway.OpenTelemetry uses

IHttpMetricsTagsFeature

to append metric tags.

No custom metrics are created.

---

## Forbidden

Gateway.OpenTelemetry must never use

- ActivityListener

- ActivityProcessor

- Custom Middleware

- Custom ActivitySource

- Custom Meter

for ASP.NET Core request enrichment.

---

## Validation

Validated using

OpenTelemetry 1.17.0

during

Spike #1.