namespace SurveyBasket.Api.Services.Authentications;

public interface IJwtProvider
{
    (string token, int expiresin) GenerateToken(ApplicationUser user);
}
