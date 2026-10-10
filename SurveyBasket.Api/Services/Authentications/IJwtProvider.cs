namespace SurveyBasket.Api.Services.Authentications;

public interface IJwtProvider
{
    (string token, int expiresin) GenerateToken(ApplicationUser user ,IEnumerable<string> roles , IEnumerable<string> permissions);

    string? ValidateToken(string token);
}
