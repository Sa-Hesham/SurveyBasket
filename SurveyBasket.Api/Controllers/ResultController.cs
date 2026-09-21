using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.Api.Mapping.Consts;
using SurveyBasket.Api.Services.VoteResultSevices;

namespace SurveyBasket.Api.Controllers;

[Route("api/Polls/{pollId}/[controller]")]
[ApiController]
[Authorize (Policy =Permissions.Results)]
public class ResultController(IVoteResultService voteResult) : ControllerBase
{
    private readonly IVoteResultService _voteResult = voteResult;

    [HttpGet("row-data")]

    public async Task<IActionResult> GetPollVotesAsync([FromRoute] int pollId ,CancellationToken ct)
    {

        var result = await  _voteResult.GetVoteResultAsync(pollId ,ct);


        return result.IsSuccess ? Ok(result.Value) : result.toProblem();    
    }




    [HttpGet("votes-per-day")] 

    public async Task<IActionResult>GetVotesperday([FromRoute] int pollId, CancellationToken ct)
    {
        var result = await _voteResult.GetVotesPErDayAsync(pollId, ct);


        return result.IsSuccess ? Ok(result.Value) : result.toProblem();
    }


    [HttpGet("votes-per-question")]

    public async Task<IActionResult> GetVotesperquestion([FromRoute] int pollId, CancellationToken ct)
    {
        var result = await _voteResult.GetVotesPerQuestionAsync(pollId, ct);


        return result.IsSuccess ? Ok(result.Value) : result.toProblem();
    }

}
