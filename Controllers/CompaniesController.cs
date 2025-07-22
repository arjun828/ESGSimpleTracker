using ESGSimpleTracker.Data;
using ESGSimpleTracker.Models;
using ESGSimpleTracker.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ESGSimpleTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompaniesController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly SimpleAnalysisService _analysisService;

    public CompaniesController(AppDbContext context, SimpleAnalysisService analysisService)
    {
        _context = context;
        _analysisService = analysisService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Company>>> GetCompanies()
    {
        return await _context.Companies.ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Company>> AddCompany(Company company)
    {
        _context.Companies.Add(company);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetCompanies), company);
    }

    [HttpGet("{id}/analyze")]
    public ActionResult<string> AnalyzeCompany(int id)
    {
        var company = _context.Companies.Find(id);
        if (company == null) return NotFound();
        return _analysisService.GetRiskAdvice(company);
    }
}