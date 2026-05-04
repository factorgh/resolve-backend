using ResolveBridge.Domain.Common;
using ResolveBridge.Domain.Enums;

namespace ResolveBridge.Domain.Entities;

public class Institution : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string LegalName { get; set; } = string.Empty;
    public InstitutionType Type { get; set; }
    public string RegistrationNumber { get; set; } = string.Empty;
    public string TaxId { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Website { get; set; } = string.Empty;
    public string LogoUrl { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    public string StreetAddress { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    
    public bool IsActive { get; set; } = true;
    public bool IsVerified { get; set; } = false;
    public decimal? CreditLimit { get; set; }
    public decimal? CurrentCreditUsed { get; set; }
    
    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<FinancialProduct> Products { get; set; } = new List<FinancialProduct>();
    public ICollection<Application> Applications { get; set; } = new List<Application>();
}
