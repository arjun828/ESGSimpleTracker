using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ESGSimpleTracker.Models;

public class Company
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Company name is required")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public required string Name { get; set; }

    [StringLength(50, ErrorMessage = "Industry cannot exceed 50 characters")]
    public string? Industry { get; set; }

    [Range(0, 100, ErrorMessage = "ESG Score must be between 0 and 100")]
    public double ESGScore { get; set; }

    [RegularExpression("Low|Medium|High", ErrorMessage = "Risk level must be Low, Medium or High")]
    public string RiskLevel { get; set; } = "Medium";

    // Navigation property
    public List<McpModel> McpModels { get; set; } = new();

    // Calculated property
    public bool IsCompliant => McpModels.Count > 0 && McpModels.All(m => m.Status == "Compliant");
}