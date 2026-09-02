using Microsoft.AspNetCore.Identity;
using SurveyBasket.Api.Dtos.Security;
using System.Security.Cryptography;

namespace SurveyBasket.Api.Services.Authentications;

public class AuthService(UserManager<ApplicationUser> _user ,IJwtProvider _JwtProvider ) : IAuthService
{
    private readonly int Expiretiontokendays = 5;
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
        var Refershtoken = GenrateRefreshtoken();
        var RefreshtokenExpiration = DateTime.UtcNow.AddDays(Expiretiontokendays);
        user.RefreshTokens.Add(new RefreshToken
        {
            Token = Refershtoken,
            ExpiresOn= RefreshtokenExpiration,  

        });
        await _user.UpdateAsync(user);

        return new UserResponse(user.Id ,user.Email!,user.FirstName,user.LastName,token , expries, Refershtoken, RefreshtokenExpiration);


       

        
    }

    public async Task<UserResponse?> GetRefreshTokenAysnc(string token, string refreshtoken, CancellationToken ct = default)
    {
       var userId =  _JwtProvider.ValidateToken(token);  
        if(userId is null)
        {
            return null;
        }
        // find user 
         var user = await _user.FindByIdAsync(userId);   
        if (user == null) 
        { 
            return null;
        
        }
        // find if user has token == Refreshtoken request
     var userRefreshToken = user.RefreshTokens.SingleOrDefault(x=>x.Token == refreshtoken && x.IsActive ) ;
        if (userRefreshToken == null) {

            return null;
        
        }

        // revoke refrsh token 
        userRefreshToken.RevokedOn = DateTime.UtcNow;
        var( newtoken, ExpireIn) = _JwtProvider.GenerateToken(user);
        var newRefreshtoken = GenrateRefreshtoken();
        var Expireon = DateTime.UtcNow.AddDays(Expiretiontokendays);
        user.RefreshTokens.Add(new RefreshToken{
            Token = newRefreshtoken,
            ExpiresOn = Expireon,    
        });
        await _user.UpdateAsync(user);
        return new UserResponse(user.Id, user.Email!, user.FirstName, user.LastName, newtoken, ExpireIn, newRefreshtoken, Expireon);
    }
    public async Task<bool> revokefreshTokenAysnc(string token, string refreshtoken, CancellationToken ct = default)
    {
        var userId = _JwtProvider.ValidateToken(token);
        if (userId is null)
        {
            return false;
        }
        // find user 
        var user = await _user.FindByIdAsync(userId);
        if (user == null)
        {
            return false;

        }
        // find if user has token == Refreshtoken request
        var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshtoken && x.IsActive);
        if (userRefreshToken == null)
        {

            return false;

        }

        // revoke refrsh token 
        userRefreshToken.RevokedOn = DateTime.UtcNow;
        await _user.UpdateAsync(user);

        return true;
    }

    private static string  GenrateRefreshtoken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }

}
