namespace ResolveBridge.Domain.Enums;

public enum UserRole
{
    SuperAdmin = 1,
    InstitutionAdmin = 2,
    InstitutionAgent = 3,
    Merchant = 4,
    DataAnalyst = 5,
    Customer = 6
}

public enum InstitutionType
{
    Bank = 1,
    Microfinance = 2,
    InsuranceCompany = 3,
    Merchant = 4,
    Fintech = 5
}

public enum ProductType
{
    Loan = 1,
    BNPL = 2,
    Insurance = 3
}

public enum LoanType
{
    Personal = 1,
    Business = 2,
    Mortgage = 3,
    Vehicle = 4,
    Education = 5
}

public enum BNPLCategory
{
    Electronics = 1,
    Appliances = 2,
    Furniture = 3,
    Fashion = 4,
    Education = 5
}

public enum InsuranceType
{
    Life = 1,
    Health = 2,
    Property = 3,
    Vehicle = 4,
    Business = 5,
    Device = 6
}

public enum ApplicationStatus
{
    Draft = 1,
    Submitted = 2,
    UnderReview = 3,
    PendingDocuments = 4,
    Approved = 5,
    Rejected = 6,
    Cancelled = 7,
    Disbursed = 8,
    Active = 9,
    Completed = 10,
    Defaulted = 11
}

public enum PaymentStatus
{
    Pending = 1,
    Partial = 2,
    Paid = 3,
    Overdue = 4,
    WrittenOff = 5
}

public enum NotificationType
{
    ApplicationUpdate = 1,
    PaymentReminder = 2,
    Approval = 3,
    Rejection = 4,
    System = 5,
    Marketing = 6
}

public enum NotificationChannel
{
    Push = 1,
    Email = 2,
    SMS = 3
}

public enum Market
{
    Ghana = 1,
    Nigeria = 2,
    Kenya = 3,
    SouthAfrica = 4
}

public enum DocumentType
{
    IDCard = 1,
    Passport = 2,
    UtilityBill = 3,
    BankStatement = 4,
    Payslip = 5,
    BusinessRegistration = 6,
    TaxDocument = 7,
    Other = 8
}

public enum KycStatus
{
    Pending = 1,
    Verified = 2,
    Rejected = 3,
    RequiresReview = 4
}

public enum EmploymentStatus
{
    Employed = 1,
    SelfEmployed = 2,
    BusinessOwner = 3,
    Contractor = 4,
    Other = 5
}

