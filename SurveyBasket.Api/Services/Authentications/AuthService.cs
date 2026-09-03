using Microsoft.AspNetCore.Identity;
using SurveyBasket.Api.Dtos.Errors;
using SurveyBasket.Api.Dtos.Security;
using System.Security.Cryptography;

namespace SurveyBasket.Api.Services.Authentications;

public class AuthService(UserManager<ApplicationUser> _user ,IJwtProvider _JwtProvider ) : IAuthService
{
    private readonly int Expiretiontokendays = 5;
     public async Task<Result<UserResponse>> LoginAsync(string email, string password, CancellationToken ct = default)
    {
       var user =  await _user.FindByEmailAsync(email);
        if (user == null) {


            return Result.Failure<UserResponse>(UserError.Error);
        
        }

      var passwordISCorrect =   await _user.CheckPasswordAsync(user, password);
        if (!passwordISCorrect)
        {
            return Result.Failure<UserResponse>(UserError.Error);
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

        var userResponse = new UserResponse(user.Id, user.Email!, user.FirstName, user.LastName, token, expries, Refershtoken, RefreshtokenExpiration);


        return Result.Succes(userResponse) ;  





    }

    public async Task<Result<UserResponse>> GetRefreshTokenAysnc(string token, string refreshtoken, CancellationToken ct = default)
    {
       var userId =  _JwtProvider.ValidateToken(token);  
        if(userId is null)
        {
            return Result.Failure<UserResponse>(UserError.Error);
        }
        // find user 
         var user = await _user.FindByIdAsync(userId);   
        if (user == null) 
        {
            return Result.Failure<UserResponse>(UserError.Error);

        }
        // find if user has token == Refreshtoken request
         var userRefreshToken = user.RefreshTokens.SingleOrDefault(x=>x.Token == refreshtoken && x.IsActive ) ;
        if (userRefreshToken == null) {

           return Result.Failure<UserResponse>(new("Refreshtoken.Error","Is not Activ / Expired ,or not Found"));
        
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

        var userResponse =   new UserResponse(user.Id, user.Email!, user.FirstName, user.LastName, newtoken, ExpireIn, newRefreshtoken, Expireon);

        return Result.Succes(userResponse);
    }
    public async Task<Result> revokefreshTokenAysnc(string token, string refreshtoken, CancellationToken ct = default)
    {
        var userId = _JwtProvider.ValidateToken(token);
        if (userId is null)
        {
            return Result.Failure(UserError.Error); 
        }
        // find user 
        var user = await _user.FindByIdAsync(userId);
        if (user == null)
        {
            return Result.Failure(UserError.Error);

        }
        // find if user has token == Refreshtoken request
        var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshtoken && x.IsActive);
        if (userRefreshToken == null)
        {

            return Result.Failure<UserResponse>(new("Refreshtoken.Error", "Is not Activ / Expired ,or not Found"));

        }

        // revoke refrsh token 
        userRefreshToken.RevokedOn = DateTime.UtcNow;
        await _user.UpdateAsync(user);

        return Result.Success();
        
    }

    private static string  GenrateRefreshtoken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }

}
