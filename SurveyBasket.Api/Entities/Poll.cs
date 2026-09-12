namespace SurveyBasket.Api.Entities;

public  sealed class Poll :BaseEntity
{

    public int Id { get; set; } 

    public string Title { get; set; } = string.Empty;   
    

    public string Summary { get; set; } = string.Empty ;   
    

   public  bool IsPublished { get; set; }

    public DateOnly SatrtsAt { get; set; }
    public DateOnly EndsAt { get; set; }

    public ICollection<Question> questions { get; set; } = [];
   public  ICollection<Vote> Votes { get; set; } = [];
        

}
