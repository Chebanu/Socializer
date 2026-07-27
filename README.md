# Socializer

See [docs/Socializer_Roadmap.md](docs/Socializer_Roadmap.md) for the full project roadmap and backlog.

## Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/) matching the version pinned in `global.json`
- [Docker](https://www.docker.com/) + Docker Compose
- [Node.js](https://nodejs.org/) 20+ (for the frontend)

## Running the backend infrastructure

```bash
cp .env.example .env   # adjust local values if needed
docker compose up
```

This boots Postgres, Redis, Azurite, Seq, the API, Prometheus, and Grafana. The API is available at `http://localhost:8080`, Swagger UI at `http://localhost:8080/swagger`, and Seq at `http://localhost:8081`.

To run only the infra dependencies and the API from your IDE instead:

```bash
docker compose up postgres redis azurite seq
```

## Local secrets

Connection strings and other local secrets are never committed. Configure them per-project with `dotnet user-secrets` from the `backend/Socializer.Api` directory:

```bash
cd backend/Socializer.Api
dotnet user-secrets set "ConnectionStrings:Postgres" "Host=localhost;Port=5432;Database=socializer;Username=socializer;Password=socializer"
```

The values above match the defaults in `.env.example`. `appsettings.json` only contains non-secret defaults (hosts, ports, feature flags); anything sensitive belongs in user-secrets locally or in the deployment's secret store in other environments.

## Database migrations

EF Core and the first migrations land in Sprint 1 (Authentication). Once available, migrations are applied with:

```bash
cd backend
dotnet ef database update --project Socializer.Infrastructure --startup-project Socializer.Api
```

## Running the API directly

```bash
cd backend/Socializer.Api
dotnet run
```

## Running the frontend

```bash
cd frontend
npm install
npm run dev
```

The dev server runs at `http://localhost:5173` and proxies `/health` to the API (`http://localhost:8080` by default — override with `VITE_API_PROXY_TARGET`) so you can confirm frontend-to-backend connectivity without dealing with CORS.

## Running tests

```bash
cd backend
dotnet test
```

## CI

Every pull request runs `dotnet build` and `dotnet test` via [GitHub Actions](.github/workflows/ci.yml).
