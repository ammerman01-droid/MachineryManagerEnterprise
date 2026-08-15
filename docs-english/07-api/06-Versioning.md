| Property         | Value              |
|------------------|--------------------|
| **Document ID**  | API-006            |
| **Title**        | API Versioning     |
| **Version**      | 4.1.0              |
| **Status**       | Approved           |
| **Owner**        | Solution Architect |
| **Created**      | 2026-07-18         |
| **Last Updated** | 2026-08-08         |

---

# 1. Purpose

This document defines the API versioning strategy for MachineryManagerEnterprise.

API versioning enables the system to evolve without breaking existing client applications.

---

# Versioning Philosophy

API evolution shall prioritize backward compatibility.

Clients shall migrate gradually.

Breaking changes shall always require a new major API version.

Public contracts evolve more slowly than internal implementations.

---

# Version Lifecycle

New Version

↓

Preview (optional)

↓

Supported

↓

Deprecated

↓

Sunset

↓

Retired

---

# 2. Objectives

The versioning strategy shall provide:

- Backward compatibility
- Predictable upgrades
- Stable integrations
- Controlled evolution

---

# 3. Versioning Principle

Breaking changes shall never be introduced into an existing API version.

Breaking changes require a new major API version.

---

# 4. URL Versioning

MachineryManagerEnterprise uses URL-based versioning.

Example

```
/api/v1/assets

/api/v1/engines

/api/v1/documents
```

Future versions

```
/api/v2/assets

/api/v2/engines
```

---

# 5. Version Format

API versions follow:

```
v1

v2

v3
```

Only the major version appears in the public URL.

---

# 6. Supported Versions

Multiple API versions may coexist.

Example

```
v1

v2
```

Each supported version remains independently maintained during its support lifecycle.

---

# 7. Breaking Changes

Examples of breaking changes

- Removing endpoints
- Renaming endpoints
- Removing response properties
- Changing property meaning
- Changing resource identifiers
- Changing HTTP status behavior

Breaking changes require a new API version.

---

# 8. Non-Breaking Changes

Examples

- Adding optional response properties
- Adding new endpoints
- Adding optional request fields
- Improving performance
- Internal implementation changes

These changes may be released within the current version.

---

# 9. Deprecation

Deprecated endpoints shall remain functional during the announced support period.

Documentation shall clearly indicate:

- Deprecated status
- Recommended replacement
- Planned removal version

---

# 10. Default Version

Clients shall explicitly request an API version.

The server shall not silently redirect requests between versions.

---

# 11. Documentation

Each API version shall maintain independent documentation.

Examples

```
OpenAPI v1

OpenAPI v2
```

Documentation shall reflect the exact behavior of the corresponding version.

---

# 12. Compatibility Policy

Existing client applications should continue working without modification throughout the supported lifetime of an API version.

Backward compatibility is preferred over rapid change.

---

# 13. Sunset Policy

When an API version reaches end of life:

- Clients shall receive advance notice.
- Documentation shall identify the sunset date.
- Migration guidance shall be provided.

---

# 14. Internal Versioning

Internal implementation versions are independent from public API versions.

Changing internal architecture does not require an API version increment.

---

# 15. Future Strategy

Future versions may support:

- Media Type Versioning
- Header Versioning
- Feature Flags
- Experimental Endpoints

The primary strategy remains URL versioning.

---

# Version Compatibility Matrix

| Client    |  v1 |  v2 |
| --------- | :-: | :-: |
| v1 Client |  ✅  |  ❌  |
| v2 Client |  ❌  |  ✅  |

---

# Decision Summary

- ✔ Clean Architecture
- ✔ .NET 10 Compatibility
- ✔ Standards Compliance
- ✔ Cloud Neutrality
- ✔ AI Readiness
- ✔ Long-term Maintainability

# Related Documents

- 00-ApiPrinciples.md
- 01-RestConventions.md
- 02-EndpointDesign.md
- 03-RequestResponseModel.md
- 04-ErrorResponses.md
- 05-PaginationFilteringSorting.md
- ADR-0035 — API Documentation and Client Generation Architecture

---

#  Revision History

| Version | Date       | Author             | Description                                           |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0   | 2026-07-18 | Solution Architect | Initial API Versioning strategy                       |
| 3.0.0   | 2026-07-18 | Solution Architect | Standardized according to Documentation Standard v3.0 |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0             |
| 4.1.0   | 2026-08-08 | Solution Architect | Corrected reference from non-existent "ADR-0005 — API Strategy" to the actual governing ADR-0035 (API Documentation and Client Generation Architecture) |