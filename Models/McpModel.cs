using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESGSimpleTracker.Models;

public class McpModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Model name is required")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string Name { get; set; } = string.Empty;
    
    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string Description { get; set; } = string.Empty;
    
    [RegularExpression(@"^\d+\.\d+$", ErrorMessage = "Invalid protocol version format")]
    public string ProtocolVersion { get; set; } = "1.0";
    
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    
    [Required]
    public string Status { get; set; } = "Draft";
    
    // Foreign key
    [Required(ErrorMessage = "Company ID is required")]
    public int CompanyId { get; set; }
    
    // Navigation properties
    [ForeignKey("CompanyId")]
    public Company? Company { get; set; }
    
    public List<McpParameter> Parameters { get; set; } = new();
}