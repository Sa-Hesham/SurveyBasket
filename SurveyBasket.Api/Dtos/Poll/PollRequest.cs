namespace SurveyBasket.Api.Dtos.Poll;

public record PollRequest
{
    public string Title { get; init; } = string.Empty;


    public string Summary { get; init; } = string.Empty;
    public bool IsPublished { get; init; }

    public DateOnly SatrtsAt { get; init; }
    public DateOnly EndsAt { get; init; }
}
