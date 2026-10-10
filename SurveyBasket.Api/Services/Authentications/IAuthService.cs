using SurveyBasket.Api.Dtos.Errors;
using SurveyBasket.Api.Dtos.Security;

namespace SurveyBasket.Api.Services.Authentications;

public interface IAuthService
{
    Task <Result<UserResponse>> LoginAsync (string email, string password , CancellationToken ct = default);
    Task<Result<UserResponse>> GetRefreshTokenAysnc(string token, string refreshtoken, CancellationToken ct = default);
    Task<Result> revokefreshTokenAysnc(string token, string refreshtoken, CancellationToken ct = default);

    Task<Result> UserRegister(RegisterRequest request, CancellationToken ct = default);

    Task<Result> ConfirmEmail(ConfirmEmailRequest request, CancellationToken ct = default);
    Task<Result> ResnedEmailConfirmation(ResendConfirmationEmailRequest request);
    Task<Result> ResendForgetPasswordConfirmation(string email);
    Task<Result> ResetPassword(ResetPasswordRequest request);
}
