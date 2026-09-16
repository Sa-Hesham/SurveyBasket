

namespace SurveyBasket.Api.Dtos.Errors;

public static class ResultExtentions
{


    public static ObjectResult toProblem (this Result result )
    {

       

        if (result.IsSuccess)
            throw new InvalidOperationException("Cannot convert success result to a problem");

        var problem = Results.Problem(statusCode: result.Error.statusCode);
        var problemDetails = problem.GetType().GetProperty(nameof(ProblemDetails))!.GetValue(problem) as ProblemDetails;


        problemDetails!.Extensions = new Dictionary<string, object?>
            {
                {
                    "errors", new[]
                    {
                        result.Error.Code,
                        result.Error.Message
                    }
               }



            };
       

            
        return new ObjectResult ( problemDetails );     

    } 


}
