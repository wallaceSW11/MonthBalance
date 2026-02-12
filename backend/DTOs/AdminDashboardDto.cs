namespace MonthBalance.API.DTOs;

public record AdminDashboardDto(
    int TotalUsers,
    int NewUsersToday,
    int NewUsersThisWeek,
    int NewUsersThisMonth,
    int ActiveUsersToday,
    int ActiveUsersThisWeek,
    int ActiveUsersThisMonth,
    int UnreadFeedbacks,
    List<UserSummaryDto> RecentUsers
);
