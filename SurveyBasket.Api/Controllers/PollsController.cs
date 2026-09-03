using Microsoft.AspNetCore.Authorization;
using SurveyBasket.Api.Services.Polls;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace SurveyBasket.Api.Controllers;

[Route("api/[Controller]")]
[ApiController]
[Authorize]
public class PollsController(IPollService pollService) :ControllerBase
{
    private readonly IPollService _pollService = pollService;
    
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PollResponse>>> GetPolls(CancellationToken ct )
    {
      
        var Poll = await _pollService.GetAllasync(ct);
      
        return Ok(Poll.Adapt<IEnumerable<PollResponse>>());

    }

    [HttpGet("{id}")]

    public async Task<ActionResult<PollResponse>> GetPollById( [FromRoute]int id) { 
    
       var poll= await _pollService.GetByIdAsync(id)   ;

        if (poll == null)
            return NotFound();

       var pollresponse = poll.Adapt<PollResponse>();

        return Ok(pollresponse);    
    
    }


    [HttpPost]

    public async Task<ActionResult<PollResponse>> AddPoll([FromBody] PollRequest request)
    {
        var poll = request.Adapt<Poll>();

        var createdpoll = await _pollService.CreateAsync(poll);

        return CreatedAtAction(nameof(GetPollById), new { id = createdpoll.Id }, createdpoll.Adapt<PollResponse>());

    }



    [HttpPut("{id}")]

    public async Task<IActionResult> UpdatePoll([FromRoute]int id,[FromBody]PollRequest request, CancellationToken ct) { 
    

       var poll= request.Adapt<Poll>();

       var result=  await _pollService.UpdateAsync(id, poll, ct);
        if (!result)
            return NotFound();


        return Ok(new { message = "Poll updated successfully." });





    }

    [HttpDelete("{id}")]

    public async Task<IActionResult> DeletePoll([FromRoute] int id, CancellationToken ct) { 
    
        var Isdeleted = await _pollService.DeleteAsync(id, ct);
        
        if(!Isdeleted) return NotFound();

        return Ok(new { message = $"Poll with {id} is Deleted successfully. " });
        
    
    }

    [HttpPut("{id}/togglePublish")]

    public async Task<IActionResult> togglePublishstatus([FromRoute] int id, CancellationToken ct)
    {

        var Ischanged = await _pollService.TogglePublishSatausAsync(id, ct);    

        if (!Ischanged) return NotFound();

        return Ok(new { message = $"Poll with {id} bublished  is changed successfully. " });


    }



}
