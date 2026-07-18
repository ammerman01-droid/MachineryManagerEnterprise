# Pagination, Filtering and Sorting

**Document ID:** MME-API-005

**Repository Path:** `docs/07-api/05-PaginationFilteringSorting.md`

**Version:** 1.0.0 (Draft)

**Status:** In Progress

**Related Documents**

- 00-ApiPrinciples.md
- 01-RestConventions.md
- 02-EndpointDesign.md
- 03-RequestResponseModel.md

---

# 1. Purpose

This document defines the standard conventions for pagination, filtering and sorting used throughout MachineryManagerEnterprise.

All collection endpoints shall follow these conventions.

---

# 2. Principles

Collection endpoints shall support:

- Pagination
- Filtering
- Sorting

The behavior shall remain identical across all API resources.

---

# 3. Pagination Parameters

The standard pagination parameters are:

| Parameter | Description |
|------------|-------------|
| page | Page number (starting from 1) |
| pageSize | Number of items per page |

Example

```
GET /assets?page=1&pageSize=25
```

---

# 4. Default Values

Unless explicitly overridden:

```
page = 1

pageSize = 25
```

---

# 5. Maximum Page Size

To protect server resources:

```
Maximum pageSize = 200
```

Requests exceeding the maximum shall be limited automatically.

---

# 6. Paged Response

Paged responses shall follow the structure below.

```json
{
  "items": [],
  "page": 1,
  "pageSize": 25,
  "totalItems": 1543,
  "totalPages": 62,
  "hasNextPage": true,
  "hasPreviousPage": false
}
```

---

# 7. Filtering

Filtering shall use query parameters.

Examples

```
GET /assets?status=Active

GET /assets?organizationId=15

GET /documents?category=Insurance

GET /maintenance-orders?priority=High
```

Each filter represents one business criterion.

---

# 8. Multiple Filters

Multiple filters may be combined.

Example

```
GET /assets?status=Active&organizationId=15
```

Filters shall be combined using logical AND.

---

# 9. Date Filtering

Date ranges shall use explicit parameters.

Example

```
GET /maintenance-orders?from=2026-01-01&to=2026-12-31
```

Dates shall use ISO-8601.

---

# 10. Sorting

Sorting shall use:

```
sort=
```

Examples

Ascending

```
GET /assets?sort=name
```

Descending

```
GET /assets?sort=-createdAt
```

A leading minus (`-`) indicates descending order.

---

# 11. Multiple Sort Fields

Multiple sort fields may be supplied.

Example

```
GET /assets?sort=status,name
```

Evaluation occurs from left to right.

---

# 12. Searching

Free-text search shall use:

```
search=
```

Example

```
GET /assets?search=CAT320
```

Search behavior shall be documented for each resource.

---

# 13. Empty Results

Successful queries with no matching records shall return:

```
200 OK
```

Example

```json
{
  "items": [],
  "page": 1,
  "pageSize": 25,
  "totalItems": 0,
  "totalPages": 0,
  "hasNextPage": false,
  "hasPreviousPage": false
}
```

Empty collections are not errors.

---

# 14. Invalid Parameters

Invalid pagination or sorting parameters shall return:

```
400 Bad Request
```

Validation shall identify the invalid parameter.

---

# 15. Performance

Filtering and sorting shall be executed by the persistence layer whenever possible.

Large datasets shall never be loaded entirely into memory before pagination.

---

# 16. Future Extensions

Future versions may support:

- Cursor Pagination
- Continuation Tokens
- Dynamic Projections
- Advanced Search
- Full-text Search

Backward compatibility shall be preserved.

---

# Revision History

| Version | Description |
|----------|-------------|
| 1.0.0 | Initial Pagination, Filtering and Sorting specification |