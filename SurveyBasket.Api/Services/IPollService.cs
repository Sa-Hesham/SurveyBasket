namespace SurveyBasket.Api.Services;

public interface IPollService
{

    Task<IEnumerable<Poll>> GetAllasync(CancellationToken ct =default);

    Task<Poll?> GetByIdAsync (int id, CancellationToken ct = default);   

   Task< Poll> CreateAsync (Poll poll, CancellationToken ct = default);    


   Task< bool> UpdateAsync (int id,Poll poll,CancellationToken ct = default);
    
    Task< bool>  DeleteAsync (int id,CancellationToken ct = default);    

    Task<bool> TogglePublishSatausAsync ( int id , CancellationToken ct  = default);





}
