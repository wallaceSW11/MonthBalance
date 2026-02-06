namespace MonthBalance.API.DTOs;

public sealed record ChangePasswordRequest(
    string CurrentPassword,
    string NewPassword
);
