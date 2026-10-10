🍽️ Restaurant Management System | ASP.NET Core Web API

A multi-branch restaurant management system built with Clean Architecture, featuring real-time order tracking via SignalR, JWT authentication with role-based authorization, and business analytics reporting.

Key Features:
- Multi-branch support (Menu, Staff, Inventory, Tables per branch)
- Real-time Kitchen Display System using SignalR (order & item status updates)
- JWT Authentication with Refresh Tokens & role-based access (Admin, Manager, Chef, Waiter)
- Full order lifecycle management (Pending → Preparing → Ready → Served)
- Inventory tracking with low-stock alerts
- Sales, Top Items & Staff Performance reports (EF Core aggregation)
- Image upload for menu items

Tech Stack: ASP.NET Core Web API, EF Core, SQL Server, SignalR, JWT, AutoMapper, Clean Architecture (API / Application / Domain / Infrastructure)
