namespace SurveyBasket.Api.Dtos.Errors;

public class QuestionError
{
    public static readonly Error QuestionDublicated = new ("Question.IsExis ", "Question Content found In same poll", StatusCodes.Status409Conflict);
    public static readonly Error QuestionNotFound = new("Question.NotFound", "Question  are Not found ", StatusCodes.Status404NotFound);

    public static readonly Error InvalideQuestin = new("Question.invalid", "InvalidQuestion", StatusCodes.Status400BadRequest);
}
