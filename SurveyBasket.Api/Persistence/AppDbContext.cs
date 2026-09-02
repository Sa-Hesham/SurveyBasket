
namespace SurveyBasket.Api.Data;

public class AppDbContext :IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> Contetx ):base( Contetx )
    {
       
    }

    public DbSet<Poll> Polls { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PollConfiguration).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    
}
