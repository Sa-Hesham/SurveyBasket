namespace SurveyBasket.Api.Dtos.Poll;

public record PollRequest
{
    public string Title { get; init; } = string.Empty;


    public string Summary { get; init; } = string.Empty;
    

    public DateOnly SatrtsAt { get; init; }
    public DateOnly EndsAt { get; init; }
}
