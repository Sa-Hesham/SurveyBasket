using Microsoft.AspNetCore.Authorization;

namespace SurveyBasket.Api.Dtos.Security.Filtters;

public class PermissionsRequirment(string permission) : IAuthorizationRequirement
{
    public string Permission { get; } = permission;
}
