namespace ESGSimpleTracker.Models;

public class Company
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Industry { get; set; }
    public double ESGScore { get; set; }
    public string RiskLevel { get; set; } = "Medium";
}