namespace SurveyBasket.Api.Validation;

public class ResetPasswordRequestValidtor:AbstractValidator<ResetPasswordRequest>
{

    public ResetPasswordRequestValidtor()
    {
        RuleFor(x => x.NewPassword)
                  .NotEmpty()
                  .Matches(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9]).{8,}$")
                  .WithMessage("Password must contain at least 8 characters, one digit, one lowercase letter, one uppercase letter, and one non-alphanumeric character.");


        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Code).NotEmpty();
    }
    
}
