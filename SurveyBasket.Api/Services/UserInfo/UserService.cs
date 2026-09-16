namespace SurveyBasket.Api.Services.UserInfo;

public class UserService(UserManager<ApplicationUser> usermanger) : IUserService
{
    private readonly UserManager<ApplicationUser> _usermanger = usermanger;

    public async Task<Result> UpdateProfileAsync(string userId, UpdateProfileRequest request)
    {
        var user = await _usermanger.FindByIdAsync(userId);

        user = request.Adapt(user);

        await  _usermanger.UpdateAsync(user!);


        return Result.Success();
    }

    public async Task<Result<UserInformationResponse>> UserInformation(string userId)
    {
      var user = await _usermanger.Users
            .Where(x=>x.Id== userId)
            .ProjectToType< UserInformationResponse >()
            .SingleAsync(); 

        return Result.Succes(user);
    }
}
