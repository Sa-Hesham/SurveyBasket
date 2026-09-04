using Microsoft.AspNetCore.Diagnostics;

namespace SurveyBasket.Api.Dtos.Errors;

public class GlobalExceptionHandling(IProblemDetailsService problemDetailsService, ILogger<GlobalExceptionHandling> logger) : IExceptionHandler
{
    private readonly IProblemDetailsService _problemDetailsService = problemDetailsService;
    private readonly ILogger<GlobalExceptionHandling> _logger = logger;

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError("Some thing Went Error {Message}", exception.Message);



        httpContext.Response.StatusCode = exception switch
        {
            ValidationException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };


           var Problemdeatils =  new ProblemDetailsContext
        {

            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Type = exception.GetType().Name,
                Title = "Error has occured",
                Detail = exception.Message,
                Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
            }


        };


        return await _problemDetailsService.TryWriteAsync(Problemdeatils);

    }
}
