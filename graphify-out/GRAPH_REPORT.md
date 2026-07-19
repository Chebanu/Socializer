# Graph Report - Socializer  (2026-07-19)

## Corpus Check
- Corpus is ~10,628 words - fits in a single context window. You may not need a graph.

## Summary
- 226 nodes · 294 edges · 17 communities (14 shown, 3 thin omitted)
- Extraction: 88% EXTRACTED · 12% INFERRED · 1% AMBIGUOUS · INFERRED: 34 edges (avg confidence: 0.91)
- Token cost: 130,000 input · 15,644 output

## Community Hubs (Navigation)
- Solution & Project Scaffolding
- Composition Root & DI Wiring
- Chat & Notifications Modules
- Infra & Core Modules Overview
- Payments & Moderation Modules
- Geo & Map Frontend
- Roadmap Meta & v2 Backlog
- Domain Base Entities & Tests
- Observability & AI Agent
- Launch Settings Config
- Integration Test Harness
- Layered Architecture Walkthrough
- Domain Exceptions
- Contracts Assembly
- Modular Monolith Architecture
- GitHub Setup Guide
- Definition of Done Checklist

## God Nodes (most connected - your core abstractions)
1. `Socializer Development Roadmap (Document)` - 18 edges
2. `User / USERS` - 12 edges
3. `Socializer.Application` - 9 edges
4. `Socializer.IntegrationTests` - 9 edges
5. `Socializer.UnitTests` - 9 edges
6. `Sprint 0 — Repository & Infra Skeleton` - 9 edges
7. `Socializer.Api` - 8 edges
8. `Socializer.Infrastructure` - 8 edges
9. `Sprint 9 — Crypto Payments for Place Subscriptions` - 8 edges
10. `Socializer.Domain` - 7 edges

## Surprising Connections (you probably didn't know these)
- `HealthEndpointTests` --references--> `Program`  [EXTRACTED]
  Socializer.IntegrationTests/HealthEndpointTests.cs → Socializer.Api/Program.cs
- `PostgreSQL Service (PostGIS)` --conceptually_related_to--> `PostgreSQL + PostGIS`  [INFERRED]
  docker-compose.yml → docs/Socializer_Roadmap.md
- `PostgreSQL Service (PostGIS)` --conceptually_related_to--> `PostgreSQL + PostGIS (Data Store)`  [INFERRED]
  docker-compose.yml → docs/Socializer_UML_Diagrams.md
- `Sprint 0 — Repository & Infra Skeleton` --references--> `PostgreSQL Service (PostGIS)`  [EXTRACTED]
  docs/Socializer_Roadmap.md → docker-compose.yml
- `Redis Service` --conceptually_related_to--> `Redis (GEO + Cache + Presence)`  [INFERRED]
  docker-compose.yml → docs/Socializer_Roadmap.md

## Import Cycles
- None detected.

## Hyperedges (group relationships)
- **SignalR Chat Realtime Flow** — docs_socializer_uml_diagrams_chat_module, docs_socializer_uml_diagrams_redis, docs_socializer_uml_diagrams_sequence_connection_chat_push, docs_socializer_roadmap_sprint_7 [INFERRED 0.80]
- **Crypto Payment Anti-Interception Design** — docs_socializer_roadmap_sprint_9, docs_socializer_roadmap_single_use_address_mechanism, docs_socializer_uml_diagrams_derived_address_uniqueness_invariant, docs_socializer_uml_diagrams_sequence_crypto_payment, docs_socializer_uml_diagrams_blockchain_watcher_worker [INFERRED 0.85]
- **Places Nearby Layered Architecture Example** — docs_socializer_uml_diagrams_place_domain_class, docs_socializer_uml_diagrams_iplacesrepository, docs_socializer_uml_diagrams_placesqueryservice, docs_socializer_uml_diagrams_placesrepository, docs_socializer_uml_diagrams_placeresponse, docs_socializer_uml_diagrams_placesnearby_endpoint [EXTRACTED 1.00]

## Communities (17 total, 3 thin omitted)

### Community 0 - "Solution & Project Scaffolding"
Cohesion: 0.08
Nodes (33): Microsoft.AspNetCore.Mvc.Testing (10.0.10), Microsoft.Extensions.Configuration.Abstractions (10.0.10), Microsoft.NET.Sdk.Web, Socializer.Api, net10.0, Socializer.Application, net10.0, Microsoft.Extensions.DependencyInjection.Abstractions (10.0.10) (+25 more)

### Community 1 - "Composition Root & DI Wiring"
Cohesion: 0.09
Nodes (17): Socializer.Application.Common.Interfaces, Socializer.Infrastructure, Socializer.Application, Socializer.Infrastructure.Common, IConfiguration, Program, WeatherForecast, Assembly (+9 more)

### Community 2 - "Chat & Notifications Modules"
Cohesion: 0.11
Nodes (24): Chat Module, Notifications Module, SignalR Realtime, Sprint 7 — Chat (SignalR), Sprint 8 — Notifications (Web Push), BlockedUser / BLOCKED_USERS, Chat / CHATS, Chat Module (SignalR, Api) (+16 more)

### Community 3 - "Infra & Core Modules Overview"
Cohesion: 0.10
Nodes (23): Azurite Blob Emulator Service, azurite_data Volume, Azure Blob Storage (Azurite locally), Docker Compose + Nginx Infra, GitHub Actions CI/CD, Auth Module, Infra Module, Launch Module (+15 more)

### Community 4 - "Payments & Moderation Modules"
Cohesion: 0.14
Nodes (22): Custom HD-Wallet Crypto Payment Module, Moderation Module, Payments Module, Per-Subscription Unique Derived Address (Anti-Interception Mechanism), Sprint 10 — Content Moderation, Sprint 6 — Places Module, Sprint 9 — Crypto Payments for Place Subscriptions, Stablecoin-on-Low-Fee-Chain Decision (USDT/USDC on TRON/Polygon) (+14 more)

### Community 5 - "Geo & Map Frontend"
Cohesion: 0.14
Nodes (20): PostgreSQL Service (PostGIS), postgres_data Volume, Redis Service, redis_data Volume, MapLibre over Leaflet Rationale, Geo Module, Map Frontend Module, PostgreSQL + PostGIS (+12 more)

### Community 6 - "Roadmap Meta & v2 Backlog"
Cohesion: 0.16
Nodes (15): Socializer Development Roadmap (Document), Junior/Strong-Junior Time-Estimate Calibration, Socializer (Product), Events/Meetups as First-Class Entity (v2), Groups / Multi-User Chats (v2), Multi-Region Deployment / Message Queue (v2), Native Mobile Apps (v2), Place Reviews/Ratings (v2) (+7 more)

### Community 7 - "Domain Base Entities & Tests"
Cohesion: 0.18
Nodes (9): Socializer.UnitTests.Common, Socializer.Domain.Common, Guid, DateTime, BaseAuditableEntity, BaseEntity, Fact, BaseAuditableEntityTests (+1 more)

### Community 8 - "Observability & AI Agent"
Cohesion: 0.26
Nodes (13): AI Monitoring Agent Module, Observability Module, Prometheus + Grafana + Serilog/Seq, Sprint 11 — Observability, Sprint 12 — AI Monitoring Agent, AI Monitoring Agent Worker, Socializer.Api Modular Monolith, Email / SMTP (+5 more)

### Community 9 - "Launch Settings Config"
Cohesion: 0.20
Nodes (9): ASPNETCORE_ENVIRONMENT, applicationUrl, commandName, dotnetRunMessages, environmentVariables, launchBrowser, profiles, http (+1 more)

### Community 10 - "Integration Test Harness"
Cohesion: 0.25
Nodes (6): Socializer.IntegrationTests, IClassFixture, Fact, HealthEndpointTests, Task, WebApplicationFactory

### Community 11 - "Layered Architecture Walkthrough"
Cohesion: 0.38
Nodes (7): IPlacesRepository (Port Example), Ports-Only-Where-Needed Layering Rule, Place (Domain Class Example), PlaceResponse (Contract DTO Example), GET /places/nearby Endpoint Example, PlacesQueryService (Application Example), PlacesRepository (Infrastructure Adapter Example)

### Community 12 - "Domain Exceptions"
Cohesion: 0.50
Nodes (3): Socializer.Domain.Exceptions, Exception, DomainException

### Community 13 - "Contracts Assembly"
Cohesion: 0.50
Nodes (3): Socializer.Contracts, Assembly, ContractsAssemblyReference

## Ambiguous Edges - Review These
- `Sprint 0 — Repository & Infra Skeleton` → `Sprint 13 — Security Hardening`  [AMBIGUOUS]
  docs/Socializer_Roadmap.md · relation: references
- `Sprint 15 — Load Testing & Performance` → `Sprint 16 — Polish & Beta Launch`  [AMBIGUOUS]
  docs/Socializer_Roadmap.md · relation: references

## Knowledge Gaps
- **71 isolated node(s):** `WeatherForecast`, `$schema`, `commandName`, `dotnetRunMessages`, `launchBrowser` (+66 more)
  These have ≤1 connection - possible missing edges or undocumented components.
- **3 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **What is the exact relationship between `Sprint 0 — Repository & Infra Skeleton` and `Sprint 13 — Security Hardening`?**
  _Edge tagged AMBIGUOUS (relation: references) - confidence is low._
- **What is the exact relationship between `Sprint 15 — Load Testing & Performance` and `Sprint 16 — Polish & Beta Launch`?**
  _Edge tagged AMBIGUOUS (relation: references) - confidence is low._
- **Why does `Socializer Development Roadmap (Document)` connect `Roadmap Meta & v2 Backlog` to `Observability & AI Agent`, `Chat & Notifications Modules`, `Payments & Moderation Modules`, `Geo & Map Frontend`?**
  _High betweenness centrality (0.092) - this node is a cross-community bridge._
- **Why does `Sprint 0 — Repository & Infra Skeleton` connect `Infra & Core Modules Overview` to `Observability & AI Agent`, `Geo & Map Frontend`?**
  _High betweenness centrality (0.058) - this node is a cross-community bridge._
- **Why does `Sequence: Connection Request → Chat → Push Notification` connect `Chat & Notifications Modules` to `Roadmap Meta & v2 Backlog`?**
  _High betweenness centrality (0.057) - this node is a cross-community bridge._
- **What connects `WeatherForecast`, `$schema`, `commandName` to the rest of the system?**
  _71 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Solution & Project Scaffolding` be split into smaller, more focused modules?**
  _Cohesion score 0.0784313725490196 - nodes in this community are weakly interconnected._