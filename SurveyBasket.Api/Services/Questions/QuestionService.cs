using SurveyBasket.Api.Dtos.Errors;
using SurveyBasket.Api.Dtos.Questions;
using SurveyBasket.Api.Services.Polls;

namespace SurveyBasket.Api.Services.Questions;

public class QuestionService(AppDbContext context) : IQuestionService
{
    private readonly AppDbContext _context = context;


    public async Task<Result<QuestionResponse>> CreatQuestionaysnc(int pollId,QuestionRequest request, CancellationToken ct = default)
    {

        var pollIsExist = await _context.Polls.AnyAsync(x => x.Id == pollId, ct);
        if (!pollIsExist)
            return Result.Failure<QuestionResponse>(PollError.PollINotDEleted);

        //cheak poll has same Question Content 

        var QuestionIsExist = await _context.questions.AnyAsync(q => q.Content == request.Content && q.Id == pollId, ct);

        if (QuestionIsExist) 
            return Result.Failure<QuestionResponse>(QuestionError.QuestionDublicated);
         

        // mapping form Request to QuestionModel
        var Question = request.Adapt<Question>();

        Question.PollId = pollId;

      

        await _context.AddAsync(Question,ct);
        
        await _context.SaveChangesAsync(ct);

       
        return Result.Succes(Question.Adapt<QuestionResponse>());



    }

    public async Task<Result<IEnumerable<QuestionResponse>>> GetAllQuestionAsync(int pollId, CancellationToken ct = default)
    {
        var PollIsExist = await _context.Polls.AnyAsync( x=>x.Id == pollId, ct);  

        if(!PollIsExist)
         return Result.Failure< IEnumerable<QuestionResponse>> (PollError.PollINotDEleted);

        var questins
            = await _context.questions.Where(p => p.PollId == pollId).Include(x=>x.Answers).AsNoTracking().ToListAsync(ct);


        if(questins == null)
        {
          
            return Result.Failure<IEnumerable<QuestionResponse>>(QuestionError.QuestionNotFound);
        }


        return Result.Succes(questins.Adapt<IEnumerable<QuestionResponse>>());
    }

    public async Task<Result<QuestionResponse>> GetById(int pollId, int qustionId, CancellationToken ct = default)
    {


        var question
          = await _context.questions
          .Where(p => p.PollId == pollId && p.Id== qustionId )
          .Include(x => x.Answers)
          .ProjectToType<QuestionResponse>()
          .AsNoTracking()
          .SingleOrDefaultAsync(ct);

        if (question is null)
            return Result.Failure<QuestionResponse>(QuestionError.QuestionNotFound);


        return Result.Succes(question);


    }

    public async Task<Result> ToggleStatusAsync(int pollId, int qustionId, CancellationToken ct = default)
    {
       var question = await _context.questions.SingleOrDefaultAsync(x=>x.PollId==pollId && x.Id== qustionId) ;

        if (question is null)   
            return Result.Failure(QuestionError.QuestionNotFound);

        question.IsActive = !question.IsActive;

        await _context.SaveChangesAsync(ct);


        return Result.Success();


    }

    public async Task<Result> UpdateAsync(int pollId, int id, QuestionRequest request, CancellationToken ct = default)
    {
        //cheak If question with ID  is same in database 
        // question1 1    >>  question2 1  poll 1 
        // question1 1 =>  question2  2   ==> invalied 


        var QuestionIsExit = await _context.questions.AnyAsync(x=>x.PollId == pollId && x.Id!= id && x.Content == request.Content ,ct);

        if (QuestionIsExit)
            return Result.Failure(QuestionError.QuestionDublicated);


        var question =  await _context.questions.Include(x=>x.Answers).FirstOrDefaultAsync(x=>x.PollId == pollId && x.Id== id ,ct ) ;

        if (question is null)
            return Result.Failure(QuestionError.QuestionNotFound);

        //change name of Content 

         question.Content = request.Content;



        // change answers denaied the rapeated answer  

        // mmmmmm thinking mmmmmmmmmmmmmmm

        // first Ineed to know  the answers in databse 

        var currentAnswers = question.Answers.Select(a=>a.Content).ToList() ;    


        //  put request new answers expect Currenanswer 

        var newAnswers = request.answers.Except(currentAnswers).ToList() ;



        // add the new answers in db 

        foreach (var item in newAnswers)
        {
            question.Answers.Add(new Answer
            {
                Content = item 

            });
        }


        question.Answers.ToList().ForEach(answer =>
        {
            answer.IsActive = request.answers.Contains(answer.Content);
        });



        await _context.SaveChangesAsync(ct);

       return  Result.Success();   




    }
}
