using SurveyBasket.Api.Dtos.Errors;

namespace SurveyBasket.Api.Services.Polls;

public interface IPollService
{

    Task<Result<IEnumerable<PollResponse>>> GetAllasync(CancellationToken ct =default);

    Task<Result<PollResponse>> GetByIdAsync (int id, CancellationToken ct = default);   

   Task<Result<PollResponse>> CreateAsync (Poll poll, CancellationToken ct = default);    


   Task< Result> UpdateAsync (int id,Poll poll,CancellationToken ct = default);
    
    Task< Result>  DeleteAsync (int id,CancellationToken ct = default);    

    Task<Result> TogglePublishSatausAsync ( int id , CancellationToken ct  = default);





}
