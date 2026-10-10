using Hangfire;
using SurveyBasket.Api.Data;
using SurveyBasket.Api.Dtos.Errors;

namespace SurveyBasket.Api.Services.Polls;

public class Pollservice(AppDbContext _context ,IPollNotfication notfication) : IPollService
{
    private readonly IPollNotfication notfication = notfication;

    public async Task<Result<PollResponse>> CreateAsync(Poll poll, CancellationToken ct = default)
    {   
        var IsExist= await _context.Polls.AnyAsync(p=>p.Title == poll.Title,ct)   ;
        if (IsExist) return Result.Failure<PollResponse>(new("poll.Duplicate","poll Is Exist title must be unique ", StatusCodes.Status409Conflict));
        await _context.Polls.AddAsync(poll,ct);    

        var Issaved = await _context.SaveChangesAsync(ct)>0;
        if (!Issaved)
            return Result.Failure<PollResponse>(PollError.PollNotSaved);

        return Result.Succes(poll.Adapt<PollResponse>());
    }

    public async Task<Result> DeleteAsync(int id, CancellationToken ct = default)
    {
        var poll = await _context.Polls.SingleOrDefaultAsync(x => x.Id == id, ct); ;
        if (poll == null)
            return Result.Failure(PollError.PollIsNotFound);
         _context.Remove(poll);
        var IsDeleted =  await _context.SaveChangesAsync(ct) > 0;

        return IsDeleted ? Result.Success(): Result.Failure(PollError.PollINotDEleted);  
    }

    public async Task<Result<IEnumerable<PollResponse>>> GetAllasync(CancellationToken ct = default) { 


       var Polls =  await _context.Polls.AsNoTracking().ToListAsync(ct); 
    
        return Polls is null ? Result.Failure<IEnumerable<PollResponse>>(PollError.PollIsNotFound) :
            Result.Succes(Polls.Adapt<IEnumerable<PollResponse>>());

    
    }


     public async Task<Result<IEnumerable<PollResponse>>> GetCurrentAsync(CancellationToken ct = default)
    {

        var polls =await  _context.Polls
          .Where(x => x.IsPublished == true && x.SatrtsAt <= DateOnly.FromDateTime(DateTime.UtcNow) && x.EndsAt >= DateOnly.FromDateTime(DateTime.UtcNow))
          .AsNoTracking()
          .ProjectToType<PollResponse>()
          .ToListAsync(ct);

        if (polls is null)
            return Result.Failure<IEnumerable<PollResponse>>(PollError.PollIsNotFound);

        return Result.Succes<IEnumerable<PollResponse>>(polls);
    }


    public async Task<Result<PollResponse>> GetByIdAsync(int id, CancellationToken ct = default)
    {
      var result =  await _context.Polls.SingleOrDefaultAsync(x => x.Id == id, ct) ;  
        
        return result is null ? Result.Failure<PollResponse>(PollError.PollIsNotFound) :
            Result.Succes(result.Adapt<PollResponse>());
    }

    public async Task<Result> TogglePublishSatausAsync(int id, CancellationToken ct = default) { 
    
        var poll = await _context.Polls.SingleOrDefaultAsync(x => x.Id == id, ct); 

        if(poll == null) return Result.Failure(PollError.PollIsNotFound);
        poll.IsPublished = !poll.IsPublished;
        BackgroundJob.Enqueue(() => notfication.SendPollNotifcation(poll.Id));
       return await  _context.SaveChangesAsync(ct)>0 ?Result.Success() : Result.Failure(new("Poll.IsBulished  " ,"IspublishedNotToggeled" , StatusCodes.Status400BadRequest)) ;
    }

    public async Task<Result> UpdateAsync(int id, Poll poll, CancellationToken ct = default)
    {
        var IsExist = await _context.Polls.AnyAsync(p => p.Title == poll.Title && p.Id != id, ct);

        if (IsExist) return Result.Failure<PollResponse>(new("poll.Duplicate", "poll Is Exist title must be unique ", StatusCodes.Status409Conflict));

        var result =  await _context.Polls.SingleOrDefaultAsync(x => x.Id == id, ct)  ;
        if (result == null) 
          return Result.Failure(PollError.PollIsNotFound);
        result.Title = poll.Title;  
        result.Summary = poll.Summary;  
        result.IsPublished = poll.IsPublished;  
        result.SatrtsAt = poll.SatrtsAt;    
        result.EndsAt = poll.EndsAt;    
     return  await _context.SaveChangesAsync(ct)>0?
            Result.Success() : 
            Result.Failure(new("Poll.Update  ", "Poll not Updated", StatusCodes.Status400BadRequest));
    }

   
}
