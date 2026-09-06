
using SurveyBasket.Api.Dtos.Questions;

namespace SurveyBasket.Api.Mapping;

public class Mappingconfig : IRegister
{
    public void Register(TypeAdapterConfig config) { 



        config.NewConfig<QuestionRequest,Question>()
            .Map(dst=>dst.Answers , src=>src.answers.Select(answer=>new Answer { Content = answer}));

       
    }
}
