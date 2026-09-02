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
            
            
    }
}
