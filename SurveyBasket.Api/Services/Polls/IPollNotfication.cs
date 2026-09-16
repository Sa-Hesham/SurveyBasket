namespace SurveyBasket.Api.Services.Polls;

public interface IPollNotfication
{

    Task SendPollNotifcation(int? PollId);
}
