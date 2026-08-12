| Property         | Value              |
|------------------|--------------------|
| **Document ID**  | DOC-DEV-011        |
| **Title**        | Build Pipeline     |
| **Version**      | 4.1.0              |
| **Status**       | Approved           |
| **Owner**        | Solution Architect |
| **Created**      | 2026-07-18         |
| **Last Updated** | 2026-08-08         |

---

# Purpose

This document defines the Continuous Integration (CI) and Continuous Delivery (CD)
pipeline strategy for the MachineryManagerEnterprise solution.

The goal is to ensure that every code change is validated automatically before
being merged into the main development branches.

---

# Objectives

The pipeline shall:

- Build the solution automatically.
- Execute automated tests.
- Verify architectural rules.
- Detect dependency issues.
- Prevent broken code from being merged.
- Produce reproducible builds.

---

# Approved Technology Stack

Formalized by ADR-0025 (Build and Deployment Architecture):

| Responsibility | Approved Technology |
|-----------------|---------------------|
| CI/CD Platform | GitHub Actions |
| Containerization | Docker |
| Local Multi-Service Orchestration | .NET Aspire |

**Kubernetes is not an approved platform standard.** No ADR currently
authorizes Kubernetes; introducing it requires a new Technology
Evaluation and ADR.

---

# Pipeline Principles

The build pipeline shall be:

- Automated
- Repeatable
- Deterministic
- Fast
- Transparent

Manual build verification should never be required.

---

# Branch Strategy

The pipeline follows the Git branching strategy.

```text
main
    │
develop
    │
feature/*
```

Different branches execute different pipeline stages.

---

# Feature Branch Pipeline

Every feature branch shall execute:

- Restore packages
- Build solution
- Static analysis
- Unit tests
- Architecture tests

No deployment shall occur.

---

# Develop Branch Pipeline

The Develop pipeline additionally performs:

- Integration tests
- Package validation
- Artifact generation

---

# Main Branch Pipeline

The Main pipeline performs:

- Full build
- Full automated test suite
- Release artifact generation
- Version tagging
- Deployment approval (when enabled)

---

# Pipeline Stages

Typical execution order:

```text
Restore

↓

Build

↓

Static Analysis

↓

Architecture Tests

↓

Unit Tests

↓

Integration Tests

↓

Publish Artifacts
```

---

# Build Configuration

Default configuration:

```text
Release
```

Debug builds should only be used during local development.

TargetFramework is defined centrally in Directory.Build.props.

Individual project files must not redefine TargetFramework unless explicitly documented.

---

# Static Analysis

Static analysis shall execute before automated tests.

Recommended tools include:

- .NET SDK Analyzers
- Roslyn Analyzers

Warnings should be treated as defects whenever practical.

---

# Architecture Validation

Architecture validation shall verify:

- Dependency Rules
- Layer Boundaries
- Namespace Rules

Recommended tools:

- NetArchTest
- ArchUnitNET

---

# Test Execution

The pipeline shall execute:

- Unit Tests
- Integration Tests (Develop/Main)
- Architecture Tests

Future additions may include:

- UI Tests
- Performance Tests
- Security Scans

---

# Artifacts

Successful builds should generate reproducible artifacts.

Examples:

- Published application
- Symbols
- Packages

Artifacts should remain immutable.

---

# Versioning

Versioning follows Semantic Versioning.

```text
MAJOR.MINOR.PATCH
```

Example:

```text
1.4.2
```

---

# Security

The pipeline shall never expose:

- Secrets
- API Keys
- Tokens
- Connection Strings

Secrets shall be managed by the CI platform's native secret store during
pipeline execution, and by HashiCorp Vault (or the Azure Key Vault
alternative) at runtime, per ADR-0034 (Configuration and Secrets
Management Architecture).

---

# Future Improvements

Future pipeline enhancements may include:

- SBOM generation
- Dependency vulnerability scanning
- Container image scanning
- Automated deployment
- Blue-Green deployment
- Canary deployment

---

# Compliance

All Pull Requests must pass the required pipeline stages before merge approval.

Direct commits to protected branches are prohibited.

---

# Related Documents

- DOC-CONVENTIONS
- DOC-DEV-005 (Dependency Rules)
- DOC-DEV-006 (Coding Standards)
- DOC-DEV-010 (Testing Strategy)
- ADR-0025 — Build and Deployment Architecture
- ADR-0034 — Configuration and Secrets Management Architecture

---

# Decision Summary

- ✔ Clean Architecture
- ✔ .NET 10 Compatibility
- ✔ Standards Compliance
- ✔ Cloud Neutrality
- ✔ AI Readiness
- ✔ Long-term Maintainability

---

# Revision History

| Version | Date       | Author             | Description                                           |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0   | 2026-07-18 | Solution Architect | Initial build pipeline strategy                       |
| 3.0.0   | 2026-07-18 | Solution Architect | Standardized according to Documentation Standard v3.0 |
| 3.1.0   | 2026-07-26 | Solution Architect | Updated project structure documentation to reflect centralized build configuration and package management introduced during Bootstrap |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0             |
| 4.1.0   | 2026-08-08 | Solution Architect | Synced with approved stack (ADR-0025/ADR-0034): named GitHub Actions, Docker, .NET Aspire, HashiCorp Vault; explicitly noted Kubernetes is not approved |