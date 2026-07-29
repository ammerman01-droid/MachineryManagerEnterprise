| Property | Value |
|----------|-------|
| **ADR ID** | ADR-0028 |
| **Title** | Client UI Architecture |
| **Status** | Accepted |
| **Version** | 1.0.0 |
| **Decision Date** | 2026-07-28 |
| **Owner** | Solution Architect |
| **Related TE** | TE-0034 – Client UI Technology Evaluation |

---

# Context

MachineryManagerEnterprise is a desktop-first enterprise application intended for long-term maintenance and cross-platform deployment.

The presentation layer must:

- remain completely independent from business logic;
- support modular application growth;
- provide a modern enterprise user experience;
- follow Clean Architecture principles.

The user interface therefore requires a consistent architectural foundation.

---

# Problem

Without a standardized UI architecture:

- presentation logic migrates into Views;
- business rules leak into UI components;
- modules become tightly coupled;
- testing becomes difficult;
- maintainability deteriorates over time.

---

# Decision Drivers

The client architecture shall provide:

- Cross-platform execution
- Clean MVVM separation
- Modular UI composition
- High maintainability
- Excellent testability
- Native desktop experience
- Fluent user interface
- Long-term architectural stability

---

# Decision

The client application shall adopt the following architecture.

| Responsibility | Technology |
|---------------|------------|
| Desktop Framework | Avalonia UI |
| Visual Component Library | FluentAvalonia |
| MVVM Framework | CommunityToolkit.Mvvm |

ReactiveUI is not adopted.

---

# Client Architecture

```text
             User

              │

              ▼

        Avalonia Views

              │

 Data Binding / Commands

              │

              ▼

 ViewModels
 (CommunityToolkit.Mvvm)

              │

              ▼

 Application Layer

              │

              ▼

 Domain Layer
```

Business logic shall never execute inside Views.

---

# Layer Responsibilities

## Views

Views are responsible only for:

- visual presentation;
- control layout;
- data binding;
- command binding;
- visual state.

Views shall contain no business logic.

---

## ViewModels

ViewModels are responsible for:

- presentation state;
- user interaction coordination;
- command execution;
- validation presentation;
- interaction with the Application Layer.

ViewModels shall not directly access Infrastructure components.

---

## Application Layer

The Application Layer:

- executes business use cases;
- coordinates domain operations;
- returns DTOs;
- remains completely UI independent.

---

# MVVM Strategy

CommunityToolkit.Mvvm shall be used for:

- ObservableObject
- ObservableProperty
- RelayCommand
- AsyncRelayCommand
- Validation
- Messenger

Source generators shall replace manual notification code whenever possible.

---

# UI Component Strategy

FluentAvalonia becomes the standard component library.

Preferred controls include:

- NavigationView
- ContentDialog
- InfoBar
- CommandBar
- Settings Controls
- Fluent Icons

Custom controls shall only be created when equivalent Fluent controls do not exist.

---

# Navigation Strategy

Navigation shall be ViewModel-driven.

Views shall never directly navigate to other Views.

Navigation services shall remain independent from business logic.

---

# Dependency Injection

Views and ViewModels shall be created through Dependency Injection.

Manual object construction is prohibited except where required by framework infrastructure.

---

# Validation Strategy

Validation shall occur in the Application Layer.

ViewModels present validation results.

Views only visualize validation state.

---

# Threading Strategy

Long-running operations shall execute asynchronously.

UI updates shall always occur on the UI thread.

Blocking the UI thread is prohibited.

---

# Theme Strategy

Supported themes:

- Light
- Dark
- System

Theme selection shall be centralized and applied consistently across the application.

---

# Modularity

Each business module may contribute:

- Views
- ViewModels
- Resources
- Navigation Entries

Modules shall not directly reference one another.

Communication occurs only through approved application interfaces.

---

# Testing Strategy

ViewModels shall be fully unit testable.

Views require minimal testing because business behavior resides within ViewModels.

UI automation is addressed separately in the testing strategy.

---

# Performance Strategy

Performance shall be optimized through:

- Virtualized controls
- Asynchronous commands
- Minimal allocations
- Lazy loading where appropriate

---

# Security

The presentation layer:

- shall not store secrets;
- shall not perform cryptographic operations;
- shall not implement authorization logic.

Security remains the responsibility of lower architectural layers.

---

# Benefits

This architecture provides:

- Clear separation of concerns
- Excellent maintainability
- High testability
- Modular desktop application structure
- Native cross-platform user experience

---

# Consequences

Positive

- Clean MVVM architecture
- Strong modularity
- Excellent long-term maintainability
- Consistent user experience

Negative

- Additional ViewModel layer
- Strict separation requires architectural discipline

These trade-offs are acceptable for enterprise software.

---

# Alternatives Considered

## Code-Behind Architecture

Rejected.

Business logic would migrate into Views.

---

## ReactiveUI

Rejected.

Reactive programming introduces unnecessary architectural complexity for the project's requirements.

---

## Windows-Only UI Frameworks

Rejected.

Cross-platform capability is an architectural requirement.

---

# Related Documents

- ADR-0001 — Clean Architecture
- ADR-0003 — Modular Monolith Architecture
- ADR-0024 — Enterprise Testing Strategy
- TE-0034 — Client UI Technology Evaluation

---

# Decision Outcome

**Accepted**

MachineryManagerEnterprise adopts:

- Avalonia UI
- FluentAvalonia
- CommunityToolkit.Mvvm

within a strict MVVM architecture where:

- Views contain presentation only;
- ViewModels coordinate presentation behavior;
- the Application Layer owns business logic;
- the Domain Layer remains completely UI independent.

---

# Revision History

| Version | Date | Author | Description |
|----------|------|--------|-------------|
| 1.0.0 | 2026-07-28 | Solution Architect | Initial version |