namespace SurveyBasket.Api.Validation;

public class PollRequestValidator :AbstractValidator<PollRequest>
{
    public PollRequestValidator()
    {
        RuleFor(p => p.Title)
            .NotEmpty()
            .Length(3, 100);


        RuleFor(p => p.Summary).NotEmpty()
            .Length(3, 1500);


        RuleFor(p => p.SatrtsAt)
           .NotEmpty()
           .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now));

        RuleFor(p => p.EndsAt)
           .NotEmpty();

        RuleFor(x => x)
            .Must(IsvalidDate)
            .WithName(nameof(PollRequest.EndsAt))
            .WithMessage("{PropertyName} should Be Greater than or equals Start Date   ");
    }
           

    private bool IsvalidDate(PollRequest value)
    {
        return value.EndsAt >= value.SatrtsAt;
    }
}
