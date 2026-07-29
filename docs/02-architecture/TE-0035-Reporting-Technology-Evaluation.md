| Property | Value |
|----------|-------|
| **Document ID** | TE-0035 |
| **Title** | Reporting Technology Evaluation |
| **Version** | 4.0.0 |
| **Status** | Approved |
| **Owner** | Solution Architect |
| **Created** | 2026-07-28 |
| **Last Updated** | 2026-07-28 |

# Purpose

This document evaluates candidate technologies for Reporting Technology Evaluation in MachineryManagerEnterprise.

The objective is to establish a unified technology selection that satisfies all functional and architectural requirements while preserving Clean Architecture principles.

The selected technologies shall support:

- Enterprise PDF Generation
- Excel Export
- Printable Reports
- Business Documents
- Financial Reports
- Operational Reports
- Long-Term Maintainability
- Cross-Platform Execution

---

# Evaluation Scope

This Technology Evaluation evaluates:

- QuestPDF
- FastReport
- RDLC
- ClosedXML

This document does **not** define:

- Report Templates
- Report Layout Standards
- Business Report Catalog
- Dashboard Architecture
- BI Strategy

These architectural decisions are documented separately in the corresponding ADR.

---

# Relationship with Related ADRs

This Technology Evaluation supports:

- **ADR-0029 — Enterprise Reporting Architecture** *(Pending)*

It depends upon:

- ADR-0001 — Clean Architecture
- ADR-0003 — Modular Monolith Architecture
- ADR-0028 — Client UI Architecture

---

# Architectural References

This evaluation is based upon:

- QuestPDF Documentation
- ClosedXML Documentation
- FastReport Documentation
- Microsoft Reporting Documentation
- Enterprise Reporting Best Practices

---

# Scope

The following technologies are evaluated:

- QuestPDF
- FastReport
- RDLC
- ClosedXML

---

# Reporting Objectives

MachineryManagerEnterprise shall support:

- PDF Documents
- Excel Documents
- Printable Reports
- Operational Reports
- Financial Reports
- Multi-Page Reports
- Internationalization
- Long-Term Maintainability

---

# Functional Requirements

The reporting platform shall support:

- PDF Generation
- Excel Export
- Tables
- Charts (where applicable)
- Headers & Footers
- Images
- Barcodes / QR Codes (future extensibility)
- Localization

---

# Architecture Principle

The evaluated component operates as an isolated infrastructure service in accordance with Clean Architecture principles and domain isolation rules.

---

# Non-Functional Requirements

The selected technologies shall provide:

- Enterprise Readiness
- Cross Platform Support
- Excellent Performance
- High Rendering Quality
- Automation
- Long-Term Viability
- Excellent Documentation
- Licensing Compatibility

---

# Candidate Technologies

| Candidate | Category |
|-----------|----------|
| QuestPDF | PDF Generation |
| FastReport | Reporting Framework |
| RDLC | Microsoft Report Technology |
| ClosedXML | Excel Generation |

---

# Evaluation Criteria

| ID | Criterion | Priority |
|----|-----------|----------|
| RP-01 | Enterprise Readiness | Critical |
| RP-02 | Cross Platform | Critical |
| RP-03 | PDF Quality | High |
| RP-04 | Excel Support | High |
| RP-05 | Performance | High |
| RP-06 | Maintainability | High |
| RP-07 | Documentation | Medium |
| RP-08 | Licensing | Critical |
| RP-09 | Automation | High |
| RP-10 | Long-Term Viability | High |

---


# 8. QuestPDF Evaluation

## Overview

QuestPDF is a modern open-source PDF generation library for .NET.

It generates PDF documents programmatically using a fluent C# API instead of report designers or XML-based layouts.

Within MachineryManagerEnterprise, QuestPDF is evaluated as the primary enterprise PDF generation technology.

---

# Architectural Role

```text
Business Data

      │

      ▼

 Report Builder

      │

      ▼

    QuestPDF

      │

      ▼

 PDF Document
```

QuestPDF is responsible only for rendering PDF documents.

Business logic remains completely outside the reporting engine.

---

# Architectural Strengths

Advantages include:

- Native .NET implementation
- Fluent API
- Cross-platform execution
- Excellent rendering quality
- Strong layout engine
- Active development
- No visual designer dependency
- Excellent maintainability

---

# Functional Capabilities

QuestPDF supports:

- Multi-page documents
- Tables
- Images
- Headers
- Footers
- Page numbering
- Rich typography
- Dynamic layouts

---

# Document Generation Model

Typical workflow:

```text
Business Data

      │

Document Model

      │

QuestPDF Layout Engine

      │

PDF Rendering

      │

Generated PDF
```

Document construction is deterministic and entirely code-driven.

---

# Layout Flexibility

QuestPDF supports:

- Dynamic page layouts
- Adaptive tables
- Nested containers
- Automatic pagination
- Rich styling

This allows complex enterprise reports to be generated without external design files.

---

# Rendering Quality

The framework produces:

- High-resolution PDF output
- Consistent typography
- Professional page layout
- Predictable rendering across operating systems

Rendering quality is considered **Excellent**.

---

# Performance

QuestPDF performs rendering entirely in managed .NET code.

Performance characteristics include:

- Low memory consumption
- Fast document generation
- Efficient layout calculation

Performance is considered **Excellent**.

---

# Cross Platform Support

Supported operating systems:

| Platform | Support |
|----------|:-------:|
| Windows | ✅ |
| Linux | ✅ |
| macOS | ✅ |

Generated PDF output remains identical across platforms.

---

# Automation

QuestPDF integrates naturally with:

- .NET CLI
- Worker Services
- Background Jobs
- CI/CD Pipelines

No interactive components are required.

---

# Enterprise Suitability

QuestPDF is appropriate for generating:

- Work Orders
- Machinery Reports
- Inspection Reports
- Maintenance Reports
- Financial Documents
- Printable Business Documents

---

# Maintainability

Advantages include:

- Source-controlled report definitions
- Strong typing
- Compile-time validation
- No external report designer
- Easy code review

Maintainability is considered **Excellent**.

---

# Licensing

QuestPDF licensing is compatible with enterprise commercial software development.

No licensing concerns affecting MachineryManagerEnterprise were identified.

---

# Technology Assessment

| Criterion | Assessment |
|-----------|------------|
| Enterprise Readiness | Excellent |
| PDF Quality | Excellent |
| Cross Platform | Excellent |
| Performance | Excellent |
| Maintainability | Excellent |
| Documentation | Excellent |
| Automation | Excellent |
| Long-Term Viability | Excellent |

---

# Advantages

- Excellent PDF rendering quality
- Fully code-driven architecture
- Strong .NET integration
- Cross-platform consistency
- Minimal operational complexity

---

# Disadvantages

- No visual report designer
- Developers construct layouts programmatically

These characteristics are considered architectural advantages for long-term maintainability.

---

# Preliminary Conclusion

QuestPDF completely satisfies the enterprise PDF generation requirements of MachineryManagerEnterprise.

It is approved as the standard technology for all PDF document generation.

---


# 9. ClosedXML Evaluation

## Overview

ClosedXML is an open-source .NET library for creating and manipulating Microsoft Excel workbooks.

Unlike Office automation libraries, ClosedXML generates native Excel files directly without requiring Microsoft Excel to be installed.

Within MachineryManagerEnterprise, ClosedXML is evaluated as the primary Excel export technology.

---

# Architectural Role

```text
Business Data

      │

      ▼

 Export Service

      │

      ▼

   ClosedXML

      │

      ▼

 Excel Workbook
```

ClosedXML is responsible only for Excel document generation.

Business logic remains completely independent from workbook generation.

---

# Architectural Strengths

Advantages include:

- Native .NET implementation
- Cross-platform execution
- Excellent Excel compatibility
- Strong object model
- No Office installation required
- Active development
- Easy integration
- Excellent maintainability

---

# Functional Capabilities

ClosedXML supports:

- Worksheets
- Tables
- Formulas
- Cell Formatting
- Conditional Formatting
- Images
- Merged Cells
- Multiple Worksheets

---

# Workbook Generation Model

Typical workflow:

```text
Business Data

      │

Workbook Model

      │

 ClosedXML

      │

 XLSX Generation

      │

 Excel File
```

Workbook generation is deterministic and entirely code-driven.

---

# Excel Compatibility

ClosedXML produces standard Office Open XML documents compatible with:

- Microsoft Excel
- LibreOffice
- OnlyOffice
- WPS Office

Generated files follow the XLSX specification.

---

# Formatting Capabilities

Supported formatting includes:

- Fonts
- Colors
- Borders
- Alignment
- Number Formats
- Tables
- Auto Filters

Professional spreadsheet output is fully supported.

---

# Formula Support

ClosedXML supports:

- Excel formulas
- Named ranges
- Formula references
- Automatic recalculation support

This enables generation of rich analytical spreadsheets.

---

# Performance

Performance characteristics include:

- Fast workbook generation
- Low memory consumption
- Efficient worksheet creation

Performance is considered **Excellent** for enterprise reporting scenarios.

---

# Cross Platform Support

Supported operating systems:

| Platform | Support |
|----------|:-------:|
| Windows | ✅ |
| Linux | ✅ |
| macOS | ✅ |

Workbook generation is platform independent.

---

# Automation

ClosedXML integrates naturally with:

- ASP.NET Core
- Worker Services
- Background Jobs
- CI/CD Pipelines

Excel documents can therefore be generated automatically without user interaction.

---

# Enterprise Suitability

ClosedXML is appropriate for generating:

- Machinery Lists
- Asset Inventories
- Financial Exports
- Operational Reports
- Analytical Worksheets
- Data Exchange Files

---

# Maintainability

Advantages include:

- Strong typing
- Readable API
- Source-controlled workbook generation
- Easy code review
- Minimal boilerplate

Maintainability is considered **Excellent**.

---

# Licensing

ClosedXML licensing is compatible with enterprise commercial software development.

No licensing concerns affecting MachineryManagerEnterprise were identified.

---

# Technology Assessment

| Criterion | Assessment |
|-----------|------------|
| Enterprise Readiness | Excellent |
| Excel Support | Excellent |
| Cross Platform | Excellent |
| Performance | Excellent |
| Maintainability | Excellent |
| Documentation | Excellent |
| Automation | Excellent |
| Long-Term Viability | Excellent |

---

# Advantages

- Native Excel generation
- Excellent OpenXML compatibility
- No Office installation required
- Strong .NET integration
- Cross-platform execution

---

# Disadvantages

- Focused exclusively on Excel documents
- Does not generate PDF reports

This specialization aligns with its intended architectural responsibility.

---

# Preliminary Conclusion

ClosedXML completely satisfies the enterprise Excel export requirements of MachineryManagerEnterprise.

It is approved as the standard technology for Excel workbook generation.

---


# 10. FastReport Evaluation

## Overview

FastReport is a commercial reporting platform providing a visual report designer and runtime reporting engine.

Unlike QuestPDF and ClosedXML, FastReport centers around designer-based report templates rather than code-first document construction.

Within MachineryManagerEnterprise, FastReport is evaluated as an enterprise reporting solution.

---

# Architectural Role

```text
Business Data

      │

      ▼

 Report Template

      │

   FastReport

      │

      ▼

 Report Rendering
```

The reporting engine consumes predefined report templates and renders output documents.

---

# Architectural Strengths

Advantages include:

- Mature reporting engine
- Visual report designer
- Rich reporting capabilities
- Multiple export formats
- Enterprise feature set
- Large commercial user base

---

# Functional Capabilities

FastReport supports:

- Visual Report Designer
- Tables
- Charts
- Images
- Barcodes
- Grouping
- Master–Detail Reports
- Multiple Export Formats

---

# Report Development Model

Typical workflow:

```text
Business Data

      │

Report Template

      │

FastReport Engine

      │

Generated Report
```

Report definitions are maintained as external template files.

---

# Export Formats

Supported outputs include:

- PDF
- Excel
- HTML
- Word
- Image Formats

---

# Cross Platform Support

Current cross-platform support is available, but the reporting ecosystem remains more mature on Windows than on Linux or macOS.

| Platform | Support |
|----------|:-------:|
| Windows | ✅ |
| Linux | ✅ |
| macOS | ✅ |

---

# Enterprise Suitability

FastReport is appropriate for:

- Traditional enterprise reporting
- Complex printable documents
- Designer-driven reporting environments

---

# Performance

Rendering performance is considered **Very Good**.

However, template interpretation introduces additional runtime overhead compared with code-first document generation.

---

# Maintainability

Report definitions exist outside the application source code.

Consequently:

- report reviews become more difficult;
- template versioning becomes more complex;
- business logic may gradually migrate into report templates.

Maintainability is therefore considered **Moderate**.

---

# Operational Characteristics

FastReport introduces additional operational considerations:

- report template management;
- designer version compatibility;
- commercial licensing.

Operational complexity is higher than code-first alternatives.

---

# Licensing

FastReport is a commercial product.

Enterprise deployment requires commercial licensing and long-term license management.

---

# Technology Assessment

| Criterion | Assessment |
|-----------|------------|
| Enterprise Readiness | Excellent |
| Reporting Capability | Excellent |
| Cross Platform | Very Good |
| Performance | Very Good |
| Maintainability | Moderate |
| Documentation | Excellent |
| Automation | Good |
| Long-Term Viability | Very Good |

---

# Advantages

- Rich reporting features
- Powerful visual designer
- Multiple export formats
- Mature commercial ecosystem

---

# Disadvantages

- Commercial licensing
- External report templates
- Higher maintenance complexity
- Designer dependency

---

# Comparison with QuestPDF

| Criterion | QuestPDF | FastReport |
|-----------|:--------:|:----------:|
| Code-First Architecture | ✅ | ❌ |
| Visual Designer | ❌ | ✅ |
| Source Control Friendliness | Excellent | Moderate |
| Licensing Simplicity | Excellent | Moderate |
| Long-Term Maintainability | Excellent | Moderate |

---

# Preliminary Conclusion

Although FastReport is a mature enterprise reporting solution, its designer-based architecture and commercial licensing increase long-term maintenance costs.

For MachineryManagerEnterprise, its advantages do not outweigh the simplicity and maintainability of the selected code-first reporting architecture.

FastReport is therefore **not selected**.

---

# 11. RDLC Evaluation

## Overview

RDLC (Report Definition Language Client-side) is Microsoft's legacy report technology originally introduced for Windows desktop applications.

It is based on XML report definitions and integrates with the Microsoft ReportViewer ecosystem.

Within MachineryManagerEnterprise, RDLC is evaluated as a potential enterprise reporting technology.

---

# Architectural Role

```text
Business Data

      │

      ▼

 RDLC Report Definition

      │

 ReportViewer Engine

      │

      ▼

 Rendered Report
```

The reporting engine renders predefined XML report definitions.

---

# Architectural Strengths

Advantages include:

- Long history within Microsoft ecosystem
- Rich report layout capabilities
- Familiar to legacy .NET developers
- Strong Windows desktop integration

---

# Functional Capabilities

RDLC supports:

- Tables
- Charts
- Images
- Grouping
- Parameters
- Expressions
- Printing
- PDF Export

---

# Report Development Model

Typical workflow:

```text
Business Data

      │

 RDLC Template

      │

 ReportViewer

      │

 Rendered Document
```

Reports are maintained separately from application source code.

---

# Platform Support

RDLC was primarily designed for Windows desktop applications.

Cross-platform support is limited and requires additional compatibility layers.

| Platform | Support |
|----------|:-------:|
| Windows | ✅ |
| Linux | ⚠ Limited |
| macOS | ⚠ Limited |

This limitation conflicts with the cross-platform objectives of MachineryManagerEnterprise.

---

# Microsoft Ecosystem Integration

RDLC integrates with:

- Visual Studio
- Microsoft ReportViewer

However, it is no longer a strategic reporting technology within the modern .NET ecosystem.

---

# Performance

Rendering performance is acceptable.

However:

- ReportViewer introduces additional runtime overhead.
- XML report processing is heavier than code-first generation.

Performance is considered **Good**.

---

# Maintainability

RDLC introduces several maintenance concerns:

- XML report definitions
- External designer dependency
- Legacy tooling
- Limited evolution in modern .NET

Maintainability is considered **Moderate**.

---

# Operational Characteristics

Operational complexity includes:

- ReportViewer dependency
- Designer compatibility
- Platform-specific behavior

---

# Enterprise Suitability

RDLC remains suitable for maintaining existing legacy systems.

It is less appropriate for new cross-platform enterprise applications.

---

# Technology Assessment

| Criterion | Assessment |
|-----------|------------|
| Enterprise Readiness | Good |
| Cross Platform | Limited |
| Reporting Capability | Very Good |
| Performance | Good |
| Maintainability | Moderate |
| Documentation | Good |
| Automation | Moderate |
| Long-Term Viability | Limited |

---

# Advantages

- Mature reporting technology
- Familiar Microsoft ecosystem
- Rich reporting capabilities

---

# Disadvantages

- Legacy technology
- Windows-oriented architecture
- Limited cross-platform support
- Additional ReportViewer dependency
- Reduced long-term strategic value

---

# Comparison with QuestPDF

| Criterion | QuestPDF | RDLC |
|-----------|:--------:|:----:|
| Cross Platform | Excellent | Limited |
| Code-First | Yes | No |
| XML Templates | No | Yes |
| Modern .NET Alignment | Excellent | Moderate |
| Long-Term Maintainability | Excellent | Moderate |

---

# Preliminary Conclusion

RDLC does not satisfy the long-term architectural objectives of MachineryManagerEnterprise.

Its Windows-centric architecture and legacy ecosystem conflict with the project's cross-platform strategy.

RDLC is therefore **not selected**.

---

# 12. Overall Technology Comparison

## Reporting Technology Matrix

| Responsibility | Approved Technology |
|---------------|---------------------|
| PDF Generation | QuestPDF |
| Excel Generation | ClosedXML |
| Designer-Based Reporting | Not Required |

---

## Technology Comparison

| Criterion | QuestPDF | ClosedXML | FastReport | RDLC |
|-----------|:--------:|:---------:|:----------:|:----:|
| Enterprise Readiness | Excellent | Excellent | Excellent | Good |
| Cross Platform | Excellent | Excellent | Excellent | Poor |
| Maintainability | Excellent | Excellent | Fair | Fair |
| Performance | Excellent | Excellent | Good | Fair |
| Automation | Excellent | Excellent | Good | Fair |
| Licensing Simplicity | Excellent | Excellent | Poor | Good |
| Long-Term Viability | Excellent | Excellent | Good | Poor |

---

# Architectural Coverage

```text
Enterprise Reporting

        │

 ┌──────┴──────────────┐

 ▼                     ▼

QuestPDF          ClosedXML

 ▼                     ▼

PDF Reports      Excel Reports
```

Designer-based reporting frameworks are intentionally excluded from the approved architecture.

---

# 13. Final Recommendation

Following the evaluation of all candidate reporting technologies, the Architecture Review Board recommends adoption of the following enterprise reporting stack.

| Category | Approved Technology |
|----------|---------------------|
| PDF Document Generation | **QuestPDF** |
| Excel Workbook Generation | **ClosedXML** |
| Designer-Based Reporting | **Not Adopted** |

The selected technologies provide a modern, fully code-driven reporting architecture that aligns with the architectural principles of MachineryManagerEnterprise.

---

# Recommended Reporting Strategy

## QuestPDF

Primary responsibility:

- Printable Documents
- Work Orders
- Maintenance Reports
- Inspection Reports
- Financial Reports
- Operational Reports

QuestPDF becomes the standard framework for all PDF document generation.

---

## ClosedXML

Primary responsibility:

- Excel Export
- Analytical Reports
- Financial Data Export
- Inventory Export
- Business Data Exchange
- Spreadsheet Generation

ClosedXML becomes the standard framework for all Excel document generation.

---

## FastReport

FastReport shall **not** be adopted.

Although technically capable, its designer-centric architecture increases:

- maintenance cost;
- version management complexity;
- dependency upon external templates.

---

## RDLC

RDLC shall **not** be adopted.

Its Windows-oriented architecture conflicts with the cross-platform objectives of MachineryManagerEnterprise.

---

# Reporting Architecture

```text
Business Data

      │

 ┌────┴────────────┐

 ▼                 ▼

QuestPDF      ClosedXML

 ▼                 ▼

 PDF            Excel

 Reports      Workbooks
```

All report generation remains entirely code-driven.

---

# Architectural Principles

The reporting subsystem shall follow the following principles:

- No business logic inside report templates.
- Report generation shall remain deterministic.
- Report definitions shall remain under source control.
- Reports shall be reproducible from application code.
- Report rendering shall remain independent from the user interface.

---

# Automation Strategy

Report generation shall support:

- Background Workers
- Application Services
- Scheduled Jobs
- Batch Processing
- CI/CD Validation

No interactive report designer is required during production execution.

---

# Cross Platform Strategy

The approved reporting stack executes identically on:

| Platform | Support |
|----------|:-------:|
| Windows | ✅ |
| Linux | ✅ |
| macOS | ✅ |

This satisfies the cross-platform deployment requirements established by the architecture.

---

# Long-Term Maintainability

The selected technologies provide:

- Source-controlled report definitions
- Strong typing
- Minimal external dependencies
- Excellent developer productivity
- Predictable evolution

The reporting architecture therefore minimizes long-term maintenance risk.

---

# Benefits

The selected reporting stack provides:

- Enterprise-grade PDF generation
- Enterprise-grade Excel generation
- Cross-platform compatibility
- Excellent automation support
- High rendering quality
- Excellent maintainability

---

# Overall Technology Comparison

The selected technology provides optimal performance, maintainability, and Clean Architecture compatibility.

## Responsibility Matrix

| Responsibility | Recommended Technology | Alternative |
|-----------------|------------------------|-------------|
| System Capability | Primary Selected | Evaluated Option |

---

# Final Recommendation Statement

The Architecture Review Board unanimously recommends adoption of:

- **QuestPDF**
- **ClosedXML**

as the official enterprise reporting technologies.

FastReport and RDLC remain evaluated alternatives but are not approved.

---

# 14. Final Decision

## Approved Reporting Platform

```text
Business Layer

      │

 Reporting Services

      │

 ┌────┴──────────┐

 ▼               ▼

QuestPDF    ClosedXML

 ▼               ▼

PDF         Excel
```

---

## Technology Decisions

| Technology | Decision | Status |
|------------|----------|--------|
| QuestPDF | Approved | ✅ |
| ClosedXML | Approved | ✅ |
| FastReport | Rejected | ❌ |
| RDLC | Rejected | ❌ |

---

## Implementation Strategy

### Phase 1

- QuestPDF integration
- PDF report generation

### Phase 2

- ClosedXML integration
- Excel export generation

### Phase 3

- Enterprise report templates
- Automated report services

Designer-based reporting technologies shall not be introduced unless explicitly approved through a future ADR.

---

## Consequences

### Positive

- Fully code-first reporting
- Excellent cross-platform compatibility
- Strong maintainability
- Excellent automation
- Reduced operational complexity

### Negative

- No visual report designer
- Developers construct report layouts programmatically

These trade-offs are acceptable because maintainability and architectural consistency are prioritized.

---

## Related Architecture Decision

Implementation of this Technology Evaluation requires:

- **ADR-0029 — Enterprise Reporting Architecture**

---

# Decision Summary

- ✔ Clean Architecture
- ✔ .NET 10 Compatibility
- ✔ Standards Compliance
- ✔ Cloud Neutrality
- ✔ AI Readiness
- ✔ Long-term Maintainability

---


# Related Documents

- SolutionStructure.md
- DependencyRules.md
- CodingStandards.md

---

# 15. Revision History

| Version | Date       | Author             | Description                                            |
|---------|------------|--------------------|--------------------------------------------------------|
| 1.0.0   | 2026-07-28 | Solution Architect | Initial technology evaluation for Reporting Technology |
| 1.1.0   | 2026-07-28 | Solution Architect | Converted star-rating (⭐) tables to text ratings (Excellent/Good/Fair/Poor/Very Poor) for consistency with the rest of the documentation |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0              |