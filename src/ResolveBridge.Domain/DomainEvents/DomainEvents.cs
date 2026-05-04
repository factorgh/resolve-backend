namespace ResolveBridge.Domain.DomainEvents;

public abstract class DomainEvent
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
    public string EventType { get; set; } = string.Empty;
}

public class ApplicationSubmittedEvent : DomainEvent
{
    public Guid ApplicationId { get; set; }
    public Guid UserId { get; set; }
    public string ApplicationNumber { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
}

public class ApplicationStatusChangedEvent : DomainEvent
{
    public Guid ApplicationId { get; set; }
    public string ApplicationNumber { get; set; } = string.Empty;
    public string OldStatus { get; set; } = string.Empty;
    public string NewStatus { get; set; } = string.Empty;
    public string? Notes { get; set; }
}

public class ApplicationApprovedEvent : DomainEvent
{
    public Guid ApplicationId { get; set; }
    public Guid UserId { get; set; }
    public string ApplicationNumber { get; set; } = string.Empty;
    public decimal ApprovedAmount { get; set; }
    public decimal InterestRate { get; set; }
}

public class ApplicationRejectedEvent : DomainEvent
{
    public Guid ApplicationId { get; set; }
    public Guid UserId { get; set; }
    public string ApplicationNumber { get; set; } = string.Empty;
    public string? RejectionReason { get; set; }
}

public class LoanDisbursedEvent : DomainEvent
{
    public Guid ApplicationId { get; set; }
    public Guid LoanLifecycleId { get; set; }
    public Guid UserId { get; set; }
    public decimal DisbursedAmount { get; set; }
}

public class PaymentReceivedEvent : DomainEvent
{
    public Guid LoanLifecycleId { get; set; }
    public Guid PaymentId { get; set; }
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    public decimal OutstandingBalance { get; set; }
}

public class PaymentOverdueEvent : DomainEvent
{
    public Guid LoanLifecycleId { get; set; }
    public Guid UserId { get; set; }
    public decimal AmountDue { get; set; }
    public int DaysOverdue { get; set; }
}

public class UserRegisteredEvent : DomainEvent
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}

public class KycVerifiedEvent : DomainEvent
{
    public Guid UserId { get; set; }
    public string NationalId { get; set; } = string.Empty;
}
