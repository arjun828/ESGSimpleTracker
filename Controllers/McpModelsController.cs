using ESGSimpleTracker.Models;
using ESGSimpleTracker.Services;
using Microsoft.AspNetCore.Mvc;

namespace ESGSimpleTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class McpModelsController : ControllerBase
{
    private readonly IMcpService _service;
    private readonly ILogger<McpModelsController> _logger;

    public McpModelsController(IMcpService service, ILogger<McpModelsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<McpModel>> GetById(int id)
    {
        try
        {
            var model = await _service.GetModelAsync(id);
            return Ok(model);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "MCP Model not found with ID {Id}", id);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving MCP model with ID {Id}", id);
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<McpModel>> Create([FromBody] McpModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdModel = await _service.CreateModelAsync(model);
            return CreatedAtAction(nameof(GetById), new { id = createdModel.Id }, createdModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating new MCP model");
            return StatusCode(500, "An error occurred while creating the model");
        }
    }

    [HttpPut("{id}/validate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Validate(int id)
    {
        try
        {
            var model = await _service.GetModelAsync(id);
            await _service.ValidateModelCompliance(model);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "MCP Model not found with ID {Id}", id);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating MCP model with ID {Id}", id);
            return StatusCode(500, "An error occurred while validating the model");
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _service.DeleteModelAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "MCP Model not found with ID {Id}", id);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting MCP model with ID {Id}", id);
            return StatusCode(500, "An error occurred while deleting the model");
        }
    }
}