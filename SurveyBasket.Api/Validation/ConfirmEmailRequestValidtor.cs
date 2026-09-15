namespace SurveyBasket.Api.Validation;

public class ConfirmEmailRequestValidtor :AbstractValidator<ConfirmEmailRequest>
{
    public ConfirmEmailRequestValidtor()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();


        RuleFor(x => x.Code)
          .NotEmpty();
    }
}
