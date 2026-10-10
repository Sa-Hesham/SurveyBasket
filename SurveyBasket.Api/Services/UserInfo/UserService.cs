namespace SurveyBasket.Api.Services.UserInfo;

public class UserService(UserManager<ApplicationUser> usermanger) : IUserService
{
    private readonly UserManager<ApplicationUser> _usermanger = usermanger;

    public async Task<Result> UpdateProfileAsync(string userId, UpdateProfileRequest request)
    {
        var user = await _usermanger.FindByIdAsync(userId);

        //user = request.Adapt(user);

        //await  _usermanger.UpdateAsync(user!);

        await _usermanger.Users
            .Where(x => x.Id == userId)
            .ExecuteUpdateAsync(setter =>
                
                setter 
                .SetProperty(x=>x.FirstName , request.FirstName)
                .SetProperty(x=>x.LastName,request.LastName)
            );


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



    public async Task<Result> ChangePassword(string  userId, ChangePasswordRequest request)
    {
        var user = await _usermanger.FindByIdAsync(userId);

        var result = await _usermanger.ChangePasswordAsync(user!, request.CurrentPassword, request.NewPassword);


        if (result.Succeeded) 
            return Result.Success();    


        var errors = result.Errors.First();



        return Result.Failure(new Error(errors.Code, errors.Description, StatusCodes.Status400BadRequest));





    }
}
