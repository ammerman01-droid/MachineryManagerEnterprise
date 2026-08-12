| Property         | Value              |
|------------------|--------------------|
| **Document ID**  | DOC-DEV-006        |
| **Title**        | Coding Standards   |
| **Version**      | 4.1.0              |
| **Status**       | Approved           |
| **Owner**        | Solution Architect |
| **Created**      | 2026-07-18         |
| **Last Updated** | 2026-08-08         |

---

# Purpose

This document defines the official coding standards for the
**MachineryManagerEnterprise** solution.

The objective is to ensure that all contributors write code with a consistent
style, predictable structure, and high maintainability.

Coding standards are intended to reduce cognitive load during development,
code reviews, debugging, and long-term maintenance.

---

# General Principles

Every source file should be:

- Readable
- Predictable
- Small
- Focused
- Easy to review
- Easy to test

Code should always optimize for maintainability rather than cleverness.

---

# Code Formatting

The project follows the default formatting rules provided by the .NET SDK.

Formatting shall be enforced through:

- `.editorconfig`
- `dotnet format`

Manual formatting differences should not appear in Pull Requests.

---

# File Organization

A source file should normally contain one public type.

Recommended order:

1. using directives
2. namespace
3. class declaration
4. constants
5. fields
6. constructors
7. public properties
8. public methods
9. private methods

---

# Class Design

Classes should:

- Have a single responsibility.
- Be cohesive.
- Avoid excessive size.
- Prefer composition over inheritance.

Large classes should be refactored.

---

# Methods

Methods should:

- Perform one logical task.
- Have descriptive names.
- Minimize nesting.
- Return early when appropriate.
- Remain short whenever practical.

Methods should not exceed approximately 40 lines unless justified.

---

# Naming

Names should clearly describe intent.

Prefer:

```csharp
CalculateMachineAvailability()
```

Avoid:

```csharp
Calc()
```

---

# Comments

Code should explain *why*, not *what*.

Avoid:

```csharp
// Increment counter
counter++;
```

Prefer:

```csharp
// Retry counter prevents infinite synchronization loops.
counter++;
```

Dead code should never remain commented.

---

# Magic Numbers

Magic numbers are prohibited.

Instead of:

```csharp
if (count > 5)
```

Use:

```csharp
const int MaximumRetries = 5;
```

---

# Null Handling

Nullable Reference Types shall remain enabled.

Null should be handled explicitly.

Avoid suppressing compiler warnings with the null-forgiving operator (`!`) unless absolutely necessary.

---

# Exceptions

Exceptions should:

- Represent exceptional situations.
- Include meaningful messages.
- Preserve inner exceptions when rethrowing.

Never swallow exceptions silently.

---

# Async Programming

Prefer asynchronous APIs.

Guidelines:

- Avoid `.Result`
- Avoid `.Wait()`
- Avoid blocking threads
- Use `CancellationToken` where applicable

---

# Dependency Injection

Never instantiate infrastructure services directly.

Incorrect:

```csharp
var repository = new MachineRepository();
```

Correct:

```csharp
public MachineService(IMachineRepository repository)
```

---

# Logging

Logging should:

- Be structured.
- Avoid sensitive information.
- Use appropriate log levels.

Business logic should not depend on logging implementations.

---

# Testing

Code should be written with testing in mind.

Avoid static state.

Avoid hidden dependencies.

Favor deterministic behavior.

---

# Performance

Optimize only when evidence exists.

Performance optimizations require measurement.

Readability remains the default priority.

---

# Open Source Policy

Only approved open-source libraries may be introduced.

Every new dependency shall have:

- Technology Evaluation (TE)
- Architecture approval (ADR)

See:

- ADR-0002 — Open Source First Policy

---

# Static Analysis

Warnings should be treated as defects.

Recommended tools:

- Roslyn Analyzers
- .NET SDK Analyzers

New warnings should not be introduced.

---

# Code Reviews

Every Pull Request should verify:

- Readability
- Simplicity
- Architecture compliance
- Dependency compliance
- Testability

Consistency is preferred over individual coding style.

---

# Compliance

All contributors shall follow these standards.

Project-wide deviations require an approved Architecture Decision Record.

---

# Decision Summary

- ✔ Clean Architecture
- ✔ .NET 10 Compatibility
- ✔ Standards Compliance
- ✔ Cloud Neutrality
- ✔ AI Readiness
- ✔ Long-term Maintainability

# Related Documents

- DOC-CONVENTIONS
- DOC-README
- DOC-DEV-001 (Development Principles)
- DOC-DEV-005 (Dependency Rules)
- DOC-DEV-007 (Naming Conventions)
- ADR-0002

---

# Revision History

| Version | Date       | Author             | Description                                           |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0   | 2026-07-18 | Solution Architect | Initial coding standards                              |
| 3.0.0   | 2026-07-18 | Solution Architect | Standardized according to Documentation Standard v3.0 |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0             |
| 4.1.0   | 2026-08-08 | Solution Architect | Review and synchronize with the latest changes        |