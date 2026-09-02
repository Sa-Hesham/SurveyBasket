
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SurveyBasket.Api.Services.Authentications;

public class JwtProvider(IOptions<JWTSetting> JWTOptions) : IJwtProvider
{
    private readonly JWTSetting _jWTOptions = JWTOptions.Value;

    public (string token, int expiresin) GenerateToken(ApplicationUser user)
    {


        List<Claim> claims = [

            new Claim (JwtRegisteredClaimNames.Sub , user.Id),
            new Claim(JwtRegisteredClaimNames.Email , user.Email!) ,
            new Claim(JwtRegisteredClaimNames.GivenName , user.FirstName),
            new Claim (JwtRegisteredClaimNames .FamilyName , user.LastName) ,
            new Claim(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString())



            ];

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jWTOptions.Key));


        var Securitytoken = new JwtSecurityToken(

            claims: claims,
            issuer: _jWTOptions.Issuer,
            audience:_jWTOptions.Audience,
            expires: DateTime.UtcNow.AddMinutes(_jWTOptions.intExpireMinutes),
            signingCredentials:new SigningCredentials(symmetricSecurityKey,SecurityAlgorithms.HmacSha256)

            );



        var tokenhandler = new JwtSecurityTokenHandler();

        var accessToken = tokenhandler.WriteToken(Securitytoken);



        return (token: accessToken, expiresin: _jWTOptions.intExpireMinutes*60);
       
    }
}
