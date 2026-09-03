namespace SurveyBasket.Api.Dtos.Errors;

public class PollError
{
    public static readonly Error PollNotSaved= new ("poll.NotSaved", "Saved Failed ");
    public static readonly Error PollIsNotFound= new ("poll.NotFound", "Poll is Not Found");
    public static readonly Error PollINotDEleted = new ("poll.NotDeleted", "Poll Can not Be Deleted");
}
