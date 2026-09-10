namespace SurveyBasket.Api.Persistence.Configuratuins;

public class VoteAnswerConfiguration : IEntityTypeConfiguration<VoteAnswer>
{
    public void Configure(EntityTypeBuilder<VoteAnswer> builder)
    {
        builder.HasIndex(p => new {p.VoteId , p.QuestionId}).IsUnique();

       
            
    }
}
