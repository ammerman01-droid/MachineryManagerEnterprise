| Property         | Value              |
|------------------|--------------------|
| **Document ID**  | API-007            |
| **Title**        | Authentication and Authorization |
| **Version**      | 4.2.0              |
| **Status**       | Approved           |
| **Owner**        | Solution Architect |
| **Created**      | 2026-07-18         |
| **Last Updated** | 2026-08-08         |

---

# 1. Purpose

This document defines the authentication and authorization strategy for MachineryManagerEnterprise APIs.

Authentication verifies identity.

Authorization determines whether an authenticated identity may perform a requested operation.

---

# Security Philosophy

Authentication establishes identity.

Authorization grants permissions.

Business Rules remain inside the Domain.

Security mechanisms shall never implement business behavior.

---

# Authentication Flow

Client

↓

Bearer Token

↓

Authentication

↓

Claims

↓

Authorization

↓

Handler

↓

Response

---

# 2. Principles

The security model shall be:

- Stateless
- Secure
- Auditable
- Scalable
- Role-based
- Claims-based

Authentication shall always occur before authorization.

---

# 3. Authentication Model

The API authenticates every protected request.

Anonymous access is permitted only for explicitly designated endpoints.

All authenticated requests shall produce an authenticated user context.

---

# 4. Authentication Mechanism

The primary authentication mechanism is:

```
Bearer Token
```

Clients shall include the access token in the HTTP Authorization header.

Example

```
Authorization: Bearer <access-token>
```

---

# 5. Transport Security

All API communication shall occur over HTTPS.

Plain HTTP shall not be supported in production environments.

---

# 6. Authorization Model

Authorization is based on business permissions.

The authorization flow is:

```text
User

↓

Identity

↓

Roles

↓

Permissions

↓

Business Operation
```

Authorization rules are defined in:

```
docs/04-modules/07-Authorization.md
```

---

# 7. Endpoint Protection

Endpoints shall be classified as:

```text
Public

Authenticated

Privileged
```

Examples

Public

```
Health Check
```

Authenticated

```
GET /assets
```

Privileged

```
DELETE /documents/{id}
```

---

# 8. Claims

Authenticated identities may contain claims such as:

```
UserId

OrganizationId

Roles

Permissions
```

Business logic shall depend only on the Application User Context, never on raw JWT claims.

---

# 9. Multi-Tenant Context

The platform is multi-tenant: multiple Organizations (customer
companies) use the platform concurrently from a single web deployment
at one address (see `00-Glossary.md`, GL-ORG-001). This is not an
optional or conditionally enabled feature.

Every authenticated request shall execute within exactly one
Organization (tenant) context, resolved from `OrganizationId`.

Cross-tenant access is prohibited unless explicitly authorized.

> **Note:** Multi-tenancy is confirmed platform architecture, not an
> optional feature. `OrganizationId` is the tenant identifier.

---

# 10. Unauthorized Requests

Missing authentication shall return:

```
401 Unauthorized
```

Example

```json
{
  "errorCode": "AUTH-001",
  "title": "Authentication Failed",
  "message": "Authentication is required.",
  "correlationId": "..."
}
```

---

# 11. Forbidden Requests

Authenticated users lacking permission shall receive:

```
403 Forbidden
```

Example

```json
{
  "errorCode": "AUTH-003",
  "title": "Access Denied",
  "message": "You do not have permission to perform this operation.",
  "correlationId": "..."
}
```

---

# 12. Token Lifetime

Access tokens shall have a configurable lifetime.

Expired tokens shall not be accepted.

Clients shall obtain new tokens using the configured authentication workflow.

---

# 13. Logging

The following events shall be logged:

- Successful authentication
- Failed authentication
- Authorization failure
- Permission changes
- User lockout
- Administrative access

Sensitive credentials shall never be logged.

---

# 14. Least Privilege

Users shall receive only the permissions required to perform their responsibilities.

Administrative permissions shall be granted sparingly.

---

# 15. Future Enhancements

Future versions may support:

- OpenID Connect
- OAuth 2.1
- Multi-Factor Authentication (MFA)
- Single Sign-On (SSO)
- External Identity Providers
- API Keys for system integrations

These additions shall remain compatible with the existing authorization model.

---

# 16. Endpoint Protection Matrix

| Endpoint Type  | Authentication |     Authorization    |
| -------------- | :------------: | :------------------: |
| Public         |        ❌       |           ❌          |
| Authenticated  |        ✅       |           ❌          |
| Protected      |        ✅       |           ✅          |
| Administrative |        ✅       | ✅ (Admin Permission) |

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
- 04-ErrorResponses.md
- 06-Versioning.md
- docs/04-modules/07-Authorization.md
- ADR-0030 — Identity and Access Management Architecture
- ADR-0026 — Enterprise Security Strategy (Data Protection & Encryption)

---

#  Revision History

| Version | Date       | Author             | Description                                           |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0   | 2026-07-18 | Solution Architect | Initial Authentication strategy                       |
| 3.0.0   | 2026-07-18 | Solution Architect | Standardized according to Documentation Standard v3.0 |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0             |
| 4.1.0   | 2026-08-02 | Solution Architect | Corrected reference from ADR-0026 (Data Protection & Encryption, unrelated) to ADR-0030 (Identity and Access Management Architecture) as the primary governing ADR |
| 4.2.0   | 2026-08-08 | Solution Architect | Clarified multi-tenancy as confirmed, mandatory platform architecture (was described as conditional); removed redundant TenantId claim, since OrganizationId is the tenant identifier |