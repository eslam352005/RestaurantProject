# 🍽️ Restaurant Management System

A multi-branch restaurant management system built with **ASP.NET Core Web API** and **Clean Architecture**. Waiters open orders, the kitchen sees them instantly on a real-time display, and managers get inventory alerts and business reports per branch.

## ✨ Features

**Multi-branch**
- Menu, tables, staff and inventory are scoped per branch
- Each branch receives only its own real-time events

**Authentication & authorization**
- JWT access tokens with refresh tokens
- Role-based access: `Admin`, `Manager`, `Chef`, `Waiter`
- Identity-backed users linked to staff records, with role and branch seeding

**Orders & kitchen (real-time)**
- Full order lifecycle: `Pending → Preparing → Ready → Served` (or `Cancelled`), with validated status transitions
- Per-item status so the chef can mark each dish ready independently; the order becomes `Ready` automatically when all items are
- Prices are captured from the menu at order time and totals are calculated server-side
- Tables are occupied / released automatically as orders open and close
- **Kitchen Display System** powered by SignalR, one group per branch

**Menu & inventory**
- Categories and menu items per branch, availability toggle, image upload for menu items
- Inventory with stock adjustment (add / deduct, never below zero) and low-stock alerts, including a real-time alert when stock drops under its threshold

**Reports (EF Core aggregation)**
- Sales per day for a date range
- Top-selling items
- Staff performance (orders handled and sales per waiter)

**Developer experience**
- Swagger UI with JWT support, unified `ResponseDto<T>` responses, Fluent API configurations, data seeding

## 🧱 Tech Stack

| Area | Technology |
|---|---|
| Framework | ASP.NET Core Web API (.NET 10) |
| Data | EF Core, SQL Server, Fluent API (one configuration class per entity) |
| Auth | ASP.NET Core Identity, JWT Bearer |
| Real-time | SignalR |
| Mapping | AutoMapper |
| Docs | Swagger / Swashbuckle |

## 🏗️ Architecture

```
Restaurant/
├── Restaurant.API             # Controllers, SignalR hub, Program.cs
├── Restaurant.Application     # Services, DTOs, interfaces, AutoMapper profiles
├── Restaurant.Domain          # Entities and enums
└── Restaurant.Infrastructure  # EF Core context & configurations, auth, seeders
```

The Application layer depends on abstractions only (`IApplicationDbContext`, `IKitchenNotifier`, ...), so real-time delivery and persistence can change without touching business logic.

## 🔄 Order flow

1. A waiter opens an order for a table (prices and total are computed on the server) and the table becomes `Occupied`
2. `OrderReceived` is pushed to the branch's kitchen screens
3. The chef moves each item through `Pending → Preparing → Ready`; `ItemStatusChanged` is pushed on every change
4. When every item is `Ready`, the order becomes `Ready` automatically
5. The waiter marks it `Served` (or cancels it); the table is freed when no other active order remains on it

## 🚀 Getting Started

### Prerequisites
- .NET 10 SDK
- SQL Server

### 1. Clone
```bash
git clone https://github.com/eslam352005/Restaurant.git
cd Restaurant
```

### 2. Configure
Put these in `appsettings.json` (use User Secrets for the signing key):

| Key | Description |
|---|---|
| `ConnectionStrings:DefaultConnection` | SQL Server connection string |
| `JwtOptions:Issuer` / `JwtOptions:Audience` | JWT issuer and audience |

```bash
cd Restaurant.API
dotnet user-secrets init
dotnet user-secrets set "JwtOptions:SecretKey" "<long-random-key>"
```

### 3. Create the database
```bash
dotnet ef database update --project Restaurant.Infrastructure --startup-project Restaurant.API
```
Roles and initial data are seeded on startup.

### 4. Run
```bash
dotnet run --project Restaurant.API
```
Open Swagger at `/swagger`.

## 📡 API Overview

| Area | Endpoints |
|---|---|
| Auth | Register, login, refresh token, logout |
| Branches / Categories / Tables / Staff | CRUD |
| Menu items | `GET /api/menu_items`, `GET /{id}`, `POST`, `PUT /{id}`, `PATCH /{id}/availability`, `DELETE /{id}`, `POST /{id}/upload-image` |
| Orders | `GET /api/orders`, `GET /{id}`, `POST`, `PUT /{id}/items`, `PATCH /{id}/status`, `PATCH /{orderId}/items/{itemId}/status`, `DELETE /{id}` |
| Inventory | `GET /api/inventory`, `POST`, `PUT /{id}`, `PATCH /{id}/adjust-stock`, `GET /low-stock-alerts` |
| Reports | `GET /api/report/sales`, `/top-items`, `/staff-performance` |

Full interactive documentation is available in Swagger.

### SignalR

Hub: `/hubs/kitchen?branchId={id}` (each connection joins the group `branch-{id}`)

| Event | Meaning |
|---|---|
| `OrderReceived` | A new order was created |
| `OrderStatusChanged` | An order's status changed |
| `ItemStatusChanged` | A single item's status changed |

## 🗺️ Roadmap
- Redis caching for the menu
- Hangfire jobs (daily reports, automatic low-stock notifications)
- Unit tests

## 👤 Author

Built by **Eslam** — [GitHub](https://github.com/eslam352005)
