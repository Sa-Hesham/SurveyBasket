using Microsoft.AspNetCore.Authorization;
using SurveyBasket.Api.Dtos.Vote;
using SurveyBasket.Api.Mapping.Consts;
using SurveyBasket.Api.Services.VoteService;
using System.Security.Claims;

namespace SurveyBasket.Api.Controllers;

[Route("api/polls/{pollId}/vote")]
[ApiController]
[Authorize(Roles =DefaultRules.MemberRuleName)]
public class VotesController(IQuestionService questionService ,IVoteService vote ) : ControllerBase
{
    private readonly IQuestionService _questionService = questionService;
    private readonly IVoteService _vote = vote;

    [HttpGet]
    public async Task<IActionResult>  start([FromRoute] int pollId, CancellationToken ct  )
    {

        var USerId = User.FindFirstValue(ClaimTypes.NameIdentifier);


        var result = await _questionService.GetAvilabeQuestionAsync(pollId, USerId! , ct);


        if(result.IsSuccess)
        {

            return Ok(result.Value);
        }



        return result.toProblem();

    }

    [HttpPost]
    public async Task<IActionResult> Vote ([FromRoute] int pollId, [FromBody] VoteRequest request  ,CancellationToken ct)
    {
        var USerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var result = await _vote.AddAsync(pollId, USerId!, request, ct);

        if (result.IsSuccess)
            return Created();

        return result.toProblem();
    }
}
