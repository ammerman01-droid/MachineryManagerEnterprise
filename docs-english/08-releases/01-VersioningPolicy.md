| Property | Value |
|----------|-------|
| **Document ID** | REL-001 |
| **Title** | Versioning Policy |
| **Version** | 4.0.0 |
| **Status** | Active |
| **Owner** | Release Manager |
| **Created** | 2026-07-18 |
| **Last Updated** | 2026-07-28 |

---

# 1. Purpose

This document defines the versioning policy for MachineryManagerEnterprise.

The versioning policy provides a consistent approach for identifying software releases, API compatibility and deployment artifacts.

---

# Versioning Philosophy

Version numbers communicate compatibility.

They do not describe implementation complexity.

Every released artifact shall expose a deterministic and reproducible version.

---

# 2. Objectives

The versioning strategy shall provide:

- Predictable evolution
- Traceability
- Release consistency
- Client compatibility
- Deployment transparency

---

# 3. Semantic Versioning

The solution follows Semantic Versioning.

```
MAJOR.MINOR.PATCH
```

Example

```
1.0.0

1.3.0

1.3.5

2.0.0
```

---

# 3. Version Hierarchy

Application Version

↓

Artifact Version

↓

Build Number

↓

Commit SHA

---

# 4. Major Version

A Major version changes when:

- Breaking API changes occur.
- Architectural redesign occurs.
- Domain contracts become incompatible.
- Existing integrations require modification.

Example

```
1.x.x

↓

2.0.0
```

---

# 5. Minor Version

A Minor version changes when:

- New business capabilities are added.
- New modules are introduced.
- New endpoints are added.
- Existing functionality remains compatible.

Example

```
1.2.0

↓

1.3.0
```

---

# 6. Patch Version

Patch versions include:

- Bug fixes
- Security fixes
- Performance improvements
- Documentation corrections

No business behavior shall intentionally change.

Example

```
1.3.2

↓

1.3.3
```

---

# 7. Build Identification

Every build shall have:

- Build Number
- Commit SHA
- Build Timestamp

Example

```
Version

1.3.0

Build

20260718.45

Commit

8c93bfa
```

---

# 8. Git Tags

Every production release shall receive a Git tag.

Example

```
v1.0.0

v1.4.0

v2.0.0
```

Tags shall reference immutable commits.

---

# 9. Artifact Version

Published artifacts shall include the software version.

Examples

```
Docker Image

1.3.0

NuGet Package

1.3.0

Application Package

1.3.0
```

Artifact versions shall match the corresponding Git tag.

---

# 10. Documentation Version

Released documentation shall correspond to the released software version.

Documentation updates shall be traceable to the same release.

---

# 11. API Version

API versioning is independent from application versioning.
Changes to internal application components shall never require a public API version change unless the external contract changes.

Example

```
Application

2.3.1

API

v1
```

An application may evolve without changing the public API version.

---

# 12. Database Version

Database schema evolution shall be managed through migrations.

Migration history shall remain synchronized with released application versions.

Database version numbers shall not replace application version numbers.

---

# 13. Compatibility Policy

Within a Major version:

- Minor upgrades should remain compatible.
- Patch upgrades shall remain compatible.

Breaking compatibility requires a new Major version.

---

# 14. Pre-release Versions

Future versions may use pre-release identifiers.

Examples

```
2.0.0-alpha

2.0.0-beta

2.0.0-rc1
```

Pre-release versions shall not be considered production releases.

---

# 15. Future Enhancements

Future versions may introduce:

- Long-Term Support (LTS)
- Monthly Release Cadence
- Automated Release Numbering
- Signed Release Artifacts
- Release Channels

---

# Version Change Matrix

| Change                   | Version |
| ------------------------ | ------- |
| Breaking API             | Major   |
| Breaking Domain Contract | Major   |
| New Feature              | Minor   |
| New Endpoint             | Minor   |
| Bug Fix                  | Patch   |
| Documentation            | Patch   |
| Security Fix             | Patch   |

---

# Decision Summary

- ✔ Clean Architecture
- ✔ .NET 10 Compatibility
- ✔ Standards Compliance
- ✔ Cloud Neutrality
- ✔ AI Readiness
- ✔ Long-term Maintainability

# Related Documents

- 00-ReleaseStrategy.md
- docs/07-api/05-ApiVersioning.md
- docs/05-development/10-BuildPipeline.md
- ADR-0008 — Versioning Policy

---

#  Revision History

| Version | Date       | Author             | Description                                           |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0   | 2026-07-18 | Solution Architect | Initial Versioning Policy                             |
| 3.0.0   | 2026-07-18 | Solution Architect | Standardized according to Documentation Standard v3.0 |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0             |