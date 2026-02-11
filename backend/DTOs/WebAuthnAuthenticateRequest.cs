namespace MonthBalance.API.DTOs;

public record WebAuthnAuthenticateRequest(
    string CredentialId,
    string AuthenticatorData,
    string ClientDataJSON,
    string Signature
);
