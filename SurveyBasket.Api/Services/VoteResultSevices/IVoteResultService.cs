using SurveyBasket.Api.Dtos.Vote;
using SurveyBasket.Api.Dtos.VoteResult;

namespace SurveyBasket.Api.Services.VoteResultSevices;

public interface IVoteResultService
{


    Task<Result<PollVoteResultResponse>> GetVoteResultAsync(int PollId, CancellationToken ct = default);
    Task<Result<IEnumerable<VotesPerDayResponse>>> GetVotesPErDayAsync(int PollId, CancellationToken ct = default);

    Task<Result<IEnumerable<VotesPerQuestionResponse>>> GetVotesPerQuestionAsync(int PollId, CancellationToken ct = default);
} 
