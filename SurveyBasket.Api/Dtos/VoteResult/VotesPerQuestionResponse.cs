namespace SurveyBasket.Api.Dtos.VoteResult;

public record VotesPerQuestionResponse(
    
    string Question,

    IEnumerable<VotesPerAnswerResponse> SelectedAnswer 
    
    
    
    );
