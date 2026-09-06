

namespace SurveyBasket.Api.Dtos.Errors;

public static class ResultExtentions
{


    public static ObjectResult toProblem (this Result result , int statusCode  )
    {

        if (result.IsSuccess)
            throw new InvalidOperationException("Cannot Convert Success to problem");
        var results = Results.Problem(statusCode: statusCode);
        var problemDetails = result.GetType().GetProperty(nameof(ProblemDetails))!.GetValue(results) as ProblemDetails;

        problemDetails!.Extensions = new Dictionary<string, object?>
            {
                {


                    "errors", new List<Error>
                    {
                        result.Error
                    }
                }



            };
       

            
        return new ObjectResult ( problemDetails );     

    } 


}
