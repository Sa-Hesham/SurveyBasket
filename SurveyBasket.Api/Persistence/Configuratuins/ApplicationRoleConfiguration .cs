using SurveyBasket.Api.Mapping.Consts;

namespace SurveyBasket.Api.Persistence.Configuratuins;

public class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        builder.HasData([


            new ApplicationRole{
                Id = DefaultRules.AdminRuleId,
                Name = DefaultRules.AdminRuleName,
                ConcurrencyStamp = DefaultRules.AdminConcurencyStamp,
                NormalizedName = DefaultRules.AdminRuleName.ToUpper()

            },
             new ApplicationRole{
                Id = DefaultRules.MEmbereId,
                Name = DefaultRules.MemberRuleName,
                ConcurrencyStamp = DefaultRules.MemberConcurencyStamp,
                NormalizedName = DefaultRules.MemberRuleName.ToUpper(),
                IsDefault = true    
                

            }


            ]);
            
            
    }
}
