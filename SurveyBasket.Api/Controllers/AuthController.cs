
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


        return result.IsSuccess ? Ok(result.Value) : result.toProblem();

    }

    [HttpPost("refreshtoken")]

    public async Task<IActionResult> Refreshtoken([FromBody] TokenRequest request, CancellationToken ct)
    {
        var result = await _Authservice.GetRefreshTokenAysnc(request.token,request.Refreshtoken, ct) ;


        return result.IsSuccess ? Ok(result.Value) : result.toProblem();

    }
    [HttpPost("revoke-refresh-token")]
    public async Task<IActionResult> RevokedRefreshtoken([FromBody] TokenRequest request, CancellationToken ct)
    {
        var result = await _Authservice.revokefreshTokenAysnc(request.token,request.Refreshtoken, ct) ;


        return result.IsSuccess ? NoContent() : result.toProblem();


    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken ct)
    {
        var result = await _Authservice.UserRegister(request, ct);


        return result.IsSuccess ? Ok() : result.toProblem();

    }
    [HttpPost("confirm-Email")]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request, CancellationToken ct)
    {
        var result = await _Authservice.ConfirmEmail(request, ct);


        return result.IsSuccess ? Ok() : result.toProblem();

    }
    [HttpPost("resend-confirm-Email")]
    public async Task<IActionResult> ResendConfirmEmail([FromBody] ResendConfirmationEmailRequest request, CancellationToken ct)
    {
        var result = await _Authservice.ResnedEmailConfirmation(request);


        return result.IsSuccess ? Ok() : result.toProblem();

    }
    [HttpPost("Forget-Password")]
    public async Task<IActionResult> ConfirmPasswordCode([FromBody] ForgetPasswordRequest request)
    {
        var result = await _Authservice.ResendForgetPasswordConfirmation(request.Email);


        return result.IsSuccess ? Ok() : result.toProblem();

    }
    [HttpPost("Reset-Password")]
    public async Task<IActionResult> ResetPAssword([FromBody] ResetPasswordRequest request)
    {
        var result = await _Authservice.ResetPassword(request);


        return result.IsSuccess ? Ok() : result.toProblem();

    }

}
