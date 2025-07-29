using ESGSimpleTracker.Data;
using ESGSimpleTracker.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "ESG Tracker API", 
        Version = "v1",
        Description = "API for tracking ESG compliance metrics"
    });
});

// Database Configuration
var usePostgreSQL = builder.Configuration.GetValue<bool>("UsePostgreSQL");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    if (usePostgreSQL)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        options.UseNpgsql(connectionString,
            x => x.MigrationsAssembly("ESGSimpleTracker"));
    }
    else
    {
        var sqliteConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")
            ?? "Data Source=esgtracker.db";
        options.UseSqlite(sqliteConnectionString,
            x => 
            {
                x.MigrationsAssembly("ESGSimpleTracker");
                x.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            });
    }
});

// Register Services
builder.Services.AddScoped<IMcpService, McpService>();
builder.Services.AddLogging();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => 
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ESG Tracker v1");
        c.RoutePrefix = string.Empty; // Serve Swagger at root
    });
}

app.UseHttpsRedirection();
app.UseRouting(); // Critical for proper routing
app.UseAuthorization();
app.MapControllers();

// Debug endpoint
app.MapGet("/debug-redirect", () => Results.Redirect("/swagger"));

// Automatic Database Migration and Seeding
try
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    var logger = services.GetRequiredService<ILogger<Program>>();

    await context.Database.MigrateAsync();

    if (!context.Companies.Any())
    {
        await SeedData.Initialize(context);
        logger.LogInformation("Database seeded with initial data");
    }
}
catch (Exception ex)
{
    var logger = app.Logger;
    logger.LogError(ex, "An error occurred while migrating/seeding the database");
}

await app.RunAsync();