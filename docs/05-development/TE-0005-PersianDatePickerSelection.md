# Technology Evaluation — Persian Date Picker Selection

**Document ID**

TE-0005

---

# Purpose

This document evaluates the available approaches for supporting Persian (Jalali) date selection within MachineryManagerEnterprise.

The selected solution shall satisfy:

- Full Jalali calendar support
- Seamless integration with MudBlazor
- Consistent Material Design
- Long-term maintainability
- Open Source licensing
- .NET 10 compatibility

---

# Candidate Solutions

| Solution | Status |
|----------|--------|
| Blazor.PersianDatePicker | Evaluated |
| MudBlazor + Jalali Adapter | Evaluated |
| Custom MudDatePicker Extension | Evaluated |

---

# Evaluation Criteria

| Criterion | Weight |
|------------|-------:|
| MudBlazor Integration | High |
| Material Design Consistency | High |
| UI Consistency | High |
| Community | Medium |
| Documentation | Medium |
| Open Source | Mandatory |
| Long-term Maintainability | High |
| .NET 10 Compatibility | High |
| Customization | High |

---

# Comparison

| Feature | Blazor.PersianDatePicker | Mud + Adapter | Custom Extension |
|----------|:-----------------------:|:-------------:|:----------------:|
| Open Source | ✅ | ✅ | ✅ |
| MudBlazor Native Look | ❌ | ✅ | ✅ |
| Material Design | Partial | Complete | Complete |
| Separate Dependency | Yes | No | No |
| UI Consistency | Medium | Excellent | Excellent |
| Long-term Maintenance | Medium | Excellent | Medium |
| Development Cost | Low | Medium | High |
| Flexibility | Medium | Excellent | Excellent |
| .NET 10 | ✅ | ✅ | ✅ |

---

# Individual Analysis

## Blazor.PersianDatePicker

### Advantages

- Ready to use
- Mature Jalali picker
- Low implementation effort

### Disadvantages

- Visual style differs from MudBlazor
- Additional UI dependency
- Separate maintenance lifecycle

---

## MudBlazor + Jalali Adapter

### Advantages

- Unified Design System
- Native MudBlazor experience
- No additional UI framework
- Easier future maintenance
- Consistent UX

### Disadvantages

- Requires adapter implementation
- Slightly higher initial effort

---

## Custom MudDatePicker Extension

### Advantages

- Maximum flexibility
- Perfect UI consistency

### Disadvantages

- Highest development cost
- Largest maintenance burden

---

# Evaluation Score

| Solution | Score |
|----------|------:|
| MudBlazor + Jalali Adapter | **96 / 100** |
| Blazor.PersianDatePicker | **88 / 100** |
| Custom Extension | **83 / 100** |

---

# Final Evaluation

For MachineryManagerEnterprise, the preferred solution is:

**MudBlazor + Jalali Adapter**

Reasons:

- Preserves the official Design System.
- Avoids introducing an additional UI component library.
- Produces a consistent user experience.
- Reduces future dependency management.
- Fits naturally into the existing Presentation architecture.

---

# Recommendation

The project should first investigate implementing Jalali support directly on top of MudBlazor.

Blazor.PersianDatePicker remains an acceptable fallback solution if:

- Native MudBlazor integration proves impractical.
- Maintenance cost becomes excessive.
- Required functionality cannot be achieved.

---

# Future Strategy

A Proof of Concept (PoC) shall be developed before making the final architectural decision.

The final implementation choice will be documented in ADR-0009.

---

# Related Documents

- ADR-0002 — Open Source First Policy
- ADR-0008 — Use MudBlazor