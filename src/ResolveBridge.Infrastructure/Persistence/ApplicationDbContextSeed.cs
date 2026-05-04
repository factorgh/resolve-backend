using ResolveBridge.Domain.Entities;
using ResolveBridge.Domain.Enums;
using ResolveBridge.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace ResolveBridge.Infrastructure.Persistence;

public static class ApplicationDbContextSeed
{
    public static async Task SeedAsync(
        ApplicationDbContext context, 
        UserManager<IdentityUser> userManager, 
        RoleManager<IdentityRole> roleManager)
    {
        // 0. Seed Roles
        await SeedRolesAsync(roleManager);

        // 1. Seed Institutions
        if (!context.Institutions.Any())
        {
            var ghanaBank = new Institution
            {
                Id = Guid.NewGuid(),
                Name = "Ghana First Bank",
                LegalName = "Ghana First Bank Limited",
                Type = InstitutionType.Bank,
                RegistrationNumber = "GFB001",
                TaxId = "GFB-TAX-001",
                Email = "info@ghanafirstbank.com",
                PhoneNumber = "+233501234567",
                Website = "https://ghanafirstbank.com",
                LogoUrl = "https://logo.clearbit.com/ghanafirstbank.com",
                Description = "The leading commercial bank in Ghana",
                StreetAddress = "123 Independence Avenue",
                City = "Accra",
                State = "Greater Accra",
                Country = "Ghana",
                PostalCode = "GA001",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var safiInsure = new Institution
            {
                Id = Guid.NewGuid(),
                Name = "Safi Insurance",
                LegalName = "Safi Insurance Company Limited",
                Type = InstitutionType.InsuranceCompany,
                RegistrationNumber = "SIC001",
                TaxId = "SIC-TAX-001",
                Email = "info@safiinsurance.com",
                PhoneNumber = "+233507654321",
                Website = "https://safiinsurance.com",
                LogoUrl = "https://logo.clearbit.com/safiinsurance.com",
                Description = "Fastest claims processing in West Africa",
                StreetAddress = "456 Ring Road",
                City = "Accra",
                State = "Greater Accra",
                Country = "Ghana",
                PostalCode = "GA002",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            context.Institutions.AddRange(ghanaBank, safiInsure);
            await context.SaveChangesAsync();

            // 2. Seed Loans
            var personalLoan = new FinancialProduct
            {
                Id = Guid.NewGuid(),
                Name = "Instant Personal Loan",
                Description = "Get up to GHS 50,000 instantly for your personal needs",
                ProductType = ProductType.Loan,
                InstitutionId = ghanaBank.Id,
                MinAmount = 1000,
                MaxAmount = 50000,
                InterestRate = 18.5m,
                MinTenureMonths = 3,
                MaxTenureMonths = 24,
                Requirements = "Valid ID, Passport picture, 3 months bank statement",
                Benefits = "Quick approval, Flexible repayment, Low interest",
                TermsAndConditions = "Must be a Ghanaian resident, 18+ years old",
                IsActive = true,
                IsFeatured = true,
                CreatedAt = DateTime.UtcNow,
                LoanDetails = new LoanProductDetails
                {
                    LoanType = LoanType.Personal,
                    Purpose = "Personal consumption",
                    RequiresCollateral = false
                }
            };

            context.FinancialProducts.Add(personalLoan);

            // 3. Seed Insurance
            var carInsurance = new FinancialProduct
            {
                Id = Guid.NewGuid(),
                Name = "Comprehensive Motor Insurance",
                Description = "Full protection for your vehicle against theft, accident and fire",
                ProductType = ProductType.Insurance,
                InstitutionId = safiInsure.Id,
                MinAmount = 500,
                MaxAmount = 10000,
                InterestRate = 0,
                MinTenureMonths = 12,
                MaxTenureMonths = 12,
                Requirements = "Vehicle registration, Driving license",
                Benefits = "24/7 roadside assistance, Free towing",
                TermsAndConditions = "Private use only",
                IsActive = true,
                IsFeatured = true,
                CreatedAt = DateTime.UtcNow,
                InsuranceDetails = new InsuranceProductDetails
                {
                    InsuranceType = InsuranceType.Vehicle,
                    MinCoverageAmount = 10000,
                    MaxCoverageAmount = 250000,
                    PremiumRates = new List<PremiumRate>
                    {
                        new() { MinAmount = 10000, MaxAmount = 50000, MonthlyPremium = 250, AnnualPremium = 2800 },
                        new() { MinAmount = 50001, MaxAmount = 100000, MonthlyPremium = 450, AnnualPremium = 5000 }
                    },
                    CoverageDetails = new List<CoverageDetail>
                    {
                        new() { Name = "Accidental Damage", IsIncluded = true, Amount = 250000 },
                        new() { Name = "Third Party Liability", IsIncluded = true, Amount = 1000000 },
                        new() { Name = "Theft", IsIncluded = true, Amount = 250000 }
                    }
                }
            };

            context.FinancialProducts.Add(carInsurance);

            // 4. Seed BNPL
            var techBnpl = new FinancialProduct
            {
                Id = Guid.NewGuid(),
                Name = "EasyTech Gadget Plan",
                Description = "Buy your favorite gadgets and pay in small installments",
                ProductType = ProductType.BNPL,
                InstitutionId = ghanaBank.Id,
                MinAmount = 500,
                MaxAmount = 15000,
                InterestRate = 5.0m,
                MinTenureMonths = 3,
                MaxTenureMonths = 6,
                Requirements = "Employment letter, Valid ID",
                Benefits = "Zero downpayment options, Instant gadget pickup",
                TermsAndConditions = "Limited to partner tech shops",
                IsActive = true,
                IsFeatured = false,
                CreatedAt = DateTime.UtcNow,
                BNPLDetails = new BNPLProductDetails
                {
                    Category = BNPLCategory.Electronics,
                    InstallmentPeriods = 4,
                    DownPaymentPercent = 10,
                    SupportedMerchants = new List<string> { "CompuGhana", "Freddies Corner" }
                }
            };

            context.FinancialProducts.Add(techBnpl);
            await context.SaveChangesAsync();
        }

        // 5. Seed News Articles
        if (!context.NewsArticles.Any())
        {
            context.NewsArticles.AddRange(
                new NewsArticle
                {
                    Id = Guid.NewGuid(),
                    Title = "The state of lending in Ghana 2026",
                    Tag = "Market Report",
                    Icon = "📈",
                    Summary = "As of 2026, Ghana has seen a 40% increase in digital lending adoption.",
                    Content = "Institutional lenders are now prioritizing alternative credit scoring models that factor in mobile money velocity and utility payment history.",
                    ReadingTimeMinutes = 5,
                    IsPublished = true,
                    PublishedAt = DateTime.UtcNow.AddDays(-2),
                    CreatedAt = DateTime.UtcNow.AddDays(-2)
                },
                new NewsArticle
                {
                    Id = Guid.NewGuid(),
                    Title = "How to boost your score by 50 points",
                    Tag = "Expert Tips",
                    Icon = "⚡",
                    Summary = "The fastest way to improve your Resolve Health Index is lower utilization.",
                    Content = "Maintain a utilization rate below 30% on your Kredete BNPL lines and ensure all mobile money repayments are made 2 days before the due date.",
                    ReadingTimeMinutes = 3,
                    IsPublished = true,
                    PublishedAt = DateTime.UtcNow.AddDays(-1),
                    CreatedAt = DateTime.UtcNow.AddDays(-1)
                },
                new NewsArticle
                {
                    Id = Guid.NewGuid(),
                    Title = "Understanding mobile money repayments",
                    Tag = "Guide",
                    Icon = "📱",
                    Summary = "ResolveBridge now supports automated direct debits from MTN MoMo.",
                    Content = "This integration ensures you never miss a payment, even when you are offline.",
                    ReadingTimeMinutes = 4,
                    IsPublished = true,
                    PublishedAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow
                }
            );
            await context.SaveChangesAsync();
        }

        await SeedUsersAsync(context, userManager);
        await context.SaveChangesAsync();
    }

    private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        if (!await roleManager.RoleExistsAsync(UserRole.SuperAdmin.ToString()))
        {
            await roleManager.CreateAsync(new IdentityRole(UserRole.SuperAdmin.ToString()));
        }

        if (!await roleManager.RoleExistsAsync(UserRole.Customer.ToString()))
        {
            await roleManager.CreateAsync(new IdentityRole(UserRole.Customer.ToString()));
        }
    }

    private static async Task SeedUsersAsync(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        if (context.AppUsers.Any()) return;

        // Admin
        var adminEmail = "admin@resolvebridge.com";
        var adminIdentity = await userManager.FindByEmailAsync(adminEmail);
        if (adminIdentity == null)
        {
            adminIdentity = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
            await userManager.CreateAsync(adminIdentity, "Resolve@123");
            await userManager.AddToRoleAsync(adminIdentity, UserRole.SuperAdmin.ToString());
        }

        var admin = new User
        {
            Id = Guid.Parse(adminIdentity.Id),
            Email = adminEmail,
            FirstName = "System",
            LastName = "Admin",
            Role = UserRole.SuperAdmin,
            Market = Market.Ghana,
            EmailVerified = true,
            CreatedAt = DateTime.UtcNow
        };

        // Customer
        var customerEmail = "customer@resolvebridge.com";
        var customerIdentity = await userManager.FindByEmailAsync(customerEmail);
        if (customerIdentity == null)
        {
            customerIdentity = new IdentityUser { UserName = customerEmail, Email = customerEmail, EmailConfirmed = true };
            await userManager.CreateAsync(customerIdentity, "Resolve@123");
            await userManager.AddToRoleAsync(customerIdentity, UserRole.Customer.ToString());
        }

        var customer = new User
        {
            Id = Guid.Parse(customerIdentity.Id),
            Email = customerEmail,
            FirstName = "Dummy",
            LastName = "Customer",
            Role = UserRole.Customer,
            Market = Market.Ghana,
            EmailVerified = true,
            CreatedAt = DateTime.UtcNow
        };

        context.AppUsers.AddRange(admin, customer);
    }
}
