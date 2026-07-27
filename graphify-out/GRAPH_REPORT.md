# Graph Report - /Users/alexeichebanu/RiderProjects/Socializer  (2026-07-27)

## Corpus Check
- 37 files · ~11,393 words
- Verdict: corpus is large enough that graph structure adds value.

## Summary
- 251 nodes · 280 edges · 32 communities (16 shown, 16 thin omitted)
- Extraction: 92% EXTRACTED · 8% INFERRED · 0% AMBIGUOUS · INFERRED: 21 edges (avg confidence: 0.91)
- Token cost: 86,632 input · 0 output

## Community Hubs (Navigation)
- Backend Solution Structure
- Docker Compose + Roadmap S0
- Frontend NPM Dependencies
- Frontend TS Config
- Social/Chat Domain (UML)
- Infra & AI Services (UML)
- Domain Entity Base + Tests
- Crypto Payments Domain (UML)
- API Health Endpoint
- API Launch Settings
- Frontend Vite TS Config
- Places Module Architecture
- DateTime Abstraction
- Infrastructure DI Setup
- UML Diagram Docs
- Application DI Setup
- Frontend Health Check UI
- Application Assembly Reference
- Contracts Assembly Reference
- Domain Exception Type
- Auth Module (UML)
- Moderation Module (UML)
- Stale Pre-Move Reference
- Stale Pre-Move Reference
- Stale Pre-Move Reference
- Stale Pre-Move Reference
- Stale Pre-Move Reference
- Stale Pre-Move Reference
- Stale Pre-Move Reference
- Stale Pre-Move Reference
- Stale Pre-Move Reference

## God Nodes (most connected - your core abstractions)
1. `compilerOptions` - 16 edges
2. `User / USERS` - 12 edges
3. `Socializer.Api` - 10 edges
4. `Socializer README` - 10 edges
5. `Socializer.Application` - 9 edges
6. `Socializer.UnitTests` - 9 edges
7. `Socializer.IntegrationTests` - 8 edges
8. `Sprint 0 — Repository & Infra Skeleton` - 8 edges
9. `Socializer.Domain` - 7 edges
10. `Socializer.Infrastructure` - 7 edges

## Surprising Connections (you probably didn't know these)
- `Socializer Frontend Entry (Vite/React root)` --references--> `Sprint 0 — Repository & Infra Skeleton`  [INFERRED]
  frontend/index.html → docs/Socializer_Roadmap.md
- `CI Workflow` --references--> `Sprint 0 — Repository & Infra Skeleton`  [INFERRED]
  .github/workflows/ci.yml → docs/Socializer_Roadmap.md
- `Socializer.sln` --shares_data_with--> `Socializer.Api`  [INFERRED]
  .github/workflows/ci.yml → docs/Socializer_Roadmap.md
- `Socializer.sln` --shares_data_with--> `Socializer.Infrastructure`  [INFERRED]
  .github/workflows/ci.yml → docs/Socializer_Roadmap.md
- `Socializer README` --references--> `Socializer Frontend Entry (Vite/React root)`  [EXTRACTED]
  README.md → frontend/index.html

## Import Cycles
- None detected.

## Hyperedges (group relationships)
- **Observability Stack (Prometheus + Grafana + Seq)** — docker_compose_prometheus, docker_compose_grafana, docker_compose_seq, ops_prometheus_prometheus_scrape_config, ops_grafana_provisioning_datasources_datasource_prometheus_datasource, docs_socializer_roadmap_sprint11 [INFERRED 0.85]
- **Sprint 0 Infra Skeleton Deliverables** — docker_compose_postgres, docker_compose_redis, docker_compose_azurite, docker_compose_seq, github_workflows_ci_ci_workflow, frontend_index_socializer_app, docs_socializer_roadmap_sprint0 [INFERRED 0.85]
- **Socializer .NET Solution Projects** — docs_socializer_roadmap_socializer_api, docs_socializer_roadmap_socializer_infrastructure, readme_socializer_api, readme_socializer_infrastructure, github_workflows_ci_socializer_sln [INFERRED 0.75]
- **SignalR Chat Realtime Flow** — docs_socializer_uml_diagrams_chat_module, docs_socializer_uml_diagrams_redis, docs_socializer_uml_diagrams_sequence_connection_chat_push [INFERRED 0.80]
- **Places Nearby Layered Architecture Example** — docs_socializer_uml_diagrams_place_domain_class, docs_socializer_uml_diagrams_iplacesrepository, docs_socializer_uml_diagrams_placesqueryservice, docs_socializer_uml_diagrams_placesrepository, docs_socializer_uml_diagrams_placeresponse, docs_socializer_uml_diagrams_placesnearby_endpoint [EXTRACTED 1.00]

## Communities (32 total, 16 thin omitted)

### Community 0 - "Backend Solution Structure"
Cohesion: 0.08
Nodes (33): Socializer.Api, net10.0, Socializer.Application, net10.0, Microsoft.Extensions.DependencyInjection.Abstractions (10.0.10), Microsoft.NET.Sdk, Socializer.Contracts, net10.0 (+25 more)

### Community 1 - "Docker Compose + Roadmap S0"
Cohesion: 0.15
Nodes (27): api service, azurite service, grafana service, postgres service (PostGIS), prometheus service, redis service, seq service, Socializer Development Roadmap & Backlog (+19 more)

### Community 2 - "Frontend NPM Dependencies"
Cohesion: 0.08
Nodes (24): dependencies, react, react-dom, devDependencies, @types/react, @types/react-dom, typescript, vite (+16 more)

### Community 3 - "Frontend TS Config"
Cohesion: 0.09
Nodes (22): compilerOptions, allowImportingTsExtensions, isolatedModules, jsx, lib, module, moduleResolution, noEmit (+14 more)

### Community 4 - "Social/Chat Domain (UML)"
Cohesion: 0.13
Nodes (19): BlockedUser / BLOCKED_USERS, Chat / CHATS, Chat Module (SignalR, Api), ChatReadState / CHAT_READ_STATE, ConnectionRequest / CONNECTION_REQUESTS, Message / MESSAGES, Nginx (TLS, Reverse Proxy, WS Upgrade), Notifications Module (Api) (+11 more)

### Community 5 - "Infra & AI Services (UML)"
Cohesion: 0.16
Nodes (18): AI Monitoring Agent Worker, Socializer.Api Modular Monolith, Azurite Blob Storage (Data Store), Content Safety API, Email / SMTP, Geo Module (Api), Geo Snapshot Worker, Grafana (+10 more)

### Community 6 - "Domain Entity Base + Tests"
Cohesion: 0.18
Nodes (7): DateTime, BaseAuditableEntity, BaseEntity, Fact, BaseAuditableEntityTests, TestEntity, Guid

### Community 7 - "Crypto Payments Domain (UML)"
Cohesion: 0.23
Nodes (12): Blockchain RPC / Provider, Blockchain Watcher Worker, Unique Derived Address Invariant, ModerationRecord / MODERATION_RECORDS, Payment / PAYMENTS, Payments Module (Api), Photo / PHOTOS, Place / PLACES (+4 more)

### Community 8 - "API Health Endpoint"
Cohesion: 0.24
Nodes (6): Program, Fact, HealthEndpointTests, IClassFixture, Task, WebApplicationFactory

### Community 9 - "API Launch Settings"
Cohesion: 0.20
Nodes (9): ASPNETCORE_ENVIRONMENT, applicationUrl, commandName, dotnetRunMessages, environmentVariables, launchBrowser, profiles, http (+1 more)

### Community 10 - "Frontend Vite TS Config"
Cohesion: 0.22
Nodes (8): compilerOptions, allowSyntheticDefaultImports, composite, module, moduleResolution, skipLibCheck, include, vite.config.ts

### Community 11 - "Places Module Architecture"
Cohesion: 0.38
Nodes (7): IPlacesRepository (Port Example), Ports-Only-Where-Needed Layering Rule, Place (Domain Class Example), PlaceResponse (Contract DTO Example), GET /places/nearby Endpoint Example, PlacesQueryService (Application Example), PlacesRepository (Infrastructure Adapter Example)

### Community 12 - "DateTime Abstraction"
Cohesion: 0.33
Nodes (4): DateTime, IDateTime, DateTime, SystemDateTime

### Community 13 - "Infrastructure DI Setup"
Cohesion: 0.40
Nodes (3): IServiceCollection, DependencyInjection, IConfiguration

### Community 14 - "UML Diagram Docs"
Cohesion: 0.40
Nodes (5): Component / Architecture Diagram, Socializer UML & Architecture Diagrams (Document), Domain / Class Diagram, Entity-Relationship Diagram, Reference Implementation Layer Walkthrough (Places Nearby)

## Knowledge Gaps
- **96 isolated node(s):** `Component / Architecture Diagram`, `Domain / Class Diagram`, `Entity-Relationship Diagram`, `Reference Implementation Layer Walkthrough (Places Nearby)`, `Auth Module (Api)` (+91 more)
  These have ≤1 connection - possible missing edges or undocumented components.
- **16 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `Socializer.Api Modular Monolith` connect `Infra & AI Services (UML)` to `Social/Chat Domain (UML)`?**
  _High betweenness centrality (0.015) - this node is a cross-community bridge._
- **Why does `User / USERS` connect `Social/Chat Domain (UML)` to `Crypto Payments Domain (UML)`?**
  _High betweenness centrality (0.013) - this node is a cross-community bridge._
- **Why does `PostgreSQL + PostGIS (Data Store)` connect `Infra & AI Services (UML)` to `Crypto Payments Domain (UML)`?**
  _High betweenness centrality (0.011) - this node is a cross-community bridge._
- **What connects `Component / Architecture Diagram`, `Domain / Class Diagram`, `Entity-Relationship Diagram` to the rest of the system?**
  _96 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Backend Solution Structure` be split into smaller, more focused modules?**
  _Cohesion score 0.0784313725490196 - nodes in this community are weakly interconnected._
- **Should `Docker Compose + Roadmap S0` be split into smaller, more focused modules?**
  _Cohesion score 0.1452991452991453 - nodes in this community are weakly interconnected._
- **Should `Frontend NPM Dependencies` be split into smaller, more focused modules?**
  _Cohesion score 0.08 - nodes in this community are weakly interconnected._