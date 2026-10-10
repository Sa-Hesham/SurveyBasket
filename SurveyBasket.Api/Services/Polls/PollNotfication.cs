using Hangfire;
using Microsoft.AspNetCore.Identity.UI.Services;
using SurveyBasket.Api.Helper;

namespace SurveyBasket.Api.Services.Polls;

public class PollNotfication(AppDbContext context
    , UserManager<ApplicationUser> user  
    , IHttpContextAccessor httpContext ,IEmailSender email) : IPollNotfication
{
    private readonly AppDbContext _context = context;
    private readonly UserManager<ApplicationUser> _user = user;
    private readonly IHttpContextAccessor _httpContext = httpContext;
    private readonly IEmailSender _email = email;

    public async Task SendPollNotifcation(int? PollId)
    {
        IEnumerable<Poll> polls = []; 


        if(PollId .HasValue) 
        {
            var poll = await _context.Polls.SingleOrDefaultAsync(x=>x.Id == PollId.Value && x.IsPublished );
            polls = [poll!];
        }
        else
        {
            polls = await _context.Polls.Where(x => x.IsPublished && x.SatrtsAt == DateOnly.FromDateTime(DateTime.UtcNow)).ToListAsync();
        }


      


        // getallusers 

        var users = await _user.Users.ToListAsync();


        var origin = _httpContext.HttpContext?.Request.Headers.Origin;
        // generate Body message 

        foreach (var poll in polls) {

            foreach (var user in users) {


           var placeHolders = new Dictionary<string, string>
        {
            { "{{name}}" ,user.FirstName },
            { "{{pollTill}}" ,poll.Title },
            { "{{endDate}}" ,poll.EndsAt.ToString() },
            { "{{url}}" ,$"{origin}/Polls/start/{poll.Id}" },
           


        };

                var body = EmailBodyHelper.GenerateEmailBody("PollNotification", placeHolders);

                await _email.SendEmailAsync(user.Email!, $"Survey Basket : new Poll {poll.Title} ", body);
            }
        
        
        }

    }
}
