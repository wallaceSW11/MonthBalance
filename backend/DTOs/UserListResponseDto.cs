namespace MonthBalance.API.DTOs;

public record UserListResponseDto(
    List<UserSummaryDto> Users,
    int TotalCount,
    int Page,
    int PageSize
);
