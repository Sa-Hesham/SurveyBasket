namespace SurveyBasket.Api.Dtos.VoteResult;

public record PollVoteResultResponse(
    string Title,
    IEnumerable<VoteResponse> Votes


    
    );

