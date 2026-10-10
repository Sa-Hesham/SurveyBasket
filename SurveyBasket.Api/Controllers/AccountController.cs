using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.Api.Services.UserInfo;
using System.Security.Claims;

namespace SurveyBasket.Api.Controllers;

[Route("me")]
[ApiController]
[Authorize]
public class AccountController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpGet]
    public async Task<IActionResult> Account()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _userService.UserInformation(userId!);

        return Ok(result.Value);
    }


    [HttpPut("info")]

    public async Task<IActionResult>UpdateProfile([FromBody]  UpdateProfileRequest request)
    {   
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
       await _userService.UpdateProfileAsync(userId!, request);


        return NoContent();

    }

    [HttpPut("change-password")]

    public async Task<IActionResult>UpdateProfile(  [FromBody] ChangePasswordRequest request)
    {   
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result =  await _userService.ChangePassword(userId!, request);


        return result.IsSuccess ? NoContent() : result.toProblem();

    }
}
