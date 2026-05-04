namespace ResolveBridge.Application.Dtos;

public class UserDocumentDto
{
    public Guid Id { get; set; }
    public string DocumentType { get; set; } = string.Empty;
    public string DocumentUrl { get; set; } = string.Empty;
    public bool IsVerified { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class KycSubmissionRequestDto
{
    public string AccountType { get; set; } = string.Empty;
    public string GhCard { get; set; } = string.Empty;
    public Microsoft.AspNetCore.Http.IFormFile? GhanaCardFront { get; set; }
    public Microsoft.AspNetCore.Http.IFormFile? GhanaCardBack { get; set; }
    public Microsoft.AspNetCore.Http.IFormFile? Certificate { get; set; }
    public Microsoft.AspNetCore.Http.IFormFile? FormA3 { get; set; }
}
public class DashboardMetricsDto
{
    public int HealthIndex { get; set; }
    public decimal CashFlow { get; set; }
    public decimal NetWorth { get; set; }
    public int CreditScore { get; set; }
    public int EligibleOffers { get; set; }
    public string HealthIndexMessage { get; set; } = string.Empty;
    public List<ChartDataPointDto> VelocityData { get; set; } = new();
    public List<HealthFactorDto> HealthFactors { get; set; } = new();
}

public class HealthFactorDto
{
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty; // e.g., Exceptional, Good, Excellent
    public string Color { get; set; } = string.Empty; // Hex color code
}

public class ChartDataPointDto
{
    public string Label { get; set; } = string.Empty;
    public decimal Value { get; set; }
}
