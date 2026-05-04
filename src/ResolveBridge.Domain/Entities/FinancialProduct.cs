using ResolveBridge.Domain.Common;
using ResolveBridge.Domain.Enums;

namespace ResolveBridge.Domain.Entities;

public class FinancialProduct : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ProductType ProductType { get; set; }
    public Guid InstitutionId { get; set; }
    public Institution Institution { get; set; } = null!;
    
    public decimal MinAmount { get; set; }
    public decimal MaxAmount { get; set; }
    public decimal InterestRate { get; set; }
    public int MinTenureMonths { get; set; }
    public int MaxTenureMonths { get; set; }
    
    public string Requirements { get; set; } = string.Empty;
    public string Benefits { get; set; } = string.Empty;
    public string TermsAndConditions { get; set; } = string.Empty;
    
    public bool IsActive { get; set; } = true;
    public bool IsFeatured { get; set; } = false;
    public int DisplayOrder { get; set; } = 0;
    
    public ICollection<Market> AvailableMarkets { get; set; } = new List<Market>();
    public ICollection<Application> Applications { get; set; } = new List<Application>();
    
    public LoanProductDetails? LoanDetails { get; set; }
    public BNPLProductDetails? BNPLDetails { get; set; }
    public InsuranceProductDetails? InsuranceDetails { get; set; }
}

public class LoanProductDetails : BaseEntity
{
    public Guid ProductId { get; set; }
    public FinancialProduct Product { get; set; } = null!;
    public LoanType LoanType { get; set; }
    public string? Purpose { get; set; }
    public bool RequiresCollateral { get; set; } = false;
    public string? CollateralTypes { get; set; }
    public decimal? ProcessingFee { get; set; }
    public decimal? EarlyRepaymentFee { get; set; }
    public decimal? LatePaymentFee { get; set; }
}

public class BNPLProductDetails : BaseEntity
{
    public Guid ProductId { get; set; }
    public FinancialProduct Product { get; set; } = null!;
    public BNPLCategory Category { get; set; }
    public List<string> SupportedMerchants { get; set; } = new();
    public int InstallmentPeriods { get; set; } = 3;
    public decimal DownPaymentPercent { get; set; } = 0;
}

public class InsuranceProductDetails : BaseEntity
{
    public Guid ProductId { get; set; }
    public FinancialProduct Product { get; set; } = null!;
    public InsuranceType InsuranceType { get; set; }
    public decimal MinCoverageAmount { get; set; }
    public decimal MaxCoverageAmount { get; set; }
    public List<PremiumRate> PremiumRates { get; set; } = new();
    public List<CoverageDetail> CoverageDetails { get; set; } = new();
    public List<ExclusionDetail> ExclusionDetails { get; set; } = new();
    public List<string> Requirements { get; set; } = new();
    public int? WaitingPeriod { get; set; }
    public string? RenewalTerms { get; set; }
}

public class PremiumRate : BaseEntity
{
    public Guid InsuranceDetailsId { get; set; }
    public decimal MinAmount { get; set; }
    public decimal MaxAmount { get; set; }
    public decimal MonthlyPremium { get; set; }
    public decimal AnnualPremium { get; set; }
}

public class CoverageDetail : BaseEntity
{
    public Guid InsuranceDetailsId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal? Amount { get; set; }
    public bool IsIncluded { get; set; }
}

public class ExclusionDetail : BaseEntity
{
    public Guid InsuranceDetailsId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}
