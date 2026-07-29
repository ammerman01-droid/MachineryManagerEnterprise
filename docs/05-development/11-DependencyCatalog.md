# Dependency Catalog

| Property | Value |
|----------|-------|
| **Document ID** | DOC-DEV-012 |
| **Version** | 2.0.0 |
| **Status** | Approved |
| **Owner** | Solution Architect |
| **Created** | 2026-07-18 |
| **Last Updated** | 2026-07-18 |

---

# Purpose

This document defines the official dependency governance process for the
MachineryManagerEnterprise solution.

It serves as the authoritative register of all third-party libraries adopted by
the project.

Every dependency introduced into the solution shall appear in this catalog.

---

# Objectives

The dependency catalog shall:

- Prevent uncontrolled package growth.
- Record architectural decisions.
- Improve maintainability.
- Simplify upgrades.
- Support security reviews.
- Ensure license compliance.

---

# Open Source First

The solution adopts an **Open Source First** policy.

Only open-source libraries may be introduced unless an approved ADR explicitly
documents an exception.

See:

- ADR-0002 – Open Source First Policy

---

# Dependency Lifecycle

Every dependency follows the same lifecycle.

```text
Need

↓

Technology Evaluation (TE)

↓

Proof of Concept (Optional)

↓

Architecture Decision Record (ADR)

↓

Approved

↓

Directory.Packages.props

↓

Implementation

↓

Maintenance
```

No package may bypass this process.

---

# Central Package Management

All NuGet package versions are managed centrally through Directory.Packages.props.
Project files contain PackageReference elements without Version attributes.

Package versions are managed centrally through Directory.Packages.props.

Project files must not define Version attributes in PackageReference elements.

The single source of truth is:

```text
Directory.Packages.props
```

Project files must never contain package versions.

---

# Dependency Categories

Dependencies are grouped into categories.

Examples:

- Framework
- Validation
- Persistence
- Mapping
- Logging
- Testing
- UI Components
- Utilities

---

# Dependency Register

Each dependency shall be documented using the following structure.

| Package | Category | TE | ADR | Status | Notes |
|----------|----------|----|-----|--------|-------|
| FluentValidation | Validation | TE-0005 | ADR-0007 | Approved | Validation framework |
| *(future packages)* | | | | | |

---

# Status Definitions

| Status | Meaning |
|---------|---------|
| Proposed | Under evaluation |
| Approved | Official dependency |
| Deprecated | Planned for removal |
| Rejected | Not accepted |

---

# Upgrade Policy

Dependencies should be updated regularly.

Before upgrading:

- Review release notes.
- Verify compatibility.
- Execute automated tests.
- Update ADR if architectural behavior changes.

---

# Security

Dependencies should be monitored for:

- Known vulnerabilities
- Unsupported versions
- License changes
- Maintenance status

Critical vulnerabilities require immediate review.

---

# Removal Policy

Unused dependencies shall be removed.

Removal process:

1. Verify no project references remain.
2. Remove from implementation.
3. Remove from Directory.Packages.props.
4. Update this catalog.
5. Close associated maintenance task.

---

# Experimental Libraries

Experimental packages shall never be added directly to production.

They must first pass through:

- Technology Evaluation
- Proof of Concept

---

# Versioning

Stable versions are preferred.

Preview packages require explicit architectural approval.

---

# Compliance

Every third-party dependency introduced into the solution shall be documented in
this catalog.

Undocumented dependencies are not permitted.

---

# Related Documents

- DOC-CONVENTIONS
- DOC-README
- ADR-0002 (Open Source First Policy)
- ADR-0007 (Use FluentValidation)
- TE-0005 (FluentValidation Evaluation)

---

# Change History

| Version | Date | Description |
|----------|------------|----------------------------------------------|
| 1.0.0 | 2026-07-18 | Initial dependency catalog |
| 2.0.0 | 2026-07-18 | Standardized according to Documentation Standard v3.0 |