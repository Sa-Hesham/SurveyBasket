namespace SurveyBasket.Api.Validation;

public class RegisterRequestValitdator :AbstractValidator<RegisterRequest>
{
    public RegisterRequestValitdator()
    {

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();


        RuleFor(x => x.FirstName)
            .NotEmpty()
            .Length(3,100);
        

        RuleFor(x=>x.LastName)
            .NotEmpty()
            .Length(3, 100);

        RuleFor(x => x.Password)
            .NotEmpty()
            .Matches(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9]).{8,}$")
            .WithMessage("Password must contain at least 8 characters, one digit, one lowercase letter, one uppercase letter, and one non-alphanumeric character.");




    }
}
