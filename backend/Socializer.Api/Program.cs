using Serilog;
using Socializer.Application;
using Socializer.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.Seq(context.Configuration["Seq:ServerUrl"] ?? "http://localhost:5341"));

// Composition root: each layer registers itself via its own AddXxx() extension method.
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseSerilogRequestLogging();

// Basic liveness/readiness probes. No dependency checks yet — see Sprint 11 (Observability).
app.MapHealthChecks("/health");
app.MapHealthChecks("/health/ready");

app.Run();

// Exposes Program to Socializer.IntegrationTests via WebApplicationFactory<Program>.
public partial class Program;
