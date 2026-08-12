| Property         | Value              |
|------------------|--------------------|
| **Document ID**  | POC-0001           |
| **Title**        | Proof of Concept — Jalali Support for MudBlazor DatePicker |
| **Version**      | 4.1.0              |
| **Status**       | Approved            |
| **Owner**        | Solution Architect |
| **Created**      | 2026-07-18         |
| **Last Updated** | 2026-08-08         |

---

# Purpose

Evaluate the feasibility of implementing a Jalali calendar while preserving
MudBlazor as the sole UI component framework.

---

# Objective

Evaluate whether MudBlazor DatePicker can fully support the Persian (Jalali) calendar without introducing an additional UI component library.

The purpose is validating the architectural feasibility of Jalali
calendar support prior to any formal Technology Evaluation being
authored. No dedicated Technology Evaluation exists for this topic yet
— it will be created only if this Proof of Concept succeeds. (A prior
revision of this document referenced "TE-0005 — Persian Date Picker
Selection," but TE-0005 is already assigned to FluentValidation; that
incorrect forward-reference has been removed.)

---

# Hypothesis

MudBlazor can remain the only UI component framework while Persian calendar functionality is provided through an adapter or conversion layer.

If successful:

- UI consistency is preserved.
- Material Design remains intact.
- No additional UI dependency is required.

---

# Success Criteria

The Proof of Concept is considered successful if all of the following conditions are met.

## Calendar

- Jalali calendar displayed correctly.

# Performance

Calendar rendering shall not introduce noticeable latency compared to the
standard MudDatePicker.

---

# Evaluation Matrix

| Requirement | Pass | Fail |
|-------------|:----:|:----:|
| Jalali Calendar | | |
| RTL | | |
| Validation | | |
| Localization | | |
| Browser Compatibility | | |
| UI Consistency | | |

---

## Date Selection

- User selects Persian dates.

---

## Conversion

Selected dates correctly convert to:

- DateOnly
- DateTime

using Gregorian values internally.

---

## Display

Dates are rendered in Persian format.

Example:

```
1405/07/15
```

---

## Localization

Support for:

- Persian culture
- RTL
- Persian month names
- Persian weekdays

---

## UX

Visual appearance must remain identical to standard MudBlazor components.

No noticeable differences should exist between:

MudTextField

MudSelect

MudDatePicker

---

## Validation

FluentValidation shall continue validating DateOnly values without modification.

---

## Accessibility

Keyboard navigation shall continue working.

---

## Browser Support

Test:

- Edge
- Chrome
- Firefox

---

# Technical Investigation

Investigate whether MudBlazor currently supports:

- Custom DateAdapter
- Custom DateConverter
- Culture injection
- Localization
- Calendar customization

---

# Risks

Possible risks include:

- Calendar rendering limitations.
- Internal Gregorian assumptions.
- Upgrade compatibility.

---

# Alternatives

If the PoC fails:

Fallback solution:

Blazor.PersianDatePicker

---

# Expected Output

One of two conclusions:

## PASS

PASS

↓

Create ADR

↓

Implement

↓

Architecture Approved

Use MudBlazor Jalali Adapter

---

## FAIL

FAIL

↓

Create ADR

↓

Select Alternative

↓

Architecture Approved

Use Blazor.PersianDatePicker

---

# Status

Pending

---

# Decision Summary

- ✔ Clean Architecture
- ✔ .NET 10 Compatibility
- ✔ Standards Compliance
- ✔ Cloud Neutrality
- ✔ AI Readiness
- ✔ Long-term Maintainability

# Related Documents

- ADR-0005
- docs/07-api/
- MudBlazor Documentation

---

#  Revision History

| Version | Date       | Author             | Description                                           |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0   | 2026-07-18 | Solution Architect | Initial PoC definition                                |
| 3.0.0   | 2026-07-18 | Solution Architect | Standardized according to Documentation Standard v3.0 |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0             |
| 4.1.0   | 2026-08-08 | Solution Architect | Removed false forward-reference to "TE-0005 — Persian Date Picker Selection" (TE-0005 is actually FluentValidation) and a broken reference to a non-existent Localization document |