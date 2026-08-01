| Property | Value |
|----------|-------|
| **Document ID** | ADR-XXXX |
| **Title** | Architecture Decision Record Template |
| **Version** | 4.0.0 |
| **Status** | Proposed / Accepted / Superseded / Deprecated |
| **Owner** | Solution Architect |
| **Created** | YYYY-MM-DD |
| **Last Updated** | 2026-07-28 |

---

# Title

Provide a concise title describing the architectural decision.

Example

```
Use Entity Framework Core
```

---

# Status

Possible values

- Proposed
- Accepted
- Deprecated
- Superseded

---

# Context

Describe the architectural problem.

Explain:

- Business context
- Technical context
- Constraints
- Existing situation

The reader should understand **why** a decision was required.

---

# Decision

Describe the selected solution.

This section should contain only the decision itself.

Avoid explaining alternatives here.

---

# Decision Drivers

List the primary factors that influenced the decision.

Examples

- Maintainability
- Performance
- Simplicity
- Open Source Policy
- Security
- Community
- Long-Term Support

---

# Alternatives Considered

List all significant alternatives.

Example

- Option A
- Option B
- Option C

Each alternative should be briefly described.

---

# Consequences

Describe the consequences of adopting this decision.

Include:

Positive consequences

Negative consequences

Trade-offs

Future limitations

---

# Architecture Impact

Describe which architectural layers are affected.

Example

- Presentation
- Application
- Infrastructure
- Domain

Also document any dependency implications.

---

# Implementation Notes

Provide implementation guidance.

Examples

- Registration
- Configuration
- Coding rules
- Migration notes

---

# Compliance Rules

Define mandatory rules that developers must follow.

Example

```
Entity Framework Core shall exist only inside Infrastructure.
```

---

# Related Technology Evaluation

Reference the corresponding Technology Evaluation.

Example

```
TE-0004
```

---

# Related Proof of Concept

Reference any Proof of Concept if applicable.

Example

```
POC-0001
```

If no POC exists, explicitly state:

```
Not Required
```

---


---

# Decision Summary

- ✔ Clean Architecture
- ✔ .NET 10 Compatibility
- ✔ Standards Compliance
- ✔ Cloud Neutrality
- ✔ AI Readiness
- ✔ Long-term Maintainability

# Related Documents

Examples

- Dependency Catalog
- Development Principles
- Coding Standards

---

# References

Official documentation

GitHub repository

External standards

---

#  Revision History

| Version | Date       | Author             | Description                                           |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0   | YYYY-MM-DD | Solution Architect | Initial decision                                      |
| 3.0.0   | 2026-07-18 | Solution Architect | Standardized according to Documentation Standard v3.0 |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0             |