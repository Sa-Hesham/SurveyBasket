namespace SurveyBasket.Api.Dtos.Errors;

public class UserError
{
    public static readonly Error Error = new Error("User.InvalidCredential ", "Invalid Email/passwowrd ", StatusCodes.Status401Unauthorized);
    public static readonly Error UserDublicated = new Error("user.Dublicated", "user had voted before  in this Poll", StatusCodes.Status409Conflict);
    public static readonly Error USerIsExist = new Error("Email.Exist", "Email Is Already Exist before", StatusCodes.Status409Conflict);
    public static readonly Error EmailNoconfirmed = new Error("Email.NotConfirmed ", "EmailNoConfirmed", StatusCodes.Status401Unauthorized);
    public static readonly Error InvalidCode = new Error("Code.Invalid ", "Invalid Code", StatusCodes.Status401Unauthorized);
    public static readonly Error EmailConfirmed = new Error("Code.Confirmed", "Email already  Confirmed Before", StatusCodes.Status400BadRequest);
}
