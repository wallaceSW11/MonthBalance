using Microsoft.EntityFrameworkCore;
using MonthBalance.Models;

namespace MonthBalance.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<MonthData> MonthData { get; set; }
    public DbSet<Income> Incomes { get; set; }
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<IncomeType> IncomeTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<MonthData>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.Year, e.Month }).IsUnique();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("NOW()");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("NOW()");
        });

        modelBuilder.Entity<Income>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.MonthData)
                  .WithMany(m => m.Incomes)
                  .HasForeignKey(e => e.MonthDataId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.IncomeType)
                  .WithMany(it => it.Incomes)
                  .HasForeignKey(e => e.IncomeTypeId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("NOW()");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("NOW()");
        });

        modelBuilder.Entity<Expense>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.MonthData)
                  .WithMany(m => m.Expenses)
                  .HasForeignKey(e => e.MonthDataId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("NOW()");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("NOW()");
        });

        modelBuilder.Entity<IncomeType>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Type).HasConversion<string>();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("NOW()");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("NOW()");
        });
    }
}
