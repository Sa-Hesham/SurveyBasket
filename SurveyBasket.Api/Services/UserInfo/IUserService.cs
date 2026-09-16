namespace SurveyBasket.Api.Services.UserInfo;

public interface IUserService
{
    public Task<Result<UserInformationResponse>> UserInformation(string userId);

    public Task<Result> UpdateProfileAsync(string  userId , UpdateProfileRequest request);
}
