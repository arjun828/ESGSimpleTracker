using ESGSimpleTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace ESGSimpleTracker.Data;

public static class SeedData
{
    public static async Task Initialize(AppDbContext context)
    {
        await SeedCompanies(context);
        await SeedMcpModels(context);
        await SeedParameters(context);
    }

    private static async Task SeedCompanies(AppDbContext context)
    {
        if (await context.Companies.AnyAsync()) return;

        var companies = new List<Company>
        {
            new() 
            { 
                Id = 1, 
                Name = "EcoTech Inc", 
                Industry = "Technology", 
                ESGScore = 85, 
                RiskLevel = "Low"
            },
            new() 
            { 
                Id = 2, 
                Name = "GreenEnergy Corp", 
                Industry = "Energy", 
                ESGScore = 72, 
                RiskLevel = "Medium" 
            },
            new()
            {
                Id = 3,
                Name = "Sustainable Foods Ltd",
                Industry = "Agriculture",
                ESGScore = 90,
                RiskLevel = "Low"
            }
        };

        await context.Companies.AddRangeAsync(companies);
        await context.SaveChangesAsync();
    }

    private static async Task SeedMcpModels(AppDbContext context)
    {
        if (await context.McpModels.AnyAsync()) return;

        var models = new List<McpModel>
        {
            new()
            {
                Id = 1,
                Name = "Carbon Footprint 2023",
                Description = "Annual carbon emissions report",
                ProtocolVersion = "1.2",
                CompanyId = 1,
                Status = "Compliant"
            },
            new()
            {
                Id = 2,
                Name = "Energy Consumption Q1",
                Description = "Quarterly energy usage metrics",
                ProtocolVersion = "1.1",
                CompanyId = 2,
                Status = "Pending"
            },
            new()
            {
                Id = 3,
                Name = "Supply Chain Analysis",
                Description = "Vendor sustainability assessment",
                ProtocolVersion = "2.0",
                CompanyId = 3,
                Status = "Compliant"
            }
        };

        await context.McpModels.AddRangeAsync(models);
        await context.SaveChangesAsync();
    }

    private static async Task SeedParameters(AppDbContext context)
    {
        if (await context.McpParameters.AnyAsync()) return;

        var parameters = new List<McpParameter>
        {
            // Parameters for Carbon Footprint model
            new() { Id = 1, Name = "Scope 1 Emissions", Value = "45.2", IsRequired = true, McpModelId = 1 },
            new() { Id = 2, Name = "Scope 2 Emissions", Value = "120.5", IsRequired = true, McpModelId = 1 },
            new() { Id = 3, Name = "Renewable Energy %", Value = "35", IsRequired = false, McpModelId = 1 },
            
            // Parameters for Energy Consumption model
            new() { Id = 4, Name = "Electricity Usage (kWh)", Value = "12500", IsRequired = true, McpModelId = 2 },
            new() { Id = 5, Name = "Fuel Consumption", Value = "3200", IsRequired = true, McpModelId = 2 },
            
            // Parameters for Supply Chain model
            new() { Id = 6, Name = "Local Suppliers %", Value = "68", IsRequired = true, McpModelId = 3 },
            new() { Id = 7, Name = "Ethical Sourcing Score", Value = "82", IsRequired = true, McpModelId = 3 }
        };

        await context.McpParameters.AddRangeAsync(parameters);
        await context.SaveChangesAsync();
    }
}