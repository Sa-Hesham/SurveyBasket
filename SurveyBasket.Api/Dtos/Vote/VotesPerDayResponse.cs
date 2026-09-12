namespace SurveyBasket.Api.Dtos.Vote;

public record VotesPerDayResponse(
    
    DateOnly Date ,
    int NumberOfVotes 
    
    );
