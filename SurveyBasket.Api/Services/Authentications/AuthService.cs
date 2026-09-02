using Microsoft.AspNetCore.Identity;
using SurveyBasket.Api.Dtos.Security;

namespace SurveyBasket.Api.Services.Authentications;

public class AuthService(UserManager<ApplicationUser> _user ,IJwtProvider _JwtProvider ) : IAuthService
{
    public async Task<UserResponse?> LoginAsync(string email, string password, CancellationToken ct = default)
    {
       var user =  await _user.FindByEmailAsync(email);
        if (user == null) {


            return null; 
        
        }

        
      var passwordISCorrect =   await _user.CheckPasswordAsync(user, password);
        if (!passwordISCorrect)
        {
            return null;
        }
        var (token, expries) = _JwtProvider.GenerateToken(user);

        return new UserResponse(user.Id ,user.Email!,user.FirstName,user.LastName,token , expries);




        
    }
}
