using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ApplicationEntity = ResolveBridge.Domain.Entities.Application;
using ResolveBridge.Domain.Entities;
using ResolveBridge.Domain.Enums;
using ResolveBridge.Application.Interfaces;

namespace ResolveBridge.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> AppUsers => Set<User>();
    public DbSet<UserDocument> UserDocuments => Set<UserDocument>();
    public DbSet<Institution> Institutions => Set<Institution>();
    public DbSet<FinancialProduct> FinancialProducts => Set<FinancialProduct>();
    public DbSet<LoanProductDetails> LoanProductDetails => Set<LoanProductDetails>();
    public DbSet<BNPLProductDetails> BNPLProductDetails => Set<BNPLProductDetails>();
    public DbSet<InsuranceProductDetails> InsuranceProductDetails => Set<InsuranceProductDetails>();
    public DbSet<PremiumRate> PremiumRates => Set<PremiumRate>();
    public DbSet<CoverageDetail> CoverageDetails => Set<CoverageDetail>();
    public DbSet<ExclusionDetail> ExclusionDetails => Set<ExclusionDetail>();
    public DbSet<ApplicationEntity> Applications => Set<ApplicationEntity>();
    public DbSet<ApplicationDocument> ApplicationDocuments => Set<ApplicationDocument>();
    public DbSet<ApplicationStatusHistory> ApplicationStatusHistories => Set<ApplicationStatusHistory>();
    public DbSet<LoanLifecycle> LoanLifecycles => Set<LoanLifecycle>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<SystemLog> SystemLogs => Set<SystemLog>();
    public DbSet<NewsArticle> NewsArticles => Set<NewsArticle>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasIndex(e => e.PhoneNumber);
            entity.HasIndex(e => e.NationalId);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.NationalId).HasMaxLength(50);
            entity.Property(e => e.Role).HasConversion<string>();
            entity.Property(e => e.KycStatus).HasConversion<string>();
            entity.Property(e => e.Market).HasConversion<string>();
            entity.Property(e => e.EmploymentStatus).HasConversion<string>();
            
            entity.Property(e => e.Goals)
                .HasColumnType("TEXT")
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new List<string>(),
                    new Microsoft.EntityFrameworkCore.ChangeTracking.ValueComparer<List<string>>(
                        (c1, c2) => c1 != null && c2 != null && c1.SequenceEqual(c2),
                        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                        c => c.ToList()));

            entity.HasMany(e => e.Documents).WithOne(d => d.User).HasForeignKey(d => d.UserId);
            entity.HasMany(e => e.Applications).WithOne(a => a.User).HasForeignKey(a => a.UserId);
            entity.HasMany(e => e.LoanLifecycles).WithOne(l => l.User).HasForeignKey(l => l.UserId);
            entity.HasMany(e => e.Notifications).WithOne(n => n.User).HasForeignKey(n => n.UserId);
        });

        builder.Entity<Institution>(entity =>
        {
            entity.ToTable("Institutions");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Name);
            entity.HasIndex(e => e.RegistrationNumber).IsUnique();
            entity.Property(e => e.Type).HasConversion<string>();
            entity.HasMany(e => e.Applications).WithOne(a => a.Institution).HasForeignKey(a => a.InstitutionId);
            entity.HasMany(e => e.Products).WithOne(p => p.Institution).HasForeignKey(p => p.InstitutionId);
        });

        builder.Entity<FinancialProduct>(entity =>
        {
            entity.ToTable("FinancialProducts");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.ProductType);
            entity.HasIndex(e => e.InstitutionId);
            entity.HasIndex(e => e.IsActive);
            entity.Property(e => e.ProductType).HasConversion<string>();
            entity.Property(e => e.MinAmount).HasPrecision(18, 2);
            entity.Property(e => e.MaxAmount).HasPrecision(18, 2);
            entity.Property(e => e.InterestRate).HasPrecision(5, 2);
            entity.HasMany(e => e.Applications).WithOne(a => a.Product).HasForeignKey(a => a.ProductId);
            entity.HasOne(e => e.LoanDetails).WithOne(l => l.Product).HasForeignKey<LoanProductDetails>(l => l.ProductId);
            entity.HasOne(e => e.BNPLDetails).WithOne(b => b.Product).HasForeignKey<BNPLProductDetails>(b => b.ProductId);
            entity.HasOne(e => e.InsuranceDetails).WithOne(i => i.Product).HasForeignKey<InsuranceProductDetails>(i => i.ProductId);
        });

        builder.Entity<InsuranceProductDetails>(entity =>
        {
            entity.ToTable("InsuranceProductDetails");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.InsuranceType).HasConversion<string>();
            entity.Property(e => e.MinCoverageAmount).HasPrecision(18, 2);
            entity.Property(e => e.MaxCoverageAmount).HasPrecision(18, 2);
            entity.HasMany(e => e.PremiumRates).WithOne().HasForeignKey(p => p.InsuranceDetailsId);
            entity.HasMany(e => e.CoverageDetails).WithOne().HasForeignKey(c => c.InsuranceDetailsId);
            entity.HasMany(e => e.ExclusionDetails).WithOne().HasForeignKey(e => e.InsuranceDetailsId);
        });

        builder.Entity<PremiumRate>(entity =>
        {
            entity.ToTable("PremiumRates");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.MinAmount).HasPrecision(18, 2);
            entity.Property(e => e.MaxAmount).HasPrecision(18, 2);
            entity.Property(e => e.MonthlyPremium).HasPrecision(18, 2);
            entity.Property(e => e.AnnualPremium).HasPrecision(18, 2);
        });

        builder.Entity<CoverageDetail>(entity =>
        {
            entity.ToTable("CoverageDetails");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Amount).HasPrecision(18, 2);
        });

        builder.Entity<ExclusionDetail>(entity =>
        {
            entity.ToTable("ExclusionDetails");
            entity.HasKey(e => e.Id);
        });

        builder.Entity<ApplicationEntity>(entity =>
        {
            entity.ToTable("Applications");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.ApplicationNumber).IsUnique();
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.InstitutionId);
            entity.HasIndex(e => e.CreatedAt);
            entity.Property(e => e.Status).HasConversion<string>();
            entity.Property(e => e.RequestedAmount).HasPrecision(18, 2);
            entity.Property(e => e.ApprovedAmount).HasPrecision(18, 2);
            entity.Property(e => e.InterestRate).HasPrecision(5, 2);
            entity.Property(e => e.MonthlyPayment).HasPrecision(18, 2);
            entity.Property(e => e.TotalRepayment).HasPrecision(18, 2);
            entity.Property(e => e.DisbursedAmount).HasPrecision(18, 2);
            entity.HasMany(e => e.Documents).WithOne(d => d.Application).HasForeignKey(d => d.ApplicationId);
            entity.HasMany(e => e.StatusHistory).WithOne(s => s.Application).HasForeignKey(s => s.ApplicationId);
            entity.HasOne(e => e.LoanLifecycle).WithOne(l => l.Application).HasForeignKey<LoanLifecycle>(l => l.ApplicationId);
            
            // Fix for SQL Server multiple cascade paths
            entity.HasOne(e => e.Institution)
                .WithMany(i => i.Applications)
                .HasForeignKey(e => e.InstitutionId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.Product)
                .WithMany(p => p.Applications)
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        builder.Entity<LoanLifecycle>(entity =>
        {
            entity.ToTable("LoanLifecycles");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.InstitutionId);
            entity.HasIndex(e => e.IsActive);
            entity.HasIndex(e => e.PaymentStatus);
            entity.Property(e => e.PaymentStatus).HasConversion<string>();
            
            // Fix for SQL Server multiple cascade paths
            entity.HasOne(e => e.User)
                .WithMany(u => u.LoanLifecycles)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            entity.Property(e => e.PrincipalAmount).HasPrecision(18, 2);
            entity.Property(e => e.InterestRate).HasPrecision(5, 2);
            entity.Property(e => e.TotalRepaymentAmount).HasPrecision(18, 2);
            entity.Property(e => e.OutstandingBalance).HasPrecision(18, 2);
            entity.Property(e => e.AmountPaid).HasPrecision(18, 2);
            entity.Property(e => e.NextPaymentAmount).HasPrecision(18, 2);
            entity.Property(e => e.LateFeesAccrued).HasPrecision(18, 2);
            entity.HasMany(e => e.Payments).WithOne(p => p.LoanLifecycle).HasForeignKey(p => p.LoanLifecycleId);
            
            // Fix for SQL Server multiple cascade paths
            entity.HasOne(e => e.Institution)
                .WithMany()
                .HasForeignKey(e => e.InstitutionId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        builder.Entity<Payment>(entity =>
        {
            entity.ToTable("Payments");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.LoanLifecycleId);
            entity.HasIndex(e => e.PaymentDate);
            entity.Property(e => e.Amount).HasPrecision(18, 2);
            entity.Property(e => e.PrincipalPaid).HasPrecision(18, 2);
            entity.Property(e => e.InterestPaid).HasPrecision(18, 2);
            entity.Property(e => e.LateFeePaid).HasPrecision(18, 2);
        });

        builder.Entity<Notification>(entity =>
        {
            entity.ToTable("Notifications");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.IsRead);
            entity.HasIndex(e => e.Type);
            entity.HasIndex(e => e.CreatedAt);
            entity.Property(e => e.Type).HasConversion<string>();
            entity.Property(e => e.Channel).HasConversion<string>();
        });

        builder.Entity<SystemLog>(entity =>
        {
            entity.ToTable("SystemLogs");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Level);
            entity.HasIndex(e => e.CreatedAt);
            entity.Property(e => e.Properties)
                .HasColumnType("TEXT")
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new Dictionary<string, string>(),
                    new Microsoft.EntityFrameworkCore.ChangeTracking.ValueComparer<Dictionary<string, string>>(
                        (c1, c2) => c1 != null && c2 != null && c1.SequenceEqual(c2),
                        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                        c => c.ToDictionary(kvp => kvp.Key, kvp => kvp.Value)));
        });
    }
}
