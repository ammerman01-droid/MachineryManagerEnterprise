| Property | Value |
|----------|-------|
| **Document ID** | APP-007 |
| **Title** | Authorization model |
| **Version** | 4.0.0 |
| **Status** | Active |
| **Owner** | Solution Architect |
| **Created** | 2026-07-18 |
| **Last Updated** | 2026-07-28 |

---

# 1. Purpose

This document defines the authorization model of MachineryManagerEnterprise.

Authorization determines **who is allowed to perform which business operation**.

Authentication identifies the user.

Authorization determines permissions.

---

# Authorization Philosophy

Authorization protects business operations.

Authorization never contains business logic.

Business Rules remain inside the Domain.

Authorization determines whether execution is allowed before the Application
Layer invokes the Handler.

---

# 2. Authorization Principles

The authorization system shall satisfy the following principles.

- Role Based Access Control (RBAC)
- Least Privilege
- Business oriented permissions
- Centralized authorization
- Auditable decisions
- Technology independent

Authorization shall never depend on user interface implementation.

---

# 3. Authorization Model

```text
User

↓

Role

↓

Permission

↓

Business Operation
```

One User may possess multiple Roles.

One Role may contain multiple Permissions.

---

# 4. Permission Categories

Permissions are grouped into business domains.

```text
Permissions

├── Asset
├── Engine
├── Components
├── Meter
├── Maintenance
├── Financial
├── Documents
├── Forecast
├── Reporting
├── Administration
└── Configuration
```

---

# 5. Standard Roles

The following roles are considered part of the core platform.

- System Administrator
- Fleet Manager
- Maintenance Manager
- Maintenance Technician
- Workshop Supervisor
- Operator
- Financial Officer
- Procurement Officer
- Document Controller
- Read-Only Auditor

Organizations may define additional roles.

---

# 6. Permission Naming Convention

Permissions shall follow:

```
<Module>.<Operation>
```

Examples

```
Asset.Create
Asset.Update
Asset.Delete

Engine.Install
Engine.Remove

Maintenance.Create
Maintenance.Complete

Document.Upload
Document.Renew

Forecast.Generate
```

---

# 7. Asset Permissions

Examples

- Asset.View
- Asset.Create
- Asset.Update
- Asset.Transfer
- Asset.Retire
- Asset.Dispose
- Asset.Export

---

# 8. Engine Permissions

Examples

- Engine.View
- Engine.Register
- Engine.Install
- Engine.Remove
- Engine.Replace
- Engine.Rebuild

---

# 9. Maintenance Permissions

Examples

- Maintenance.Plan
- Maintenance.Schedule
- Maintenance.Start
- Maintenance.Complete
- Maintenance.Cancel
- Failure.Register
- Inspection.Register

---

# 10. Financial Permissions

Examples

- Financial.View
- Financial.RecordExpense
- Financial.CalculateDepreciation
- Financial.ViewOwnershipCost

Financial permissions should be granted carefully.

---

# 11. Document Permissions

Examples

- Document.View
- Document.Upload
- Document.Replace
- Document.Archive
- Document.Export

---

# 12. Forecast Permissions

Examples

- Forecast.View
- Forecast.Generate
- Forecast.Compare

Forecast generation may require elevated permissions.

---

# 13. Reporting Permissions

Examples

- Report.View
- Report.Generate
- Report.Export

---

# 14. Administrative Permissions

Examples

- User.Create
- User.Disable
- Role.Assign
- Permission.Assign
- Organization.Manage
- Configuration.Manage

Administrative permissions shall be limited.

---

# 15. Authorization Flow

Typical authorization flow:

```text
Request

↓

Authentication

↓

Resolve User

↓

Resolve Roles

↓

Resolve Permissions

↓

Authorize

↓

Execute Handler
```

Authorization shall occur before business execution.

---

# 16. Authorization Failures

When authorization fails:

- business state shall remain unchanged;
- no Domain Event shall be published;
- the attempt shall be logged.

---

# 17. Audit Requirements

Every authorization-sensitive operation shall record:

- User
- Time
- Operation
- Resource
- Result
- Source

Audit records are immutable.

---

# 18. Future Authorization Features

Future versions may support:

- Resource-based authorization
- Organization-level permissions
- Temporary permissions
- Delegation
- Approval workflows
- Multi-factor authorization for critical operations

---

# 19. Permission Resolution

Authorization shall occur in the following order:

1. Authenticate User
2. Resolve Organization
3. Resolve Roles
4. Resolve Permissions
5. Evaluate Policy
6. Execute Handler

---

| Permission           | Command                    |
| -------------------- | -------------------------- |
| Asset.Create         | RegisterAssetCommand       |
| Engine.Install       | InstallEngineCommand       |
| Maintenance.Complete | CompleteMaintenanceCommand |

---

# Decision Summary

- ✔ Clean Architecture
- ✔ .NET 10 Compatibility
- ✔ Standards Compliance
- ✔ Cloud Neutrality
- ✔ AI Readiness
- ✔ Long-term Maintainability

# Related Documents

- 06-Workflows.md
- 04-Handlers.md
- 02-Commands.md
- 03-Queries.md
- docs/03-domain/07-BusinessRules.md
- ADR-0006 — Authorization Model

---

# Revision History

| Version | Date       | Author             | Description                                           |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0   | 2026-07-18 | Solution Architect | Initial Authorization model                           |
| 3.0.0   | 2026-07-18 | Solution Architect | Standardized according to Documentation Standard v3.0 |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0             |