namespace SurveyBasket.Api.Dtos.Errors;

public class QuestionError
{
    public static readonly Error QuestionDublicated = new ("Question.IsExis ", "Question Content found In same poll");
    public static readonly Error QuestionNotFound = new("Question.NotFound", "Question  are Not found ");
}
