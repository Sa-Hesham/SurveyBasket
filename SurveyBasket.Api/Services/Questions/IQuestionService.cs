using SurveyBasket.Api.Dtos.Errors;
using SurveyBasket.Api.Dtos.Questions;

namespace SurveyBasket.Api.Services.Questions;

public interface IQuestionService
{
    Task<Result<QuestionResponse>> CreatQuestionaysnc( int pollId ,QuestionRequest request ,CancellationToken ct = default );

    Task<Result<IEnumerable<QuestionResponse>>> GetAllQuestionAsync(int pollId, CancellationToken ct = default);

    Task<Result<QuestionResponse>> GetById(int pollId, int qustionId ,CancellationToken ct = default);



    Task<Result> ToggleStatusAsync(int pollId, int qustionId, CancellationToken ct = default);


    Task<Result> UpdateAsync(int pollId, int id ,QuestionRequest request, CancellationToken ct = default);






}
