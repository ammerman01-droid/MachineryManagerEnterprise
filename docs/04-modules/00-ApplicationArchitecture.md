# Application Architecture

**Document ID:** MME-APP-000

**Repository Path:** `docs/04-application/00-ApplicationArchitecture.md`

**Version:** 1.0.0 (Draft)

**Status:** In Progress

**Related Documents**

- docs/03-domain/03-DomainModel.md
- docs/03-domain/04-Aggregates.md
- docs/03-domain/05-DomainServices.md
- docs/03-domain/06-DomainEvents.md
- docs/03-domain/07-BusinessRules.md

---

# 1. Purpose

This document defines the responsibilities of the Application Layer.

The Application Layer coordinates business use cases without containing business rules.

It acts as the orchestration layer between:

- User Interface
- Domain Layer
- Infrastructure Layer

---

# 2. Responsibilities

The Application Layer is responsible for:

- Executing Use Cases
- Coordinating Aggregates
- Managing Transactions
- Calling Domain Services
- Publishing Domain Events
- Calling Infrastructure Services
- Authorization
- Validation of Application Requests

The Application Layer shall never contain business rules.

Business rules belong exclusively to the Domain Layer.

---

# 3. Architectural Position

```text
Presentation Layer

        │

        ▼

Application Layer

        │

        ▼

Domain Layer

        │

        ▼

Infrastructure Layer
```

---

# 4. Core Principles

The Application Layer follows these principles.

- Thin orchestration layer
- No business logic
- Technology independent
- Use Case oriented
- Transaction boundary owner
- Dependency inversion

---

# 5. Main Building Blocks

The Application Layer consists of:

```text
Application

├── Use Cases
├── Commands
├── Queries
├── Command Handlers
├── Query Handlers
├── Application Services
├── Workflow Coordinators
└── Authorization Policies
```

Each block has a single responsibility.

---

# 6. Application Flow

A typical request follows this sequence.

```text
User

↓

Controller

↓

Application Command

↓

Command Handler

↓

Domain Aggregate

↓

Domain Events

↓

Infrastructure

↓

Response
```

No controller shall communicate directly with the Domain Layer.

---

# 7. Transaction Boundary

The Application Layer owns transaction boundaries.

One Use Case normally executes inside one transaction.

If multiple Aggregates participate, consistency shall be managed according to Domain rules.

---

# 8. Error Handling

The Application Layer is responsible for:

- translating Domain Exceptions
- returning application results
- logging execution failures
- preserving transaction integrity

Business validation errors shall never be converted into Infrastructure errors.

---

# 9. Future Documents

The Application Layer is further specified by:

- 01-UseCases.md
- 02-Commands.md
- 03-Queries.md
- 04-Handlers.md
- 05-ApplicationServices.md
- 06-Workflows.md
- 07-Authorization.md

---

# Revision History

| Version | Description |
|----------|-------------|
| 1.0.0 | Initial Application Architecture |