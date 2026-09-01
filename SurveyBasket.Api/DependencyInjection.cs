
using Microsoft.EntityFrameworkCore;
using SurveyBasket.Api.Data;

public static class DependencyInjection
{
    public static IServiceCollection Dependcies (this IServiceCollection Services, IConfiguration _configuration)
    {

        Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

        Services.AddSwagger();
       
        //mapster
        Services.AddMapster();
        //fluent Vlaodation
        Services.AddValidion();


        Services.DomainServices();

        Services.DataBase(_configuration);

        return Services;    
    }


    public static IServiceCollection AddMapster(this IServiceCollection Services)
    {
        var mapping = TypeAdapterConfig.GlobalSettings;
        mapping.Scan(Assembly.GetExecutingAssembly());
        Services.AddSingleton<IMapper>(new Mapper(mapping));
        return Services;
    }
    public static IServiceCollection AddValidion(this IServiceCollection Services)
    {
        Services.AddValidatorsFromAssemblyContaining<Program>();
        Services.AddFluentValidationAutoValidation();

        return Services;    

    }
    public static IServiceCollection AddSwagger(this IServiceCollection Services)
    {
        Services.AddEndpointsApiExplorer();
        Services.AddSwaggerGen();
        return Services;
    }

    public static IServiceCollection DomainServices (this IServiceCollection Services)
    {
        Services.AddScoped<IPollService, Pollservice>();
        return Services;
    }


    public static IServiceCollection DataBase(this IServiceCollection Services ,IConfiguration configuration)
    {
        Services.AddDbContext<AppDbContext>(option =>
        {
            option.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

        });

        return Services;
    }
}
