using Microsoft.AspNetCore.Authorization;

namespace SurveyBasket.Api.Dtos.Security.Filtters;

public class PermissionAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
{
    private readonly AuthorizationOptions _options ;

    public PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
    {
        _options = options.Value;
    }

    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        var policy = await  base.GetPolicyAsync(policyName);
        if (policy != null) 

                return policy;



        var permissionPolicy = new AuthorizationPolicyBuilder()
            .AddRequirements(new PermissionsRequirment(policyName))
            .Build();






        _options.AddPolicy(policyName, permissionPolicy);


        return permissionPolicy;
    }
}
