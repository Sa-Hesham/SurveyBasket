using SurveyBasket.Api.Mapping.Consts;

namespace SurveyBasket.Api.Persistence.Configuratuins;

public class RoleCalimConfiguration : IEntityTypeConfiguration<IdentityRoleClaim<string>>
{
    public void Configure(EntityTypeBuilder<IdentityRoleClaim<string>> builder)
    {

        var RolePermission = Permissions.GEtAllPermissions();

        var identityroles = new List<IdentityRoleClaim<string>>();

        for (int i = 0; i < RolePermission.Count; i++)    
        {
            identityroles.Add(new IdentityRoleClaim<string>
            {
                Id = i + 1,
                ClaimType = Permissions.Type ,
                ClaimValue = RolePermission[i],
                RoleId = DefaultRules.AdminRuleId ,
                


            });
            
        


        }
        builder.HasData(identityroles);
            
            
    }
}
