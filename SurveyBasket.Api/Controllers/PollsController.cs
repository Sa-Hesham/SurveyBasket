using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Primitives;
using SurveyBasket.Api.Dtos.Errors;
using SurveyBasket.Api.Mapping.Consts;
using SurveyBasket.Api.Services.Polls;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace SurveyBasket.Api.Controllers;

[Route("api/[Controller]")]
[ApiController]

public class PollsController(IPollService pollService) :ControllerBase
{
    private readonly IPollService _pollService = pollService;
    
    
    [HttpGet]
    [Authorize(Policy = Permissions.GetRoles)]
    public async Task<ActionResult<IEnumerable<PollResponse>>> GetPolls(CancellationToken ct )
    {
      
        var result = await _pollService.GetAllasync(ct);
      
        return result.IsSuccess ?Ok(result.Value) : result.toProblem();

    }


    [HttpGet("Current")]
    [Authorize(Roles = DefaultRules.MemberRuleName)]
    public async Task<IActionResult> GetCurrent(CancellationToken ct)
    {

        var result = await _pollService.GetCurrentAsync(ct);

        return result.IsSuccess ? Ok(result.Value) : result.toProblem();

    }

    [HttpGet("{id}")]
    [Authorize(Policy = Permissions.GetPolls)]
    public async Task<ActionResult<PollResponse>> GetPollById( [FromRoute]int id, CancellationToken ct) { 
    
       var result= await _pollService.GetByIdAsync(id,ct)   ;


        return result.IsSuccess ? Ok(result.Value): result.toProblem();

    }


    [HttpPost]
    [Authorize(Policy = Permissions.AddPolls)]
    public async Task<ActionResult<PollResponse>> AddPoll([FromBody] PollRequest request,CancellationToken ct)
    {
        var poll = request.Adapt<Poll>();

        var result = await _pollService.CreateAsync(poll,ct);

        return result.IsSuccess ? CreatedAtAction(nameof(GetPollById), new { id = result.Value.Id }, result.Value) :
            result.toProblem();


    }



    [HttpPut("{id}")]
    [Authorize(Policy = Permissions.AddPolls)]
    public async Task<IActionResult> updatepoll([FromRoute] int id, [FromBody] PollRequest request, CancellationToken ct)
    {


        var poll = request.Adapt<Poll>();

        var result = await _pollService.UpdateAsync(id, poll, ct);


        return result.IsSuccess? Ok("Poll saved Successfully") : result.toProblem();







    }

    [HttpDelete("{id}")]
    [Authorize(Policy = Permissions.DeletePolls)]
    public async Task<IActionResult> DeletePoll([FromRoute] int id, CancellationToken ct)
    {

        var result = await _pollService.DeleteAsync(id, ct);



        return result.IsSuccess ? Ok(new { message = $"Poll with {id} is Deleted successfully. " })
            : result.toProblem();


    }

    [HttpPut("{id}/togglePublish")]
    [Authorize(Policy = Permissions.UpdatePolls)]
    public async Task<IActionResult> togglePublishstatus([FromRoute] int id, CancellationToken ct)
    {

        var result = await _pollService.TogglePublishSatausAsync(id, ct);



        return result.IsSuccess ? Ok(new { message = $"Poll with {id} bublished  is changed successfully. " })
            : result.toProblem();



    }



}
