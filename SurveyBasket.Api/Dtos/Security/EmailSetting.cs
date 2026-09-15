namespace SurveyBasket.Api.Dtos.Security;

public class EmailSetting
{
    public const string Name = "MailSettings";
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!; 
    public string DisplayName{ get; set; } = null!; 
    public string Url { get; set; } = null!;

    public int Port { get; set; } 
}
