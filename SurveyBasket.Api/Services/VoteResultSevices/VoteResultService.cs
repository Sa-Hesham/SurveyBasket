using SurveyBasket.Api.Dtos.Questions;
using SurveyBasket.Api.Dtos.Vote;
using SurveyBasket.Api.Dtos.VoteResult;
using SurveyBasket.Api.Services.VoteService;

namespace SurveyBasket.Api.Services.VoteResultSevices;

public class VoteResultService(AppDbContext context) :  IVoteResultService 
{
    private readonly AppDbContext _context = context;

    public async Task<Result<PollVoteResultResponse>> GetVoteResultAsync(int PollId, CancellationToken ct = default)
    {

        var VoteResult = await _context.Polls
            .Where(p => p.Id == PollId)
            .Select(p => new PollVoteResultResponse(

                     p.Title,
                     p.Votes.Select(v=> new VoteResponse(

                   $"{v.User.FirstName} {v.User.LastName}",
                   v.SubmittedOn,
                     v.VoteAnswers.Select(a=>new QuestionAndAnswerResponse (
                       
                       a.Question.Content,
                       a.Answer.Content
                       
                       
                       ))




                    ))




                )).FirstOrDefaultAsync(ct);



        if (VoteResult is null)
            return Result.Failure<PollVoteResultResponse>(PollError.PollIsNotFound);

        return Result.Succes(VoteResult);
    }

    public async Task<Result<IEnumerable<VotesPerDayResponse>>> GetVotesPErDayAsync(int PollId, CancellationToken ct = default)
    {
        var PollIsExist = await _context.Polls.AnyAsync(x => x.Id == PollId, ct);

        if (!PollIsExist)
            return Result.Failure<IEnumerable<VotesPerDayResponse>>(PollError.PollINotDEleted);




        var votesPerDay = await _context.Votes
            .Where(v=>v.PollId==PollId)
            .GroupBy(v => new { Date = DateOnly.FromDateTime(v.SubmittedOn) })
            .Select(v => new VotesPerDayResponse( 
               
                v.Key.Date,
                v.Count()
                )).ToArrayAsync(ct);


        return Result.Succes<IEnumerable<VotesPerDayResponse>>(votesPerDay);
           
    }
}
