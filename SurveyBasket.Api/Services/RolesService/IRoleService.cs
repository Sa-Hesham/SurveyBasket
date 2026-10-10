using SurveyBasket.Api.Dtos.Security.Roles;

namespace SurveyBasket.Api.Services.RolesService;

public interface IRoleService
{
    Task<IEnumerable<RoleResponse>> GetAll(bool? includeDisabled = false, CancellationToken ct = default!);
    Task<Result<RoleDetailsReponse>> getByidAsync(string id);
    Task<Result<RoleDetailsReponse>> AddAsync(RoleRequest request);
    Task<Result> UpdateAsync(string id, RoleRequest request);
    Task<Result> ToggleStatusAsync(string id);
}
