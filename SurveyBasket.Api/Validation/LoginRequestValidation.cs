using Microsoft.AspNetCore.Identity.Data;

namespace SurveyBasket.Api.Validation;

public class LoginRequestValidation : AbstractValidator<LoginRequestDto>
{
    public LoginRequestValidation()
    {
        RuleFor(l => l.Email).
            NotEmpty()
            .EmailAddress();


        RuleFor(l => l.Password).NotEmpty();
    }
}
