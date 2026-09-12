namespace SurveyBasket.Api.Dtos.Questions;

public record QuestionResponse(

    int Id ,
    string Content,
    IEnumerable<AnswerResponse> Answers

    );

