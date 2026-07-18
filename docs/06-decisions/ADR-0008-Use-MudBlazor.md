# ADR-0008 — Use MudBlazor as the UI Framework and Material Design as the Design System

**Status:** Accepted

**Date:** 2026-07-18

**Decision Makers**

- Solution Architect
- Development Team

---

# Context

MachineryManagerEnterprise requires a modern UI framework capable of supporting:

- .NET 10
- Blazor Web App
- Responsive Design
- Enterprise Applications
- Accessibility
- Long-term Maintainability
- Component Consistency

The project follows the Open Source First Policy.

---

# Problem

The Presentation Layer requires:

- Rich reusable components
- Unified visual language
- Consistent user experience
- Long-term support
- Large community
- Active development

Without a defined design system, the UI becomes inconsistent over time.

---

# Considered Options

- MudBlazor
- Fluent UI Blazor
- Ant Design Blazor
- Blazorise
- Radzen Blazor Community

See:

TE-0004 — UI Framework Evaluation

---

# Decision

The project adopts:

- **MudBlazor** as the primary UI Component Library.
- **Material Design** as the official Design System.

---

# Architectural Rules

## UI Components

All new pages shall use MudBlazor components whenever possible.

---

## Design Language

Material Design becomes the official design language.

The following principles shall remain consistent:

- Typography
- Spacing
- Elevation
- Color System
- Icons
- Forms
- Dialogs
- Navigation

---

## Custom Components

Reusable business components shall be built on top of MudBlazor rather than replacing it.

---

## Third-party UI Components

Additional UI libraries are prohibited unless:

1. MudBlazor lacks the required capability.
2. A Technology Evaluation is completed.
3. A dedicated ADR is approved.

---

## Styling

Project-wide styling shall use:

- MudTheme
- CSS Variables where appropriate
- Component encapsulation

Large amounts of page-specific CSS should be avoided.

---

## Icons

Material Icons shall be the default icon set.

---

## Responsiveness

Layouts shall be mobile-first and responsive.

MudGrid and MudBreakpointProvider should be preferred.

---

## Accessibility

UI components shall preserve accessibility features provided by MudBlazor whenever possible.

---

# Consequences

## Positive

- Unified user experience
- Faster UI development
- Consistent design language
- Large Open Source ecosystem
- Excellent documentation

---

## Negative

- Material Design conventions become part of the project identity.
- Future migration to another UI framework would require substantial Presentation Layer changes.

---

# Constraints

Business logic shall never exist inside UI components.

Presentation remains responsible only for:

- Rendering
- User interaction
- Input collection

Business rules remain inside Application and Domain layers.

---

# Future Considerations

Additional Open Source component libraries may be integrated only after architectural review.

MudBlazor remains the foundation of the Presentation Layer.

---

# Related Documents

- ADR-0001 — Clean Architecture
- ADR-0002 — Open Source First Policy
- TE-0004 — UI Framework Evaluation

---

# References

- MudBlazor Documentation
- Material Design Guidelines
- Microsoft Blazor Documentation