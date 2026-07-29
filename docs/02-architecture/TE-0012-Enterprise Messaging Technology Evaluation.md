| Property | Value |
|----------|-------|
| **Document ID** | TE-0012 |
| **Title** | Enterprise Messaging Technology Evaluation |
| **Version** | 4.0.0 |
| **Status** | Approved |
| **Owner** | Solution Architect |
| **Created** | 2026-07-26 |
| **Last Updated** | 2026-07-28 |

---

# Purpose

This document evaluates candidate technologies for Enterprise Messaging Technology Evaluation in MachineryManagerEnterprise.

The objective is to establish a unified technology selection that satisfies all functional and architectural requirements while preserving Clean Architecture principles.

---


# Relationship with Previous Technology Evaluations

This Technology Evaluation builds upon the foundation established in TE-0001 (.NET 10 Platform) and aligns with the enterprise architecture rules defined across the solution.

---

# Architectural References

This evaluation is based on:

- ADR-0001 — Clean Architecture
- SolutionStructure.md
- DependencyRules.md
- CodingStandards.md

---

# Functional Requirements

The selected technology shall support:

- core enterprise capabilities required by MachineryManagerEnterprise;
- Clean Architecture separation of domain models from infrastructure details;
- seamless integration with .NET 10 runtime and Dependency Injection;
- high performance execution and asynchronous operations.

---

# Non-Functional Requirements

The solution should provide:

- enterprise reliability and scalability;
- long-term maintainability and cloud neutrality;
- zero vendor lock-in;
- optimal developer experience and testability.

---

# Candidate Technologies

| Technology | Purpose | Status |
|------------|---------|--------|
| Selected Primary Engine | Enterprise Infrastructure | Selected |
| Alternative Engine | Comparison Candidate | Evaluated |

---

# Architecture Principle

The technology operates strictly within the Infrastructure or Application layers, keeping Domain logic completely clean and independent.

---

# Evaluation Scope

This Technology Evaluation only evaluates technology selection.

Implementation details are defined by the corresponding Architecture Decision Records (ADRs).

---

# 1. Architectural Reference

This evaluation is based on:

- ADR-0001 — Clean Architecture
- ADR-0016 — Enterprise Messaging Architecture

The selected technology shall satisfy the architectural constraints defined by these Architecture Decision Records.

---

# 2. Functional Requirements

The messaging platform should support:

- asynchronous communication;
- publish / subscribe patterns;
- point-to-point communication;
- event-driven architecture;
- command messaging;
- notification delivery;
- distributed processing;
- retry mechanisms;
- dead-letter handling;
- message durability;
- transactional consistency where applicable.

---

# 3. Non-Functional Requirements

The technology should provide:

- high reliability;
- scalability;
- cross-platform compatibility;
- .NET ecosystem support;
- observability;
- monitoring capabilities;
- production readiness;
- long-term maintainability;
- active community;
- enterprise adoption.

---

# 4. Candidate Technologies

The following technologies have been selected for evaluation:

- RabbitMQ
- Apache Kafka
- Azure Service Bus
- MassTransit
- NServiceBus
- Rebus

---

# 5. Technology Classification

The candidate technologies belong to different architectural categories.

## Message Brokers

These technologies provide the messaging infrastructure.

- RabbitMQ
- Apache Kafka
- Azure Service Bus

---

## Messaging Frameworks

These technologies provide programming abstractions above messaging infrastructure.

- MassTransit
- NServiceBus
- Rebus

Messaging Frameworks are not direct alternatives to Message Brokers.

They are complementary technologies.

The final architecture may use one Broker together with one Messaging Framework.

---

# 6. Evaluation Criteria

Candidate technologies shall be evaluated using the following criteria.

| ID | Criterion | Weight |
|----|-----------|--------|
| C1 | Architectural Compatibility | Critical |
| C2 | Clean Architecture Support | Critical |
| C3 | Scalability | High |
| C4 | Reliability | High |
| C5 | Performance | High |
| C6 | Operational Simplicity | Medium |
| C7 | Observability | Medium |
| C8 | Community Maturity | Medium |
| C9 | Enterprise Adoption | Medium |
| C10 | Cloud Readiness | Medium |
| C11 | On-Premise Support | Medium |
| C12 | Cross Platform Support | Medium |
| C13 | Documentation Quality | Medium |
| C14 | Licensing | Medium |
| C15 | Long-Term Maintainability | Critical |

---

# 7. Evaluation Methodology

Each technology shall receive a score from:

- 1 = Poor
- 2 = Fair
- 3 = Good
- 4 = Very Good
- 5 = Excellent

Weighted scores shall be used only for comparison.

The final architectural decision shall consider:

- mandatory architectural constraints;
- platform requirements;
- operational complexity;
- long-term maintainability.

Raw scores alone shall never determine the final recommendation.

---

# 8. Technology Comparison Matrix

| Criterion | RabbitMQ | Kafka | Azure Service Bus | MassTransit | NServiceBus | Rebus |
|-----------|-----------|--------|-------------------|-------------|-------------|-------|
| Architectural Compatibility | | | | | | |
| Clean Architecture Support | | | | | | |
| Scalability | | | | | | |
| Reliability | | | | | | |
| Performance | | | | | | |
| Operational Simplicity | | | | | | |
| Observability | | | | | | |
| Community Maturity | | | | | | |
| Enterprise Adoption | | | | | | |
| Cloud Readiness | | | | | | |
| On-Premise Support | | | | | | |
| Cross Platform Support | | | | | | |
| Documentation Quality | | | | | | |
| Licensing | | | | | | |
| Maintainability | | | | | | |

---

# 9. RabbitMQ Evaluation

## Overview

RabbitMQ is an open-source message broker implementing the Advanced Message Queuing Protocol (AMQP).

It is designed for reliable asynchronous messaging between distributed applications and is widely adopted in enterprise systems.

RabbitMQ focuses on:

- message routing;
- reliable delivery;
- queue management;
- publish/subscribe communication;
- request/reply messaging.

---

## Architectural Strengths

RabbitMQ aligns well with the architectural principles defined by ADR-0016.

### Advantages

- Mature and stable enterprise platform.
- Strong support for asynchronous messaging.
- Excellent routing capabilities.
- Supports multiple messaging patterns.
- High reliability.
- Excellent .NET ecosystem support.
- Strong community.
- Proven production deployments.
- Cross-platform.
- Suitable for on-premise deployment.
- Well suited for modular enterprise systems.

---

## Architectural Weaknesses

RabbitMQ is primarily designed for message delivery.

It is **not** intended to become:

- event storage;
- event streaming platform;
- analytical pipeline;
- long-term message history.

Large-scale event replay scenarios may require complementary technologies.

---

## Operational Characteristics

RabbitMQ provides:

- acknowledgements;
- retries;
- dead-letter queues;
- durable queues;
- clustering;
- high availability;
- management console;
- monitoring endpoints.

Operational complexity remains moderate.

---

## Scalability

RabbitMQ scales efficiently for typical enterprise workloads.

Horizontal scalability is supported through clustering and federation.

Very high-throughput event streaming workloads may require different architectural approaches.

---

## Security

RabbitMQ supports:

- authentication;
- authorization;
- TLS encryption;
- virtual hosts;
- permission management.

Additional enterprise identity integration depends on deployment architecture.

---

## Suitability for MachineryManagerEnterprise

RabbitMQ appears highly compatible with the following architectural capabilities:

- Workspace Synchronization
- Enterprise Messaging
- Notifications
- Internal Messenger
- Background Processing
- Integration Events

Its communication model closely matches the messaging architecture established by ADR-0016.

---

## Architectural Fit

| Criterion | Assessment |
|-----------|------------|
| Clean Architecture | Excellent |
| Modular Architecture | Excellent |
| Distributed Workspace | Excellent |
| Event-driven Communication | Excellent |
| Technology Independence | Excellent |
| Cloud Deployment | Very Good |
| On-Premise Deployment | Excellent |
| Operational Complexity | Moderate |

---

## Preliminary Conclusion

RabbitMQ satisfies nearly all architectural requirements defined for the MachineryManagerEnterprise platform.

It should remain a primary candidate for the messaging infrastructure.

Final recommendation is deferred until all candidate technologies have been evaluated.

---

# 10. Apache Kafka Evaluation

## Overview

Apache Kafka is a distributed event streaming platform designed for high-throughput, fault-tolerant and scalable event processing.

Unlike traditional message brokers, Kafka is designed around persistent event logs rather than transient message queues.

Its architecture emphasizes:

- event streaming;
- immutable event logs;
- distributed scalability;
- high-throughput data pipelines;
- replayable event history.

---

## Architectural Strengths

Kafka provides several architectural advantages.

### Advantages

- Extremely high throughput.
- Horizontal scalability.
- Durable event storage.
- Event replay capability.
- Strong support for event sourcing architectures.
- Excellent fault tolerance.
- Large enterprise adoption.
- Mature ecosystem.
- Cloud-native deployment support.
- Suitable for large distributed systems.

---

## Architectural Weaknesses

Kafka introduces significantly greater architectural and operational complexity than traditional message brokers.

Kafka is optimized for event streaming rather than traditional enterprise messaging.

For many enterprise applications it may provide capabilities that exceed actual architectural requirements.

---

## Operational Characteristics

Kafka provides:

- distributed partitions;
- replication;
- durable logs;
- consumer groups;
- replay support;
- high availability;
- horizontal scaling.

However it also requires:

- cluster management;
- partition planning;
- operational monitoring;
- storage management.

Operational complexity is considered high.

---

## Scalability

Kafka is one of the most scalable messaging platforms currently available.

It is particularly well suited for:

- very large event volumes;
- streaming analytics;
- real-time processing;
- distributed event processing.

---

## Security

Kafka supports:

- authentication;
- authorization;
- TLS encryption;
- ACL management.

Enterprise identity integration depends on deployment architecture.

---

## Suitability for MachineryManagerEnterprise

Kafka aligns well with architectures that require continuous event streaming.

Potential usage scenarios include:

- large-scale telemetry;
- IoT event collection;
- operational analytics;
- historical event replay.

For the current MachineryManagerEnterprise architecture, many Kafka capabilities are not immediate architectural requirements.

The platform currently emphasizes reliable enterprise messaging rather than continuous event streaming.

---

## Architectural Fit

| Criterion | Assessment |
|-----------|------------|
| Clean Architecture | Excellent |
| Modular Architecture | Excellent |
| Distributed Workspace | Very Good |
| Event-driven Communication | Excellent |
| Technology Independence | Excellent |
| Cloud Deployment | Excellent |
| On-Premise Deployment | Very Good |
| Operational Complexity | High |

---

## Preliminary Conclusion

Apache Kafka is an excellent technology for large-scale event streaming platforms.

However, for the current architectural requirements of MachineryManagerEnterprise, Kafka introduces additional operational complexity while providing capabilities that are not yet required.

Kafka should remain a secondary candidate unless future architectural requirements include:

- large-scale event streaming;
- IoT telemetry processing;
- analytics pipelines;
- event replay as a primary architectural capability.

---

# 11. Azure Service Bus Evaluation

## Overview

Azure Service Bus is Microsoft's fully managed enterprise messaging service designed for reliable communication between distributed applications running primarily within the Microsoft Azure ecosystem.

Unlike self-hosted messaging platforms, Azure Service Bus is delivered as a Platform-as-a-Service (PaaS) offering where infrastructure management is handled by the cloud provider.

It is designed around:

- enterprise messaging;
- reliable delivery;
- queue-based communication;
- publish/subscribe messaging;
- cloud-native deployment.

---

## Architectural Strengths

Azure Service Bus provides several architectural advantages.

### Advantages

- Fully managed messaging infrastructure.
- Excellent reliability.
- Native Azure ecosystem integration.
- Built-in high availability.
- Automatic scaling.
- Dead-letter queues.
- Duplicate detection.
- Transaction support.
- Enterprise-grade security.
- Minimal infrastructure maintenance.

---

## Architectural Weaknesses

Azure Service Bus introduces a dependency on a specific cloud provider.

The architecture itself remains provider-independent (per ADR-0016), but the implementation becomes tightly coupled to Microsoft Azure.

This may reduce deployment flexibility for organizations requiring:

- on-premise deployment;
- hybrid deployment;
- cloud portability.

---

## Operational Characteristics

Azure Service Bus provides:

- managed queues;
- managed topics;
- automatic failover;
- message durability;
- monitoring through Azure platform services.

Operational complexity is significantly lower than self-hosted brokers.

However, operational control is also more limited because infrastructure is managed by the provider.

---

## Scalability

Azure Service Bus scales efficiently within Azure.

Scaling is largely transparent to application developers.

Very large event-streaming workloads are generally better suited to Azure Event Hubs or Apache Kafka.

---

## Security

Azure Service Bus supports:

- Azure Active Directory integration;
- Managed Identity;
- Role-Based Access Control (RBAC);
- TLS encryption;
- Shared Access Signatures (SAS).

Security capabilities are among its strongest characteristics.

---

## Suitability for MachineryManagerEnterprise

Azure Service Bus aligns well with architectures deployed entirely on Microsoft Azure.

Potential usage scenarios include:

- cloud-native enterprise deployments;
- distributed cloud services;
- Azure-hosted business modules;
- managed infrastructure environments.

However, the current architectural direction of MachineryManagerEnterprise explicitly targets deployment flexibility across:

- on-premise;
- private cloud;
- public cloud;
- hybrid environments.

Under these architectural constraints, Azure Service Bus introduces an unnecessary deployment dependency.

---

## Architectural Fit

| Criterion | Assessment |
|-----------|------------|
| Clean Architecture | Excellent |
| Modular Architecture | Excellent |
| Distributed Workspace | Excellent |
| Event-driven Communication | Excellent |
| Technology Independence | Good |
| Cloud Deployment | Excellent |
| On-Premise Deployment | Poor |
| Operational Complexity | Very Low |

---

## Preliminary Conclusion

Azure Service Bus is an excellent enterprise messaging platform for organizations standardized on Microsoft Azure.

However, for MachineryManagerEnterprise, whose architecture explicitly supports deployment independence across multiple environments, Azure Service Bus reduces infrastructure portability.

It should therefore be considered a specialized deployment option rather than the default messaging platform.

---

# 12. MassTransit Evaluation

## Overview

MassTransit is an open-source distributed application framework for .NET that provides a high-level abstraction over enterprise messaging infrastructures.

MassTransit is not a message broker.

Instead, it provides an architectural programming model above messaging infrastructure such as:

- RabbitMQ
- Azure Service Bus
- Amazon SQS
- ActiveMQ
- Apache Kafka (limited scenarios)

Its objective is to simplify distributed application development while preserving architectural separation.

---

## Architectural Strengths

MassTransit aligns extremely well with modern .NET enterprise architecture.

### Advantages

- Excellent Clean Architecture compatibility.
- Strong abstraction over transport technologies.
- Provider-independent programming model.
- Native dependency injection support.
- Strong support for asynchronous workflows.
- Saga support.
- State machine support.
- Retry policies.
- Outbox support.
- Consumer pipeline.
- Request / Response support.
- Publish / Subscribe support.
- Excellent .NET integration.
- Active community.
- Mature documentation.

---

## Architectural Weaknesses

MassTransit introduces an additional abstraction layer.

Although this abstraction significantly improves maintainability, it also requires developers to understand:

- messaging concepts;
- consumers;
- sagas;
- middleware pipelines.

Learning curve is considered moderate.

---

## Operational Characteristics

MassTransit itself manages no infrastructure.

Operational responsibilities remain with the selected message broker.

MassTransit provides:

- consumer lifecycle management;
- endpoint configuration;
- retry orchestration;
- message serialization abstraction;
- middleware pipeline;
- fault handling.

---

## Scalability

MassTransit inherits scalability from the selected messaging infrastructure.

It introduces negligible architectural overhead.

Scaling characteristics therefore depend primarily on the underlying broker.

---

## Security

Security is delegated to the selected transport.

MassTransit itself does not weaken architectural security boundaries.

It integrates naturally with platform authentication and authorization models.

---

## Suitability for MachineryManagerEnterprise

MassTransit aligns exceptionally well with:

- ADR-0001 (Clean Architecture)
- ADR-0016 (Enterprise Messaging)
- ADR-0017 (Artificial Intelligence)
- ADR-0018 (External Integration)

Its abstraction model directly supports the architectural requirement that business modules remain independent from messaging technologies.

This is one of the strongest architectural advantages for the current project.

---

## Architectural Fit

| Criterion | Assessment |
|-----------|------------|
| Clean Architecture | Excellent |
| Modular Architecture | Excellent |
| Technology Independence | Excellent |
| Provider Independence | Excellent |
| Distributed Workspace | Excellent |
| Maintainability | Excellent |
| Operational Complexity | Low |
| Cloud Compatibility | Excellent |
| On-Premise Compatibility | Excellent |

---

## Preliminary Conclusion

MassTransit appears to be one of the strongest candidates for implementing the Enterprise Messaging Architecture.

Its greatest architectural advantage is complete separation between business logic and messaging infrastructure.

When combined with an appropriate broker, MassTransit provides a robust, maintainable and technology-independent messaging architecture fully aligned with the architectural principles established for MachineryManagerEnterprise.

Final recommendation is deferred until all messaging frameworks have been evaluated.

---

# 13. NServiceBus Evaluation

## Overview

NServiceBus is a commercial enterprise messaging framework for .NET designed to simplify the development of distributed systems through high-level messaging abstractions.

Like MassTransit, NServiceBus is **not** a message broker.

It provides an architectural programming model over supported messaging infrastructures while emphasizing reliability, consistency and enterprise governance.

Supported transports include (depending on licensing and edition):

- RabbitMQ
- Azure Service Bus
- SQL Transport
- Amazon SQS
- Others

---

## Architectural Strengths

NServiceBus was designed specifically for enterprise applications.

### Advantages

- Excellent Clean Architecture compatibility.
- Strong transport abstraction.
- Mature Saga implementation.
- Reliable Outbox pattern.
- Advanced retry mechanisms.
- Excellent message versioning support.
- Rich monitoring ecosystem.
- Enterprise governance features.
- Strong documentation.
- Proven enterprise adoption.
- Stable API surface.
- Excellent long-term support.

---

## Architectural Weaknesses

NServiceBus is a commercial product.

Its licensing model introduces:

- licensing cost;
- vendor dependency;
- feature availability depending on edition.

Some advanced capabilities are tied to commercial licensing.

---

## Operational Characteristics

NServiceBus provides:

- endpoint management;
- automatic retries;
- delayed delivery;
- timeout management;
- message auditing;
- error queues;
- saga persistence;
- monitoring integration.

Operational maturity is considered excellent.

---

## Scalability

NServiceBus scales according to the selected transport.

The framework itself introduces minimal runtime overhead.

Large distributed enterprise systems have successfully adopted NServiceBus for many years.

---

## Security

Security is delegated to the selected transport.

NServiceBus fully supports secure architectural boundaries while providing facilities for endpoint isolation and secure message handling.

---

## Suitability for MachineryManagerEnterprise

Architecturally, NServiceBus satisfies nearly all requirements established by:

- ADR-0001
- ADR-0016
- ADR-0017
- ADR-0018

Its strongest characteristics are:

- enterprise governance;
- operational maturity;
- reliability;
- long-term maintainability.

Its primary architectural disadvantage is the dependency on commercial licensing.

---

## Architectural Fit

| Criterion | Assessment |
|-----------|------------|
| Clean Architecture | Excellent |
| Modular Architecture | Excellent |
| Technology Independence | Excellent |
| Provider Independence | Excellent |
| Enterprise Governance | Excellent |
| Maintainability | Excellent |
| Operational Complexity | Low |
| Cloud Compatibility | Excellent |
| On-Premise Compatibility | Excellent |
| Licensing | Commercial |

---

## Preliminary Conclusion

From a purely architectural perspective, NServiceBus is one of the strongest messaging frameworks available for .NET enterprise applications.

Its governance, reliability and architectural maturity are outstanding.

However, the commercial licensing model introduces cost and vendor dependency considerations.

For organizations prioritizing enterprise support and governance, NServiceBus is an excellent candidate.

For organizations preferring open-source technologies while maintaining architectural quality, alternatives such as MassTransit should also be considered.

---

# 14. Rebus Evaluation

## Overview

Rebus is an open-source .NET messaging library designed to simplify asynchronous messaging through a lightweight abstraction over multiple messaging transports.

Like MassTransit and NServiceBus, Rebus is not a message broker.

It focuses on providing a straightforward messaging API while keeping infrastructure concerns separated from business logic.

Supported transports include:

- RabbitMQ
- Azure Service Bus
- SQL Server
- MSMQ
- Amazon SQS
- Others

---

## Architectural Strengths

### Advantages

- Open source.
- Lightweight architecture.
- Simple programming model.
- Excellent transport abstraction.
- Good dependency injection support.
- Reliable asynchronous messaging.
- Cross-platform.
- Easy adoption.
- Low runtime overhead.
- Good maintainability for medium-sized systems.

---

## Architectural Weaknesses

Compared to MassTransit and NServiceBus, Rebus provides fewer enterprise-level capabilities.

Large distributed systems may require additional implementation effort for:

- complex workflows;
- advanced Saga orchestration;
- operational governance;
- enterprise monitoring.

---

## Operational Characteristics

Rebus provides:

- retries;
- delayed delivery;
- handler pipelines;
- transport abstraction;
- routing;
- serialization abstraction.

Operational complexity is low.

---

## Scalability

Rebus scales according to the selected transport.

The framework itself remains lightweight and introduces minimal processing overhead.

Very large enterprise deployments may require additional architectural infrastructure beyond Rebus itself.

---

## Security

Security responsibilities remain delegated to the underlying transport.

Rebus integrates cleanly with existing platform security mechanisms.

---

## Suitability for MachineryManagerEnterprise

Rebus satisfies the architectural principles established by:

- ADR-0001
- ADR-0016
- ADR-0017
- ADR-0018

However, its lightweight design means several enterprise capabilities would require additional implementation compared with MassTransit or NServiceBus.

For medium-sized systems it represents an attractive solution.

For large enterprise platforms it may require supplementary infrastructure.

---

## Architectural Fit

| Criterion | Assessment |
|-----------|------------|
| Clean Architecture | Excellent |
| Modular Architecture | Excellent |
| Technology Independence | Excellent |
| Provider Independence | Excellent |
| Maintainability | Very Good |
| Operational Complexity | Very Low |
| Cloud Compatibility | Excellent |
| On-Premise Compatibility | Excellent |
| Enterprise Governance | Moderate |

---

## Preliminary Conclusion

Rebus is a capable and lightweight messaging framework.

Its simplicity makes it attractive for projects seeking minimal infrastructure complexity.

However, MachineryManagerEnterprise is expected to evolve as a long-lived enterprise platform supporting multiple business modules and advanced distributed capabilities.

Under these assumptions, frameworks providing richer enterprise features should receive higher preference.

---

# 15. Comparative Summary

## Message Brokers

| Technology | RabbitMQ | Kafka | Azure Service Bus |
|------------|----------|--------|-------------------|
| Enterprise Messaging | Excellent | Very Good | Excellent |
| Event Streaming | Fair | Excellent | Good |
| Operational Complexity | Low | High | Very Low |
| On-Premise Support | Excellent | Excellent | Poor |
| Cloud Support | Excellent | Excellent | Excellent |
| Technology Independence | Excellent | Excellent | Moderate |
| Architecture Compatibility | Excellent | Very Good | Good |
| Overall Assessment | ⭐⭐⭐⭐⭐ | ⭐⭐⭐⭐ | ⭐⭐⭐ |

---

## Messaging Frameworks

| Technology | MassTransit | NServiceBus | Rebus |
|------------|-------------|-------------|-------|
| Clean Architecture | Excellent | Excellent | Excellent |
| Transport Abstraction | Excellent | Excellent | Excellent |
| Enterprise Features | Excellent | Excellent | Good |
| Operational Governance | Very Good | Excellent | Moderate |
| Learning Curve | Moderate | Moderate | Low |
| Licensing | Open Source | Commercial | Open Source |
| Architecture Compatibility | Excellent | Excellent | Very Good |
| Overall Assessment | ⭐⭐⭐⭐⭐ | ⭐⭐⭐⭐⭐ | ⭐⭐⭐⭐ |

---

# 16. Candidate Architecture Combinations

| Combination | Assessment |
|-------------|------------|
| RabbitMQ + MassTransit | ⭐⭐⭐⭐⭐ |
| RabbitMQ + NServiceBus | ⭐⭐⭐⭐ |
| RabbitMQ + Rebus | ⭐⭐⭐⭐ |
| Kafka + MassTransit | ⭐⭐⭐ |
| Kafka + NServiceBus | ⭐⭐⭐ |
| Azure Service Bus + MassTransit | ⭐⭐⭐ |
| Azure Service Bus + NServiceBus | ⭐⭐⭐⭐ |

---

# 17. Recommendation

## Recommended Architecture

RabbitMQ together with MassTransit is recommended as the default Enterprise Messaging implementation for MachineryManagerEnterprise.

### Rationale

This combination provides:

- full compliance with ADR-0016;
- complete technology independence;
- excellent Clean Architecture support;
- strong .NET ecosystem integration;
- deployment flexibility;
- open-source licensing;
- long-term maintainability.

---

## Alternative Architecture

RabbitMQ together with NServiceBus is recommended where enterprise governance, commercial support and operational tooling are organizational priorities.

---

## Specialized Architecture

Apache Kafka should be considered only if future architectural requirements evolve toward:

- continuous event streaming;
- telemetry processing;
- large-scale analytics;
- event replay.

---

## Cloud-Specific Architecture

Azure Service Bus should be considered only for deployments standardized entirely on Microsoft Azure.

---

## Not Recommended as Default

The following technologies are not recommended as the default messaging architecture for MachineryManagerEnterprise:

- Apache Kafka (operational complexity exceeds current needs)
- Azure Service Bus (deployment dependency)
- Rebus (limited enterprise capabilities compared with MassTransit)

---

# 18. Final Decision

After evaluation of architectural requirements, operational characteristics and long-term maintainability, the recommended messaging architecture is:

```text
Messaging Broker
    RabbitMQ

Messaging Framework
    MassTransit
```

This decision best satisfies:

- ADR-0001
- ADR-0016
- ADR-0017
- ADR-0018

Technology selection may be revisited if future architectural requirements significantly change.

---

# 19. Revision History

| Version | Date       | Author             | Description |
|---------|------------|--------------------|-------------|
| 1.0.0   | 2026-07-26 | Solution Architect | Initial version |
| 1.3.0   | 2026-07-28 | Solution Architect | New section added (Evaluation Scope) |




# Overall Technology Comparison

The selected technology provides optimal performance, maintainability, and Clean Architecture compatibility.

## Responsibility Matrix

| Responsibility | Recommended Technology | Alternative |
|-----------------|------------------------|-------------|
| System Capability | Primary Selected | Evaluated Option |

---

# Final Recommendation

Adopt the selected technology as the official platform standard for MachineryManagerEnterprise.

---

# Final Decision

| Component | Decision |
|-----------|----------|
| Primary Selected Technology | Approved |

---

# Decision Summary

The selected technology stack satisfies all architectural requirements.

- ✔ Clean Architecture
- ✔ .NET 10 Compatibility
- ✔ Standards Compliance
- ✔ Cloud Neutrality
- ✔ AI Readiness
- ✔ Long-term Maintainability

---

# Related ADR

- ADR-0001 — Clean Architecture
- ADR-0015 — Deployment Architecture

---

# Related Documents

- SolutionStructure.md
- DependencyRules.md
- CodingStandards.md

---

# Revision History

| Version | Date       | Author             | Description                                 |
|---------|------------|--------------------|---------------------------------------------|
| 1.0.0   | 2026-07-26 | Solution Architect | Initial technology evaluation for Messaging |
| 1.3.0   | 2026-07-28 | Solution Architect | New section added (Evaluation Scope)        |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0   |