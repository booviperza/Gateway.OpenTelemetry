# ADR-005 : Gateway Tagging Model

Status

Accepted

---

## Context

Gateway.OpenTelemetry enriches OpenTelemetry telemetry
with Gateway-specific information.

Gateway.OpenTelemetry must never duplicate tags already
defined by OpenTelemetry Semantic Conventions.

---

## Principles

Use OpenTelemetry Semantic Conventions whenever
possible.

Gateway-specific tags are added only when equivalent
standard tags do not exist.

---

## Standard Tags

The following tags are owned by OpenTelemetry.

Examples

http.request.method

http.route

http.response.status_code

server.address

server.port

network.protocol.version

url.path

error.type

Gateway.OpenTelemetry must never redefine these tags.

---

## Gateway Tags

Gateway.OpenTelemetry owns only Gateway-specific tags.

Current tags

gateway.yarp.route_id

gateway.yarp.cluster_id

gateway.yarp.destination_id

Future tags

gateway.tenant.id

gateway.policy.id

gateway.backend.region

gateway.backend.zone

---

## Naming Convention

Gateway tags

gateway.*

YARP tags

gateway.yarp.*

Future integrations

gateway.openiddict.*

gateway.grpc.*

gateway.signalr.*

---

## Source of Truth

YARP Route Id

feature.Route.Config.RouteId

YARP Cluster Id

feature.Cluster.Config.ClusterId

YARP Destination Id

feature.ProxiedDestination?.DestinationId

These APIs were validated against

YARP 2.3.0

during

Spike #2.

---

## Forbidden

Do not create

gateway.route

gateway.endpoint

gateway.status

gateway.method

These values already exist using
OpenTelemetry semantic conventions.

---

## Design Goal

Gateway.OpenTelemetry complements OpenTelemetry.

It never replaces OpenTelemetry semantic conventions.