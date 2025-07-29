using System.ComponentModel.DataAnnotations;

namespace ESGSimpleTracker.Models;

public class McpParameter
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Parameter name is required")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Parameter value is required")]
    public string Value { get; set; } = string.Empty;
    
    public bool IsRequired { get; set; }
    
    // Foreign key
    public int McpModelId { get; set; }
    
    // Navigation property
    public McpModel? McpModel { get; set; }
}