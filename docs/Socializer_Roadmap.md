---
title: Socializer — Development Roadmap & Backlog
subtitle: Sprint-by-sprint delivery plan for a production-grade geo-social pet project
author: "Project owner: Alexios"
date: Version 1.0 — 2026-07-18
---

# 0. How to Use This Document

This is a **working document**, not a plan you write once and forget. Treat it the way a solo founder-engineer treats a product backlog:

- **Sprints are Milestones.** Each `Sprint N` section below maps 1:1 to a GitHub Milestone. Copy the sprint title as the milestone name (e.g. `Sprint 3 — Redis GEO Core`).
- **Backlog items are Issues.** Section 6 ("Full Backlog") lists every task as a checkbox. Each checkbox is meant to become one GitHub Issue. The `[module]` tag on each line maps to a GitHub Label — see Section 8 for the exact label set.
- **Checkboxes are the source of truth for progress.** Tick them in this file *or* close the corresponding issue — pick one and be consistent. If you move fully to GitHub Projects, this file becomes the read-only "why did we plan it this way" reference, and the Project board becomes the "what's left" reference.
- **Sprint length assumption:** the generic "1–2 weeks per sprint" rule of thumb has been replaced with per-sprint estimates calibrated to a **junior / strong-junior** solo developer working part-time (evenings/weekends). Each sprint below now carries its own `Estimated time` + `Difficulty` line. These are still estimates, not commitments — re-measure your actual velocity after Sprint 2 and adjust the remaining sprints by that factor, same as before.
- **Order is a default, not a law.** Dependencies between sprints are called out explicitly. If you get bored of backend and want to touch the map UI early, Sprint 5 (Map Frontend) only hard-depends on Sprint 3 (Redis GEO Core) — you can pull it forward.
- **Architecture lives in the diagrams doc.** This document deliberately does **not** re-explain component architecture, DB schema, or API contracts in prose — that's `Socializer_UML_Diagrams.md` (component diagram, domain/class diagram, ER diagram, sequence diagrams). This document is scope and sequencing only.

---

# 1. Product Scope Recap

**Socializer** is a location-based social discovery web app:

- Users see nearby people on a live map and can send a connection request to start a chat.
- Sponsored **Places** (bars, cafés, coworkings) appear on the same map as meetup points; place owners pay a **monthly crypto subscription** to stay listed.
- Content moderation keeps places/photos clean by default, with an explicit opt-in for 18+ categories.
- The system watches itself: an **AI monitoring agent** reads Prometheus metrics every 20 minutes, summarizes anomalies via an LLM, and emails the owner when something looks wrong.

**MVP boundary — explicitly OUT of scope for Sprints 0–16:**

- Native mobile apps (web-first, responsive only)
- Group chats / events / communities
- Paid user-side premium tier (only Place subscriptions are monetized in MVP)
- Multi-region deployment, Kubernetes, Kafka/RabbitMQ (single VPS + Docker Compose is enough until real load proves otherwise)
- Recommendation engine (interests-based matching) — noted as a v2 idea, not built now

---

# 2. Tech Stack Snapshot

| Layer | Choice | Notes |
|---|---|---|
| Backend | ASP.NET Core (Modular Monolith) | No microservices until a module demonstrably needs independent scaling |
| Frontend | React + MapLibre GL | MapLibre over Leaflet: vector tiles, better perf, open-source |
| Primary DB | PostgreSQL + PostGIS | Durable geo storage, `geography(Point,4326)`, GiST index |
| Live geo / cache / presence | Redis (GEO commands + pub/sub) | `GEOADD`/`GEOSEARCH` for hot location data; snapshotted into PostGIS periodically |
| Realtime | SignalR (+ Redis backplane, added once multi-instance is real) | Chat, typing indicators, presence |
| Object storage | Azure Blob Storage (Azurite locally) | User/place photos |
| Payments | Custom HD-wallet crypto payment module | Per-place unique receive address, on-chain watcher worker |
| Messaging/queue | none in MVP | Revisit RabbitMQ/Kafka only post-launch if event volume demands it |
| Observability | Prometheus + Grafana + Serilog/Seq | Feeds the AI monitoring agent |
| Infra | Docker Compose + Nginx | Single VPS to start |
| CI/CD | GitHub Actions | Build, test, lint, image publish |

---

# 3. Delivery Model & Tracking

1. Create a GitHub repo `socializer` (or reuse an existing one).
2. Enable **GitHub Projects** (Board view). Columns: `Backlog → Ready → In Progress → In Review → Done`.
3. Create one **Milestone per Sprint** (17 milestones, Sprint 0 → Sprint 16).
4. Import Section 6 checkboxes as Issues (manually at first; once you're past Sprint 2, consider a small script using the GitHub CLI (`gh issue create`) fed from this file — that itself can be a Sprint 0 nice-to-have).
5. Every Issue gets: one `module:*` label, one `type:*` label, one `size:*` label, one `priority:*` label, and is assigned to its Sprint's milestone. Full label set in Section 8.
6. Definition of Done (Section 5) applies to **every** issue, not just sprint-level goals — treat it as your PR checklist template.

---

# 4. Sprint-by-Sprint Roadmap

**Time estimates below are calibrated for a junior / strong-junior solo developer working part-time (evenings/weekends)** — not the generic mid-level "1–2 weeks" assumption this document used before. Ranges are wide on purpose; treat the low end as "if it clicks fast," the high end as "if you hit an unfamiliar wall," which for this level is the normal case, not a failure.

| Sprint | Title | Est. time (your level) | Difficulty |
|---|---|---|---|
| 0 | Repository & Infra Skeleton | 1.5–2 weeks | Standard |
| 1 | Authentication | 2–2.5 weeks | Standard |
| 2 | User Profile Module | 1.5–2 weeks | Standard |
| 3 | Geo Core (Redis GEO) | 2–3 weeks | Standard→Hard |
| 4 | Geo Persistence (PostGIS) | 2.5–3.5 weeks | Hard |
| 5 | Map Frontend (MapLibre) | 1.5–2.5 weeks | Standard |
| 6 | Places Module | 1.5–2 weeks | Standard |
| 7 | Chat (SignalR) | 2.5–3.5 weeks | Hard |
| 8 | Notifications (Web Push) | 1.5–2.5 weeks | Standard |
| 9 | Crypto Payments | 4–6 weeks | Very Hard |
| 10 | Content Moderation | 1.5–2 weeks | Standard |
| 11 | Observability | 1.5–2.5 weeks | Standard |
| 12 | AI Monitoring Agent | 2–3 weeks | Hard |
| 13 | Security Hardening | 2.5–4 weeks | Very Hard |
| 14 | Deployment | 2–3 weeks | Standard→Hard |
| 15 | Load Testing & Performance | 2.5–3.5 weeks | Hard |
| 16 | Polish & Beta Launch | 1.5–2.5 weeks | Standard |
| **Total** | | **≈ 34–50 weeks (≈ 8–12 months)** | |

**Why the range is so wide, and why that's OK:** the low end assumes each sprint's unfamiliar technology (PostGIS, SignalR, HD wallets, LLM prompting, load testing) clicks on roughly the first real attempt. The high end assumes you hit at least one genuine wall per sprint and need to stop, read, experiment, and retry — which, at junior/strong-junior level, is not a sign anything's wrong; it's the actual mechanism by which this project turns you into a stronger engineer. Sprints 9 and 13 get the widest margins because they're the two where "I don't know what I don't know yet" costs the most time, regardless of level.

## Sprint 0 — Repository & Infra Skeleton

**Goal:** An empty app that boots, in Docker, with CI green, before a single feature exists.

**Depends on:** nothing.

**Estimated time (junior/strong-junior, part-time):** 1.5–2 weeks · **Difficulty:** Standard

**Scope:**
- [x] Initialize solution: `Socializer.Api`, `Socializer.Application`, `Socializer.Domain`, `Socializer.Infrastructure`, `Socializer.Contracts`
- [x] Initialize `Socializer.UnitTests`, `Socializer.IntegrationTests` projects
- [x] `.editorconfig`, `Directory.Build.props`, nullable enabled, analyzers on
- [x] Docker Compose skeleton: `postgres`, `redis`, `azurite` services, named volumes
- [x] Backend Dockerfile (multi-stage build)
- [x] `appsettings.json` + `appsettings.Development.json` + secrets via `dotnet user-secrets` (never commit secrets)
- [x] Swagger/OpenAPI wired up
- [x] `/health` and `/health/ready` endpoints (basic, no dependency checks yet)
- [x] Serilog console sink configured; Seq service added to Compose
- [x] GitHub Actions: build + test on every PR
- [x] React app scaffolded (Vite), talks to `/health` to prove connectivity
- [x] README with local dev instructions (`docker compose up`, run migrations, run frontend)

**Definition of Done:** `docker compose up` boots Postgres+Redis+Azurite+API+Frontend; CI is green on a trivial PR; hitting `/health` from the React app returns 200.

---

## Sprint 1 — Authentication

**Goal:** Users can register, log in, and stay logged in safely.

**Depends on:** Sprint 0.

**Estimated time (junior/strong-junior, part-time):** 2–2.5 weeks · **Difficulty:** Standard

**Scope:**
- [ ] `Users` table + EF Core migration (auth fields only: email, password hash, created at)
- [ ] Password hashing (ASP.NET Core Identity or a manual PBKDF2/Argon2 implementation — decide and document why)
- [ ] `POST /auth/register` with email confirmation token (emailed via a stub/log sink in dev)
- [ ] `POST /auth/login` issuing JWT access token
- [ ] Refresh token issuance, storage (hashed in DB), and rotation on use
- [ ] `POST /auth/refresh`
- [ ] `POST /auth/logout` (revoke refresh token)
- [ ] Role model: `User`, `PlaceOwner`, `Admin` (schema only, enforcement comes with each module)
- [ ] Rate limiting on `/auth/*` (ASP.NET Core built-in rate limiter)
- [ ] Password reset flow (request + confirm)
- [ ] FluentValidation (or equivalent) on all auth DTOs
- [ ] Integration tests: register → confirm → login → refresh → logout

**Definition of Done:** A fresh user can go from registration to an authenticated API call using only documented endpoints; expired/rotated refresh tokens are rejected; brute-force login is rate-limited.

---

## Sprint 2 — User Profile Module

**Goal:** A user has an editable profile with photos and privacy settings.

**Depends on:** Sprint 1.

**Estimated time (junior/strong-junior, part-time):** 1.5–2 weeks · **Difficulty:** Standard

**Scope:**
- [ ] `Profiles` table: display name, bio, birth date (→ derive age, never trust client-sent age), interests (tag list)
- [ ] `GET/PUT /users/me/profile`
- [ ] Photo upload endpoint → Azurite Blob container `user-photos`, size/type validation, max N photos
- [ ] Photo deletion + reordering (primary photo concept)
- [ ] Age gate: enforce minimum age (e.g. 18) at registration/profile level — reject earlier, don't just hide later
- [ ] Privacy settings entity: `ShowOnMap` (bool), `Allow18PlusPlaces` (bool, explicit opt-in), visibility radius override
- [ ] `GET /users/{id}` public profile view (respecting privacy settings of the viewer's relationship — MVP: just respects target's `ShowOnMap`)
- [ ] Block/unblock user (simple table, enforced later in Geo + Chat modules)
- [ ] Unit tests for age calculation, privacy default values

**Definition of Done:** A registered user can complete a profile with at least one photo, toggle `ShowOnMap` off and confirm (once Sprint 3/5 land) they disappear from nearby search.

---

## Sprint 3 — Geo Core (Redis GEO)

**Goal:** Live location tracking and fast nearby-user search, in-memory.

**Depends on:** Sprint 2. **Hard prerequisite for:** Sprint 5.

**Estimated time (junior/strong-junior, part-time):** 2–3 weeks · **Difficulty:** Standard→Hard (first real hands-on Redis GEO usage)

**Scope:**
- [ ] `POST /geo/location` — client pushes coordinates (throttled client-side to ~every 15s)
- [ ] Server writes to Redis via `GEOADD` keyed by a single `users:live` geo set
- [ ] Presence tracking: `online` / `offline` / `away` via Redis key with TTL, refreshed on each location ping
- [ ] `GET /geo/nearby?radius=` using `GEOSEARCH`, returns raw candidate user IDs + distance
- [ ] Post-filter pipeline: exclude self, blocked users, `ShowOnMap=false`, banned accounts
- [ ] Enrich filtered results with minimal profile data (name, primary photo, distance) — no N+1 queries, batch fetch
- [ ] Rate-limit `/geo/location` per user (prevent spam updates)
- [ ] Config: max radius, min radius, default radius
- [ ] Load test script (basic) simulating N concurrent location updates — establishes a baseline before PostGIS/history even exists

**Definition of Done:** Two logged-in test accounts within the configured radius of each other appear in one another's `/geo/nearby` response within one location-ping interval; a blocked or hidden user never appears.

---

## Sprint 4 — Geo Persistence (PostGIS)

**Goal:** Durable location history and geo queries that don't depend on Redis being warm.

**Depends on:** Sprint 3.

**Estimated time (junior/strong-junior, part-time):** 2.5–3.5 weeks · **Difficulty:** Hard (PostGIS/geography types and GiST indexing are usually brand new)

**Scope:**
- [ ] `UserLocations` table with `geography(Point,4326)` column
- [ ] GiST index on the geography column
- [ ] Background job (`BackgroundService`) snapshotting Redis live positions into PostGIS on an interval (e.g. every 2–5 minutes) instead of on every ping
- [ ] Migration: enable PostGIS extension, add `ST_DWithin`-based query as a fallback/verification path against `GEOSEARCH` results
- [ ] Data retention policy: how long to keep raw history (e.g. 30 days), a cleanup job
- [ ] Decide and document: is location history ever surfaced to the user (e.g. "places you've been"), or purely internal/analytics? (Affects privacy copy later.)
- [ ] Integration test comparing Redis `GEOSEARCH` result set against a PostGIS `ST_DWithin` query for the same fixture data — they must agree

**Definition of Done:** Killing and restarting Redis does not lose the ability to reconstruct recent approximate positions from Postgres; the retention job actually deletes old rows on schedule.

---

## Sprint 5 — Map Frontend (MapLibre)

**Goal:** Users can actually *see* the map, themselves, and nearby people.

**Depends on:** Sprint 3.

**Estimated time (junior/strong-junior, part-time):** 1.5–2.5 weeks · **Difficulty:** Standard

**Scope:**
- [ ] Integrate MapLibre GL in React
- [ ] Get + display browser geolocation (permission prompt UX, denial fallback state)
- [ ] Push location to `/geo/location` on an interval + on significant movement
- [ ] Render self marker, nearby-user markers (from `/geo/nearby`), with live polling or a lightweight subscribe mechanism
- [ ] Radius selector UI, re-queries `/geo/nearby` on change
- [ ] Marker click → mini profile card (photo, name, distance, "connect" button — button is a stub until Sprint 7)
- [ ] Loading/error/empty states (no geolocation permission, no nearby users, network error)
- [ ] Basic responsive layout (mobile-first, since this is a "walk around a city" use case)

**Definition of Done:** Opening the app in two browser sessions (or two devices) with location permission granted, physically/simulated nearby, shows each user as a marker on the other's map within one polling cycle.

---

## Sprint 6 — Places Module

**Goal:** Sponsored venues exist as a first-class entity and render on the map.

**Depends on:** Sprint 4 (PostGIS), Sprint 5 (map rendering).

**Estimated time (junior/strong-junior, part-time):** 1.5–2 weeks · **Difficulty:** Standard (mostly reusing patterns from Sprints 3–5)

**Layer walkthrough:** see `Socializer_UML_Diagrams.md`, Section 5 ("Reference Implementation — Layer Walkthrough") for a worked example of the nearby-places query traced through Domain → Application → Infrastructure → Contracts → Api before starting this sprint's code.

**Scope:**
- [ ] `Places` table: name, description, category, `geography(Point,4326)` location, owner user id, moderation status, `subscription_paid_until`
- [ ] `POST/PUT /places` (owner-only, requires `PlaceOwner` role)
- [ ] Place photo upload (Azurite `place-photos` container)
- [ ] `GET /places/nearby?radius=` (PostGIS `ST_DWithin`, since places are far lower write-frequency than users — no Redis needed here)
- [ ] Only show places with `moderation_status = Approved` **and** `subscription_paid_until >= now()` on the public endpoint
- [ ] Category enum (bar, café, coworking, park, etc.) + optional 18+ category flag
- [ ] Map integration: render place markers distinctly from user markers, with an info popup
- [ ] Owner dashboard (basic React view): list/edit own places, see subscription status
- [ ] Unit + integration tests for the "unpaid places never appear publicly" rule — this is a business-critical invariant, test it explicitly

**Definition of Done:** A place created by an owner does **not** appear on the public map until both moderation-approved and subscription-active; toggling either off immediately removes it from `/places/nearby`.

---

## Sprint 7 — Chat (SignalR)

**Goal:** Two connected users can exchange real-time messages.

**Depends on:** Sprint 2 (profiles), Sprint 3/5 (people need a way to discover each other first).

**Estimated time (junior/strong-junior, part-time):** 2.5–3.5 weeks · **Difficulty:** Hard (SignalR hub lifecycle + realtime state is a common first-time stumbling block)

**Scope:**
- [ ] `ConnectionRequests` table (sender, receiver, status: pending/accepted/declined)
- [ ] `POST /connections/request`, `POST /connections/{id}/accept`, `POST /connections/{id}/decline`
- [ ] `Chats` + `Messages` tables (a chat is created only after a connection request is accepted)
- [ ] SignalR Hub: `SendMessage`, `JoinChat`, typing indicator events
- [ ] Message persistence on send (don't rely on SignalR alone — REST fallback for history)
- [ ] `GET /chats/{id}/messages` (paginated history)
- [ ] Read-receipt tracking (`last_read_message_id` per user per chat)
- [ ] Online/offline indicator in chat UI, backed by the presence data from Sprint 3
- [ ] Blocked-user enforcement: a blocked user cannot send a new connection request or message
- [ ] React chat UI: conversation list, message thread, typing indicator, unread badge
- [ ] Document (don't yet implement) the Redis backplane requirement for SignalR — implement it here if you already suspect multi-instance soon, otherwise defer to Sprint 14/15 and note it as a deployment risk

**Definition of Done:** User A requests a connection, User B accepts, both can exchange messages in real time, and reloading the page restores full history from REST.

---

## Sprint 8 — Notifications (Web Push)

**Goal:** Users get notified of new connection requests/messages without needing the tab open.

**Depends on:** Sprint 7.

**Estimated time (junior/strong-junior, part-time):** 1.5–2.5 weeks · **Difficulty:** Standard

**Scope:**
- [ ] VAPID key generation and storage (server config)
- [ ] `PushSubscriptions` table (per-user, per-browser)
- [ ] Frontend: request notification permission, register Service Worker, subscribe, send subscription to backend
- [ ] Server-side push send helper (Web Push protocol)
- [ ] Trigger push on: new connection request, new message (title/body must **not** contain message content — "You have a new message" only, per the explicit privacy requirement)
- [ ] Notification preferences (mute per-chat, global on/off)
- [ ] Service Worker click handling → opens the app to the relevant chat/request
- [ ] Handle expired/invalid push subscriptions (cleanup on send failure)

**Definition of Done:** With the app tab closed, a new connection request triggers an OS-level browser notification with no message content visible in it; clicking it opens the right conversation.

---

## Sprint 9 — Crypto Payments for Place Subscriptions

**Goal:** A place owner can pay a monthly subscription in crypto, verifiably, without payment-interception risk.

**Depends on:** Sprint 6 (Places must exist).

**Estimated time (junior/strong-junior, part-time):** 4–6 weeks · **Difficulty:** Very Hard (HD wallet derivation, chain integration, and money-correctness logic are unfamiliar domains for almost everyone at this stage — budget the most slack here of any sprint)

**Scope:**
- [ ] Decide and document the chain/asset (recommendation: a stablecoin on a low-fee chain, e.g. USDT/USDC on TRON or Polygon, to avoid subscription price swinging with volatile crypto prices — capture the decision and why)
- [ ] HD wallet setup: master seed (stored in a secrets manager / environment secret, **never** in source control or DB in plaintext), BIP-32/44 derivation
- [ ] `Subscriptions` table: place id, derived address, derivation index, amount due, status (`AwaitingPayment`/`Paid`/`Expired`), paid-until date
- [ ] On subscription creation: derive the **next unused index**, generate a unique receive address per place/billing-cycle (this is the core anti-interception mechanism — a leaked address is single-use)
- [ ] `Payments` table: address, tx hash, amount, confirmations, matched subscription id
- [ ] Blockchain watcher `BackgroundService`: polls chain (via a node RPC or a provider API) for incoming transactions to any address in the active derivation range
- [ ] Matching logic: tx hash + amount + address → mark `Payments` row, require N confirmations before marking `Paid`
- [ ] On `Paid`: extend `Place.subscription_paid_until`, emit an internal event/log
- [ ] Idempotency: a matched tx hash can only ever activate one subscription (dedupe key = tx hash)
- [ ] Owner-facing UI: "Send exactly X USDT to this address" with a QR code, live status polling
- [ ] Expiry job: subscriptions past `paid_until` flip the place back to unlisted (ties into Sprint 6's "only paid places show" invariant)
- [ ] Integration tests against a testnet or a mocked chain client — do **not** test against real mainnet funds

**Definition of Done:** Two different places get two different addresses for the same billing cycle; a payment to Place A's address never activates Place B's subscription even if amounts match; an underpayment or wrong-address payment does not silently mark anything paid.

---

## Sprint 10 — Content Moderation

**Goal:** Photos and place listings are checked before going public; 18+ content is opt-in only.

**Depends on:** Sprint 2 (user photos), Sprint 6 (places).

**Estimated time (junior/strong-junior, part-time):** 1.5–2 weeks · **Difficulty:** Standard (mostly third-party API integration)

**Scope:**
- [ ] `ModerationRecords` table: target type (photo/place), target id, status, provider response, reviewed at
- [ ] Integration with a content-safety API (OpenAI Moderation API or Azure AI Content Safety — pick one, document why) for text (place name/description, bio) and images (photos)
- [ ] Moderation runs automatically on: profile photo upload, place creation/edit, place photo upload
- [ ] Default status `Pending` until the moderation call returns; content is **not public** while pending
- [ ] Auto-reject clearly-flagged content; auto-approve clean content; borderline cases queue for manual review (simple admin-only endpoint/screen)
- [ ] Enforce `Allow18PlusPlaces` opt-in from Sprint 2: 18+-flagged places are filtered out of `/places/nearby` for users who haven't opted in, regardless of moderation status
- [ ] Manual admin review endpoints: list pending, approve, reject
- [ ] Re-run moderation on edit (editing a place description re-triggers the check, doesn't just trust the original approval)

**Definition of Done:** A newly uploaded photo is not publicly visible until moderation resolves it as approved; a user who has not opted into 18+ content never sees an 18+ place on the map even if it's approved and paid.

---

## Sprint 11 — Observability

**Goal:** The system exposes enough signal to actually know if it's healthy.

**Depends on:** Sprint 0 (infra), ideally after most modules exist so there's something to measure.

**Estimated time (junior/strong-junior, part-time):** 1.5–2.5 weeks · **Difficulty:** Standard (mostly config/wiring, not novel logic)

**Scope:**
- [ ] Prometheus metrics endpoint (`/metrics`) via a .NET metrics library (e.g. `prometheus-net`)
- [ ] Custom metrics: request duration histogram, error rate counter, active SignalR connections gauge, Redis GEO query latency, background job durations (blockchain watcher, snapshot job, moderation queue)
- [ ] Prometheus service in Docker Compose, scrape config targeting the API
- [ ] Grafana service in Docker Compose, provisioned dashboards (don't hand-build them in the UI and lose them — check dashboard JSON into the repo)
- [ ] Dashboards: HTTP overview, error rates, Redis/Postgres health, SignalR connections, payment watcher status
- [ ] `/health/ready` expanded to actually check Postgres, Redis, Azurite connectivity (not just "process is up")
- [ ] Correlation IDs threaded through Serilog so a single request's logs are traceable end to end
- [ ] Alerting rules defined in Prometheus (even if not yet wired to a notification channel — that's Sprint 12's job via the AI agent, or Alertmanager as a fallback)

**Definition of Done:** Grafana, opened cold after a fresh `docker compose up`, shows live dashboards with no manual clicking required; killing the Postgres container flips `/health/ready` to unhealthy within one check interval.

---

## Sprint 12 — AI Monitoring Agent

**Goal:** An automated "junior SRE" that reads metrics every 20 minutes and emails you if something's off.

**Depends on:** Sprint 11.

**Estimated time (junior/strong-junior, part-time):** 2–3 weeks · **Difficulty:** Hard (prompt design for reliable structured output takes iteration)

**Scope:**
- [ ] New `BackgroundService`, timer-triggered every 20 minutes (configurable)
- [ ] Prometheus HTTP API client: run a fixed set of PromQL queries (error rate last 20m, p95/p99 latency, memory growth, CPU, SignalR disconnect count, 5xx count, slow-query indicator if available)
- [ ] Log tail/query (Seq API or structured log file) for recent ERROR/WARN entries, deduplicated
- [ ] Prompt template: fixed system prompt ("You are a DevOps engineer reviewing metrics for a small production app...") + the collected metrics/log excerpt as structured input
- [ ] Call to an LLM API (Claude) with that prompt, requesting a structured summary (status per area + free-text findings)
- [ ] Threshold logic: only send an email if the agent (or a simple rule layer) flags something above a "nothing to report" baseline — don't spam an email every 20 minutes when everything's fine; instead send a lightweight "all clear" digest daily and a real alert immediately when something's flagged
- [ ] Email delivery (SMTP or a transactional email provider) to the owner's address
- [ ] Store each run's report (DB or flat file) so you can look back at trend history, not just the latest snapshot
- [ ] Guardrails: cap PromQL query cost/time range, timeout the LLM call, never let this worker's failure affect the main app (isolate exceptions)

**Definition of Done:** Deliberately breaking something (e.g. stopping Redis, or forcing 500s from a debug endpoint) results in an email arriving within one 20-minute cycle describing the anomaly in plain language, with a plausible root-cause guess.

---

## Sprint 13 — Security Hardening

**Goal:** A pass dedicated entirely to closing gaps that feature work doesn't naturally cover.

**Depends on:** most prior sprints (this is a hardening pass over the whole surface).

**Estimated time (junior/strong-junior, part-time):** 2.5–4 weeks · **Difficulty:** Very Hard (the hard part is knowing what to check, not implementing the fix — consider pairing this sprint with outside review/checklist resources)

**Scope:**
- [ ] CORS policy locked to actual frontend origin(s), not `AllowAll`
- [ ] CSP headers configured (and tuned so MapLibre/SignalR/Web Push still work)
- [ ] Global rate limiting policy review (not just `/auth/*` — geo updates, chat sends, place creation)
- [ ] Input validation audit: every DTO has explicit validation, no relying on EF constraints alone
- [ ] SQL injection review (should be near-zero risk with EF Core parameterized queries, but audit any raw SQL/Dapper usage)
- [ ] Secrets audit: confirm nothing sensitive (JWT signing key, wallet seed, API keys, SMTP creds) is in source control or plain appsettings committed to git
- [ ] JWT hardening: short-lived access tokens, refresh token rotation confirmed working, revocation on password change
- [ ] File upload hardening: content-type sniffing beyond extension, size limits enforced server-side, stored filenames never trust client input
- [ ] Dependency vulnerability scan (`dotnet list package --vulnerable`, `npm audit`) and fix criticals
- [ ] HTTPS-only enforcement (redirect http→https, HSTS header)
- [ ] Basic penetration-style self-check: try to access another user's data by ID manipulation (IDOR check) across every module

**Definition of Done:** A written short security checklist (can literally be this section) is fully ticked, with any accepted risks explicitly documented as "known, accepted, revisit if X" rather than silently skipped.

---

## Sprint 14 — Deployment

**Goal:** The app runs on a real VPS, reachable over the internet, with TLS.

**Depends on:** Sprint 13 (don't expose an unhardened app publicly).

**Estimated time (junior/strong-junior, part-time):** 2–3 weeks · **Difficulty:** Standard→Hard (Nginx/TLS/CI-CD wiring is fiddly the first time, but well-documented)

**Scope:**
- [ ] Provision a VPS (sizing based on Sprint 15 load-test results if available, otherwise a reasonable default)
- [ ] Production Docker Compose profile (resource limits, restart policies, no dev-only services like Azurite unless you're intentionally staying off real Azure Blob)
- [ ] Nginx reverse proxy config: routing to API and frontend, WebSocket upgrade headers for SignalR
- [ ] TLS via Let's Encrypt/Certbot, auto-renewal
- [ ] Environment-specific secrets management on the VPS (not committed, not baked into images)
- [ ] Database backup strategy (scheduled `pg_dump`, off-box storage)
- [ ] Deployment script or GitHub Actions workflow: build → push image → SSH deploy → run migrations → health check
- [ ] Rollback plan documented (previous image tag redeploy)
- [ ] DNS + domain pointed at the VPS
- [ ] Smoke test checklist run against production immediately after each deploy

**Definition of Done:** The app is reachable at a real domain over HTTPS, a fresh signup → nearby search → chat → place payment flow works end-to-end against production infrastructure, and a database backup file exists and has been test-restored at least once.

---

## Sprint 15 — Load Testing & Performance

**Goal:** Know your actual limits before real users find them for you.

**Depends on:** Sprint 14 (test against a prod-like environment).

**Estimated time (junior/strong-junior, part-time):** 2.5–3.5 weeks · **Difficulty:** Hard (interpreting load-test results and finding the actual bottleneck takes practice)

**Scope:**
- [ ] Load test tool selection (k6 or NBomber) and baseline scripts for: location update endpoint, nearby search, chat message send, SignalR connection churn
- [ ] Identify and fix N+1 queries (EF Core `AsNoTracking` where read-only, projection instead of full entity loads)
- [ ] Add/verify indexes matching actual query patterns (not just the ones assumed at design time)
- [ ] Redis GEO query performance at a simulated realistic user count
- [ ] SignalR scale test: many concurrent connections, confirm whether/when the Redis backplane becomes necessary (implement it now if the number is closer than expected)
- [ ] Response caching review where safe (place list, public profile fields)
- [ ] Compiled EF Core queries for the hottest read paths, if profiling justifies it
- [ ] Document the load ceiling found ("system holds up to X concurrent users / Y requests/sec before Z degrades") — this becomes both a resume talking point and a real capacity plan

**Definition of Done:** A written report exists with actual numbers (not "it feels fast"), at least one real bottleneck was found and fixed during this sprint, and the fix is verified by re-running the same load test.

---

## Sprint 16 — Polish & Beta Launch

**Goal:** Ship a version real strangers can use without you standing over their shoulder.

**Depends on:** everything above.

**Estimated time (junior/strong-junior, part-time):** 1.5–2.5 weeks · **Difficulty:** Standard

**Scope:**
- [ ] Full manual bug bash across every flow (registration → profile → map → connect → chat → notification → place creation → payment → moderation)
- [ ] Empty/loading/error states audited across the whole frontend, not just the map
- [ ] Copy pass: onboarding text, privacy explanations (especially around location sharing and 18+ opt-in — this needs to be genuinely clear, not just legally-covering)
- [ ] Terms of Service / Privacy Policy draft (even a basic one — you're handling location data and payments)
- [ ] Analytics review: confirm Grafana dashboards and the AI agent reports are actually useful day-to-day, not just theoretically wired up
- [ ] Invite a small closed beta group, collect feedback
- [ ] Triage beta feedback into a v2 backlog (see Section 7)
- [ ] Public beta launch checklist executed

**Definition of Done:** A handful of real external users have gone through the full flow unassisted and produced usable feedback; nothing in the bug bash is rated launch-blocking.

---

# 5. Global Definition of Done

Apply this checklist to **every** issue/PR, regardless of sprint:

- [ ] Code compiles with no new analyzer warnings
- [ ] Unit tests added/updated for new logic; integration tests added for new endpoints
- [ ] No secrets, connection strings, or API keys committed
- [ ] Swagger/OpenAPI reflects the change (for API changes)
- [ ] Manual smoke test performed locally via Docker Compose
- [ ] Relevant Grafana/metrics touched if the change affects a monitored path (post-Sprint 11)
- [ ] PR description explains the *why*, not just the *what*

---

# 6. Full Backlog (Appendix)

Grouped by module. Each line is meant to become one GitHub Issue: `[module] Title`. Sprint mapping in brackets at the end of each item where useful (`S0`…`S16`); unmarked items are candidates to slot into whichever sprint has room, or v2.

### [infra] Infrastructure & DevOps
- [x] Solution/project structure scaffolded (S0)
- [x] Docker Compose base file (S0)
- [x] Backend Dockerfile, multi-stage (S0)
- [ ] Frontend Dockerfile / build pipeline (S0)
- [x] `.editorconfig` + analyzers (S0)
- [x] GitHub Actions CI: build + test (S0)
- [ ] GitHub Actions CI: lint frontend (S0)
- [x] Seq service in Compose (S0)
- [x] `dotnet user-secrets` documented in README (S0)
- [ ] EF Core migrations bootstrap + `dotnet ef database update` documented (S0)
- [ ] Docker Compose prod profile (S14)
- [ ] Nginx reverse proxy config (S14)
- [ ] TLS via Certbot + auto-renew cron (S14)
- [ ] Deployment GitHub Actions workflow (S14)
- [ ] Database backup script + off-box storage (S14)
- [ ] Rollback runbook document (S14)
- [ ] `gh issue create` import script from this roadmap (nice-to-have, S0/S1)
- [ ] Staging environment separate from production (v2)

### [auth] Authentication
- [ ] `Users` table + migration (S1)
- [ ] Password hashing implementation (S1)
- [ ] Register endpoint + email confirmation token (S1)
- [ ] Login endpoint issuing JWT (S1)
- [ ] Refresh token issuance + rotation (S1)
- [ ] Refresh endpoint (S1)
- [ ] Logout / revoke endpoint (S1)
- [ ] Role schema: User/PlaceOwner/Admin (S1)
- [ ] Rate limiting on auth endpoints (S1)
- [ ] Password reset request + confirm flow (S1)
- [ ] FluentValidation on auth DTOs (S1)
- [ ] Integration test: full auth lifecycle (S1)
- [ ] Revoke all refresh tokens on password change (S13)
- [ ] 2FA (v2)
- [ ] OAuth login (Google/Apple) (v2)

### [users] User Profile
- [ ] `Profiles` table + migration (S2)
- [ ] Get/update own profile endpoints (S2)
- [ ] Photo upload to Azurite (S2)
- [ ] Photo delete/reorder (S2)
- [ ] Server-side age gate enforcement (S2)
- [ ] Privacy settings entity (`ShowOnMap`, `Allow18PlusPlaces`, radius override) (S2)
- [ ] Public profile view endpoint (S2)
- [ ] Block/unblock user table + endpoints (S2)
- [ ] Unit tests: age calc, privacy defaults (S2)
- [ ] Interests tag list + tag search (v2, unless pulled forward)
- [ ] Profile completeness indicator (nice-to-have)

### [geo] Geo Core
- [ ] `POST /geo/location` endpoint (S3)
- [ ] Redis `GEOADD` write path (S3)
- [ ] Presence TTL keys (online/offline/away) (S3)
- [ ] `GET /geo/nearby` via `GEOSEARCH` (S3)
- [ ] Post-filter: self/blocked/hidden/banned exclusion (S3)
- [ ] Batch profile enrichment for nearby results (S3)
- [ ] Rate limit on location updates (S3)
- [ ] Configurable min/max/default radius (S3)
- [ ] Baseline load test script for geo endpoints (S3)
- [ ] `UserLocations` PostGIS table + migration (S4)
- [ ] GiST index on geography column (S4)
- [ ] Redis→PostGIS snapshot background job (S4)
- [ ] Retention policy + cleanup job (S4)
- [ ] Redis vs PostGIS result-consistency integration test (S4)

### [map] Map Frontend
- [ ] MapLibre GL integration (S5)
- [ ] Browser geolocation permission flow (S5)
- [ ] Location push interval + significant-move trigger (S5)
- [ ] Self + nearby markers rendering (S5)
- [ ] Radius selector UI (S5)
- [ ] Marker click → mini profile card (S5)
- [ ] Empty/error/loading states for map (S5)
- [ ] Mobile-responsive layout (S5)
- [ ] Place markers rendered distinctly (S6)
- [ ] Live/streaming marker updates instead of polling (v2 perf upgrade)

### [places] Places
- [ ] `Places` table + migration (S6)
- [ ] Create/update place endpoints, owner-only (S6)
- [ ] Place photo upload (S6)
- [ ] `GET /places/nearby` via PostGIS (S6)
- [ ] Enforce paid+approved visibility rule (S6)
- [ ] Category enum + 18+ flag (S6)
- [ ] Owner dashboard UI (S6)
- [ ] Test: unpaid/unapproved places never public (S6)
- [ ] Place trending score (likes + visits + messages + dwell time) (v2)
- [ ] Place reviews/ratings (v2)

### [chat] Chat
- [ ] `ConnectionRequests` table + endpoints (S7)
- [ ] `Chats`/`Messages` tables (S7)
- [ ] SignalR Hub: send/join/typing (S7)
- [ ] Message persistence on send (S7)
- [ ] Paginated message history endpoint (S7)
- [ ] Read-receipt tracking (S7)
- [ ] Online/offline indicator in chat (S7)
- [ ] Blocked-user enforcement in chat (S7)
- [ ] Chat UI: list, thread, typing, unread badge (S7)
- [ ] Redis SignalR backplane (S7 or deferred to S14/15 — pick one explicitly)
- [ ] Message delivery retry/offline queue (v2)

### [notifications] Notifications
- [ ] VAPID keys + config (S8)
- [ ] `PushSubscriptions` table (S8)
- [ ] Frontend Service Worker + subscribe flow (S8)
- [ ] Push send helper (S8)
- [ ] Trigger on connection request / message, content-free body (S8)
- [ ] Notification preferences (mute/global toggle) (S8)
- [ ] Service Worker click → deep link (S8)
- [ ] Expired subscription cleanup (S8)
- [ ] Email digest fallback for users without push enabled (v2)

### [payments] Crypto Payments
- [ ] Chain/asset decision documented (S9)
- [ ] HD wallet master seed setup + secret storage (S9)
- [ ] `Subscriptions` table + migration (S9)
- [ ] Per-place unique address derivation on subscription creation (S9)
- [ ] `Payments` table + migration (S9)
- [ ] Blockchain watcher `BackgroundService` (S9)
- [ ] Tx matching logic (address + amount + confirmations) (S9)
- [ ] Idempotent activation keyed by tx hash (S9)
- [ ] Subscription expiry job (S9)
- [ ] Owner payment UI (address, QR, live status) (S9)
- [ ] Integration tests against testnet/mock chain client (S9)
- [ ] Underpayment/overpayment handling policy (S9)
- [ ] Multi-cycle renewal reminders (email, ties into S8/notifications) (v2)
- [ ] Refund/dispute process (manual, documented) (v2)

### [moderation] Content Moderation
- [ ] `ModerationRecords` table (S10)
- [ ] Content-safety API integration (text + image) (S10)
- [ ] Auto-trigger on photo upload / place create-edit (S10)
- [ ] Pending-hides-from-public enforcement (S10)
- [ ] Auto-approve/auto-reject thresholds (S10)
- [ ] Manual admin review endpoints/screen (S10)
- [ ] 18+ opt-in enforcement in place visibility (S10)
- [ ] Re-run moderation on content edit (S10)
- [ ] User reporting/flagging flow (v2)

### [observability] Observability
- [ ] `/metrics` endpoint via prometheus-net (S11)
- [ ] Custom metrics: latency, errors, SignalR connections, Redis geo latency, job durations (S11)
- [ ] Prometheus service + scrape config (S11)
- [ ] Grafana service + dashboards-as-code (S11)
- [ ] Deep `/health/ready` checks (Postgres/Redis/Azurite) (S11)
- [ ] Correlation IDs in Serilog (S11)
- [ ] Prometheus alerting rules (S11)

### [ai-agent] AI Monitoring Agent
- [ ] Timer `BackgroundService`, 20-minute interval (S12)
- [ ] Prometheus query client + fixed PromQL set (S12)
- [ ] Log tail/query integration (S12)
- [ ] LLM prompt template + structured output request (S12)
- [ ] Threshold logic (alert vs. daily digest) (S12)
- [ ] Email delivery integration (S12)
- [ ] Report history storage (S12)
- [ ] Failure isolation guardrails (timeouts, exception containment) (S12)
- [ ] Slack/Telegram delivery channel in addition to email (v2)

### [security] Security Hardening
- [ ] CORS lockdown (S13)
- [ ] CSP headers tuned for MapLibre/SignalR/Push (S13)
- [ ] Rate limiting review across all write endpoints (S13)
- [ ] Input validation audit (S13)
- [ ] Raw SQL/Dapper injection audit (S13)
- [ ] Secrets-in-repo audit (S13)
- [ ] JWT/refresh hardening review (S13)
- [ ] File upload hardening (content sniffing, size limits) (S13)
- [ ] Dependency vulnerability scan + fixes (S13)
- [ ] HTTPS-only + HSTS (S13)
- [ ] IDOR self-check across all modules (S13)

### [perf] Performance & Load
- [ ] Load test tool selection + baseline scripts (S15)
- [ ] N+1 query audit and fixes (S15)
- [ ] Index audit against real query patterns (S15)
- [ ] Redis GEO perf test at scale (S15)
- [ ] SignalR concurrent-connection scale test (S15)
- [ ] Response caching for safe read paths (S15)
- [ ] Compiled EF Core queries for hot paths (S15)
- [ ] Written capacity report (S15)

### [launch] Polish & Beta
- [ ] Full manual bug bash across all flows (S16)
- [ ] Empty/loading/error state audit (S16)
- [ ] Onboarding + privacy copy pass (S16)
- [ ] Terms of Service / Privacy Policy draft (S16)
- [ ] Analytics/dashboard usefulness review (S16)
- [ ] Closed beta group invite + feedback collection (S16)
- [ ] Beta feedback triage into v2 backlog (S16)
- [ ] Public beta launch checklist (S16)

---

# 7. v2 Ideas (Explicitly Deferred, Not Forgotten)

- Recommendation engine (interests, language, time-of-day, favorite places → suggested people)
- Premium user subscription tier
- Events / meetups as a first-class entity
- Groups / multi-user chats
- User reviews/ratings for places
- Push channel diversification (Slack/Telegram for the AI agent, not just email)
- Multi-region deployment, message queue (RabbitMQ/Kafka) if event volume justifies it
- Native mobile apps
- Recurring subscription auto-renewal UX improvements, refund/dispute handling

---

# 8. GitHub Setup Guide

**Labels:**
- Module: `module:infra`, `module:auth`, `module:users`, `module:geo`, `module:map`, `module:places`, `module:chat`, `module:notifications`, `module:payments`, `module:moderation`, `module:observability`, `module:ai-agent`, `module:security`, `module:perf`, `module:launch`
- Type: `type:backend`, `type:frontend`, `type:infra`, `type:docs`, `type:test`
- Priority: `priority:P0` (blocking), `priority:P1` (important), `priority:P2` (nice-to-have)
- Size: `size:S` (<2h), `size:M` (half day), `size:L` (multi-day — consider splitting)

**Milestones:** one per sprint, named exactly as the `## Sprint N — Title` headers above, so this document and the board never drift out of sync in naming.

**Board columns:** `Backlog → Ready → In Progress → In Review → Done`. Only pull an issue into `Ready` once its dependencies (called out in each sprint's "Depends on") are actually met.
