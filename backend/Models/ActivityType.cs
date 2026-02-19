namespace MonthBalance.API.Models;

public enum ActivityType
{
    // Auth
    UserRegistered,
    UserLogin,
    UserLogout,
    PasswordChanged,
    PasswordResetRequested,
    PasswordResetCompleted,
    
    // MonthData
    MonthDataCreated,
    MonthDataViewed,
    MonthDataUpdated,
    MonthDataDeleted,
    
    // Income
    IncomeCreated,
    IncomeUpdated,
    IncomeDeleted,
    IncomeTypeCreated,
    IncomeTypeUpdated,
    IncomeTypeDeleted,
    
    // Expense
    ExpenseCreated,
    ExpenseUpdated,
    ExpenseDeleted,
    ExpenseTypeCreated,
    ExpenseTypeUpdated,
    ExpenseTypeDeleted,
    
    // Feedback
    FeedbackSent,
    
    // Admin
    AdminPanelAccessed
}
