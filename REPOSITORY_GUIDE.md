| Property         | Value              |
|------------------|--------------------|
| **Document ID**  | MME-GUIDE-001      |
| **Title**        | Repository Guide   |
| **Version**      | 4.1.0              |
| **Status**       | Approved           |
| **Owner**        | Solution Architect |
| **Created**      | 2026-07-18         |
| **Last Updated** | 2026-08-08         |

---

# Purpose

This document describes the overall repository organization, development
principles, and navigation guidelines.

It serves as the entry point for developers after reading the project README.

---

# Repository Structure

```
Repository
│
├── README.md
├── REPOSITORY_GUIDE.md
├── PROJECT_CHARTER.md
├── PROJECT_PROGRESS.md
├── DOCUMENTATION_REVIEW_CHECKLIST.md
│
├── docs-english/
├── src/
├── tests/
│
├── Directory.Build.props
├── Directory.Packages.props
├── MachineryManagerEnterprise.slnx
│
└── .github/
```

---

# Folder Responsibilities

## docs

Contains all project documentation.

- Vision
- Architecture
- Domain
- Modules
- Development
- Decisions
- API
- Releases
- Proof of Concepts

---

## src

Contains all production source code.

- Host
- Modules
- Shared
- BuildingBlocks

---

## tests

Contains all automated tests.

---

## .github

Repository automation.

- CI
- Workflows
- Templates

---

# Root Documents

## README

General introduction.

---

## Repository Guide

Repository organization.

---

## Project Charter

Project vision and objectives.

---

## Project Progress

Project history and milestones.

---

## Documentation Review Checklist

Documentation quality assurance.

---

# Repository Principles

- Documentation First
- Architecture First
- Clean Architecture
- Modular Monolith
- Domain Driven Design
- Open Source First

---

# Documentation Flow

README

↓

Repository Guide

↓

Project Charter

↓

Documentation

↓

Implementation

---

# Related Documents

README

PROJECT_CHARTER

DOCUMENT_CONVENTIONS

ADR-INDEX

---

#  Revision History

| Version | Date       | Author             | Description                                           |
|---------|------------|--------------------|-------------------------------------------------------|
| 1.0.0   | 2026-07-18 | Solution Architect | Initial Repository Guide                              |
| 2.0.0   | 2026-07-18 | Solution Architect | Documentation architecture reorganized                |
| 3.0.0   | 2026-07-18 | Solution Architect | Standardized according to Documentation Standard v3.0 |
| 4.0.0   | 2026-07-28 | Solution Architect | Upgraded to Documentation Standard v4.0.0             |
| 4.1.0   | 2026-08-08 | Solution Architect | Review and synchronize with the latest changes        |