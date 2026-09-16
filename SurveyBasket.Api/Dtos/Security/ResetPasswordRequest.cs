namespace SurveyBasket.Api.Dtos.Security;

public record ResetPasswordRequest(
    
    string Email ,
    string Code , 
    string NewPassword
    
    
    );

