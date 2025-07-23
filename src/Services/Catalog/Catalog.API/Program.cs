using Catalog.API.Middleware;
using Catalog.Application;
using Catalog.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/catalog-api-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Catalog API", Version = "v1" });
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add application layers
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Add health checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<Catalog.Infrastructure.Data.CatalogContext>();

var app = builder.Build();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseCors("CorsPolicy");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

// Apply migrations
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<Catalog.Infrastructure.Data.CatalogContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    try
    {
        logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(Catalog.Infrastructure.Data.CatalogContext).Name);

        if (app.Environment.IsDevelopment())
        {
            await context.Database.EnsureCreatedAsync();
            // Or use migrations:
            // await context.Database.MigrateAsync();
        }
        else
        {
            await context.Database.MigrateAsync();
        }

        logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(Catalog.Infrastructure.Data.CatalogContext).Name);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while migrating the database used on context {DbContextName}", typeof(Catalog.Infrastructure.Data.CatalogContext).Name);
    }
}

try
{
    Log.Information("Starting Catalog API");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Catalog API terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}