# ADR-001 : Project Vision

Status

Accepted

---

## Context

Gateway.OpenTelemetry is an extension library for OpenTelemetry.

It is NOT

- a metrics framework
- a tracing framework
- a logging framework

OpenTelemetry already provides these capabilities.

The responsibility of Gateway.OpenTelemetry is to provide
Gateway-specific context.

---

## Goals

Gateway.OpenTelemetry enriches OpenTelemetry telemetry with
Gateway-specific information.

Examples

- gateway.route
- gateway.cluster
- gateway.destination
- gateway.proxy

---

## Non Goals

Gateway.OpenTelemetry will never

- create custom counters
- create custom histograms
- replace OpenTelemetry Instrumentation
- replace OpenTelemetry SDK

---

## Design Principles

- OpenTelemetry Native

- Vendor Neutral

- Zero Custom Metrics

- Zero Custom Tracing

- Plugin Architecture

- ASP.NET Core First

---

## Architecture

Application

↓

OpenTelemetry Instrumentation

↓

Gateway.OpenTelemetry

↓

Exporter

Gateway.OpenTelemetry only enriches telemetry.

It never owns telemetry generation.