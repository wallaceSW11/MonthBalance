namespace MonthBalance.API.DTOs;

public record WebAuthnRegisterChallengeResponse(
    string Challenge,
    string UserId,
    string RpId,
    string RpName,
    int Timeout
);
