
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
   
}
