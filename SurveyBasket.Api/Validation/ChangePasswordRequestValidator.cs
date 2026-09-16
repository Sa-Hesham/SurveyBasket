namespace SurveyBasket.Api.Validation;

public class ChangePasswordRequestValidator :AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .NotEmpty();
           

        RuleFor(x => x.NewPassword)
                   .NotEmpty()
                   .Matches(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9]).{8,}$")
                   .WithMessage("Password must contain at least 8 characters, one digit, one lowercase letter, one uppercase letter, and one non-alphanumeric character.")
                   .NotEqual(x=>x.CurrentPassword)
                   .WithMessage("invalid password the new Password Should not Equal the old Password");




    }
}
