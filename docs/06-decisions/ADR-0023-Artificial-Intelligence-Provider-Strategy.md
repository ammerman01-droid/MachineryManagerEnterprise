# Artificial Intelligence Provider Strategy

| Property | Value |
|----------|-------|
| **Document ID** | ADR-0023 |
| **Version** | 1.1.0 |
| **Status** | Accepted |
| **Owner** | Solution Architect |
| **Created** | 2026-07-28 |
| **Last Updated** | 2026-07-28 |

---

# Context

MachineryManagerEnterprise introduces enterprise Artificial Intelligence capabilities including:

- Semantic Search
- Enterprise AI Assistant
- Retrieval-Augmented Generation (RAG)
- Intelligent Maintenance Recommendations
- AI Copilot
- Knowledge Retrieval

TE-0029 concluded that no single AI provider should become a permanent architectural dependency.

Instead, the platform shall support multiple providers while selecting one provider as the operational default.

---

# Problem

The platform must avoid coupling business logic to a specific AI provider.

Without an abstraction:

- every provider change affects application code;
- cloud vendor lock-in increases;
- testing becomes difficult;
- local inference cannot be introduced without refactoring.

---

# Decision Drivers

The architecture shall satisfy:

- Clean Architecture
- Dependency Inversion
- Provider Independence
- Enterprise Security
- Hybrid Deployment
- Long-Term Maintainability
- AI Extensibility

---

# Decision

The platform adopts a **Hybrid AI Provider Strategy**.

The application communicates only with an internal abstraction.

Concrete AI providers are implemented entirely inside the Infrastructure layer.

---

# Approved Provider Hierarchy

```text
Application

      │

      ▼

IAIProvider

      │

 ┌────┼───────────────┐

 ▼    ▼               ▼

Azure OpenAI

OpenAI

Ollama
```

The Application Layer never references provider SDKs directly.

---

# Primary Provider

**Azure OpenAI** is approved as the default production provider.

Reasons:

- Enterprise governance
- Azure Active Directory integration
- Microsoft ecosystem compatibility
- Enterprise compliance
- Operational simplicity
- High-quality inference

---

# Secondary Provider

**OpenAI** is approved as an optional secondary provider.

Typical use cases:

- evaluation of new models;
- controlled failover;
- experimentation;
- future migration scenarios.

---

# Local Provider

**Ollama** is approved as the local inference provider.

Typical use cases:

- offline environments;
- air-gapped deployments;
- customer on-premise installations;
- privacy-sensitive workloads;
- development environments.

---

# Provider Abstraction

The following abstraction shall be introduced.

```text
IAIProvider

    ├── GenerateEmbeddingAsync()

    ├── ChatCompletionAsync()

    ├── StreamingCompletionAsync()

    ├── ToolCallingAsync()
```

Business logic shall depend only upon this interface.

---

# Dependency Direction

```text
Application

      │

IAIProvider

      │

Infrastructure

      │

Azure OpenAI

OpenAI

Ollama
```

No provider-specific implementation shall leak into higher architectural layers.

---

# Configuration Strategy

Provider selection shall be configuration driven.

Example:

```json
{
  "AI": {
    "Provider": "AzureOpenAI"
  }
}
```

Future values may include:

- AzureOpenAI
- OpenAI
- Ollama

No source-code modification shall be required when switching providers.

---

# Failover Strategy

The architecture supports optional provider failover.

Example:

```text
Primary

Azure OpenAI

      │

Failure

      ▼

OpenAI

      │

Failure

      ▼

Ollama
```

Automatic failover is optional and shall be implemented only when business requirements justify the additional operational complexity.

---

# Embedding Strategy

Embedding generation shall use the configured provider.

The generated vectors are stored in the approved Vector Database (Qdrant).

Embeddings remain provider-independent artifacts.

---

# Prompt Strategy

Prompt templates shall be provider neutral.

They shall not contain:

- provider-specific syntax;
- SDK dependencies;
- vendor-specific formatting.

---

# Security

Provider credentials shall never be stored in application code.

Secrets shall be managed through the approved enterprise secret management solution.

Production authentication shall use managed identity whenever supported.

---

# Logging

Provider implementations shall expose standardized telemetry including:

- latency;
- token usage;
- model name;
- provider identifier;
- failure reason.

Application logging shall remain provider independent.

---

# Benefits

This architecture provides:

- provider independence;
- clean separation of concerns;
- easier testing;
- hybrid deployment capability;
- future extensibility;
- reduced vendor lock-in.

---

# Consequences

Positive:

- Clean Architecture compliance
- Replaceable providers
- Long-term flexibility
- Easier evolution of AI capabilities

Negative:

- Additional abstraction layer
- Slight increase in implementation complexity
- More integration testing

---

# Alternatives Considered

## Direct Azure OpenAI Integration

Rejected.

Reason:

Violates Dependency Inversion and creates long-term provider coupling.

---

## OpenAI Only

Rejected.

Reason:

Does not align with the approved Microsoft enterprise infrastructure strategy.

---

## Ollama Only

Rejected.

Reason:

Operational complexity is unnecessary for the initial enterprise deployment.

---

## Single Provider Without Abstraction

Rejected.

Reason:

Future provider replacement would require application refactoring.

---

# Related Documents

- ADR-0001 — Clean Architecture
- ADR-0002 — CQRS Architecture
- ADR-0022 — AI Knowledge Retrieval Architecture
- TE-0029 — Artificial Intelligence Provider Technology Evaluation

---

# Decision Outcome

**Accepted**

MachineryManagerEnterprise adopts a Hybrid AI Provider Strategy in which:

- Azure OpenAI is the default production provider.
- OpenAI is an approved secondary provider.
- Ollama is the approved local inference provider.
- All provider implementations are accessed exclusively through the `IAIProvider` abstraction.

---

# Revision History

| Version | Date       | Author             | Description     |
|---------|------------|--------------------|-----------------|
| 1.0.0   | 2026-07-28 | Solution Architect | Initial version |
| 1.1.0   | 2026-07-28 | Solution Architect | Header reformatted to comply with the official Standard Document Header in DOCUMENT_CONVENTIONS.md |