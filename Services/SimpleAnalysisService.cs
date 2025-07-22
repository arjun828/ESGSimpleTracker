using ESGSimpleTracker.Models;

namespace ESGSimpleTracker.Services;

public class SimpleAnalysisService
{
    public string GetRiskAdvice(Company company)
    {
        return company.ESGScore switch
        {
            >= 80 => $"✅ Low Risk - Excellent ESG ({company.ESGScore})",
            >= 60 => $"⚠️ Moderate Risk - Good ESG ({company.ESGScore})",
            _ => $"🚨 High Risk - Needs Review ({company.ESGScore})"
        };
    }
}