
using System.Security.Claims;

namespace SurveyBasket.Api.Data;

public class AppDbContext :IdentityDbContext<ApplicationUser>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AppDbContext(DbContextOptions<AppDbContext> context ,IHttpContextAccessor httpContextAccessor):base( context)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public DbSet<Poll> Polls { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PollConfiguration).Assembly);

        base.OnModelCreating(modelBuilder);
    }

  
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default )
    {

        var CurrentuserId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var entries = ChangeTracker.Entries<BaseEntity>();
        foreach (var entityentry in entries) { 
        
        if(entityentry.State == EntityState.Added ) 
         {
                entityentry.Property(x => x.CreatedById).CurrentValue = CurrentuserId!;
        
        } else if (entityentry.State== EntityState.Modified)
            {
                entityentry.Property(x=>x.UpdatedById).CurrentValue = CurrentuserId!;
                entityentry.Property(x => x.UpdatedOn).CurrentValue = DateTime.UtcNow;
            }
       
        
    }
        return base.SaveChangesAsync(cancellationToken);


    }
}