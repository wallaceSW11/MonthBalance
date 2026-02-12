namespace MonthBalance.API.DTOs;

public record ResetPasswordRequest(string Token, string NewPassword);
