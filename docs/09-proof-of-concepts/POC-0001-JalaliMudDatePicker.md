# Proof of Concept — Jalali Support for MudBlazor DatePicker

**Document ID**

POC-0001

---

# Objective

Evaluate whether MudBlazor DatePicker can fully support the Persian (Jalali) calendar without introducing an additional UI component library.

The purpose is validating the architectural decision proposed in:

TE-0005 — Persian Date Picker Selection

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

MudBlazor Jalali Adapter is adopted.

↓

ADR-0009

Use MudBlazor Jalali Adapter

---

## FAIL

Blazor.PersianDatePicker is adopted.

↓

ADR-0009

Use Blazor.PersianDatePicker

---

# Status

Pending