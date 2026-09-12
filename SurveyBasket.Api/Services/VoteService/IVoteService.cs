using SurveyBasket.Api.Dtos.Errors;
using SurveyBasket.Api.Dtos.Vote;

namespace SurveyBasket.Api.Services.VoteService;

public interface IVoteService
{
    Task<Result>AddAsync ( int Pollid , string userId ,VoteRequest request ,CancellationToken ct = default);    
}
