namespace SurveyBasket.Api.Dtos.Vote;

public record VoteRequest(

    IEnumerable<VoteAnswerRequest> Answers
   

    );

