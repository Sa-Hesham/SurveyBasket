using SurveyBasket.Api.Dtos.Security.Roles;

namespace SurveyBasket.Api.Validation;

public class RoleRequestValidator :AbstractValidator<RoleRequest>
{
    public RoleRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(3,200);
            
        RuleFor(x=>x.Permissions)
            .NotEmpty()
            .NotNull();

        RuleFor(x => x.Permissions)
            .Must(x => x.Distinct().Count() == x.Count())
            .WithMessage("you Can not add the smae duplicated value ")
            .When(x => x.Permissions != null);

    }
}
