using System.Text.Json.Serialization;
using KitchenSync.Api.Hubs;
using KitchenSync.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// ====== Configs base ======
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Enums como string (ex.: "Completed")
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// SignalR com enums como string
builder.Services.AddSignalR()
    .AddJsonProtocol(opts =>
    {
        opts.PayloadSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// ====== CORS ======
var allowedOrigins = new[]
{
    "http://localhost:3000",
    "https://localhost:3000",
    "https://sync-front.vercel.app"   // <<< domínio do front na Vercel
};

builder.Services.AddCors(options =>
{
    options.AddPolicy("frontend", policy =>
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()); // necessário para SignalR/cookies
});

builder.Services.AddSingleton<InMemoryStore>();

// ====== Swagger ======
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// CORS deve vir antes dos mapeamentos
app.UseCors("frontend");

// Swagger em Dev OU quando ENABLE_SWAGGER=true no ambiente
if (app.Environment.IsDevelopment() ||
    string.Equals(Environment.GetEnvironmentVariable("ENABLE_SWAGGER"), "true", StringComparison.OrdinalIgnoreCase))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ====== Endpoints ======
app.MapControllers();
app.MapHub<KitchenHub>("/hubs/kitchen");

// Health check simples
app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.Run();
