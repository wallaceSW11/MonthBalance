namespace MonthBalance.API.DTOs;

public record WebAuthnRegisterRequest(
    string CredentialId,
    string PublicKey,
    string[] Transports,
    long Counter
);
