# ADR-000 : Engineering Rules

Status

Accepted

---

## Purpose

This document defines the engineering rules for
Gateway.OpenTelemetry.

Every contributor must follow these rules.

---

## Rule 1

Every commit must build successfully.

A project that does not build must never be committed.

---

## Rule 2

Public APIs are frozen after ADR approval.

Changing a public API requires a new ADR.

---

## Rule 3

No reverse dependency is allowed.

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

---

## Rule 4

No implementation before architecture approval.

Implementation starts only after the related ADR
is accepted.

---

## Rule 5

Every feature must have

- sample

or

- unit tests

before merge.

---

## Rule 6

Framework projects must not depend on exporters.

Applications own exporters.

---

## Rule 7

Framework projects must not create telemetry.

Framework projects only enrich telemetry.