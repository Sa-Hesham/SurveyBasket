using Hangfire;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using SurveyBasket.Api.Helper;
using SurveyBasket.Api.Mapping.Consts;
using System.Security.Cryptography;

namespace SurveyBasket.Api.Services.Authentications;

public class AuthService(UserManager<ApplicationUser> _user,
    SignInManager<ApplicationUser>signInManager ,
    IJwtProvider _JwtProvider 
    ,IEmailSender email,
    IHttpContextAccessor httpcontext, 
    AppDbContext context
    ,ILogger<AuthService> logger ) : IAuthService
{
    private readonly int Expiretiontokendays = 5;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly IEmailSender _email = email;
    private readonly IHttpContextAccessor _httpcontext = httpcontext;
    private readonly AppDbContext _context = context;
    private readonly ILogger<AuthService> _logger = logger;

    public async Task<Result<UserResponse>> LoginAsync(string email, string password, CancellationToken ct = default)
    {
       
        if (await _user.FindByEmailAsync(email) is not { } user) {


            return Result.Failure<UserResponse>(UserError.Error);
        
        }


       

        var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
        if (result.Succeeded)
        {
            var (roles, permissions) = await GeTUSerRolesAndPermisions(user, ct);
            var (token, expries) = _JwtProvider.GenerateToken(user,roles,permissions);
            var Refershtoken = GenrateRefreshtoken();
            var RefreshtokenExpiration = DateTime.UtcNow.AddDays(Expiretiontokendays);
            user.RefreshTokens.Add(new RefreshToken
            {
                Token = Refershtoken,
                ExpiresOn = RefreshtokenExpiration,

            });
            await _user.UpdateAsync(user);

            var userResponse = new UserResponse(user.Id, user.Email!, user.FirstName, user.LastName, token, expries, Refershtoken, RefreshtokenExpiration);


            return Result.Succes(userResponse);


        }


        return Result.Failure<UserResponse>( result.IsNotAllowed? UserError.EmailNoconfirmed :UserError.Error);


    }

    public async Task<Result<UserResponse>> GetRefreshTokenAysnc(string token, string refreshtoken, CancellationToken ct = default)
    {
       var userId =  _JwtProvider.ValidateToken(token);  
        if(userId is null)
        {
            return Result.Failure<UserResponse>(UserError.Error);
        }
        // find user 
         var user = await _user.FindByIdAsync(userId);   
        if (user == null) 
        {
            return Result.Failure<UserResponse>(UserError.Error);

        }
        // find if user has token == Refreshtoken request
         var userRefreshToken = user.RefreshTokens.SingleOrDefault(x=>x.Token == refreshtoken && x.IsActive ) ;
        if (userRefreshToken == null) {

           return Result.Failure<UserResponse>(new("Refreshtoken.Error","Is not Activ / Expired ,or not Found",StatusCodes.Status401Unauthorized));
        
        }

        // revoke refrsh token 
        userRefreshToken.RevokedOn = DateTime.UtcNow;
        var (roles, permissions) = await GeTUSerRolesAndPermisions(user, ct);
        var ( newtoken, ExpireIn) = _JwtProvider.GenerateToken(user, roles, permissions);
        var newRefreshtoken = GenrateRefreshtoken();

        var Expireon = DateTime.UtcNow.AddDays(Expiretiontokendays);

        user.RefreshTokens.Add(new RefreshToken{
            Token = newRefreshtoken,
            ExpiresOn = Expireon,    
        });
        await _user.UpdateAsync(user);

        var userResponse =   new UserResponse(user.Id, user.Email!, user.FirstName, user.LastName, newtoken, ExpireIn, newRefreshtoken, Expireon);

        return Result.Succes(userResponse);
    }
    public async Task<Result> revokefreshTokenAysnc(string token, string refreshtoken, CancellationToken ct = default)
    {
        var userId = _JwtProvider.ValidateToken(token);
        if (userId is null)
        {
            return Result.Failure(UserError.Error); 
        }
        // find user 
        var user = await _user.FindByIdAsync(userId);
        if (user == null)
        {
            return Result.Failure(UserError.Error);

        }
        // find if user has token == Refreshtoken request
        var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshtoken && x.IsActive);
        if (userRefreshToken == null)
        {

            return Result.Failure<UserResponse>(new("Refreshtoken.Error", "Is not Activ / Expired ,or not Found", StatusCodes.Status401Unauthorized));

        }

        // revoke refrsh token 
        userRefreshToken.RevokedOn = DateTime.UtcNow;
        await _user.UpdateAsync(user);

        return Result.Success();
        
    }

    
 
    public async Task<Result> UserRegister(RegisterRequest request, CancellationToken ct = default)
    {
        var emailIsexist = await _user.Users.AnyAsync(x => x.Email == request.Email, ct);

        if (emailIsexist)
        {

            return Result.Failure(UserError.USerIsExist);

        }

        var user = new ApplicationUser
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.Email,
        };

        var result = await _user.CreateAsync(user, request.Password);
       
        if (result.Succeeded)
        {   
            //generate Code 
            var code = await _user.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code))  ;
            _logger.LogInformation("Generate Confirmation Code:{Code} ", code);
        
            await SendEmailAsync(user, code);
            return Result.Success();

        }

        var error = result.Errors.First();

        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));


    }

    public async Task<Result> ConfirmEmail(ConfirmEmailRequest request, CancellationToken ct)
    {

        if (await _user.FindByIdAsync(request.UserId) is not { } user)
            return Result.Failure(UserError.InvalidCode);



        if (user.EmailConfirmed)
            return Result.Failure(UserError.EmailConfirmed);

        string code = request.Code;
        try
        {
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

        }
        catch (FormatException)
        {

            return Result.Failure(UserError.InvalidCode);

        }
        var result = await _user.ConfirmEmailAsync(user, code);
        if(result.Succeeded)
        {
           await _user.AddToRoleAsync(user, DefaultRules.MemberRuleName);
            return Result.Success();
        }
          

         var error = result.Errors.First();

        return Result.Failure(new Error(error.Code ,error.Description,StatusCodes.Status400BadRequest))       ;

    }


    public async Task<Result>ResnedEmailConfirmation(ResendConfirmationEmailRequest request)
    {
        if (await _user.FindByEmailAsync(request.Email) is not { } user)
            return Result.Success();

        if(user.EmailConfirmed)
            return Result.Failure(UserError.EmailConfirmed);

        string code = await _user.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        _logger.LogInformation("Generate Confirmation Code:{Code} ", code);


        await SendEmailAsync (user , code)  ;


        return Result.Success();    

    }


    public async Task<Result> ResendForgetPasswordConfirmation(string email)
    {
        if(await _user.FindByEmailAsync(email) is not { } usersystem)
            return Result.Success();

        var code =  await  _user.GeneratePasswordResetTokenAsync(usersystem);
        code = WebEncoders.Base64UrlEncode (Encoding.UTF8.GetBytes(code));

        await SendPassowrdConfirmationAsync (usersystem, code)   ;

        return Result.Success();
    }

    public async Task<Result> ResetPassword(ResetPasswordRequest request)
    {

        var userSystem = await _user.FindByEmailAsync(request.Email);

        if (userSystem is null || !userSystem.EmailConfirmed)
        {
            return Result.Failure(UserError.InvalidCode);
        }

        IdentityResult result;

        try
        {
            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
            result= await _user.ResetPasswordAsync(userSystem, code,request.NewPassword) ;
           
        }
        catch (FormatException)
        {
            result = IdentityResult.Failed(_user.ErrorDescriber.InvalidToken());
          
        }

        if (result.Succeeded)
            return Result.Success();
        var errror = result.Errors.First();
        return Result.Failure(new Error(errror.Code, errror.Description, StatusCodes.Status401Unauthorized));
    }
    private static string GenrateRefreshtoken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }


    private async  Task SendEmailAsync ( ApplicationUser user  , string code  )
    {
        var origin = _httpcontext.HttpContext?.Request.Headers.Origin;

        var body = EmailBodyHelper.GenerateEmailBody("EmailVerification",
            new Dictionary<string, string>
            {

                    {"{{name}}"  ,user.FirstName } ,
                    {"{{action_url}}" , $"{origin}/api/Auth/confirm-Email?UserId={user.Id}&code={code}" }


            }

        );

        BackgroundJob.Enqueue(() => _email.SendEmailAsync(user.Email!, "✔️ Email Confiramtion", body));


          await Task.CompletedTask;



    }
    private async  Task SendPassowrdConfirmationAsync ( ApplicationUser user  , string code  )
    {
        var origin = _httpcontext.HttpContext?.Request.Headers.Origin;

        var body = EmailBodyHelper.GenerateEmailBody("ForgetPassword",
            new Dictionary<string, string>
            {

                    {"{{name}}"  ,user.FirstName } ,
                    {"{{action_url}}" , $"{origin}/api/Auth/Forget-Password?UserEmail={user.Email}&code={code}" }


            }

        );

        BackgroundJob.Enqueue(() => _email.SendEmailAsync(user.Email!, "✔️ Survey Bakset : Passowrd Confiramtion", body));


          await Task.CompletedTask;



    }



    private async Task<(IEnumerable<string> roles , IEnumerable<string>Permisinos )> GeTUSerRolesAndPermisions ( ApplicationUser user  ,CancellationToken ct =default)
    {

        var userroles = await _user.GetRolesAsync(user);

        var rolePermissions = await (from r in _context.Roles
                               join p in _context.RoleClaims
                               on r.Id equals p.RoleId
                               where userroles.Contains(r.Name!)
                               select (p.ClaimValue)

                               ).Distinct()
                               .ToListAsync(ct);








        return (userroles, rolePermissions);

    }
}
