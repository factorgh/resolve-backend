using ResolveBridge.Domain.Common;
using ResolveBridge.Domain.Enums;

namespace ResolveBridge.Domain.Entities;

public class User : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? NationalId { get; set; }
    public string? IdType { get; set; }
    public Market Market { get; set; } = Market.Ghana;
    public UserRole Role { get; set; } = UserRole.Customer;
    public KycStatus KycStatus { get; set; } = KycStatus.Pending;
    public EmploymentStatus? EmploymentStatus { get; set; }
    public string? Occupation { get; set; }
    public string? MonthlyIncome { get; set; }
    public string? LoanDurationPreference { get; set; }
    public List<string> Goals { get; set; } = new List<string>();

    // Institutional & Residential Details
    public string? Title { get; set; }
    public string? MaritalStatus { get; set; }
    public string? Gender { get; set; }
    public string? Nationality { get; set; }
    public string? Dependants { get; set; }
    public string? ResidentialAddress { get; set; }
    public string? City { get; set; }
    public string? Mmda { get; set; }
    public string? Landmark { get; set; }
    public string? Employer { get; set; }
    public string? Sector { get; set; }
    public string? SsnitNo { get; set; }
    public string? WorkAddress { get; set; }
    public string? YearsWithEmployer { get; set; }
    
    public bool IsActive { get; set; } = true;
    public bool EmailVerified { get; set; } = false;
    public bool PhoneVerified { get; set; } = false;
    public DateTime? LastLoginAt { get; set; }
    
    public Guid? InstitutionId { get; set; }
    public Institution? Institution { get; set; }
    
    public ICollection<UserDocument> Documents { get; set; } = new List<UserDocument>();
    public ICollection<Application> Applications { get; set; } = new List<Application>();
    public ICollection<LoanLifecycle> LoanLifecycles { get; set; } = new List<LoanLifecycle>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    
    public string FullName => $"{FirstName} {LastName}";
}

public class UserDocument : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public DocumentType Type { get; set; }
    public string DocumentUrl { get; set; } = string.Empty;
    public string? DocumentNumber { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public bool IsVerified { get; set; } = false;
    public string? VerificationNotes { get; set; }
}
