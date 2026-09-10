namespace SurveyBasket.Api.Dtos.Errors;

public class UserError
{
    public static readonly Error Error = new Error("User.InvalidCredential ", "Invalid Email/passwowrd ", StatusCodes.Status401Unauthorized);
    public static readonly Error UserDublicated = new Error("user.Dublicated", "user had voted before  in this Poll", StatusCodes.Status409Conflict);
}
