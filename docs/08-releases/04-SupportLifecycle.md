# Support Lifecycle

| Property | Value |
|----------|-------|
| **Document ID** | REL-004 |
| **Version** | 3.0.0 |
| **Status** | Active |
| **Owner** | Release Manager |
| **Created** | 2026-07-18 |
| **Last Updated** | 2026-07-18 |

---

# 1. Purpose

This document defines the support lifecycle for MachineryManagerEnterprise releases.

It specifies how long released versions remain supported and what kinds of maintenance are provided during each stage.

---

# Support Philosophy

Every released version has a clearly defined support lifecycle.

Support shall be transparent, predictable and traceable.

Clients shall always know:

- which versions are supported;
- which versions are deprecated;
- when support ends.

---

# 2. Objectives

The support lifecycle shall provide:

- Predictable maintenance
- Stable production environments
- Controlled upgrades
- Long-term planning
- Transparent support policy

---

# 3. Lifecycle

Every release progresses through the following stages.

```text
Released

↓

Supported

↓

Maintenance

↓

Deprecated

↓

End of Support

↓

Archived
```

---

# Support Responsibilities

| Stage | Responsible |
|--------|-------------|
| Released | Release Manager |
| Supported | Development Team |
| Maintenance | Maintenance Team |
| Deprecated | Product Owner |
| End of Support | Architecture Board |
| Archived | Documentation Team |

---

# 4. Released

A released version:

- has passed all validation stages;
- has complete documentation;
- has tagged source code;
- has published release artifacts.

---

# 5. Supported

Supported releases receive:

- Bug fixes
- Security fixes
- Critical stability improvements
- Documentation corrections

Supported versions remain recommended for production.

---

# 6. Maintenance

Maintenance releases may include:

- Patch updates
- Performance improvements
- Reliability improvements

Maintenance releases shall not introduce breaking behavior.

---

# 7. Deprecated

A version becomes deprecated when:

- A newer recommended version exists.
- Migration guidance has been published.
- Future removal has been announced.

Deprecated versions remain operational during the support period.

---

# 8. End of Support

A version reaches End of Support when:

- Support period expires.
- Security updates stop.
- Bug fixes stop.
- Official assistance ends.

Clients should migrate before this stage.

---

# 9. Archived

Archived versions remain available for historical purposes.

Archived releases shall include:

- Release Notes
- Documentation
- Version Information
- Git Tag

Archived releases receive no further maintenance.

---

# 10. Security Updates

Security updates shall be provided only for supported versions.

Unsupported versions shall not receive security patches.

---

# 11. Upgrade Policy

Upgrading between Patch releases should require no application changes.

Minor upgrades shall remain backward compatible.

Major upgrades may require migration.

---

# 12. Migration Guidance

Whenever a newer version supersedes an existing release:

Migration documentation should include:

- Breaking changes
- API differences
- Configuration changes
- Database migration guidance

---

# 13. Support Responsibilities

The project shall maintain:

- Release documentation
- Version history
- Migration guides
- Security notices

Support activities shall remain traceable.

---

# 14. Future Enhancements

Future versions may introduce:

- Long-Term Support (LTS)
- Extended Support
- Community Support
- Commercial Support
- Automatic Upgrade Advisors

---

# 15. Support Matrix

| Version        | Bug Fix | Security | Documentation |
| -------------- | :-----: | :------: | :-----------: |
| Supported      |    ✅    |     ✅    |       ✅       |
| Maintenance    |    ✅    |     ✅    |       ✅       |
| Deprecated     |    ❌    |     ✅    |       ✅       |
| End of Support |    ❌    |     ❌    |       ❌       |
| Archived       |    ❌    |     ❌    |       ❌       |

---

# Related Documents

- 00-ReleaseStrategy.md
- 01-VersioningPolicy.md
- 02-ReleaseProcess.md
- 03-DeploymentStrategy.md
- docs/07-api/05-ApiVersioning.md

---

# Change History

| Version | Date | Description |
|----------|------------|---------------------------------------------|
| 1.0.0 | Initial | Initial Support Lifecycle |
| 3.0.0 | 2026-07-18 | Standardized according to Documentation Standard v3.0 |