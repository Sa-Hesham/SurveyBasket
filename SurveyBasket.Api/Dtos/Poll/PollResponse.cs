namespace SurveyBasket.Api.Dtos.Poll;

public record PollResponse
{
    public int Id { get; init; }

    public string Title { get; init; } = string.Empty;


    public string Summary { get; init; } = string.Empty;
    public bool IsPublished { get; init; }

    public DateOnly SatrtsAt { get; init; }
    public DateOnly EndsAt { get; init; }
}

