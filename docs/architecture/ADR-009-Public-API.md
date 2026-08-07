# ADR-009 : Public API Design

Status

Accepted

---

## Context

Gateway.OpenTelemetry is an extension of OpenTelemetry.

It does not replace the OpenTelemetry SDK or create a
parallel configuration model.

The public API must follow the conventions already used
by OpenTelemetry.

---

## Principles

Gateway.OpenTelemetry extends OpenTelemetry.

It does not introduce a custom builder.

It does not introduce a custom registration pipeline.

Users should continue configuring telemetry through the
OpenTelemetryBuilder provided by OpenTelemetry.

---

## Registration

Applications first register OpenTelemetry.

Example

builder.Services
    .AddOpenTelemetry();

Gateway.OpenTelemetry extends that registration.

Example

builder.Services
    .AddOpenTelemetry()
    .AddGateway();

Gateway-specific integrations are added afterwards.

Example

builder.Services
    .AddOpenTelemetry()
    .AddGateway()
    .UseYarp();

---

## Design Rules

Gateway.OpenTelemetry must never replace
OpenTelemetryBuilder.

Gateway.OpenTelemetry must never require a custom
builder object.

Every extension returns the same
OpenTelemetryBuilder instance to support fluent
configuration.

---

## Package Responsibilities

Gateway.OpenTelemetry.AspNetCore

Provides

AddGateway()

Gateway.OpenTelemetry.Yarp

Provides

UseYarp()

Future packages

UseOpenIddict()

UseGrpc()

UseSignalR()

---

## Dependency Rules

Extensions must only depend on

OpenTelemetryBuilder

No extension may depend on another plugin.

Gateway.OpenTelemetry.Yarp depends on
Gateway.OpenTelemetry.AspNetCore only through
public extension points.

---

## Example

builder.Services
    .AddOpenTelemetry()
    .AddGateway()
    .UseYarp();

---

## Goals

- OpenTelemetry-first
- Fluent API
- Minimal API surface
- No duplicate builder
- Easy plugin extensibility