using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Income> Incomes { get; set; }
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<MonthData> MonthData { get; set; }
    public DbSet<MonthIncome> MonthIncomes { get; set; }
    public DbSet<MonthExpense> MonthExpenses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Income
        modelBuilder.Entity<Income>()
            .HasOne(i => i.User)
            .WithMany(u => u.Incomes)
            .HasForeignKey(i => i.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Expense
        modelBuilder.Entity<Expense>()
            .HasOne(e => e.User)
            .WithMany(u => u.Expenses)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // MonthData
        modelBuilder.Entity<MonthData>()
            .HasOne(md => md.User)
            .WithMany(u => u.MonthData)
            .HasForeignKey(md => md.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MonthData>()
            .HasIndex(md => new { md.UserId, md.Year, md.Month })
            .IsUnique();

        // MonthIncome
        modelBuilder.Entity<MonthIncome>()
            .HasOne(mi => mi.MonthData)
            .WithMany(md => md.MonthIncomes)
            .HasForeignKey(mi => mi.MonthDataId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MonthIncome>()
            .HasOne(mi => mi.Income)
            .WithMany(i => i.MonthIncomes)
            .HasForeignKey(mi => mi.IncomeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MonthIncome>()
            .HasIndex(mi => new { mi.MonthDataId, mi.IncomeId })
            .IsUnique();

        // MonthExpense
        modelBuilder.Entity<MonthExpense>()
            .HasOne(me => me.MonthData)
            .WithMany(md => md.MonthExpenses)
            .HasForeignKey(me => me.MonthDataId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MonthExpense>()
            .HasOne(me => me.Expense)
            .WithMany(e => e.MonthExpenses)
            .HasForeignKey(me => me.ExpenseId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MonthExpense>()
            .HasIndex(me => new { me.MonthDataId, me.ExpenseId })
            .IsUnique();
    }
}
