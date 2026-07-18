# Socializer — UML & Architecture Diagrams

All diagrams are Mermaid — they render natively in GitHub (README, PRs, Issues, wikis) and in most IDE Markdown previews. No external tool required; edit the code blocks directly as the design evolves.

Companion document: `Socializer_Roadmap.md` (sprint plan + backlog). This file is the "what it looks like" reference; the roadmap is the "in what order do we build it" reference.

---

## 1. Component / Architecture Diagram

High-level view of how the pieces talk to each other: modular monolith backend, its data stores, and the background workers that don't sit on the request path.

```mermaid
flowchart TB
    RC["React SPA<br/>MapLibre GL"]
    SW["Service Worker<br/>Web Push"]
    NGX["Nginx<br/>TLS · reverse proxy · WS upgrade"]

    subgraph Api["Socializer.Api — Modular Monolith"]
        direction TB
        subgraph ApiRow1[" "]
            direction LR
            AuthM["Auth"]
            UsersM["Users"]
            GeoM["Geo"]
            PlacesM["Places"]
        end
        subgraph ApiRow2[" "]
            direction LR
            ChatM["Chat<br/>SignalR"]
            NotifM["Notifications"]
            PayM["Payments"]
            ModM["Moderation"]
        end
    end

    subgraph Data["Data Stores"]
        direction LR
        PG[("PostgreSQL<br/>+ PostGIS")]
        RD[("Redis<br/>GEO · Cache · Presence")]
        AZ[("Azurite<br/>Blob Storage")]
    end

    subgraph Workers["Background Workers"]
        direction LR
        SnapW["Geo Snapshot"]
        ChainW["Blockchain<br/>Watcher"]
        ModW["Moderation<br/>Worker"]
        AiW["AI Monitoring<br/>Agent"]
    end

    subgraph Obs["Observability"]
        direction LR
        Prom["Prometheus"]
        Graf["Grafana"]
        SeqL["Seq / Serilog"]
    end

    subgraph External["External Services"]
        direction LR
        Chain["Blockchain<br/>RPC"]
        CSafety["Content Safety<br/>API"]
        LLM["LLM API"]
        Mail["Email / SMTP"]
    end

    RC -->|HTTPS + WebSocket| NGX
    NGX --> Api
    Api --> Data
    Api -. metrics .-> Prom
    Api -. logs .-> SeqL
    Prom --> Graf
    Workers --> Data
    Workers --> External
    NotifM -. push .-> SW
    SW -.-> RC
```

**Reading notes**

- This is the *shape* of the system, not every wire — one aggregated arrow stands in for "every module in this box talks to every store in that box over synchronous calls." Solid = request path, dashed = async/background/observability.
- The exact who-talks-to-whom detail (which the arrows would otherwise tangle trying to show) is spelled out below instead of on the canvas:

  | From | To | Why |
  |---|---|---|
  | Geo Module | Redis | live `GEOADD`/`GEOSEARCH` — this is the hot path, not Postgres |
  | Chat Module | Redis | SignalR backplane — only load-bearing once you run >1 API instance (Sprint 7 ships without it, Sprint 14/15 may add it) |
  | Users/Places Modules | Azurite | photo blobs |
  | Geo Snapshot Worker | Redis → Postgres | periodic flush of live positions into durable history |
  | Blockchain Watcher | Blockchain RPC → Postgres | polls chain, matches tx to a `Subscription`, marks it paid |
  | Moderation Worker | Content Safety API → Postgres | async check on photo/place upload |
  | AI Monitoring Agent | Prometheus + Seq → LLM API → Email | the 20-minute self-monitoring cycle (see sequence diagram 4.4) |

- Everything under **Workers** runs independently of HTTP requests; a slow blockchain RPC or LLM call never blocks a user-facing endpoint.

---

## 2. Domain / Class Diagram

Core domain entities and their relationships, independent of exact DB column types (see the ER diagram for that level of detail).

```mermaid
classDiagram
    class User {
        +Guid Id
        +string Email
        +string PasswordHash
        +bool EmailConfirmed
        +DateTime BirthDate
        +DateTime CreatedAt
        +bool IsBanned
    }

    class Role {
        +Guid Id
        +string Name
    }

    class Profile {
        +Guid Id
        +Guid UserId
        +string DisplayName
        +string Bio
        +List~string~ Interests
    }

    class PrivacySettings {
        +Guid UserId
        +bool ShowOnMap
        +bool Allow18PlusPlaces
        +int RadiusOverrideMeters
    }

    class Photo {
        +Guid Id
        +Guid OwnerId
        +OwnerType OwnerType
        +string BlobUrl
        +int SortOrder
        +ModerationStatus Status
    }

    class UserLocation {
        +Guid Id
        +Guid UserId
        +Point Location
        +DateTime RecordedAt
    }

    class BlockedUser {
        +Guid BlockerId
        +Guid BlockedId
        +DateTime CreatedAt
    }

    class Place {
        +Guid Id
        +Guid OwnerId
        +string Name
        +string Description
        +PlaceCategory Category
        +bool Is18Plus
        +Point Location
        +ModerationStatus ModerationStatus
        +DateTime SubscriptionPaidUntil
    }

    class Subscription {
        +Guid Id
        +Guid PlaceId
        +string DerivedAddress
        +int DerivationIndex
        +decimal AmountDue
        +SubscriptionStatus Status
        +DateTime PaidUntil
    }

    class Payment {
        +Guid Id
        +Guid SubscriptionId
        +string TxHash
        +decimal Amount
        +int Confirmations
        +DateTime DetectedAt
    }

    class ConnectionRequest {
        +Guid Id
        +Guid SenderId
        +Guid ReceiverId
        +ConnectionStatus Status
        +DateTime CreatedAt
    }

    class Chat {
        +Guid Id
        +Guid UserAId
        +Guid UserBId
        +DateTime CreatedAt
    }

    class Message {
        +Guid Id
        +Guid ChatId
        +Guid SenderId
        +string Body
        +DateTime SentAt
    }

    class PushSubscription {
        +Guid Id
        +Guid UserId
        +string Endpoint
        +string P256dhKey
        +string AuthKey
    }

    class ModerationRecord {
        +Guid Id
        +Guid TargetId
        +ModerationTargetType TargetType
        +ModerationStatus Status
        +string ProviderResponse
        +DateTime ReviewedAt
    }

    User "1" --> "1" Profile : has
    User "1" --> "1" PrivacySettings : has
    User "1" --> "*" Photo : uploads
    User "*" --> "*" Role : has
    User "1" --> "*" UserLocation : tracked as
    User "1" --> "*" BlockedUser : blocks
    User "1" --> "*" Place : owns
    User "1" --> "*" ConnectionRequest : sends
    User "1" --> "*" Message : sends
    User "1" --> "*" PushSubscription : registers

    Place "1" --> "*" Photo : has
    Place "1" --> "1" Subscription : billed via
    Subscription "1" --> "*" Payment : receives
    Place "1" --> "1" ModerationRecord : reviewed via

    ConnectionRequest "1" --> "0..1" Chat : creates on accept
    Chat "1" --> "*" Message : contains
    Photo "1" --> "0..1" ModerationRecord : reviewed via
```

---

## 3. Entity-Relationship (ER) Diagram

Database-level view: tables, keys, and cardinalities — this is what the EF Core migrations in Sprints 1–10 should converge toward.

```mermaid
erDiagram
    USERS ||--o| PROFILES : "has"
    USERS ||--o| PRIVACY_SETTINGS : "has"
    USERS ||--o{ PHOTOS : "uploads"
    USERS ||--o{ USER_ROLES : "assigned"
    ROLES ||--o{ USER_ROLES : "grants"
    USERS ||--o{ USER_LOCATIONS : "tracked_in"
    USERS ||--o{ BLOCKED_USERS : "blocker"
    USERS ||--o{ BLOCKED_USERS : "blocked"
    USERS ||--o{ PLACES : "owns"
    USERS ||--o{ CONNECTION_REQUESTS : "sender"
    USERS ||--o{ CONNECTION_REQUESTS : "receiver"
    USERS ||--o{ MESSAGES : "sends"
    USERS ||--o{ PUSH_SUBSCRIPTIONS : "registers"
    USERS ||--o{ REFRESH_TOKENS : "owns"

    PLACES ||--o{ PHOTOS : "has"
    PLACES ||--|| SUBSCRIPTIONS : "billed_via"
    SUBSCRIPTIONS ||--o{ PAYMENTS : "receives"
    PLACES ||--o| MODERATION_RECORDS : "reviewed_via"
    PHOTOS ||--o| MODERATION_RECORDS : "reviewed_via"

    CONNECTION_REQUESTS ||--o| CHATS : "creates_on_accept"
    CHATS ||--o{ MESSAGES : "contains"
    CHATS ||--o{ CHAT_READ_STATE : "tracked_by"
    USERS ||--o{ CHAT_READ_STATE : "reads"

    USERS {
        uuid id PK
        string email UK
        string password_hash
        bool email_confirmed
        date birth_date
        bool is_banned
        timestamptz created_at
    }

    REFRESH_TOKENS {
        uuid id PK
        uuid user_id FK
        string token_hash
        timestamptz expires_at
        bool revoked
    }

    ROLES {
        uuid id PK
        string name UK
    }

    USER_ROLES {
        uuid user_id FK
        uuid role_id FK
    }

    PROFILES {
        uuid id PK
        uuid user_id FK
        string display_name
        string bio
        string[] interests
    }

    PRIVACY_SETTINGS {
        uuid user_id PK_FK
        bool show_on_map
        bool allow_18plus_places
        int radius_override_m
    }

    PHOTOS {
        uuid id PK
        uuid owner_id FK
        string owner_type
        string blob_url
        int sort_order
        string moderation_status
    }

    USER_LOCATIONS {
        uuid id PK
        uuid user_id FK
        geography location
        timestamptz recorded_at
    }

    BLOCKED_USERS {
        uuid blocker_id FK
        uuid blocked_id FK
        timestamptz created_at
    }

    PLACES {
        uuid id PK
        uuid owner_id FK
        string name
        string description
        string category
        bool is_18plus
        geography location
        string moderation_status
        timestamptz subscription_paid_until
    }

    SUBSCRIPTIONS {
        uuid id PK
        uuid place_id FK
        string derived_address UK
        int derivation_index UK
        decimal amount_due
        string status
        timestamptz paid_until
    }

    PAYMENTS {
        uuid id PK
        uuid subscription_id FK
        string tx_hash UK
        decimal amount
        int confirmations
        timestamptz detected_at
    }

    MODERATION_RECORDS {
        uuid id PK
        uuid target_id
        string target_type
        string status
        string provider_response
        timestamptz reviewed_at
    }

    CONNECTION_REQUESTS {
        uuid id PK
        uuid sender_id FK
        uuid receiver_id FK
        string status
        timestamptz created_at
    }

    CHATS {
        uuid id PK
        uuid user_a_id FK
        uuid user_b_id FK
        timestamptz created_at
    }

    MESSAGES {
        uuid id PK
        uuid chat_id FK
        uuid sender_id FK
        string body
        timestamptz sent_at
    }

    CHAT_READ_STATE {
        uuid chat_id FK
        uuid user_id FK
        uuid last_read_message_id
    }

    PUSH_SUBSCRIPTIONS {
        uuid id PK
        uuid user_id FK
        string endpoint
        string p256dh_key
        string auth_key
    }
```

**Key invariants encoded here (test these explicitly, not just via manual QA):**

- `SUBSCRIPTIONS.derived_address` and `derivation_index` are unique — no two places ever share a receive address.
- `PAYMENTS.tx_hash` is unique — a single on-chain transaction can activate exactly one subscription.
- `PLACES` visibility (Sprint 6/10) is a *derived* condition (`moderation_status = Approved AND subscription_paid_until >= now() AND (NOT is_18plus OR viewer.allow_18plus_places)`), not a stored flag — don't let it drift out of sync by caching it somewhere else.

---

## 4. Sequence Diagrams — Key Flows

### 4.1 Nearby User Search

```mermaid
sequenceDiagram
    actor U as User (Browser)
    participant FE as React SPA
    participant API as Geo Module (API)
    participant RD as Redis (GEO)
    participant PG as PostgreSQL

    U->>FE: Grant location permission
    loop every ~15s or on significant move
        FE->>API: POST /geo/location {lat, lng}
        API->>RD: GEOADD users:live
        API->>RD: SET presence:{userId} online (TTL)
    end

    U->>FE: Open/refresh map, set radius
    FE->>API: GET /geo/nearby?radius=500
    API->>RD: GEOSEARCH users:live FROMMEMBER self BYRADIUS 500m
    RD-->>API: candidate userIds + distances
    API->>API: filter out self / blocked / hidden / banned
    API->>PG: batch fetch profile summaries for remaining ids
    PG-->>API: profile summaries
    API-->>FE: [{userId, name, photo, distance}, ...]
    FE-->>U: render markers on MapLibre
```

### 4.2 Crypto Payment for a Place Subscription

```mermaid
sequenceDiagram
    actor O as Place Owner
    participant FE as Owner Dashboard
    participant API as Payments Module
    participant PG as PostgreSQL
    participant W as Blockchain Watcher (Worker)
    participant Chain as Blockchain RPC/Provider

    O->>FE: Click "Subscribe / Renew"
    FE->>API: POST /places/{id}/subscriptions
    API->>API: derive NEXT unused HD wallet index
    API->>PG: INSERT Subscription {address, index, amountDue, status=AwaitingPayment}
    API-->>FE: {address, amountDue, qrPayload}
    FE-->>O: show QR + exact amount

    O->>Chain: send payment to derived address

    loop poll every N seconds
        W->>Chain: get transactions for watched address range
        Chain-->>W: tx list
        W->>W: match tx.to == subscription.address AND tx.amount == amountDue
        alt match found and confirmations >= threshold
            W->>PG: INSERT Payment {txHash UNIQUE, amount, confirmations}
            W->>PG: UPDATE Subscription SET status=Paid, paidUntil=now()+1 month
            W->>PG: UPDATE Place SET subscriptionPaidUntil
        else underpaid / wrong address / not yet confirmed
            W->>W: leave status=AwaitingPayment, log for review
        end
    end

    FE->>API: GET /places/{id}/subscriptions/{id}/status (polling)
    API-->>FE: status = Paid
    FE-->>O: "Your place is live"
```

### 4.3 Connection Request → Chat → Push Notification

```mermaid
sequenceDiagram
    actor A as User A
    actor B as User B
    participant FE_A as A's SPA
    participant FE_B as B's SPA (tab closed)
    participant API as Chat/Notifications Module
    participant Hub as SignalR Hub
    participant PG as PostgreSQL
    participant Push as Web Push Service

    A->>FE_A: Click "Connect" on B's marker
    FE_A->>API: POST /connections/request {receiverId=B}
    API->>PG: INSERT ConnectionRequest {status=Pending}
    API->>Push: send push "New connection request" (no content) to B
    Push-->>FE_B: OS notification (Service Worker), tab closed

    B->>FE_B: Open app from notification
    FE_B->>API: POST /connections/{id}/accept
    API->>PG: UPDATE status=Accepted
    API->>PG: INSERT Chat {userA, userB}
    API-->>FE_B: chatId
    API-->>FE_A: chatId (via SignalR or poll)

    A->>Hub: JoinChat(chatId)
    B->>Hub: JoinChat(chatId)
    A->>Hub: SendMessage(chatId, "hey!")
    Hub->>PG: INSERT Message
    Hub-->>B: message event (if B connected)
    alt B not connected to hub
        API->>Push: send push "New message" (no content) to B
    end
```

### 4.4 AI Monitoring Agent Cycle

```mermaid
sequenceDiagram
    participant Timer as BackgroundService Timer (20 min)
    participant Agent as AI Monitoring Agent
    participant Prom as Prometheus
    participant Seq as Seq / Logs
    participant LLM as LLM API (Claude)
    participant Mail as Email

    Timer->>Agent: tick
    Agent->>Prom: query error rate, p95/p99 latency, memory, CPU, SignalR disconnects (last 20m)
    Prom-->>Agent: metric series
    Agent->>Seq: query recent ERROR/WARN logs (last 20m)
    Seq-->>Agent: log excerpt (deduplicated)

    Agent->>Agent: build structured prompt (metrics + logs + fixed system instructions)
    Agent->>LLM: request analysis, structured summary
    LLM-->>Agent: {status per area, findings, likely causes}

    alt anomaly flagged
        Agent->>Mail: send immediate alert email
    else nothing notable AND daily digest window reached
        Agent->>Mail: send "all clear" daily digest
    else nothing notable, not digest time
        Agent->>Agent: store report, no email
    end

    Agent->>Agent: persist report history (DB or file) for trend lookback
```

---

## 5. Reference Implementation — Layer Walkthrough (Places Nearby Query)

A worked example tying the four layers together end to end, for the "show nearby places" use case. This is a **reference for how to write Sprint 6**, not code that's been added to the actual solution yet — nothing here exists in the repo until that sprint starts.

**Domain** (`Socializer.Domain/Places/Place.cs`) — behavior-protected entity, knows nothing about Postgres or HTTP:

```csharp
public class Place : BaseAuditableEntity
{
    public string Name { get; private set; } = null!;
    public PlaceCategory Category { get; private set; }
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    public bool Is18Plus { get; private set; }
    public ModerationStatus ModerationStatus { get; private set; } = ModerationStatus.Pending;
    public DateTime SubscriptionPaidUntil { get; private set; }

    public void MarkAsPaid(DateTime paidUntil)
    {
        if (ModerationStatus == ModerationStatus.Rejected)
            throw new DomainException("Cannot activate a subscription for a rejected place.");
        SubscriptionPaidUntil = paidUntil;
    }

    public bool IsPubliclyVisible(bool viewerAllows18Plus) =>
        ModerationStatus == ModerationStatus.Approved
        && SubscriptionPaidUntil >= DateTime.UtcNow
        && (!Is18Plus || viewerAllows18Plus);
}
```

**Application** (`Socializer.Application/Places/`) — the port (interface) plus the real orchestration code, including the Domain→Contract mapping. The mapping lives here — not in Infrastructure — because Application is the only layer that references both `Socializer.Domain` and `Socializer.Contracts` at once:

```csharp
// IPlacesRepository.cs — port
public interface IPlacesRepository
{
    Task<List<Place>> GetNearbyAsync(double latitude, double longitude, double radiusMeters, CancellationToken ct);
}

// PlacesQueryService.cs — real class, real code, not an interface
public class PlacesQueryService(IPlacesRepository repository)
{
    public async Task<List<PlaceResponse>> GetNearbyAsync(double lat, double lng, double radiusMeters, CancellationToken ct)
    {
        List<Place> domainPlaces = await repository.GetNearbyAsync(lat, lng, radiusMeters, ct); // → Infrastructure

        return domainPlaces
            .Select(p => new PlaceResponse(p.Id, p.Name, p.Category.ToString(), p.Latitude, p.Longitude))
            .ToList(); // pure in-memory mapping, no external tech involved — stays right here
    }
}
```

**Infrastructure** (`Socializer.Infrastructure/Places/PlacesRepository.cs`) — the adapter; once Sprint 4 lands, this becomes a real PostGIS `ST_DWithin` query via EF Core. It has no reference to `Socializer.Contracts` and never will — it only ever returns Domain objects:

```csharp
public class PlacesRepository(ApplicationDbContext db) : IPlacesRepository
{
    public async Task<List<Place>> GetNearbyAsync(double lat, double lng, double radiusMeters, CancellationToken ct)
    {
        var origin = new Point(lng, lat) { SRID = 4326 };
        return await db.Places
            .AsNoTracking()
            .Where(p => p.Location.IsWithinDistance(origin, radiusMeters))
            .ToListAsync(ct);
    }
}
```

**Contracts** (`Socializer.Contracts/Places/PlaceResponse.cs`) — the dumb DTO that crosses the wire:

```csharp
public record PlaceResponse(Guid Id, string Name, string Category, double Latitude, double Longitude);
```

**Api** (`Socializer.Api/Program.cs`) — thin, just wires the endpoint to the Application service:

```csharp
app.MapGet("/places/nearby", async (double lat, double lng, double radiusMeters, PlacesQueryService svc, CancellationToken ct) =>
    Results.Ok(await svc.GetNearbyAsync(lat, lng, radiusMeters, ct)));
```

**The one rule this example is meant to make concrete:** a port (interface) only exists where Application needs something it *can't* do itself without touching an external system (the DB, in this case). The Domain→Contract mapping needs no external system at all — it's just in-memory object construction — so it's plain code directly inside `PlacesQueryService`, not hidden behind another interface.
