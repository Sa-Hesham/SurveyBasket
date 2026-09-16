namespace SurveyBasket.Api.Dtos.Security;

public record ChangePasswordRequest(string CurrentPassword, string NewPassword);

