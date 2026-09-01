using SurveyBasket.Api.Persistence.Configuratuins;

namespace SurveyBasket.Api.Data;

public class AppDbContext :DbContext
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
