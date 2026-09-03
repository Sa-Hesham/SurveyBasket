using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Primitives;
using SurveyBasket.Api.Dtos.Errors;
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
    public async Task<ActionResult<IEnumerable<PollResponse>>> GetPolls(CancellationToken ct )
    {
      
        var result = await _pollService.GetAllasync(ct);
      
        return result.IsSuccess ?Ok(result.Value) : Problem(


            statusCode: StatusCodes.Status404NotFound,
            title: result.Error.Code,
            detail: result.Error.Message


            );

    }

    [HttpGet("{id}")]

    public async Task<ActionResult<PollResponse>> GetPollById( [FromRoute]int id, CancellationToken ct) { 
    
       var result= await _pollService.GetByIdAsync(id,ct)   ;


        return result.IsSuccess ? Ok(result.Value): Problem(
            
            
            statusCode:StatusCodes.Status404NotFound,
            title : result.Error.Code,
            detail: result.Error.Message


            );    
    
    }


    [HttpPost]
    [Authorize]
    public async Task<ActionResult<PollResponse>> AddPoll([FromBody] PollRequest request,CancellationToken ct)
    {
        var poll = request.Adapt<Poll>();

        var result = await _pollService.CreateAsync(poll,ct);

        return result.IsSuccess ? CreatedAtAction(nameof(GetPollById), new { id = result.Value.Id }, result.Value)
            : Problem(

            statusCode: StatusCodes.Status409Conflict,
            title: "Conflict",
            detail: result.Error.Message




            );


    }



    [HttpPut("{id}")]

    public async Task<IActionResult> updatepoll([FromRoute] int id, [FromBody] PollRequest request, CancellationToken ct)
    {


        var poll = request.Adapt<Poll>();

        var result = await _pollService.UpdateAsync(id, poll, ct);


        return result.IsSuccess? Ok("Poll saved Successfully") : Problem(
            
            statusCode:StatusCodes.Status400BadRequest ,
            title:result.Error.Code,
            detail:result.Error.Message
            
            
            
            
            ); 

        





        }

    [HttpDelete("{id}")]

    public async Task<IActionResult> DeletePoll([FromRoute] int id, CancellationToken ct)
    {

        var result = await _pollService.DeleteAsync(id, ct);



        return result.IsSuccess ? Ok(new { message = $"Poll with {id} is Deleted successfully. " })
            : Problem(

            statusCode: StatusCodes.Status400BadRequest,
            title: result.Error.Code,
            detail: result.Error.Message




            );



    }

    [HttpPut("{id}/togglePublish")]

    public async Task<IActionResult> togglePublishstatus([FromRoute] int id, CancellationToken ct)
    {

        var result = await _pollService.TogglePublishSatausAsync(id, ct);



        return result.IsSuccess ? Ok(new { message = $"Poll with {id} bublished  is changed successfully. " })
            : Problem(

            statusCode: StatusCodes.Status400BadRequest,
            title: result.Error.Code,
            detail: result.Error.Message




            );



    }



}
