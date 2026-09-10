using SurveyBasket.Api.Dtos.Vote;

namespace SurveyBasket.Api.Validation;

public class AnswerRequestValidator :AbstractValidator<VoteAnswerRequest>
{
    public AnswerRequestValidator()
    {
        RuleFor(x => x.QuestionId)
            .GreaterThan(0);

        RuleFor(x => x.AnswerId)
            .GreaterThan(0);
    }
}
