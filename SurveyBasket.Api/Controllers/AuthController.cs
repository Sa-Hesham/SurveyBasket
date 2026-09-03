
using System.Reflection.Metadata.Ecma335;

namespace SurveyBasket.Api.Dtos;

[Route("api/[controller]")]
[ApiController()]
public class AuthController(IAuthService _Authservice) : ControllerBase
{
    [HttpPost("login")]

    public async Task<IActionResult> UserLoginAsync(LoginRequestDto request , CancellationToken token )
    {
        var result  = await _Authservice.LoginAsync(request.Email, request.Password, token);


        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);

    }

    [HttpPost("refreshtoken")]

    public async Task<IActionResult> Refreshtoken([FromBody] TokenRequest request, CancellationToken ct)
    {
        var result = await _Authservice.GetRefreshTokenAysnc(request.token,request.Refreshtoken, ct) ;


        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
    [HttpPost("revoke-refresh-token")]
    public async Task<IActionResult> RevokedRefreshtoken([FromBody] TokenRequest request, CancellationToken ct)
    {
        var result = await _Authservice.revokefreshTokenAysnc(request.token,request.Refreshtoken, ct) ;


        return result.IsSuccess ? NoContent() : BadRequest(result.Error);



    } 
   
}
