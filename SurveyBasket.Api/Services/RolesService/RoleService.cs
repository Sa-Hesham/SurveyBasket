using SurveyBasket.Api.Dtos.Security.Roles;
using SurveyBasket.Api.Mapping.Consts;

namespace SurveyBasket.Api.Services.RolesService;

public class RoleService(RoleManager<ApplicationRole> roleManager  , AppDbContext context ) : IRoleService
{
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
    private readonly AppDbContext _context = context;

    public async Task<IEnumerable<RoleResponse>> GetAll(bool ? includeDisabled =false,CancellationToken ct = default!)
    {
       return await _roleManager.Roles
            .Where(x => !x.IsDefault &&(!x.IsDeleted || includeDisabled.HasValue && includeDisabled.Value))
            .ProjectToType<RoleResponse>()
            .ToListAsync(ct);

    }


    public async Task<Result<RoleDetailsReponse>> getByidAsync(string id)
    {
        if (await _roleManager.FindByIdAsync(id) is not { } role)
            return Result.Failure<RoleDetailsReponse>(new("Role.notFound", $"Role with Id {id} no found", StatusCodes.Status404NotFound));

        var permissions = await _roleManager.GetClaimsAsync(role);
        var response = new RoleDetailsReponse(role.Id, role.Name!, role.IsDeleted, permissions.Select(x => x.Value));
        return Result.Succes(response);
    }
    public async Task<Result<RoleDetailsReponse>> AddAsync( RoleRequest request )
    {
        var roleisexist = await _roleManager.RoleExistsAsync(request.Name);
        if(roleisexist) 
            return Result.Failure<RoleDetailsReponse>(new("Role.duplicated", $"Role is excist", StatusCodes.Status409Conflict));

        var allowPermissions = request.Permissions.Except(Permissions.GEtAllPermissions()).Any();
        if (allowPermissions)
        {
            return Result.Failure<RoleDetailsReponse>(new("Role.InvalidPermissions", $"Invalid Permissions", StatusCodes.Status400BadRequest));
        }
        var role = new ApplicationRole
        {
            Name = request.Name,
            ConcurrencyStamp = Guid.NewGuid().ToString(),

        };

        var result = await _roleManager.CreateAsync(role);

        if (result.Succeeded)
        {
            var permissions = request.Permissions.Select(x=>new IdentityRoleClaim<string>
            {
                ClaimType = Permissions.Type ,
                ClaimValue = x ,
                RoleId = role.Id,   
                
            });

            await _context.AddRangeAsync(permissions);
            await _context.SaveChangesAsync();
            var response = new RoleDetailsReponse(role.Id, role.Name!, role.IsDeleted, request.Permissions);
            return Result.Succes(response);

        }
       
        
            var errors = result.Errors.First();
            return Result.Failure<RoleDetailsReponse>(new(errors.Code, errors.Description, StatusCodes.Status400BadRequest));
       
    }



    public async Task<Result> UpdateAsync(string id , RoleRequest request)
    {
        var roleisexist = await _roleManager.Roles.AnyAsync(x=>x.Name == request.Name && x.Id != id);
        if (roleisexist)
            return Result.Failure<RoleDetailsReponse>(new("Role.duplicated", $"Role is excist", StatusCodes.Status409Conflict));


        if (await _roleManager.FindByIdAsync(id) is not { } role)
            return Result.Failure<RoleDetailsReponse>(new("Role.notFound", $"Role with Id {id} no found", StatusCodes.Status404NotFound));

        var allowPermissions = request.Permissions.Except(Permissions.GEtAllPermissions()).Any();
        if (allowPermissions)
            return Result.Failure<RoleDetailsReponse>(new("Role.InvalidPermissions", $"Invalid Permissions", StatusCodes.Status400BadRequest));
      
        role.Name = request.Name;   

        var result = await _roleManager.UpdateAsync(role);   

    

        if (result.Succeeded)
        {
            var rolePermssions = await _context.RoleClaims
                .Where(x => x.RoleId == id)
                .Select(x => x.ClaimValue).ToListAsync();


            var newPermission = request.Permissions.Except(rolePermssions)
                .Select(x => new IdentityRoleClaim<string>
            {
                ClaimType = Permissions.Type,
                ClaimValue = x,
                RoleId = role.Id,

            });


            var removedPermissions = rolePermssions.Except(request.Permissions);

            await _context.RoleClaims
                .Where(x=>x.RoleId==id && removedPermissions.Contains(x.ClaimValue))
                .ExecuteDeleteAsync();

            await _context.AddRangeAsync(newPermission);
             await _context.SaveChangesAsync();

            return Result.Success();
        }



        var errors = result.Errors.First();
        return Result.Failure(new(errors.Code, errors.Description, StatusCodes.Status400BadRequest));

    }



    public async Task<Result> ToggleStatusAsync(string id)
    {
        if (await _roleManager.FindByIdAsync(id) is not { } role)
            return Result.Failure<RoleDetailsReponse>(new("Role.notFound", $"Role with Id {id} no found", StatusCodes.Status404NotFound));

        role.IsDeleted = !role.IsDeleted;

        await _roleManager.UpdateAsync(role);

        return Result.Success();
    }

}
