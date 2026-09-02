
using System.Reflection.Metadata.Ecma335;

namespace SurveyBasket.Api.Dtos;

[Route("api/[controller]")]
[ApiController()]
public class AuthController(IAuthService _Authservice) : ControllerBase
{
    [HttpPost("login")]

    public async Task<IActionResult> UserLoginAsync(LoginRequestDto request , CancellationToken token )
    {
        var user = await _Authservice.LoginAsync(request.Email, request.Password, token);


       return  user is null ? BadRequest("Invalid Email or passwowrd ") : Ok(user); 

    }

    [HttpPost("refreshtoken")]

    public async Task<IActionResult> Refreshtoken([FromBody] TokenRequest request, CancellationToken ct)
    {
        var user = await _Authservice.GetRefreshTokenAysnc(request.token,request.Refreshtoken, ct) ;


        return user is null ? BadRequest("invalid token "):Ok(user);        
    }
    [HttpPost("revoke-refresh-token")]
    public async Task<IActionResult> RevokedRefreshtoken([FromBody] TokenRequest request, CancellationToken ct)
    {
        var isRevoked = await _Authservice.revokefreshTokenAysnc(request.token,request.Refreshtoken, ct) ;


        return isRevoked ? Ok() : BadRequest("Operation Failed");


           
    } 
   
}
