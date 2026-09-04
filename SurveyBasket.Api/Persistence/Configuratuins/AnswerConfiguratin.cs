namespace SurveyBasket.Api.Persistence.Configuratuins;

public class AnswerConfiguratin : IEntityTypeConfiguration<Answer>
{
    public void Configure(EntityTypeBuilder<Answer> builder)
    {
        builder.HasIndex(a => new {a.QuestionId , a.Content}).IsUnique();

        builder.Property(A => A.Content)
            .HasMaxLength(1000);
            

       
            
    }
}
