using ResolveBridge.Application.Common;

namespace ResolveBridge.Application.Dtos;

public class FinancialProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ProductType { get; set; } = string.Empty;
    public Guid InstitutionId { get; set; }
    public string InstitutionName { get; set; } = string.Empty;
    public string InstitutionLogoUrl { get; set; } = string.Empty;
    public decimal MinAmount { get; set; }
    public decimal MaxAmount { get; set; }
    public decimal InterestRate { get; set; }
    public int MinTenureMonths { get; set; }
    public int MaxTenureMonths { get; set; }
    public string Requirements { get; set; } = string.Empty;
    public string Benefits { get; set; } = string.Empty;
    public string TermsAndConditions { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public bool IsFeatured { get; set; }
    public int DisplayOrder { get; set; }
    public List<string> AvailableMarkets { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public int ApplicationCount { get; set; }
    public object? ProductDetails { get; set; }
    public int TrustScore { get; set; }
    public int MatchPercentage { get; set; }
}

public class ProductSummaryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ProductType { get; set; } = string.Empty;
    public string InstitutionName { get; set; } = string.Empty;
    public decimal MinAmount { get; set; }
    public decimal MaxAmount { get; set; }
    public decimal InterestRate { get; set; }
    public int MinTenureMonths { get; set; }
    public int MaxTenureMonths { get; set; }
    public bool IsFeatured { get; set; }
}

public class CreateFinancialProductRequestDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ProductType { get; set; } = string.Empty;
    public decimal MinAmount { get; set; }
    public decimal MaxAmount { get; set; }
    public decimal InterestRate { get; set; }
    public int MinTenureMonths { get; set; }
    public int MaxTenureMonths { get; set; }
    public string Requirements { get; set; } = string.Empty;
    public string Benefits { get; set; } = string.Empty;
    public string TermsAndConditions { get; set; } = string.Empty;
    public List<string> AvailableMarkets { get; set; } = new();
    public bool IsFeatured { get; set; }
    public int DisplayOrder { get; set; }
    public LoanProductDetailsRequestDto? LoanDetails { get; set; }
    public BNPLProductDetailsRequestDto? BNPLDetails { get; set; }
    public InsuranceProductDetailsRequestDto? InsuranceDetails { get; set; }
}

public class UpdateFinancialProductRequestDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal? MinAmount { get; set; }
    public decimal? MaxAmount { get; set; }
    public decimal? InterestRate { get; set; }
    public int? MinTenureMonths { get; set; }
    public int? MaxTenureMonths { get; set; }
    public string? Requirements { get; set; }
    public string? Benefits { get; set; }
    public string? TermsAndConditions { get; set; }
    public List<string>? AvailableMarkets { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsFeatured { get; set; }
    public int? DisplayOrder { get; set; }
}

public class LoanProductDetailsRequestDto
{
    public string LoanType { get; set; } = string.Empty;
    public string? Purpose { get; set; }
    public bool RequiresCollateral { get; set; }
    public string? CollateralTypes { get; set; }
    public decimal? ProcessingFee { get; set; }
    public decimal? EarlyRepaymentFee { get; set; }
    public decimal? LatePaymentFee { get; set; }
}

public class BNPLProductDetailsRequestDto
{
    public string Category { get; set; } = string.Empty;
    public List<string> SupportedMerchants { get; set; } = new();
    public int InstallmentPeriods { get; set; } = 3;
    public decimal DownPaymentPercent { get; set; } = 0;
}

public class LoanProductDto
{
    public Guid Id { get; set; }
    public Guid InstitutionId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ProductDescription { get; set; } = string.Empty;
    public string LoanType { get; set; } = string.Empty;
    public decimal MinAmount { get; set; }
    public decimal MaxAmount { get; set; }
    public decimal MinInterestRate { get; set; }
    public decimal MaxInterestRate { get; set; }
    public int MinTenureMonths { get; set; }
    public int MaxTenureMonths { get; set; }
    public string InstitutionName { get; set; } = string.Empty;
    public string? InstitutionLogo { get; set; }
    public List<string> Requirements { get; set; } = new();
    public List<string> DocumentRequirements { get; set; } = new();
    public bool IsAvailable { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class BNPLProductDto
{
    public Guid Id { get; set; }
    public Guid MerchantId { get; set; }
    public string ProductId { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public string? ProductDescription { get; set; }
    public string? ProductImageUrl { get; set; }
    public string Category { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Currency { get; set; } = "GHS";
    public List<InstallmentPlanDto> InstallmentPlans { get; set; } = new();
    public string? MerchantName { get; set; }
    public string? MerchantLogo { get; set; }
    public bool IsAvailable { get; set; }
    public int? StockQuantity { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class InstallmentPlanDto
{
    public Guid Id { get; set; }
    public int NumberOfInstallments { get; set; }
    public decimal InstallmentAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal InterestRate { get; set; }
    public decimal? ProcessingFee { get; set; }
    public DateTime? FirstPaymentDate { get; set; }
    public string Frequency { get; set; } = "Monthly";
    public bool? IsDefault { get; set; }
}

public class InsuranceProductDetailsRequestDto
{
    public string InsuranceType { get; set; } = string.Empty;
    public decimal MinCoverageAmount { get; set; }
    public decimal MaxCoverageAmount { get; set; }
    public List<PremiumRateDto> PremiumRates { get; set; } = new();
    public List<CoverageDetailDto> CoverageDetails { get; set; } = new();
    public List<ExclusionDetailDto> ExclusionDetails { get; set; } = new();
    public List<string> Requirements { get; set; } = new();
    public int? WaitingPeriod { get; set; }
}

public class InsuranceProductDto
{
    public Guid Id { get; set; }
    public Guid ProviderId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string? ProductDescription { get; set; }
    public string ProductType { get; set; } = string.Empty;
    public string? ProviderName { get; set; }
    public string? ProviderLogo { get; set; }
    public decimal MinCoverageAmount { get; set; }
    public decimal MaxCoverageAmount { get; set; }
    public List<PremiumRateDto> PremiumRates { get; set; } = new();
    public List<CoverageDetailDto> CoverageDetails { get; set; } = new();
    public List<ExclusionDetailDto> ExclusionDetails { get; set; } = new();
    public bool IsAvailable { get; set; }
    public List<string> Requirements { get; set; } = new();
    public int? WaitingPeriod { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class PremiumRateDto
{
    public CoverageRangeDto CoverageRange { get; set; } = new();
    public decimal MonthlyPremium { get; set; }
    public decimal AnnualPremium { get; set; }
    public Dictionary<string, object>? Factors { get; set; }
}

public class CoverageRangeDto
{
    public decimal MinAmount { get; set; }
    public decimal MaxAmount { get; set; }
}

public class CoverageDetailDto
{
    public Guid Id { get; set; }
    public string CoverageName { get; set; } = string.Empty;
    public string? CoverageDescription { get; set; }
    public decimal? CoverageAmount { get; set; }
    public bool IsIncluded { get; set; }
}

public class ExclusionDetailDto
{
    public Guid Id { get; set; }
    public string ExclusionName { get; set; } = string.Empty;
    public string? ExclusionDescription { get; set; }
}

public class ProductFilterRequestDto : PagedRequest
{
    public string? ProductType { get; set; }
    public string? InstitutionId { get; set; }
    public decimal? MinAmount { get; set; }
    public decimal? MaxAmount { get; set; }
    public int? MinTenureMonths { get; set; }
    public int? MaxTenureMonths { get; set; }
    public string? Market { get; set; }
    public bool? IsFeatured { get; set; }
    public string? LoanType { get; set; }
    public string? BNPLCategory { get; set; }
    public string? InsuranceType { get; set; }
}
