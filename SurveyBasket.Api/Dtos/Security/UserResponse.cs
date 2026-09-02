namespace SurveyBasket.Api.Dtos.Security;

public record UserResponse(

    string Id ,
    string Email,
    string FirstName , 
    string LastName,
    string Token,
    int ExpiresIn

    
    
    );

