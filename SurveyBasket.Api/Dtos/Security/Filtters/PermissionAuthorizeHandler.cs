using Microsoft.AspNetCore.Authorization;
using SurveyBasket.Api.Mapping.Consts;

namespace SurveyBasket.Api.Dtos.Security.Filtters;

public class PermissionAuthorizeHandler : AuthorizationHandler<PermissionsRequirment>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionsRequirment requirement)
    {
        var user = context.User.Identity;

        if(user is null || !user.IsAuthenticated)
        {
            return;
        }

        var hasspermission = context.User.Claims.Any(x=>x.Value == requirement.Permission && x.Type == Permissions.Type);
        if (!hasspermission)
            return;


        context.Succeed(requirement);
        return;
        
    }
}
