| Property | Value |
|----------|-------|
| **Technology Evaluation ID** | TE-0020 |
| **Title** | Authentication and Identity Technology Evaluation (.NET 10) |
| **Version** | 1.3.0 |
| **Status** | Proposed |
| **Owner** | Solution Architect |
| **Created** | 2026-07-26 |
| **Last Updated** | 2026-07-28 |

# Purpose

This document evaluates authentication and identity technologies for MachineryManagerEnterprise.

The authentication subsystem is responsible for establishing the identity of users and services while providing a secure, extensible and maintainable authorization infrastructure.

The selected solution must support both current enterprise requirements and future expansion toward distributed services and external integrations.

---

# Evaluation Scope

This Technology Evaluation only evaluates technology selection.

Implementation details are defined by the corresponding Architecture Decision Records (ADRs).

---

# Architectural References

This evaluation is based on:

- ADR-0001 — Clean Architecture
- ADR-0008 — Security Architecture
- ADR-0015 — Deployment Architecture
- ADR-0017 — External Integration Architecture

Authentication shall remain:

- provider independent;
- standards compliant;
- secure by default;
- cloud neutral;
- compatible with .NET 10.

---

# Functional Requirements

The platform requires support for:

- username/password authentication;
- JWT authentication;
- role-based authorization;
- policy-based authorization;
- refresh tokens;
- external identity providers;
- service-to-service authentication;
- API authentication;
- desktop client authentication;
- future Single Sign-On support.

---

# Non-Functional Requirements

The selected solution should provide:

- enterprise security;
- extensibility;
- standards compliance;
- scalability;
- maintainability;
- cloud neutrality;
- excellent .NET 10 integration.

---

# Candidate Technologies

## Identity Management

| Technology | Role |
|------------|------|
| ASP.NET Core Identity | Local Identity Management |
| OpenIddict | OAuth2 / OpenID Connect Server |
| Duende IdentityServer | Enterprise Identity Server |

---

## Token Format

| Technology | Role |
|------------|------|
| JWT (RFC 7519) | Access Token |
| Reference Tokens | Alternative Token Strategy |

---

## External Identity Providers

| Technology | Role |
|------------|------|
| Microsoft Entra ID | Enterprise Identity |
| Google Identity | External Login |
| GitHub Identity | Developer Login |

---

# Evaluation Criteria

| ID | Criterion | Weight |
|----|-----------|--------|
| A1 | Security | Critical |
| A2 | Standards Compliance | Critical |
| A3 | Clean Architecture Compatibility | Critical |
| A4 | Cloud Neutrality | High |
| A5 | Extensibility | High |
| A6 | Operational Simplicity | Medium |
| A7 | Community & Maturity | High |
| A8 | .NET 10 Compatibility | Critical |

---

# Architecture Principle

Authentication must remain isolated from business logic.

```text
Presentation Layer

        │

        ▼

Authentication Abstraction

        │

        ▼

Identity Provider

        │

 ┌──────────────┬──────────────┐

 ▼              ▼

ASP.NET     OpenIddict
Identity
```

Business modules never authenticate users directly.

They consume only authenticated identities and authorization information.

---

# 5. ASP.NET Core Identity Evaluation

## Overview

ASP.NET Core Identity is Microsoft's official identity management framework for .NET applications.

It provides local user management including:

- user accounts;
- password hashing;
- roles;
- claims;
- lockout;
- password reset;
- email confirmation;
- two-factor authentication.

Identity integrates natively with Entity Framework Core and ASP.NET Core authentication middleware.

---


# 5. ASP.NET Core Identity Evaluation

## Overview

ASP.NET Core Identity is Microsoft's official authentication and identity management framework for ASP.NET Core applications.

It provides a complete local identity solution including:

- user management;
- password hashing;
- roles;
- claims;
- lockout;
- password recovery;
- email confirmation;
- multi-factor authentication.

Identity integrates natively with:

- ASP.NET Core Authentication;
- Entity Framework Core;
- Authorization Middleware;
- Dependency Injection.

Within MachineryManagerEnterprise it is evaluated as the primary local identity management solution.

---

# Architectural Role

Identity belongs to the Infrastructure Security layer.

```text
Presentation Layer

        │

Authentication Middleware

        │

        ▼

Authentication Abstraction

        │

        ▼

ASP.NET Core Identity

        │

        ▼

Identity Store
```

Business modules never manipulate Identity entities directly.

---

# Architectural Strengths

## Advantages

- Official Microsoft framework.
- Native .NET 10 support.
- Excellent ASP.NET Core integration.
- Role management.
- Claims management.
- Password hashing.
- MFA support.
- Lockout protection.
- Security stamp validation.
- Mature ecosystem.
- Large community.

---

# Architectural Weaknesses

ASP.NET Core Identity is intentionally focused on **identity management**, not enterprise identity federation.

Limitations include:

- not an OAuth2 Authorization Server;
- not an OpenID Connect Provider;
- no enterprise federation by itself;
- limited cross-application SSO.

These capabilities require complementary technologies such as OpenIddict.

---

# Operational Characteristics

Identity provides:

- user lifecycle;
- credential management;
- role administration;
- claim assignment;
- password policies;
- account security.

Operational complexity is considered low.

---

# Scalability

Identity scales well for:

- enterprise web APIs;
- modular monoliths;
- medium-to-large deployments.

Horizontal scaling depends primarily on the backing database rather than the framework itself.

Scalability is considered excellent.

---

# Security

Security is one of Identity's strongest characteristics.

Built-in capabilities include:

- PBKDF2 password hashing;
- configurable password policies;
- account lockout;
- security stamps;
- two-factor authentication;
- cookie protection;
- claims-based authorization.

Security is considered excellent.

---

# Deployment Flexibility

Supported environments:

- Windows
- Linux
- Containers
- Kubernetes
- Cloud
- Hybrid
- On-Premise

Deployment flexibility is excellent.

---

# AI Compatibility

Identity itself is AI-neutral.

However, AI services benefit from authenticated identities carrying:

- user claims;
- permissions;
- tenant information;
- auditing metadata.

This enables secure authorization of AI-assisted operations.

---

# Maintainability

Identity provides:

- stable APIs;
- long-term Microsoft support;
- strong documentation;
- predictable upgrade path.

Maintainability is considered excellent.

---

# Extensibility

Identity supports customization through:

- custom user entities;
- custom role entities;
- claims transformation;
- custom stores;
- external authentication providers.

This flexibility makes it suitable for enterprise systems.

---

# Typical Usage

Suitable scenarios:

```text
User Accounts

Password Authentication

Roles

Claims

MFA

Local Identity Management
```

Unsuitable scenarios:

```text
Enterprise SSO

OAuth2 Authorization Server

OpenID Connect Provider

Identity Federation
```

These capabilities belong to dedicated identity-server technologies.

---

# Architectural Fit

| Criterion | Assessment |
|-----------|------------|
| Clean Architecture | Excellent |
| Security | Excellent |
| Maintainability | Excellent |
| Extensibility | Excellent |
| Standards Compliance | Very Good |
| Enterprise Readiness | Excellent |

---

# Relationship with OpenIddict

Identity manages users.

OpenIddict issues tokens.

```text
User

        │

        ▼

ASP.NET Core Identity

        │

        ▼

OpenIddict

        │

        ▼

JWT Access Token
```

The two technologies complement one another rather than compete.

---

# Preliminary Conclusion

ASP.NET Core Identity represents the strongest choice for local identity management within MachineryManagerEnterprise.

It should become the standard mechanism for:

- user accounts;
- password management;
- roles;
- claims;
- authentication.

Federated authentication and token issuance will be handled separately by OpenIddict.

---


# 6. OpenIddict Evaluation

## Overview

OpenIddict is a modern, open-source implementation of the OAuth 2.1 and OpenID Connect specifications for ASP.NET Core.

Unlike ASP.NET Core Identity, which is responsible for **identity management**, OpenIddict is responsible for **token issuance and authorization protocols**.

OpenIddict enables MachineryManagerEnterprise to function as its own authorization server while remaining fully integrated with the .NET ecosystem.

---

# Architectural Role

OpenIddict belongs to the Identity Provider layer.

```text
Presentation Layer

        │

Authentication Middleware

        │

        ▼

ASP.NET Core Identity

        │

        ▼

OpenIddict

        │

        ▼

OAuth2 / OpenID Connect

        │

        ▼

JWT Access Tokens
```

Identity authenticates users.

OpenIddict issues standards-compliant security tokens.

---

# Architectural Strengths

## Advantages

- Official .NET-first implementation.
- Open Source.
- Native ASP.NET Core integration.
- OAuth 2.1 support.
- OpenID Connect support.
- JWT token issuance.
- Refresh token support.
- Authorization Code Flow.
- PKCE support.
- Device Flow support.
- Fine-grained customization.
- Excellent .NET 10 compatibility.

---

# Architectural Weaknesses

OpenIddict intentionally focuses on authorization protocols.

It does not provide:

- user management;
- password policies;
- account lifecycle;
- role management.

Those responsibilities remain within ASP.NET Core Identity.

Deployment complexity is greater than simple JWT middleware because an Authorization Server must be configured.

---

# Operational Characteristics

OpenIddict provides:

- authorization endpoints;
- token endpoints;
- discovery endpoints;
- signing infrastructure;
- client registration;
- refresh token lifecycle.

Operational complexity is considered moderate.

---

# Scalability

OpenIddict scales well across:

- modular monoliths;
- enterprise APIs;
- distributed services;
- Kubernetes deployments.

Scalability is considered excellent.

---

# Security

OpenIddict supports modern authentication standards including:

- OAuth 2.1;
- OpenID Connect;
- PKCE;
- signed JWTs;
- refresh token rotation;
- secure token validation.

Security is considered excellent.

---

# Standards Compliance

OpenIddict follows modern authentication standards.

Supported specifications include:

- OAuth 2.x
- OpenID Connect
- PKCE
- JWT
- RFC-compliant token flows

Standards compliance is considered excellent.

---

# Deployment Flexibility

Supported environments include:

- Windows
- Linux
- Containers
- Kubernetes
- Cloud
- Hybrid
- On-Premise

Deployment flexibility is excellent.

---

# AI Compatibility

AI components frequently invoke protected APIs.

OpenIddict provides:

- service authentication;
- delegated authorization;
- secure AI API access;
- machine-to-machine authentication.

This aligns well with future AI expansion.

---

# Maintainability

Advantages include:

- active community;
- modern architecture;
- strong documentation;
- native .NET ecosystem integration.

Maintainability is considered excellent.

---

# Relationship with ASP.NET Core Identity

Responsibilities remain clearly separated.

```text
Identity

        │

User Authentication

        │

        ▼

ASP.NET Core Identity

        │

Authenticated Principal

        │

        ▼

OpenIddict

        │

Token Issuance
```

Identity manages users.

OpenIddict manages authorization.

---

# Comparison with JWT Middleware

| Capability | JWT Middleware | OpenIddict |
|------------|----------------|------------|
| Token Validation | Excellent | Excellent |
| Token Issuance | No | Excellent |
| OAuth2 | No | Excellent |
| OpenID Connect | No | Excellent |
| Refresh Tokens | Limited | Excellent |
| Authorization Server | No | Excellent |

---

# Architectural Fit

| Criterion | Assessment |
|-----------|------------|
| Clean Architecture | Excellent |
| Standards Compliance | Excellent |
| Enterprise Security | Excellent |
| Extensibility | Excellent |
| Cloud Neutrality | Excellent |
| Enterprise Readiness | Excellent |

---

# Preliminary Conclusion

OpenIddict represents the strongest choice for implementing an OAuth 2.1 / OpenID Connect Authorization Server within MachineryManagerEnterprise.

It complements ASP.NET Core Identity rather than replacing it.

Identity remains responsible for user management, while OpenIddict provides standards-compliant authentication and authorization protocols.

---


# 7. Duende IdentityServer Evaluation

## Overview

Duende IdentityServer is the commercial successor to IdentityServer4 and represents one of the most mature OAuth 2.1 / OpenID Connect authorization server implementations for .NET.

It provides enterprise-grade identity federation capabilities and is widely used in large-scale distributed systems.

Unlike OpenIddict, Duende is a **commercial product** requiring licensing for most production scenarios.

Within MachineryManagerEnterprise, it is evaluated as a potential enterprise authorization server.

---

# Architectural Role

Duende occupies the Authorization Server layer.

```text
Presentation Layer

        │

Authentication

        │

        ▼

Identity Management

        │

        ▼

Duende IdentityServer

        │

        ▼

OAuth2 / OpenID Connect

        │

        ▼

Access Tokens
```

Identity data is typically provided by ASP.NET Core Identity or another identity source.

---

# Architectural Strengths

## Advantages

- Extremely mature.
- Enterprise feature set.
- OAuth 2.1 support.
- OpenID Connect support.
- Token introspection.
- Dynamic client management.
- Federation support.
- Fine-grained configuration.
- Excellent documentation.
- Large enterprise adoption.

---

# Architectural Weaknesses

Several factors reduce its suitability for MachineryManagerEnterprise.

### Commercial Licensing

Duende requires commercial licensing for most enterprise deployments.

This introduces:

- recurring licensing cost;
- procurement dependency;
- legal review;
- vendor dependency.

### Architecture Complexity

Duende targets sophisticated identity infrastructures.

Many capabilities exceed the needs of MachineryManagerEnterprise.

---

# Operational Characteristics

Duende supports:

- authorization server;
- identity federation;
- client registration;
- discovery endpoints;
- token issuance;
- token validation;
- introspection;
- device flow.

Operational complexity is considered moderate to high.

---

# Scalability

Duende scales well across:

- enterprise deployments;
- distributed services;
- Kubernetes;
- cloud environments.

Scalability is considered excellent.

---

# Security

Security capabilities include:

- OAuth 2.1;
- OpenID Connect;
- PKCE;
- refresh token rotation;
- signing key management;
- standards-compliant token issuance.

Security is considered excellent.

---

# Standards Compliance

Supported standards include:

- OAuth 2.x
- OpenID Connect
- PKCE
- JWT
- RFC-compliant authorization flows

Standards compliance is excellent.

---

# Deployment Flexibility

Supported environments:

- Windows
- Linux
- Containers
- Kubernetes
- Cloud
- Hybrid
- On-Premise

Deployment flexibility is excellent.

---

# AI Compatibility

Duende supports secure authentication for:

- AI APIs;
- machine-to-machine communication;
- external AI providers;
- delegated authorization.

Compatibility is excellent.

---

# Maintainability

Advantages include:

- excellent documentation;
- mature architecture;
- predictable behavior.

However, licensing introduces an additional maintenance concern for long-term ownership.

Maintainability is considered very good.

---

# Licensing Considerations

| Aspect | Assessment |
|---------|------------|
| Licensing Cost | High |
| Vendor Dependency | Moderate |
| Open Source Availability | No |
| Community Edition | Limited |

For long-lived enterprise systems, licensing becomes a strategic architectural consideration.

---

# Comparison with OpenIddict

| Capability | OpenIddict | Duende |
|------------|------------|---------|
| OAuth2 | Excellent | Excellent |
| OpenID Connect | Excellent | Excellent |
| Enterprise Features | Very Good | Excellent |
| Licensing | Open Source | Commercial |
| .NET Integration | Excellent | Excellent |
| Operational Complexity | Moderate | Higher |

---

# Architectural Fit

| Criterion | Assessment |
|-----------|------------|
| Clean Architecture | Excellent |
| Enterprise Security | Excellent |
| Standards Compliance | Excellent |
| Cloud Neutrality | Excellent |
| Cost Efficiency | Moderate |
| Enterprise Readiness | Excellent |

---

# Preliminary Conclusion

Duende IdentityServer is an outstanding enterprise authorization server.

However, MachineryManagerEnterprise prioritizes:

- provider independence;
- cost efficiency;
- open-source ecosystem;
- maintainability.

Because OpenIddict provides the required functionality without introducing commercial licensing, Duende does not offer sufficient architectural benefit to justify becoming the primary identity platform.

Duende may remain a supported alternative for organizations that already standardize on it, but it should not be the recommended default.

---


# 8. JSON Web Token (JWT) Evaluation

## Overview

JSON Web Token (JWT), defined by RFC 7519, is the de facto standard format for stateless access tokens in modern distributed systems.

A JWT contains digitally signed claims that can be validated by resource servers without requiring access to centralized session state.

Within MachineryManagerEnterprise, JWT is evaluated as the primary access token format.

---

# Architectural Role

JWT belongs to the Token layer.

```text
User

      │

      ▼

Authentication

      │

      ▼

Authorization Server

      │

      ▼

JWT Access Token

      │

      ▼

Protected API
```

The token represents authenticated identity and authorization claims.

---

# Architectural Strengths

## Advantages

- Industry standard.
- Stateless.
- High performance.
- Self-contained.
- Excellent interoperability.
- Native .NET support.
- Cloud neutral.
- Microservice friendly.
- API friendly.
- Excellent ecosystem support.

---

# Architectural Weaknesses

Because JWT is self-contained, issued tokens remain valid until expiration.

Consequently:

- immediate revocation is difficult;
- permissions cannot be changed for already-issued tokens;
- very long expiration periods increase security risk.

These limitations are commonly mitigated by:

- short-lived access tokens;
- refresh tokens;
- key rotation.

---

# Operational Characteristics

JWT provides:

- authenticated identity;
- claims transport;
- stateless validation;
- cryptographic integrity.

Operational complexity is very low.

---

# Scalability

JWT is exceptionally scalable.

Resource servers validate tokens locally.

No centralized session storage is required.

Scalability is considered excellent.

---

# Reliability

JWT introduces no runtime dependency on an authentication server during validation.

This significantly improves availability of protected APIs.

Reliability is considered excellent.

---

# Security

Security depends upon proper implementation.

Recommended practices include:

- signed tokens;
- short expiration;
- HTTPS only;
- refresh token rotation;
- strong signing keys.

Security is considered excellent when implemented correctly.

---

# Standards Compliance

JWT complies with:

- RFC 7519
- OAuth 2.x
- OpenID Connect

Standards compliance is excellent.

---

# Deployment Flexibility

Supported environments:

- Windows
- Linux
- Containers
- Kubernetes
- Cloud
- Hybrid
- On-Premise

Deployment flexibility is excellent.

---

# AI Compatibility

JWT is ideal for securing AI endpoints.

Examples include:

- AI assistant APIs;
- embedding services;
- semantic search;
- inference endpoints;
- machine-to-machine AI communication.

AI compatibility is considered excellent.

---

# Maintainability

JWT is:

- mature;
- well documented;
- widely supported.

Maintainability is excellent.

---

# Typical Usage

Suitable scenarios:

```text
REST APIs

Desktop Clients

Mobile Clients

Microservices

Machine-to-Machine Authentication

AI Services
```

Unsuitable scenarios:

```text
Immediate Session Revocation

Very Long Sessions

Stateful Web Sessions
```

---

# Relationship with OpenIddict

```text
OpenIddict

      │

Issues

      ▼

JWT

      │

Validated by

      ▼

Protected APIs
```

OpenIddict issues JWTs.

Applications validate JWTs.

---

# Architectural Fit

| Criterion | Assessment |
|-----------|------------|
| Performance | Excellent |
| Scalability | Excellent |
| Standards Compliance | Excellent |
| Cloud Neutrality | Excellent |
| Enterprise Readiness | Excellent |
| AI Compatibility | Excellent |

---

# Preliminary Conclusion

JWT should become the standard access token format for MachineryManagerEnterprise.

Its stateless architecture, industry adoption, and excellent scalability make it the strongest choice for securing:

- APIs;
- desktop clients;
- AI services;
- future distributed components.

The recommended implementation shall combine:

- short-lived JWT access tokens;
- refresh token rotation;
- secure signing key management.

---


# 9. Reference Token Evaluation

## Overview

Reference Tokens are opaque identifiers that represent an authenticated session rather than carrying user claims directly.

Unlike JWTs, a Reference Token contains **no readable information**.

Each API request requires token introspection against the Authorization Server to retrieve identity and authorization information.

Reference Tokens are commonly used in highly security-sensitive environments where immediate token revocation is a primary requirement.

---

# Architectural Role

Reference Tokens belong to the Token layer.

```text
User

      │

      ▼

Authentication

      │

      ▼

Authorization Server

      │

Issues

      ▼

Reference Token

      │

Introspection

      ▼

Authorization Server

      │

Returns Claims

      ▼

Protected API
```

Unlike JWT, the API cannot validate the token independently.

---

# Architectural Strengths

## Advantages

- Immediate revocation.
- Centralized authorization.
- No sensitive data inside token.
- Smaller token size.
- Excellent security control.
- Supports dynamic permission changes.
- Suitable for zero-trust environments.

---

# Architectural Weaknesses

Reference Tokens introduce additional infrastructure dependency.

Every protected request requires:

- network communication;
- token introspection;
- authorization server availability.

Consequently:

- increased latency;
- higher infrastructure load;
- reduced availability during authorization server outages.

---

# Operational Characteristics

Reference Tokens require:

- token store;
- introspection endpoint;
- authorization server availability;
- token cache (recommended).

Operational complexity is considered high.

---

# Scalability

Scalability depends upon the Authorization Server.

Without aggressive caching:

- every API request performs introspection.

Large-scale distributed systems therefore require additional caching layers.

Scalability is considered moderate.

---

# Reliability

Because APIs depend upon Authorization Server availability:

- authorization outages affect all protected APIs.

Reliability is therefore lower than JWT.

---

# Security

Reference Tokens provide several important security benefits:

- immediate revocation;
- centralized authorization;
- permission changes become effective immediately;
- stolen tokens expose no claims.

Security is considered outstanding.

---

# Standards Compliance

Reference Tokens are fully compatible with:

- OAuth 2.x
- OpenID Connect

They are widely used in enterprise authorization systems.

---

# Deployment Flexibility

Supported environments include:

- Windows
- Linux
- Containers
- Kubernetes
- Cloud
- Hybrid
- On-Premise

Deployment flexibility is excellent.

---

# AI Compatibility

Reference Tokens can secure AI endpoints.

However, AI workloads often generate very high request volumes.

Continuous token introspection may introduce unnecessary latency for:

- semantic search;
- embedding generation;
- inference services.

---

# Maintainability

Reference Tokens require maintaining:

- token storage;
- introspection endpoint;
- caching infrastructure;
- authorization server health.

Maintainability is considered moderate.

---

# Comparison with JWT

| Capability | JWT | Reference Token |
|------------|-----|-----------------|
| Stateless | Excellent | Poor |
| Immediate Revocation | Poor | Excellent |
| Performance | Excellent | Moderate |
| Infrastructure Dependency | Low | High |
| API Scalability | Excellent | Moderate |
| Security | Excellent | Excellent |

---

# Typical Usage

Suitable scenarios:

```text
Highly Sensitive APIs

Financial Systems

Government Systems

Zero-Trust Architectures
```

Less suitable scenarios:

```text
Public APIs

High-throughput Microservices

AI Inference

Large-scale Distributed APIs
```

---

# Architectural Fit

| Criterion | Assessment |
|-----------|------------|
| Security | Excellent |
| Revocation | Excellent |
| Performance | Moderate |
| Scalability | Moderate |
| Enterprise Readiness | Excellent |
| Cloud Neutrality | Excellent |

---

# Relationship with JWT

The two token formats address different priorities.

```text
High Performance

        │

       JWT

----------------------------

Immediate Revocation

        │

Reference Token
```

JWT optimizes scalability.

Reference Tokens optimize centralized control.

---

# Preliminary Conclusion

Reference Tokens provide excellent security and immediate revocation capabilities.

However, MachineryManagerEnterprise prioritizes:

- high performance;
- scalable APIs;
- AI integration;
- distributed architecture.

These priorities align more closely with short-lived JWT access tokens combined with refresh tokens.

Reference Tokens should therefore remain an optional strategy for specialized high-security deployments rather than the platform's default access token format.

---


# 10. Microsoft Entra ID Evaluation

## Overview

Microsoft Entra ID (formerly Azure Active Directory) is Microsoft's enterprise Identity Provider (IdP) and Identity and Access Management (IAM) platform.

It provides:

- enterprise authentication;
- Single Sign-On (SSO);
- OAuth 2.1;
- OpenID Connect;
- SAML 2.0;
- Conditional Access;
- Multi-Factor Authentication (MFA);
- enterprise directory services.

Within MachineryManagerEnterprise it is evaluated as an external enterprise identity provider rather than the application's primary identity system.

---

# Architectural Role

Microsoft Entra ID belongs to the External Identity Provider layer.

```text
Enterprise User

        │

        ▼

Microsoft Entra ID

        │

OpenID Connect

        │

        ▼

MachineryManagerEnterprise

        │

Authentication Abstraction

        ▼

Business Modules
```

The application consumes authenticated identities rather than managing enterprise credentials itself.

---

# Architectural Strengths

## Advantages

- Enterprise-grade identity platform.
- Native OAuth 2.1 support.
- OpenID Connect support.
- Single Sign-On.
- Conditional Access.
- Multi-Factor Authentication.
- Enterprise governance.
- Large enterprise adoption.
- Excellent Microsoft ecosystem integration.
- Managed cloud infrastructure.

---

# Architectural Weaknesses

The primary limitation is platform dependence.

Using Microsoft Entra ID introduces:

- Azure ecosystem dependency;
- tenant administration requirements;
- cloud-provider coupling;
- licensing considerations for advanced capabilities.

These characteristics reduce cloud neutrality.

---

# Operational Characteristics

Microsoft Entra ID provides:

- enterprise authentication;
- centralized identity management;
- SSO;
- external federation;
- organizational directory.

Operational complexity for the application is low because identity infrastructure is externally managed.

---

# Scalability

Microsoft Entra ID is designed for global enterprise deployments.

It supports:

- millions of users;
- worldwide availability;
- distributed authentication;
- enterprise federation.

Scalability is considered excellent.

---

# Reliability

Reliability benefits include:

- Microsoft's globally distributed infrastructure;
- high availability;
- resilient authentication services.

Reliability is considered excellent.

---

# Security

Security capabilities include:

- MFA;
- Conditional Access;
- Identity Protection;
- Passwordless Authentication;
- Risk-based authentication;
- Device compliance integration.

Security is considered outstanding.

---

# Standards Compliance

Supported standards include:

- OAuth 2.x
- OpenID Connect
- SAML 2.0
- SCIM

Standards compliance is excellent.

---

# Deployment Flexibility

Supported environments:

- Azure
- Microsoft 365
- Hybrid Enterprise

Although standards-based protocols allow integration with non-Azure applications, the identity infrastructure itself remains Microsoft-hosted.

Deployment flexibility is therefore considered **moderate**.

---

# AI Compatibility

Microsoft Entra ID integrates naturally with:

- Azure OpenAI;
- Microsoft Copilot ecosystem;
- Azure AI services;
- Microsoft Graph.

Compatibility with Microsoft's AI ecosystem is excellent.

---

# Maintainability

Identity infrastructure maintenance is largely delegated to Microsoft.

Advantages include:

- automatic updates;
- managed security;
- enterprise administration tools.

Maintainability is considered excellent.

---

# Typical Usage

Suitable scenarios:

```text
Corporate Employees

Enterprise SSO

Azure-based Organizations

Microsoft 365 Integration

Hybrid Enterprise Identity
```

Less suitable scenarios:

```text
Standalone Commercial Software

Cloud-Neutral Products

Independent Customer Deployments
```

---

# Comparison with Local Identity

| Capability | ASP.NET Core Identity | Microsoft Entra ID |
|------------|----------------------|--------------------|
| Local User Management | Excellent | No |
| Enterprise SSO | Limited | Excellent |
| OAuth2 Provider | Through OpenIddict | Excellent |
| Cloud Neutrality | Excellent | Moderate |
| Enterprise Federation | Limited | Excellent |
| Licensing Simplicity | Excellent | Moderate |

---

# Architectural Fit

| Criterion | Assessment |
|-----------|------------|
| Enterprise Security | Excellent |
| Standards Compliance | Excellent |
| Cloud Neutrality | Moderate |
| Clean Architecture | Excellent |
| Enterprise Readiness | Excellent |
| Cost Efficiency | Good |

---

# Relationship with OpenIddict

OpenIddict and Microsoft Entra ID are not competitors.

Typical integration:

```text
Microsoft Entra ID

        │

External Login

        ▼

MachineryManagerEnterprise

        │

ASP.NET Core Identity

        │

OpenIddict

        ▼

Application JWT
```

This architecture allows the application to accept enterprise identities while maintaining its own authorization model.

---

# Preliminary Conclusion

Microsoft Entra ID is an excellent enterprise identity provider for organizations already invested in the Microsoft ecosystem.

However, MachineryManagerEnterprise explicitly targets:

- cloud neutrality;
- deployment independence;
- provider independence.

Therefore Microsoft Entra ID should be treated as an **optional external authentication provider**, not as the platform's primary identity solution.

The recommended core identity architecture remains:

- ASP.NET Core Identity
- OpenIddict
- JWT

with Microsoft Entra ID available as an enterprise federation option.

---


# 11. Google Identity Evaluation

## Overview

Google Identity provides authentication services for users possessing Google Accounts.

It supports:

- OpenID Connect;
- OAuth 2.x;
- Single Sign-On;
- Social Login;
- Mobile authentication;
- Web authentication.

Google Identity is evaluated as an optional external identity provider for MachineryManagerEnterprise.

Its primary purpose is to simplify authentication for external users rather than enterprise workforce identity.

---

# Architectural Role

Google Identity belongs to the External Authentication Provider layer.

```text
Google User

      │

      ▼

Google Identity

      │

OpenID Connect

      ▼

MachineryManagerEnterprise

      │

Authentication Abstraction

      ▼

Business Modules
```

Business logic remains completely independent from Google-specific APIs.

---

# Architectural Strengths

## Advantages

- Industry standard protocols.
- Excellent OpenID Connect implementation.
- Massive user adoption.
- Simple user onboarding.
- Mature ecosystem.
- Native OAuth support.
- Broad platform compatibility.
- Minimal operational overhead.

---

# Architectural Weaknesses

Google Identity is intended for consumer authentication.

Limitations include:

- no enterprise directory;
- no enterprise governance;
- no organization-level authorization;
- unsuitable for internal workforce management.

It complements rather than replaces the platform's primary identity infrastructure.

---

# Operational Characteristics

Google Identity provides:

- authentication;
- identity verification;
- delegated login.

The application continues to own:

- authorization;
- roles;
- permissions;
- business identity.

Operational complexity is low.

---

# Scalability

Google Identity is globally distributed.

Scalability is considered excellent.

---

# Reliability

Authentication availability benefits from Google's global infrastructure.

Reliability is considered excellent.

---

# Security

Security capabilities include:

- MFA support;
- modern OAuth flows;
- OpenID Connect;
- PKCE;
- Google account security.

Security is considered excellent.

---

# Standards Compliance

Supported standards include:

- OAuth 2.x
- OpenID Connect

Standards compliance is excellent.

---

# Deployment Flexibility

Google Identity is cloud-hosted.

Applications remain portable because integration relies upon standard protocols.

Deployment flexibility is considered good.

---

# AI Compatibility

Google Identity has no special AI integration advantages.

It simply authenticates users before AI functionality becomes available.

Compatibility is considered neutral.

---

# Maintainability

Maintenance effort is minimal.

Google manages:

- authentication infrastructure;
- protocol evolution;
- security updates.

Maintainability is considered excellent.

---

# Typical Usage

Suitable scenarios:

```text
Customer Login

Public Applications

Partner Access

Consumer Authentication
```

Less suitable scenarios:

```text
Internal Enterprise Identity

Corporate Workforce

Organization-wide Authorization
```

---

# Comparison with ASP.NET Core Identity

| Capability | ASP.NET Core Identity | Google Identity |
|------------|----------------------|-----------------|
| Local User Management | Excellent | No |
| Consumer Login | Good | Excellent |
| Enterprise Identity | Good | Limited |
| External Authentication | Moderate | Excellent |
| Authorization | Excellent | No |

---

# Architectural Fit

| Criterion | Assessment |
|-----------|------------|
| Clean Architecture | Excellent |
| Standards Compliance | Excellent |
| Consumer Authentication | Excellent |
| Enterprise Identity | Moderate |
| Maintainability | Excellent |
| Enterprise Readiness | Good |

---

# Relationship with ASP.NET Core Identity

Google Identity authenticates the user.

ASP.NET Core Identity owns the application's authorization model.

```text
Google Identity

      │

Authentication

      ▼

ASP.NET Core Identity

      │

Application User

      ▼

Authorization
```

This separation preserves Clean Architecture while supporting external authentication.

---

# Preliminary Conclusion

Google Identity is an excellent optional authentication provider for customer-facing scenarios.

It should be supported as an external login provider.

However, it should not replace the platform's primary identity infrastructure based upon:

- ASP.NET Core Identity;
- OpenIddict;
- JWT.

---


# 12. GitHub Identity Evaluation

## Overview

GitHub Identity provides OAuth 2.0 and OpenID Connect authentication for users with GitHub accounts.

It is primarily intended for:

- developers;
- open-source communities;
- DevOps platforms;
- engineering portals.

Within MachineryManagerEnterprise, GitHub Identity is evaluated as an optional external authentication provider for development-oriented deployments.

It is **not** intended to become a primary enterprise identity provider.

---

# Architectural Role

GitHub Identity belongs to the External Authentication Provider layer.

```text
GitHub User

      │

      ▼

GitHub Identity

      │

OAuth2 / OIDC

      ▼

MachineryManagerEnterprise

      │

Authentication Abstraction

      ▼

Business Modules
```

Business modules remain completely unaware of GitHub-specific authentication.

---

# Architectural Strengths

## Advantages

- Simple OAuth integration.
- OpenID Connect support.
- Excellent developer adoption.
- Mature APIs.
- Minimal infrastructure.
- Useful for engineering environments.
- Cross-platform.

---

# Architectural Weaknesses

GitHub Identity is not designed for enterprise workforce identity.

Limitations include:

- no enterprise directory;
- no organizational identity governance;
- no enterprise authorization model;
- unsuitable for business user management.

Its usefulness is primarily limited to developer-facing applications.

---

# Operational Characteristics

GitHub Identity provides:

- authentication;
- identity verification;
- OAuth authorization.

The application remains responsible for:

- authorization;
- roles;
- permissions;
- application user lifecycle.

Operational complexity is very low.

---

# Scalability

GitHub Identity benefits from GitHub's global infrastructure.

Scalability is considered excellent.

---

# Reliability

Authentication availability is generally excellent.

However, application availability depends upon GitHub authentication services during login.

Reliability is considered very good.

---

# Security

Security capabilities include:

- OAuth 2.x;
- OpenID Connect;
- PKCE;
- GitHub account protection;
- MFA support.

Security is considered excellent.

---

# Standards Compliance

Supported standards include:

- OAuth 2.x
- OpenID Connect

Standards compliance is excellent.

---

# Deployment Flexibility

GitHub Identity is cloud-hosted.

Applications remain portable because integration uses standard protocols.

Deployment flexibility is considered good.

---

# AI Compatibility

GitHub Identity provides no AI-specific capabilities.

It simply authenticates users before AI services are accessed.

Compatibility is considered neutral.

---

# Maintainability

Maintenance effort is minimal.

GitHub maintains:

- authentication infrastructure;
- OAuth implementation;
- protocol evolution.

Maintainability is considered excellent.

---

# Typical Usage

Suitable scenarios:

```text
Developer Portals

Engineering Tools

Internal DevOps Systems

Open Source Communities
```

Less suitable scenarios:

```text
Enterprise Workforce

Customer Identity

Corporate SSO
```

---

# Comparison with Google Identity

| Capability | Google Identity | GitHub Identity |
|------------|----------------|-----------------|
| Consumer Adoption | Excellent | Good |
| Developer Adoption | Good | Excellent |
| Enterprise Identity | Moderate | Limited |
| OAuth Support | Excellent | Excellent |
| OpenID Connect | Excellent | Excellent |

---

# Architectural Fit

| Criterion | Assessment |
|-----------|------------|
| Clean Architecture | Excellent |
| Standards Compliance | Excellent |
| Developer Authentication | Excellent |
| Enterprise Identity | Limited |
| Maintainability | Excellent |
| Enterprise Readiness | Moderate |

---

# Relationship with ASP.NET Core Identity

GitHub Identity authenticates the developer.

ASP.NET Core Identity owns the application's authorization model.

```text
GitHub Identity

      │

Authentication

      ▼

ASP.NET Core Identity

      │

Application User

      ▼

Authorization
```

This separation preserves Clean Architecture while supporting external developer authentication.

---

# Preliminary Conclusion

GitHub Identity is an excellent optional authentication provider for developer-oriented scenarios.

It should be supported only where GitHub-based authentication provides business value.

It should **not** replace the primary authentication architecture based upon:

- ASP.NET Core Identity;
- OpenIddict;
- JWT.

---


# 13. Overall Technology Comparison

Authentication within MachineryManagerEnterprise consists of multiple complementary responsibilities.

No single technology satisfies every architectural requirement.

The recommended architecture separates:

- identity management;
- token issuance;
- access token format;
- external authentication providers.

---

# Responsibility Matrix

| Responsibility | Recommended Technology | Alternative | Purpose |
|----------------|------------------------|-------------|---------|
| Local User Management | ASP.NET Core Identity | Custom Identity Store | User Accounts |
| OAuth2 / OIDC Server | OpenIddict | Duende IdentityServer | Authorization Server |
| Access Token Format | JWT | Reference Token | API Authentication |
| Enterprise External Identity | Microsoft Entra ID | Other Enterprise IdPs | Corporate SSO |
| Consumer Authentication | Google Identity | GitHub Identity | External Login |
| Developer Authentication | GitHub Identity | Google Identity | Developer Login |

---

# Capability Comparison

| Capability | ASP.NET Core Identity | OpenIddict | Duende | JWT | Reference Token | Entra ID | Google | GitHub |
|------------|----------------------|------------|---------|-----|-----------------|-----------|---------|---------|
| User Management | Excellent | No | No | No | No | Moderate | No | No |
| OAuth2 / OIDC | Limited | Excellent | Excellent | Format Only | Format Only | Excellent | Excellent | Excellent |
| Authorization Server | No | Excellent | Excellent | No | No | Excellent | No | No |
| Stateless Authentication | No | Yes | Yes | Excellent | Poor | Yes | Yes | Yes |
| Immediate Revocation | Moderate | Excellent | Excellent | Poor | Excellent | Excellent | Good | Good |
| Enterprise SSO | Limited | Good | Excellent | No | No | Excellent | Limited | Limited |
| Consumer Login | Moderate | Moderate | Moderate | No | No | Moderate | Excellent | Good |
| Developer Login | Moderate | Moderate | Moderate | No | No | Moderate | Good | Excellent |
| Licensing | Open Source | Open Source | Commercial | Open Standard | Open Standard | Microsoft Licensing | Free | Free |
| Cloud Neutrality | Excellent | Excellent | Excellent | Excellent | Excellent | Moderate | Good | Good |

---

# Cloud Neutrality Assessment

| Technology | Cloud Neutrality |
|------------|-----------------|
| ASP.NET Core Identity | Excellent |
| OpenIddict | Excellent |
| JWT | Excellent |
| Reference Tokens | Excellent |
| Duende IdentityServer | Excellent |
| Microsoft Entra ID | Moderate |
| Google Identity | Good |
| GitHub Identity | Good |

---

# Enterprise Suitability

| Technology | Enterprise Readiness |
|------------|---------------------|
| ASP.NET Core Identity | Excellent |
| OpenIddict | Excellent |
| JWT | Excellent |
| Reference Tokens | Excellent |
| Duende IdentityServer | Excellent |
| Microsoft Entra ID | Excellent |
| Google Identity | Good |
| GitHub Identity | Moderate |

---

# AI Compatibility

| Technology | AI Compatibility |
|------------|------------------|
| ASP.NET Core Identity | Excellent |
| OpenIddict | Excellent |
| JWT | Excellent |
| Reference Tokens | Good |
| Microsoft Entra ID | Excellent (Azure Ecosystem) |
| Google Identity | Neutral |
| GitHub Identity | Neutral |

---

# Clean Architecture Compliance

The preferred architecture maintains strict separation of responsibilities.

```text
                External Identity Providers

      ┌──────────────┬──────────────┬──────────────┐
      │              │              │
      ▼              ▼              ▼

 Microsoft Entra   Google      GitHub Identity

                 │

                 ▼

        ASP.NET Core Identity

                 │

                 ▼

             OpenIddict

                 │

                 ▼

          JWT Access Tokens

                 │

                 ▼

          Protected Resources

                 │

                 ▼

            Business Modules
```

This layering ensures:

- provider independence;
- standards compliance;
- infrastructure isolation;
- future extensibility.

---

# Cost Comparison

| Technology | Cost |
|------------|------|
| ASP.NET Core Identity | Free |
| OpenIddict | Free |
| JWT | Free |
| Reference Tokens | Free |
| Microsoft Entra ID | License Dependent |
| Google Identity | Free |
| GitHub Identity | Free |
| Duende IdentityServer | Commercial License |

---

# Risk Assessment

| Technology | Primary Risk |
|------------|--------------|
| ASP.NET Core Identity | Local identity maintenance |
| OpenIddict | Authorization server configuration |
| JWT | Token revocation strategy |
| Reference Tokens | Performance overhead |
| Microsoft Entra ID | Vendor dependency |
| Google Identity | Consumer-only identity |
| GitHub Identity | Developer-only identity |
| Duende IdentityServer | Commercial licensing |

---

# Overall Evaluation

| Criterion | Recommended Choice |
|-----------|--------------------|
| Local Identity | ASP.NET Core Identity |
| Authorization Server | OpenIddict |
| Access Tokens | JWT |
| Enterprise Federation | Microsoft Entra ID (Optional) |
| Consumer Login | Google Identity (Optional) |
| Developer Login | GitHub Identity (Optional) |

The technologies complement each other rather than compete. Together they form a complete, standards-based authentication architecture that aligns with MachineryManagerEnterprise's goals of Clean Architecture, cloud neutrality, maintainability, and future scalability.

---


# 14. Final Recommendation

After evaluating all candidate technologies, the following authentication architecture is recommended for MachineryManagerEnterprise.

## Core Authentication Stack

| Responsibility | Selected Technology | Rationale |
|----------------|---------------------|-----------|
| Local Identity Management | ASP.NET Core Identity | Mature, secure, fully integrated with .NET 10 |
| Authorization Server | OpenIddict | Open-source, standards compliant, cloud-neutral |
| Access Token Format | JWT | High-performance, stateless, scalable |
| Refresh Tokens | OpenIddict | Secure token lifecycle management |
| Authorization Model | Policy + Claims + Roles | Flexible enterprise authorization |

---

## Optional External Identity Providers

The following providers should be supported as optional authentication sources.

| Provider | Purpose | Status |
|----------|---------|--------|
| Microsoft Entra ID | Enterprise SSO | Optional |
| Google Identity | Customer Authentication | Optional |
| GitHub Identity | Developer Authentication | Optional |

These providers authenticate users but **do not replace** the application's internal authorization model.

---

# Recommended Authentication Architecture

```text
                External Identity Providers

        ┌────────────┬────────────┬────────────┐
        │            │            │
        ▼            ▼            ▼

 Microsoft Entra   Google      GitHub

                  │

                  ▼

        ASP.NET Core Identity

                  │

                  ▼

              OpenIddict

                  │

                  ▼

      JWT Access / Refresh Tokens

                  │

                  ▼

          Authorization Middleware

                  │

                  ▼

            Business Modules
```

---

# Recommended Authorization Strategy

Authentication determines **who** the caller is.

Authorization determines **what** the caller is allowed to do.

The platform should implement authorization using:

- Claims
- Roles
- Policies
- Resource-based Authorization (when required)

Business logic shall **never** inspect JWTs directly.

Instead, business services consume only the authenticated `ClaimsPrincipal` exposed by the authentication abstraction.

---

# Token Strategy

| Token Type | Lifetime | Purpose |
|------------|----------|---------|
| Access Token (JWT) | Short-lived (e.g., 10–30 minutes) | API authorization |
| Refresh Token | Long-lived | Session continuation |
| Reference Token | Not used by default | Reserved for specialized deployments |

This approach provides:

- excellent scalability;
- minimal infrastructure dependency;
- strong security;
- standards compliance.

---

# Security Recommendations

The following security practices are mandatory:

- HTTPS everywhere;
- signed JWT tokens;
- short-lived access tokens;
- refresh token rotation;
- secure signing-key storage;
- periodic signing-key rotation;
- policy-based authorization;
- least-privilege principle;
- MFA support (when required);
- external provider isolation.

---

# Cloud Neutrality

The recommended architecture intentionally avoids cloud lock-in.

Core components remain:

- open-source;
- provider independent;
- deployable on-premises;
- deployable in any cloud;
- compatible with Kubernetes.

Cloud-specific providers (such as Microsoft Entra ID) are treated as optional integrations.

---

# AI Readiness

The selected architecture fully supports future AI capabilities.

Examples include:

- authenticated AI assistants;
- secured embedding services;
- protected semantic-search APIs;
- machine-to-machine AI communication;
- AI background processing.

JWT-based authentication minimizes latency for AI workloads while remaining standards compliant.

---

# Final Decision

The Architecture Review Board approves the following authentication stack as the standard platform architecture.

| Component | Decision |
|----------|----------|
| ASP.NET Core Identity | Approved |
| OpenIddict | Approved |
| JWT Access Tokens | Approved |
| Refresh Tokens | Approved |
| Microsoft Entra ID | Optional |
| Google Identity | Optional |
| GitHub Identity | Optional |
| Duende IdentityServer | Rejected (commercial licensing not justified) |
| Reference Tokens | Not selected as default |

---

# Decision Summary

The selected solution satisfies all architectural objectives:

- ✔ Clean Architecture
- ✔ Enterprise Security
- ✔ .NET 10 Compatibility
- ✔ Cloud Neutrality
- ✔ Standards Compliance
- ✔ High Scalability
- ✔ Low Operational Complexity
- ✔ AI Readiness
- ✔ Long-term Maintainability

This authentication architecture is therefore adopted as the enterprise standard for MachineryManagerEnterprise.

---

# Revision History

| Version | Date | Author | Description |
|----------|------|--------|-------------|
| 1.0.0 | 2026-07-27 | Solution Architect | Initial version |
| 1.3.0   | 2026-07-28 | New section added (Evaluation Scope) |