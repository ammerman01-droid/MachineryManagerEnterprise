# MachineryManagerEnterprise Project Charter

**Document ID:** MME-CHARTER-001  
**Version:** 1.0.0  
**Status:** Approved  
**Last Updated:** 2026-07-17

---

# Purpose

This document is the constitutional charter of the MachineryManagerEnterprise project.

It defines the long-term vision, engineering principles, architectural decisions, development workflow, documentation standards, and collaboration rules governing the project.

No architectural decision is considered permanent unless it is reflected in the project documentation or source code.

---

# Project Vision

MachineryManagerEnterprise is an Enterprise Asset Lifecycle Management (EALM) platform designed to manage the complete lifecycle of heavy machinery, industrial equipment, fleet assets and replaceable components.

The system aims to become a long-term, maintainable and extensible enterprise platform rather than a simple maintenance application.

---

# Long-Term Objectives

The project shall provide capabilities for:

- Organization Management
- Asset Management
- Asset Model Management
- Engine Management
- Component Lifecycle Management
- Meter Management
- Preventive Maintenance
- Corrective Maintenance
- Breakdown Management
- Fuel Management
- Lubricant Management
- Cost Management
- Depreciation
- Forecasting
- Notifications
- Knowledge Management
- Document Management
- Image Gallery
- Reporting
- Analytics
- Decision Support

---

# Fundamental Engineering Principles

The project shall always follow these principles.

1. Domain Driven Design (DDD)

Business concepts drive software architecture.

---

2. Clean Architecture

Business rules remain independent from frameworks and infrastructure.

---

3. Modular Monolith

Modules are isolated while remaining deployable as a single application.

Future migration to microservices shall remain possible.

---

4. Code First

Database schema is generated from the domain model.

Domain model is the source of truth.

---

5. Documentation First

Important architectural decisions shall be documented before implementation.

Documentation evolves together with source code.

---

6. Git First

Every completed deliverable shall be committed.

Repository history is part of project documentation.

---

7. Long-Term Maintainability

Every design decision should prioritize maintainability over short-term convenience.

---

# Technology Stack

Backend

- .NET 9
- ASP.NET Core
- Blazor Server

Database

- SQL Server
- Entity Framework Core
- Code First

Architecture

- DDD
- Clean Architecture
- Modular Monolith

---

# User Interface Principles

The application is designed as a responsive web application.

Supported platforms:

- Desktop
- Laptop
- Tablet
- Mobile Browser

The UI must support multiple visual themes.

Changing themes shall never affect functionality.

Accessibility and usability are considered first-class requirements.

---

# Domain Philosophy

The project models reality rather than database tables.

The primary source of truth consists of:

- Measurements
- Transactions
- Lifecycle Events

Current values are derived from historical events whenever possible.

---

# Asset Philosophy

Everything with an operational lifecycle is considered an Asset.

Examples include:

- Machines
- Engines
- Attachments
- Replaceable Components
- Future Trackable Equipment

Every asset owns its own history.

---

# Replaceable Components

Components may:

- be installed
- be removed
- be repaired
- be transferred
- be rebuilt
- be scrapped

The system shall preserve the complete installation history.

---

# Meter Philosophy

Operating hours or distance are never lost.

Replacing a meter never changes lifetime operational values.

The system distinguishes between:

- Physical Meter Reading
- Operational Usage
- Non-operational Usage
- Meter Replacement

---

# Engine Philosophy

Engine Models define technical specifications.

Engine Instances represent physical engines.

Engines:

- may already have operating hours
- may be rebuilt
- may be transferred
- may belong to multiple machines during their lifecycle

Engine lifecycle is independent from machine lifecycle.

---

# Financial Principles

The system preserves:

- Purchase Price
- Current Book Value
- Depreciation
- Maintenance Cost
- Fuel Cost
- Operating Cost

Historical values are never overwritten.

---

# Forecasting Principles

Forecasts are generated from historical operational data.

Examples include:

- Fuel Consumption
- Lubricant Consumption
- Filter Replacement
- Spare Parts Consumption
- Maintenance Planning

Forecasts assist decision making but never replace recorded operational data.

---

# Documentation Rules

Project documentation is part of the source code.

Documentation shall evolve together with implementation.

No major architectural decision shall exist only in conversation.

---

# Development Workflow

Every feature follows this order:

1. Business Analysis
2. Documentation
3. Domain Design
4. Implementation
5. Testing
6. Review
7. Commit
8. Push

---

# Repository Structure

```
src/
tests/
docs/
tools/
```

---

# Documentation Structure

The documentation shall remain organized and categorized.

Major categories include:

- Vision
- Architecture
- Domain
- Modules
- Development
- Decisions
- Releases

---

# Git Workflow

Development follows Git Flow principles.

Main branch remains stable.

Each significant feature is developed in its own branch.

Every completed deliverable is committed independently.

---

# Naming Principles

Names shall represent business concepts.

Avoid abbreviations whenever possible.

Business terminology has priority over technical terminology.

---

# Project Memory Principle

The official memory of the project is the repository.

Neither conversations nor individuals are considered the source of truth.

Whenever an important decision is made, it must be documented.

---

# AI Collaboration Principle

Artificial Intelligence is considered an engineering assistant.

AI suggestions are valuable only after being documented and approved.

Approved documentation has higher authority than conversational history.

---

# Change Management

Architectural changes must:

- be documented
- be reviewed
- preserve backward compatibility whenever practical

---

# Success Criteria

The project is successful when:

- architecture remains understandable
- business rules remain explicit
- source code remains maintainable
- documentation remains synchronized
- new developers can understand the project quickly

---

# Final Principle

> Build software that remains understandable ten years from now.

Every design decision shall prioritize clarity, maintainability and long-term value over short-term speed.