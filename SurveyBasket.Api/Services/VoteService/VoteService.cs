using SurveyBasket.Api.Dtos.Errors;
using SurveyBasket.Api.Dtos.Vote;

namespace SurveyBasket.Api.Services.VoteService;

public class VoteService(AppDbContext context) : IVoteService
{
    private readonly AppDbContext _context = context;

    public async Task<Result> AddAsync(int pollId, string userId, VoteRequest request, CancellationToken ct = default)
    {

        var hasVote = await _context.Votes.AnyAsync(v=>v.PollId==pollId && v.UserId ==userId,ct);
        if (hasVote)
        {
            return Result.Failure(UserError.UserDublicated);
        }


        var IexistPoll = await _context.Polls.AnyAsync(p => p.Id == pollId && p.IsPublished && p.SatrtsAt <= DateOnly.FromDateTime(DateTime.UtcNow)
        && p.EndsAt >= DateOnly.FromDateTime(DateTime.UtcNow));



        if (!IexistPoll) 
        {
            return Result.Failure(PollError.PollIsNotFound);
        
        }

        //cheak if questionRequst Id == Database QuestionsId

        var QuestionIsExit = await _context.questions
            .Where(x=>x.PollId==pollId && x.IsActive)
            .Select(x=>x.Id).ToListAsync(ct);


        if (!request.Answers.Select(a => a.QuestionId).SequenceEqual(QuestionIsExit))
        {
            return Result.Failure(QuestionError.InvalideQuestin);
        }
        var Vote = new Vote()
        {
            PollId = pollId,
            UserId = userId,
            VoteAnswers = request.Answers.Select(a => new VoteAnswer
            {
                QuestionId = a.QuestionId,
                AnswerId = a.AnswerId,

            }).ToList(),    


        };


        await _context.Votes.AddAsync(Vote ,ct);
        await _context.SaveChangesAsync(ct);

        return Result.Success();




        
    }
}
