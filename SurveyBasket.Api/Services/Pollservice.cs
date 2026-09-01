using SurveyBasket.Api.Data;

namespace SurveyBasket.Api.Services;

public class Pollservice(AppDbContext _context) : IPollService
{
    public async Task<Poll> CreateAsync(Poll poll, CancellationToken ct = default)
    {   
        await _context.Polls.AddAsync(poll,ct);    

         await _context.SaveChangesAsync(ct); 

        return poll;    
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var poll = await GetByIdAsync(id ,ct);
        if (poll == null)
            return false;
         _context.Remove(poll);
        return await _context.SaveChangesAsync(ct) > 0;
    }

    public async Task<IEnumerable<Poll>> GetAllasync(CancellationToken ct = default) => await _context.Polls.AsNoTracking().ToListAsync(ct);   
    

    public async Task<Poll?> GetByIdAsync(int id, CancellationToken ct = default)
    {
      return await _context.Polls.SingleOrDefaultAsync(x => x.Id == id, ct) ;      
    }

    public async Task<bool> TogglePublishSatausAsync(int id, CancellationToken ct = default)
    {
       var poll = await GetByIdAsync(id, ct) ;
        if(poll == null) return false;
        poll.IsPublished = !poll.IsPublished;   
       return await  _context.SaveChangesAsync(ct)>0;
    }

    public async Task<bool> UpdateAsync(int id, Poll poll, CancellationToken ct = default)
    {
        var result = await GetByIdAsync(id, ct)  ;
        if (result == null) 
          return  false;
        result.Title = poll.Title;  
        result.Summary = poll.Summary;  
        result.IsPublished = poll.IsPublished;  
        result.SatrtsAt = poll.SatrtsAt;    
        result.EndsAt = poll.EndsAt;    
     return  await _context.SaveChangesAsync(ct)>0;  
    }
}
