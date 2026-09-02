using SurveyBasket.Api.Dtos.Security;

namespace SurveyBasket.Api.Services.Authentications;

public interface IAuthService
{
    Task <UserResponse?> LoginAsync (string email, string password , CancellationToken ct = default);
}
