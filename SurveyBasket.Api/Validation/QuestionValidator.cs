using SurveyBasket.Api.Dtos.Questions;

namespace SurveyBasket.Api.Validation;

public class QuestionValidator :AbstractValidator<QuestionRequest>
{

    public QuestionValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty()
            .Length(3,1000);

        RuleFor(x => x.answers)
            .NotEmpty()
            .NotNull();

        RuleFor(x => x.answers)
            .Must(x => x.Count > 1)
            .WithMessage("Question should at least To Answers")
            .When(x => x.answers != null);


        RuleFor(x => x.answers)
           .Must(x => x.Distinct().Count() == x.Count)
           .WithMessage("Question can not Dubllicated  Answers with same question")
            .When(x => x.answers != null);
    }
}
