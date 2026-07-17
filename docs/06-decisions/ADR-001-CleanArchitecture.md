# ADR-001

## Title

Use Clean Architecture

---

## Status

Accepted

---

## Context

The project is expected to evolve for many years and support multiple companies, projects, users, workflows, and business modules.

Maintainability and testability are primary goals.

---

## Decision

The application will follow Clean Architecture.

Dependencies always point inward.

Domain layer never depends on Infrastructure.

Application layer contains use cases.

Infrastructure contains external implementations.

UI only communicates with the Application layer.

---

## Consequences

Advantages

- Testable
- Maintainable
- Scalable
- Independent of UI
- Independent of Database

Disadvantages

- More initial complexity

---

Decision Date

2026