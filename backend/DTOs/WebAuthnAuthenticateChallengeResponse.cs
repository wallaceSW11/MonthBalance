namespace MonthBalance.API.DTOs;

public record WebAuthnAuthenticateChallengeResponse(
    string Challenge,
    List<AllowedCredential> AllowCredentials,
    int Timeout,
    string RpId
);

public record AllowedCredential(
    string Id,
    string Type,
    string[] Transports
);
