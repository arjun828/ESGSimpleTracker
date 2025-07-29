using Microsoft.EntityFrameworkCore;
using ESGSimpleTracker.Models;

namespace ESGSimpleTracker.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
    
    public DbSet<Company> Companies { get; set; } = null!;
    public DbSet<McpModel> McpModels { get; set; } = null!;
    public DbSet<McpParameter> McpParameters { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure Company-McpModel relationship
        modelBuilder.Entity<Company>()
            .HasMany(c => c.McpModels)
            .WithOne(m => m.Company)
            .HasForeignKey(m => m.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure McpModel-Parameters relationship
        modelBuilder.Entity<McpModel>()
            .HasMany(m => m.Parameters)
            .WithOne(p => p.McpModel)
            .HasForeignKey(p => p.McpModelId)
            .OnDelete(DeleteBehavior.Cascade);

        // Seed initial data
        modelBuilder.Entity<Company>().HasData(
            new Company { Id = 1, Name = "EcoTech Inc", Industry = "Technology", ESGScore = 85, RiskLevel = "Low" },
            new Company { Id = 2, Name = "GreenEnergy Corp", Industry = "Energy", ESGScore = 72, RiskLevel = "Medium" }
        );
    }
}