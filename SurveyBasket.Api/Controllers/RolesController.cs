using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.Api.Dtos.Security.Roles;
using SurveyBasket.Api.Mapping.Consts;
using SurveyBasket.Api.Services.RolesService;

namespace SurveyBasket.Api.Controllers;

[Route("api/[controller]")]
[ApiController]

public class RolesController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RolesController(IRoleService roleService)
    {
       _roleService = roleService;
    }


    [HttpGet]
    [Authorize(Policy = Permissions.GetRoles)]

    public async Task<IActionResult> GetAll([FromQuery] bool includeDeisabled  , CancellationToken ct )
    {
        var roles = await _roleService.GetAll(includeDeisabled , ct);    

        return Ok(roles);   
    }
    [HttpGet("{Id}")]
    [Authorize(Policy = Permissions.GetRoles)]

    public async Task<IActionResult> GetRolebyId([FromRoute] string Id , CancellationToken ct)
    {
        var result = await _roleService.getByidAsync(Id); 

            return result.IsSuccess ? Ok(result.Value) : result.toProblem();  
    }
    [HttpPost()]
    [Authorize(Policy = Permissions.AddRoles)]

    public async Task<IActionResult> add([FromBody] RoleRequest request )
    {
        var result = await _roleService.AddAsync(request); 

            return result.IsSuccess ? CreatedAtAction(nameof(GetRolebyId) , new {result.Value.Id} ,result.Value) : result.toProblem();  
    }
    [HttpPost("{id}")]
    [Authorize(Policy = Permissions.UpdateRoles)]

    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] RoleRequest request )
    {
        var result = await _roleService.UpdateAsync(id,request); 

            return result.IsSuccess ? NoContent() : result.toProblem();  
    }

    [HttpPut("{id}/toggle-status")]
    [Authorize(Policy=Permissions.UpdateRoles)]
    public async Task<IActionResult> ToggleStatus([FromRoute] string id)
    {
        var result = await _roleService.ToggleStatusAsync(id);

        return result.IsSuccess ? NoContent() : result.toProblem();
    }


}
