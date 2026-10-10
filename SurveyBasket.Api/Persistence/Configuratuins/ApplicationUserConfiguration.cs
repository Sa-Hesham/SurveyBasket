using SurveyBasket.Api.Mapping.Consts;

namespace SurveyBasket.Api.Persistence.Configuratuins;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(u => u.FirstName).HasMaxLength(100);
        builder.Property(u => u.LastName).HasMaxLength(100);
        builder.OwnsMany(u => u.RefreshTokens, r =>
        {
            r.ToTable("RefershTokens");
            r.WithOwner().HasForeignKey("UserId");
           

        });
      

        builder.HasData(new ApplicationUser
        {
            Id = DefaultUsers.AdminId,
            Email = DefaultUsers.AdminEmail,
            FirstName = "Admin",
            LastName = "SurveyBasket",
            UserName = DefaultUsers.AdminEmail,
            NormalizedUserName = DefaultUsers.AdminEmail.ToUpper(),
            NormalizedEmail = DefaultUsers.AdminEmail.ToUpper(),
            SecurityStamp = DefaultUsers.AdminSecurityStamp,
            ConcurrencyStamp = DefaultUsers.AdminConcurencyStamp,
            EmailConfirmed = true,
            PasswordHash =DefaultUsers.AdminPasswordHash,


        });
    }
}
