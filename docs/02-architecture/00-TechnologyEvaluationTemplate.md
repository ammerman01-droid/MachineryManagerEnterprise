# Technology Evaluation Template

| Property | Value |
|----------|-------|
| **Document ID** | TEMPLATE-TE |
| **Version** | 4.0.0 |
| **Status** | Template |
| **Owner** | Solution Architect |
| **Created** | YYYY-MM-DD |
| **Last Updated** | 2026-07-27 |

---

# Purpose

Explain what this Technology Evaluation covers.

This section should state:

- what capability or architectural concern is being addressed;
- why the evaluation is being performed now;
- the expected outcome (a recommended technology stack, not a final ADR).

---

# Evaluation Scope

This Technology Evaluation only evaluates technology selection.

Implementation details are defined by the corresponding Architecture Decision Records (ADRs).

---

# Relationship with Previous Technology Evaluations

If this evaluation extends, complements, or re-evaluates a previously approved
Technology Evaluation and ADR, state that relationship explicitly here.

Examples:

- This evaluation builds on TE-000X / ADR-000X and does not replace it.
- This evaluation re-evaluates TE-000X / ADR-000X. The previously approved
  technology is included as the incumbent candidate.

If there is no relationship with prior evaluations, state:

```text
This evaluation does not supersede or depend on any previously approved
Technology Evaluation.
```

---

# Architectural References

List the approved documents this evaluation is grounded in.

Examples:

- ADR-000X — <Title>
- SolutionStructure.md
- DependencyRules.md
- CodingStandards.md

---

# Scope

Define exactly what is included in this evaluation, and what is explicitly
out of scope.

---

# Functional Requirements

List the capabilities the selected solution shall support.

---

# Non-Functional Requirements

List qualities the solution should provide, for example:

- performance;
- maintainability;
- .NET 10 compatibility;
- cloud neutrality;
- security;
- developer experience.

---

# Candidate Technologies

List every technology considered.

| Technology | Purpose | Status |
|------------|---------|--------|
| Candidate A | | Evaluated |
| Candidate B | | Evaluated |
| Candidate C | | Evaluated |

If this evaluation re-evaluates a previously approved technology, mark it
explicitly as **Incumbent**.

---

# Evaluation Criteria

| ID | Criterion | Weight |
|----|-----------|--------|
| A1 | Standards Compliance | Critical |
| A2 | .NET 10 Compatibility | Critical |
| A3 | Clean Architecture Compatibility | Critical |
| A4 | Developer Experience | High |
| A5 | Performance | High |
| A6 | Maintainability | High |
| A7 | Community & Maturity | High |
| A8 | Security | High |

Adjust criteria to fit the specific technology domain being evaluated.

---

# Architecture Principle

State the guiding architectural principle for this domain, ideally supported
by a short ASCII flow diagram showing how the candidate technologies fit into
the existing Clean Architecture layers.

---

# Candidate Deep-Dive Evaluations

For **every** candidate technology, produce a numbered section following this
exact structure. Numbering continues sequentially starting after the fixed
sections above (Architecture Principle is typically section 4, so the first
candidate deep-dive is section 5).

## N. `<Candidate Name>` Evaluation

### Overview

What the technology is and what problem it solves.

### Architectural Role

Where the technology sits in the layered architecture. Include an ASCII
diagram when it clarifies the data/dependency flow.

### Architectural Strengths

Bullet list of advantages.

### Architectural Weaknesses

Bullet list of disadvantages and limitations.

### Operational Characteristics

How the technology behaves at runtime / in operation.

### Scalability

Assessment of how the technology scales with data volume, load, or team size.

### Reliability

(Include when relevant to the domain, e.g. background processing, messaging.)

### Security

Known security model, considerations, and any risks.

### Standards Compliance

(Include when relevant, e.g. OpenAPI, OAuth2, OpenID Connect.)

### Deployment Flexibility

Cross-platform / cloud-neutral characteristics (Windows, Linux, Containers,
Kubernetes, Cloud, Hybrid, On-Premise).

### AI Compatibility

How well the technology supports or integrates with AI tooling, agents, or
future AI-driven features, where relevant.

### Maintainability

Long-term maintenance cost and complexity.

### Typical Usage / Recommended Usage

Suitable and unsuitable scenarios for this technology.

### Architectural Fit

| Criterion | Assessment |
|-----------|------------|
| ... | ... |

### Relationship with `<Related Candidate or Existing Technology>`

(Include when the candidate composes with, competes with, or depends on
another candidate or an already-approved technology.)

### Comparison with `<Related Candidate>`

(Include when a direct head-to-head comparison clarifies the decision.)

### Preliminary Conclusion

Short conclusion for this specific candidate before the overall comparison.

---

# Overall Technology Comparison

## Responsibility Matrix

| Responsibility | Recommended Technology | Alternative | Purpose |
|-----------------|------------------------|--------------|---------|
| | | | |

## Capability Comparison

| Capability | Candidate A | Candidate B | Candidate C |
|------------|-------------|-------------|-------------|
| | | | |

## Cloud Neutrality Assessment

## Enterprise Suitability

## AI Compatibility

## Clean Architecture Compliance

## Cost Comparison

## Risk Assessment

Examples:

- Vendor lock-in
- Low community activity
- Breaking changes
- Migration complexity

## Overall Evaluation

Summarize the comparison across all candidates before the final
recommendation.

---

# Final Recommendation

## Core Technology Stack

| Responsibility | Selected Technology | Rationale |
|-----------------|----------------------|-----------|
| | | |

## Recommended Architecture

ASCII diagram showing how the selected technologies compose together.

## Build Pipeline Integration

(Include when the technology affects the build/CI pipeline.)

## Security Recommendations

## Cloud Neutrality

## AI Readiness

---

# Final Decision

| Component | Decision |
|-----------|----------|
| | Approved / Rejected |

---

# Decision Summary

Checklist confirming the recommended stack satisfies architectural goals, for
example:

- Clean Architecture
- .NET 10 Compatibility
- Standards Compliance
- Developer Experience
- Cloud Neutrality
- Maintainability
- AI Readiness

State clearly whether this evaluation supersedes a prior ADR, or whether it
reaffirms one.

---

# Related ADR

Reference the Architecture Decision Record that will formalize this
evaluation's outcome.

```
ADR-000X
```

If this evaluation reaffirms an existing ADR rather than requiring a new one,
state that explicitly.

---

# Related Documents

- ADR
- POC (if applicable)
- Dependency Catalog
- Related TEs

---

# References

Official documentation

GitHub repository

Benchmarks

Independent articles

---

# Revision History

| Version | Date | Author | Description |
|----------|------|--------|-------------|
| 1.0.0 | YYYY-MM-DD | Solution Architect | Initial version |
