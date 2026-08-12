| Property         | Value              |
|------------------|--------------------|
| **Document ID**  | ADR-0034           |
| **Title**        | Configuration and Secrets Management Architecture |
| **Version**      | 4.1.0              |
| **Status**       | Approved           |
| **Owner**        | Solution Architect |
| **Created**      | 2026-08-02         |
| **Last Updated** | 2026-08-08         |

---

# Context

TE-0018 — Configuration and Secrets Management Technology Evaluation was
approved but had no corresponding Architecture Decision Record. The
platform requires a consistent, strongly typed approach to configuration
and a clear boundary between non-sensitive defaults and sensitive
secrets across development, on-premises, and Azure deployments.

---

# Decision

MachineryManagerEnterprise adopts the following configuration and
secrets architecture, formalizing TE-0018:

| Responsibility | Selected Technology |
|-----------------|---------------------|
| Configuration Abstraction | Microsoft.Extensions.Configuration |
| Strongly Typed Configuration | Microsoft.Extensions.Options |
| Default Configuration | appsettings.json (non-sensitive only) |
| Deployment Overrides | Environment Variables |
| Development Secrets | .NET User Secrets |
| Enterprise Secret Store | HashiCorp Vault |
| Azure Alternative | Azure Key Vault |
| Feature Flags | Microsoft.FeatureManagement |

Business modules shall never access configuration providers directly.
Only strongly typed Options shall be injected into application services.
JSON configuration files shall contain only non-sensitive defaults.

---

# Decision Drivers

- Clean Architecture (no direct provider access from business modules)
- Cloud Neutrality (Vault as primary, Azure Key Vault as alternative)
- Security (no secrets in source-controlled files)
- Standards Compliance

---

# Alternatives Considered

Refer to TE-0018 for the full candidate comparison across configuration
libraries and secret stores.

---

# Consequences

**Positive**

- Clear separation of non-sensitive configuration from secrets.
- Strongly typed configuration reduces runtime configuration errors.

**Negative / Trade-offs**

- Requires operating HashiCorp Vault (or Azure Key Vault) infrastructure
  in production environments.

---

# Architecture Impact

- Infrastructure layer owns provider registration.
- Application layer consumes only strongly typed Options.

---

# Implementation Notes

- Register Options types via `IOptions<T>` / `IOptionsSnapshot<T>`.
- .NET User Secrets shall be mandatory during local development.

---

# Compliance Rules

```
Business modules shall never access IConfiguration, environment
variables, or a secret store directly. Only strongly typed Options
objects shall be injected.
```

---

# Related Technology Evaluation

```
TE-0018
```

---

# Related Proof of Concept

```
Not Required
```

---

# Decision Summary

- ✔ Clean Architecture
- ✔ .NET 10 Compatibility
- ✔ Standards Compliance
- ✔ Cloud Neutrality
- ✔ Long-term Maintainability

# Related Documents

- ADR-0001 — Adopt Clean Architecture
- ADR-0002 — Adopt Open Source First Policy
- TE-0018 — Configuration and Secrets Management Technology Evaluation

---

# References

- Microsoft.Extensions.Configuration Documentation
- HashiCorp Vault Documentation

---

#  Revision History

| Version | Date       | Author             | Description                                           |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0   | 2026-08-02 | Solution Architect | Initial decision, formalizing previously unratified TE-0018 |
| 4.1.0   | 2026-08-08 | Solution Architect | Review and synchronize with the latest changes |