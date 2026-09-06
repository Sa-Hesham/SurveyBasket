using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.Api.Dtos.Errors;
using SurveyBasket.Api.Dtos.Questions;
using SurveyBasket.Api.Services.Questions;

namespace SurveyBasket.Api.Controllers;

[Route("api/Poll/{PollId}/[controller]")]
[ApiController]

public class QuestionController(IQuestionService questionService) : ControllerBase
{
    private readonly IQuestionService _questionService = questionService;

    [HttpPost]

    public async Task<IActionResult> AddQuestionasync([FromRoute] int pollid, [FromBody] QuestionRequest request, CancellationToken ct)
    {

        var result = await _questionService.CreatQuestionaysnc(pollid, request, ct);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return result.Error.Equals(QuestionError.QuestionDublicated)
            ? result.toProblem(StatusCodes.Status409Conflict)
            : result.toProblem(StatusCodes.Status404NotFound);

    }


    [HttpGet]

    public async Task<IActionResult> Getall([FromRoute] int pollid, CancellationToken ct)
    {
        var result = await _questionService.GetAllQuestionAsync(pollid, ct);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        return result.toProblem(StatusCodes.Status404NotFound);
    }



    [HttpGet("{id}")]

    public async Task<IActionResult> GetById([FromRoute] int pollid, [FromRoute] int id, CancellationToken ct)
    {
        var result = await _questionService.GetById(pollid, id, ct);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }


        return result.toProblem(StatusCodes.Status404NotFound);

    }


    [HttpPut("{Id}/ToggleStatus")] 

    public async Task<IActionResult>ToggleStatus([FromRoute] int pollid, [FromRoute] int id, CancellationToken ct)
    {
        var result = await _questionService.ToggleStatusAsync(pollid, id, ct);


        return result.IsSuccess ? NoContent() : result.toProblem(StatusCodes.Status404NotFound);


    }



    [HttpPut("{id}")]



    public async Task<IActionResult> UpdateQuestion ([FromRoute] int pollid, [FromRoute] int id, [FromBody]QuestionRequest request  ,CancellationToken ct)
    {
      var result =   await _questionService.UpdateAsync(pollid, id, request, ct);


        if (result.IsSuccess)
        {
            return NoContent();
        }

        return result.Error.Equals(QuestionError.QuestionDublicated)
            ? result.toProblem(StatusCodes.Status409Conflict)
            : result.toProblem(StatusCodes.Status404NotFound);

    }
}

