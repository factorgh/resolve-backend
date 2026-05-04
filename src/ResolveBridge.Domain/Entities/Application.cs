using ResolveBridge.Domain.Common;
using ResolveBridge.Domain.Enums;

namespace ResolveBridge.Domain.Entities;

public class Application : BaseEntity
{
    public string ApplicationNumber { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public Guid ProductId { get; set; }
    public FinancialProduct Product { get; set; } = null!;
    public Guid InstitutionId { get; set; }
    public Institution Institution { get; set; } = null!;
    
    public ApplicationStatus Status { get; set; } = ApplicationStatus.Draft;
    public decimal RequestedAmount { get; set; }
    public int RequestedTenureMonths { get; set; }
    public string Purpose { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public string? Metadata { get; set; }
    
    public decimal? ApprovedAmount { get; set; }
    public int? ApprovedTenureMonths { get; set; }
    public decimal? InterestRate { get; set; }
    public decimal? MonthlyPayment { get; set; }
    public decimal? TotalRepayment { get; set; }
    public DateTime? DecisionDate { get; set; }
    public string? DecisionNotes { get; set; }
    public string? ReviewedById { get; set; }
    public string? ReviewedByName { get; set; }
    
    public DateTime? SubmittedAt { get; set; }
    public DateTime? DisbursedAt { get; set; }
    public decimal? DisbursedAmount { get; set; }
    
    public ICollection<ApplicationDocument> Documents { get; set; } = new List<ApplicationDocument>();
    public ICollection<ApplicationStatusHistory> StatusHistory { get; set; } = new List<ApplicationStatusHistory>();
    public LoanLifecycle? LoanLifecycle { get; set; }
}

public class ApplicationDocument : BaseEntity
{
    public Guid ApplicationId { get; set; }
    public Application Application { get; set; } = null!;
    public DocumentType Type { get; set; }
    public string DocumentUrl { get; set; } = string.Empty;
    public string? OriginalFileName { get; set; }
    public long? FileSize { get; set; }
    public string? MimeType { get; set; }
    public bool IsVerified { get; set; } = false;
}

public class ApplicationStatusHistory : BaseEntity
{
    public Guid ApplicationId { get; set; }
    public Application Application { get; set; } = null!;
    public ApplicationStatus OldStatus { get; set; }
    public ApplicationStatus NewStatus { get; set; }
    public string? Notes { get; set; }
    public string ChangedById { get; set; } = string.Empty;
    public string ChangedByName { get; set; } = string.Empty;
}
