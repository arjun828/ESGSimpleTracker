using ESGSimpleTracker.Models;

namespace ESGSimpleTracker.Services;

public interface IMcpService
{
    Task<McpModel> GetModelAsync(int id);
    Task<McpModel> CreateModelAsync(McpModel model);
    Task ValidateModelCompliance(McpModel model);
    Task DeleteModelAsync(int id);
}