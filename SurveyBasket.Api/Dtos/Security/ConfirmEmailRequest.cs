namespace SurveyBasket.Api.Dtos.Security;

public record ConfirmEmailRequest(string UserId, string Code);

