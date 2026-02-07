namespace MonthBalance.API.DTOs;

public record WebAuthnRegisterResponse(
    bool Success,
    string Message
);
