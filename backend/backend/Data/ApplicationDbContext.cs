using Microsoft.EntityFrameworkCore;
using MonthBalance.API.Models;

namespace MonthBalance.API.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<MonthData> MonthData { get; set; } = null!;
    public DbSet<IncomeTypeModel> IncomeTypes { get; set; } = null!;
    public DbSet<Income> Incomes { get; set; } = null!;
    public DbSet<ExpenseTypeModel> ExpenseTypes { get; set; } = null!;
    public DbSet<Expense> Expenses { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        ConfigureUser(modelBuilder);
        ConfigureMonthData(modelBuilder);
        ConfigureIncomeTypeModel(modelBuilder);
        ConfigureIncome(modelBuilder);
        ConfigureExpenseTypeModel(modelBuilder);
        ConfigureExpense(modelBuilder);
    }
    
    private static void ConfigureUser(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });
    }
    
    private static void ConfigureMonthData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MonthData>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.UserId, e.Year, e.Month }).IsUnique();
            entity.Property(e => e.LastAccessed).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            
            entity.HasOne(e => e.User)
                .WithMany(u => u.MonthData)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
    
    private static void ConfigureIncomeTypeModel(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IncomeTypeModel>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Type).HasConversion<string>();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            
            entity.HasOne(e => e.User)
                .WithMany(u => u.IncomeTypes)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
    
    private static void ConfigureIncome(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Income>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            
            entity.HasOne(e => e.MonthData)
                .WithMany(m => m.Incomes)
                .HasForeignKey(e => e.MonthDataId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(e => e.IncomeType)
                .WithMany(t => t.Incomes)
                .HasForeignKey(e => e.IncomeTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
    
    private static void ConfigureExpenseTypeModel(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ExpenseTypeModel>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            
            entity.HasOne(e => e.User)
                .WithMany(u => u.ExpenseTypes)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
    
    private static void ConfigureExpense(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Expense>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            
            entity.HasOne(e => e.MonthData)
                .WithMany(m => m.Expenses)
                .HasForeignKey(e => e.MonthDataId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(e => e.ExpenseType)
                .WithMany(t => t.Expenses)
                .HasForeignKey(e => e.ExpenseTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
