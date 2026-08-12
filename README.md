| Property         | Value              |
|------------------|--------------------|
| **Document ID**  | MME-README-001     |
| **Title**        | Enterprise Asset Lifecycle Management Platform for Heavy Equipment & Machinery |
| **Version**      | 4.1.0              |
| **Status**       | Approved           |
| **Owner**        | Solution Architect |
| **Created**      | 2026-07-18         |
| **Last Updated** | 2026-08-08         |


---

## AI Contributors

Any AI assistant participating in this repository must first read:

1. AI_ENGINEERING_CONTRACT.md
2. docs-english/README.md

before generating any implementation.

---

# Overview

MachineryManagerEnterprise is an Enterprise Asset Management (EAM) platform designed for organizations that own, operate, maintain and manage heavy machinery and industrial equipment.

The platform is built using modern .NET technologies and follows Domain-Driven Design (DDD), Clean Architecture and Modular Monolith principles.

Unlike traditional maintenance software, MachineryManagerEnterprise manages the complete lifecycle of operational assets, from acquisition to retirement.

---

# Vision

To build one of the most comprehensive Enterprise Asset Lifecycle Management platforms capable of managing:

- Heavy Equipment
- Construction Machinery
- Mining Equipment
- Industrial Machines
- Fleet Assets
- Replaceable Components
- Operational Resources

while providing complete operational, financial and analytical visibility.

---

# Core Principles

The project is based on the following architectural principles:

- Domain Driven Design (DDD)
- Clean Architecture
- Modular Monolith
- Code First Development
- Event-Oriented Domain Model
- Lifecycle First Design
- Ubiquitous Language
- SOLID Principles
- CQRS Ready
- Future Microservice Ready

---

# Platform Goals

The system is designed to support the complete lifecycle of enterprise assets including:

- Asset Registration
- Equipment Classification
- Organizational Structure
- Projects & Job Sites
- Meter Readings
- Engine Management
- Replaceable Components
- Repairs
- Preventive Maintenance
- Fuel Management
- Lubricant Management
- Cost Management
- Financial Management
- Depreciation
- Attachments
- Knowledge Library
- Technical Documents
- Image Gallery
- Notifications
- Reporting
- Analytics
- Forecasting
- Decision Support

---

# Architectural Philosophy

The system does **NOT** consider current values as the source of truth.

Instead, the source of truth is composed of:

- Measurements
- Transactions
- Lifecycle Events

Every calculated value (such as total operating hours, maintenance cost, operating cost, asset value and forecasts) is derived from historical operational data.

---

# Trackable Assets

The platform treats operational assets as first-class domain objects.

Examples include:

- Machines
- Engines
- Attachments
- Replaceable Components
- Future Trackable Assets

Each asset has its own lifecycle, history and operational records.

---

# Replaceable Components

Certain components can be installed, removed, repaired and transferred between machines.

Examples:

- Engine
- Transmission
- Hydraulic Pump
- Final Drive
- Battery
- Tires
- Future Components

The architecture fully supports complete installation history.

---

# Meter System

The platform supports multiple meter types.

Examples:

- Hour Meter
- Odometer
- Distance Meter
- Future Meter Types

Features include:

- Meter Replacement
- Initial Reading
- Previous Reading
- Operational Hours
- Non-operational Hours
- Historical Readings

Replacing a meter never resets the actual lifetime operating hours of an asset.

---

# Maintenance Management

Supported maintenance capabilities include:

- Preventive Maintenance
- Corrective Maintenance
- Breakdown Maintenance
- Outsourced Maintenance
- Maintenance Approval Workflow
- Service Orders
- Repair Orders
- Parts Consumption
- Labor Tracking
- Attachments

---

# Fuel & Fluid Management

The system supports recording and analysis of:

- Diesel
- Gasoline
- Engine Oil
- Hydraulic Oil
- Gear Oil
- Transmission Oil
- Coolant
- Grease
- Brake Fluid
- Other Fluids

Both scheduled replacement and top-up operations are supported independently.

---

# Asset Intelligence

Future versions of the platform include intelligent decision support.

Capabilities include:

- Consumption Analytics
- Consumption Forecasting
- Maintenance Forecasting
- Cost Prediction
- Asset Health Index
- Replacement Recommendation
- Operational Analytics

---

# Financial Management

Financial capabilities include:

- Purchase Information
- Asset Cost
- Operating Cost
- Maintenance Cost
- Fuel Cost
- Lubricant Cost
- Depreciation
- Book Value
- Estimated Market Value

---

# Documentation

Project documentation is located inside the **docs-english/** directory.

Major documents include:

- Vision
- Architecture
- Ubiquitous Language
- Domain Model
- Capability Model
- Aggregate Design
- Event Model
- Lifecycle Model

---

# Technology Stack

Web Backend & UI

- .NET 10
- ASP.NET Core
- Blazor (Server / WebAssembly)
- MudBlazor

Desktop & Mobile Client

- .NET MAUI (see `docs-english/06-decisions/ADR-0013-Client Application Architecture.md`)

Data & Caching

- Entity Framework Core (SQL Server)
- SQLite / LiteDB (embedded workspace databases)
- FusionCache (IMemoryCache + Redis)

Messaging & Background Processing

- MassTransit / RabbitMQ
- Quartz.NET

AI & Search

- Semantic Kernel (Azure OpenAI / OpenAI / Ollama)
- Qdrant (vector search)
- SQL Server Full-Text Search (default) / OpenSearch (escalation)

Identity & Security

- ASP.NET Core Identity / OpenIddict
- HashiCorp Vault

Observability

- Serilog, OpenTelemetry, Prometheus, Grafana

Architecture

- Domain Driven Design
- Clean Architecture
- Modular Monolith

Development

- Code First
- Git
- GitHub

See `docs-english/02-architecture/01-Architecture.md` and
`docs-english/05-development/11-DependencyCatalog.md` for the complete,
authoritative technology stack and every underlying ADR.

---

# User Interface

The platform provides two client experiences:

- A responsive Blazor web application (Desktop, Laptop, Tablet, Mobile
  Browser).
- A native .NET MAUI application for Desktop and Mobile, per
  ADR-0013 / TE-0010.

The UI supports multiple visual themes allowing users to switch color schemes without affecting functionality.

---

# Project Status

Current Phase

Foundation & Domain Design

Current Milestone

Domain Architecture

---

# Repository Structure

```
src/

    Host/

    Modules/

    BuildingBlocks/

docs-english/

tests/

tools/
```

---

# Development Workflow

Each completed milestone produces:

- Documentation
- Source Code
- Git Commit
- Git Tag

Documentation is always synchronized with source code.

---

# License

Private Repository

All Rights Reserved.

---

# Acknowledgments

This project is being developed with a long-term enterprise vision emphasizing maintainability, scalability, traceability and operational excellence.

---

#  Revision History

| Version | Date       | Author             | Description                                           |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0   | 2026-07-18 | Solution Architect | Initial documentation structure                       |
| 2.0.0   | 2026-07-18 | Solution Architect | Documentation architecture reorganized                |
| 3.0.0   | 2026-07-18 | Solution Architect | Standardized according to Documentation Standard v3.0 |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0             |
| 4.1.0   | 2026-08-08 | Solution Architect | Review and synchronize with the latest changes        |