using ResolveBridge.Domain.Common;
using ResolveBridge.Domain.Enums;

namespace ResolveBridge.Domain.Entities;

public class LoanLifecycle : BaseEntity
{
    public Guid ApplicationId { get; set; }
    public Application Application { get; set; } = null!;
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public Guid InstitutionId { get; set; }
    public Institution Institution { get; set; } = null!;
    
    public decimal PrincipalAmount { get; set; }
    public decimal InterestRate { get; set; }
    public int TenureMonths { get; set; }
    public decimal TotalRepaymentAmount { get; set; }
    public decimal OutstandingBalance { get; set; }
    public decimal AmountPaid { get; set; }
    
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime? NextPaymentDate { get; set; }
    public decimal? NextPaymentAmount { get; set; }
    
    public int InstallmentsPaid { get; set; } = 0;
    public int TotalInstallments { get; set; }
    public int LatePayments { get; set; } = 0;
    public decimal? LateFeesAccrued { get; set; }
    
    public bool IsActive { get; set; } = true;
    public DateTime? ClosedAt { get; set; }
    public string? ClosureReason { get; set; }
    
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}

public class Payment : BaseEntity
{
    public Guid LoanLifecycleId { get; set; }
    public LoanLifecycle LoanLifecycle { get; set; } = null!;
    public string PaymentReference { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal PrincipalPaid { get; set; }
    public decimal InterestPaid { get; set; }
    public decimal? LateFeePaid { get; set; }
    public DateTime PaymentDate { get; set; }
    public DateTime DueDate { get; set; }
    public bool IsOnTime { get; set; }
    public string? PaymentMethod { get; set; }
    public string? TransactionId { get; set; }
    public string? Notes { get; set; }
}
