using RecAI.Application;
using RecAI.Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();        // produces /openapi/v1.json
builder.Services.AddHealthChecks();   // built-in health-check service

// Each layer registers its own dependencies — the composition root just calls them.
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();              // serves the OpenAPI JSON
    app.MapScalarApiReference();  // interactive docs at /scalar/v1
}

app.UseHttpsRedirection();
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();