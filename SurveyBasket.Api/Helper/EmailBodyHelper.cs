namespace SurveyBasket.Api.Helper;

public static class EmailBodyHelper
{
    public static string GenerateEmailBody (string Temlpate, Dictionary<string, string> TemplateModel )
    {

        var filePath = $"{Directory.GetCurrentDirectory()}/Templetes/{Temlpate}.html";
       
        var streamreader = new StreamReader ( filePath );   

        var body = streamreader.ReadToEnd ();   

        streamreader.Close();

        foreach (var item in TemplateModel) {

            body = body.Replace(item.Key, item.Value);

        }

        return body ;   
    }
}
