using ESGSimpleTracker.Data;
using ESGSimpleTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace ESGSimpleTracker.Services;

public class McpService : IMcpService
{
    private readonly AppDbContext _context;
    private readonly ILogger<McpService> _logger;

    public McpService(AppDbContext context, ILogger<McpService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<McpModel> GetModelAsync(int id)
    {
        try
        {
            var model = await _context.McpModels
                .Include(m => m.Parameters)
                .Include(m => m.Company)
                .FirstOrDefaultAsync(m => m.Id == id);

            return model ?? throw new KeyNotFoundException($"Model with ID {id} not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving MCP model with ID {Id}", id);
            throw;
        }
    }

    public async Task<McpModel> CreateModelAsync(McpModel model)
    {
        try
        {
            model.LastUpdated = DateTime.UtcNow;
            _context.McpModels.Add(model);
            await _context.SaveChangesAsync();
            return model;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating new MCP model");
            throw;
        }
    }

    public async Task ValidateModelCompliance(McpModel model)
    {
        try
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            
            model.LastUpdated = DateTime.UtcNow;
            model.Status = "Validated";
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating MCP model with ID {Id}", model?.Id);
            throw;
        }
    }

    public async Task DeleteModelAsync(int id)
    {
        try
        {
            var model = await GetModelAsync(id);
            _context.McpModels.Remove(model);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting MCP model with ID {Id}", id);
            throw;
        }
    }
}