namespace SurveyBasket.Api.Dtos.Errors;

public class UserError
{
    public static readonly Error Error = new Error("User.InvalidCredential ", "Invalid Email/passwowrd ");
}
