namespace SurveyBasket.Api.Validation;

public class TokenRequestValidator :AbstractValidator<TokenRequest>
{

    public TokenRequestValidator()
    {
        RuleFor(x=>x.token).NotEmpty(); 
        RuleFor(x=>x.Refreshtoken).NotEmpty(); 
    }
}
