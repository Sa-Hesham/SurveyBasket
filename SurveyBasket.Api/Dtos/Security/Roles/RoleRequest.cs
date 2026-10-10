namespace SurveyBasket.Api.Dtos.Security.Roles;

public record RoleRequest(

    string Name ,
    List<string> Permissions
    
    
    );

