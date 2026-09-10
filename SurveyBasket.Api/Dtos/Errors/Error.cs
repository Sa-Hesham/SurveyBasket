namespace SurveyBasket.Api.Dtos.Errors;

public record Error (string Code , string Message ,int ? statusCode)
{
   public static readonly Error None = new(string.Empty,string.Empty ,null);

}
