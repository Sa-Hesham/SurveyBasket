namespace SurveyBasket.Api.Dtos.Security.Roles;

public record RoleDetailsReponse(

    string Id ,
    string Name , 
    bool IsDeleted , 
    IEnumerable<string> Permissons
    
    
    );
