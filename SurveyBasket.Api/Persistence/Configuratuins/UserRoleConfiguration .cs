using SurveyBasket.Api.Mapping.Consts;

namespace SurveyBasket.Api.Persistence.Configuratuins;

public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
    {

        builder.HasData ( new IdentityUserRole<string>
        {

            UserId = DefaultUsers.AdminId,
            RoleId = DefaultRules.AdminRuleId   



        } );
            
            
    }
}
