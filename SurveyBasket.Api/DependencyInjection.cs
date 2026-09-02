
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SurveyBasket.Api.Data;
using SurveyBasket.Api.Services.Authentications;
using SurveyBasket.Api.Services.Polls;
using System.Text;

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

        Services.AuthConfiguration(_configuration);

        Services.DataBase(_configuration);

        return Services;    
    }


   private   static IServiceCollection AddMapster(this IServiceCollection Services)
    {
        var mapping = TypeAdapterConfig.GlobalSettings;
        mapping.Scan(Assembly.GetExecutingAssembly());
        Services.AddSingleton<IMapper>(new Mapper(mapping));
        return Services;
    }
    private static IServiceCollection AddValidion(this IServiceCollection Services)
    {
        Services.AddValidatorsFromAssemblyContaining<Program>();
        Services.AddFluentValidationAutoValidation();

        return Services;    

    }
    private static IServiceCollection AddSwagger(this IServiceCollection Services)
    {
        Services.AddEndpointsApiExplorer();
        Services.AddSwaggerGen();
        return Services;
    }

    private static IServiceCollection DomainServices (this IServiceCollection Services)
    {
        Services.AddScoped<IPollService, Pollservice>();
        Services.AddScoped<IAuthService,AuthService>(); 
        return Services;
    }


    private static IServiceCollection DataBase(this IServiceCollection Services ,IConfiguration configuration)
    {
        Services.AddDbContext<AppDbContext>(option =>
        {
            option.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

        });

       Services.AddIdentity<ApplicationUser,IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>();   
            
        return Services;
    }


    private static IServiceCollection  AuthConfiguration(this IServiceCollection Services, IConfiguration configuration)
    {

        //Services.Configure<JWTSetting>(configuration.GetSection(JWTSetting.Name));
        Services.AddOptions<JWTSetting>()
            .BindConfiguration(JWTSetting.Name)
            .ValidateDataAnnotations()
            .ValidateOnStart(); 

        var jwtSettings = configuration.GetSection(JWTSetting.Name).Get<JWTSetting>();  
        Services.AddAuthentication(option =>
        {
            option.DefaultAuthenticateScheme= JwtBearerDefaults.AuthenticationScheme;  
            option.DefaultChallengeScheme= JwtBearerDefaults.AuthenticationScheme;

        }).AddJwtBearer(o =>
        {
            o.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings?.Key!)),
                ValidIssuer = jwtSettings?.Issuer,
                ValidAudience = jwtSettings?.Audience,

            };
        });
       


        return Services;
    } 
}
