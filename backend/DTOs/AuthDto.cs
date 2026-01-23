namespace backend.DTOs;

public record LoginDto(
    string Email,
    string Password
);

public record RegisterDto(
    string Email,
    string Password,
    string ConfirmPassword
);

public record AuthResponseDto(
    string Token,
    string Email,
    DateTime ExpiresAt
);
