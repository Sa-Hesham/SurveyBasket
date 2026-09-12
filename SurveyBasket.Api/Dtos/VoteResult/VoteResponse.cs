namespace SurveyBasket.Api.Dtos.VoteResult;

public record VoteResponse(

    string VoterName ,
    DateTime VoteDate , 
    IEnumerable<QuestionAndAnswerResponse> QuestionsWithAnswers



    );
