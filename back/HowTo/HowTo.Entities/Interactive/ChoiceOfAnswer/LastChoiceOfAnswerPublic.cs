using System.Linq;
using HowTo.Entities.Interactive.Base;
using HowTo.Entities.Interactive.ChoiceOfAnswers;
using Newtonsoft.Json;

namespace HowTo.Entities.Interactive.ChoiceOfAnswer;

public class LastChoiceOfAnswerPublic : LastInteractivePublicBase
{
    public LastChoiceOfAnswerPublic(UpsertInteractiveReplyRequest request, ChoiceOfAnswerDto choiceOfAnswerDto)
    {
        InteractiveId = request.InteractiveId;
        ArticleId = request.ArticleId;
        CourseId = request.CourseId;
        Answers = request.UpsertReplyChoiceOfAnswer.Answers;
        SuccessAnswers = ValidateChoiceOfAnswer(request.UpsertReplyChoiceOfAnswer.Answers,
            JsonConvert.DeserializeObject<bool[]>(choiceOfAnswerDto.AnswersJsonBoolArray));
        InteractiveType = InteractiveType.ChoiceOfAnswer;
    }
    
    public LastChoiceOfAnswerPublic(LastChoiceOfAnswerDto dto)
    {
        InteractiveId = dto.InteractiveId;
        ArticleId = dto.ArticleId;
        CourseId = dto.CourseId;
        Answers = JsonConvert.DeserializeObject<bool[]>(dto.AnswersJsonBoolArray);
        SuccessAnswers = JsonConvert.DeserializeObject<bool[]>(dto.SuccessAnswersJsonBoolArray);
        InteractiveType = InteractiveType.ChoiceOfAnswer;
    }
    public bool[] Answers { get; set; }
    public bool[]? SuccessAnswers { get; set; }

    private bool[] ValidateChoiceOfAnswer(bool[] request, bool[] solution)
    {
        for (int i = 0; i < solution.Length; i++)
        {
            if (request.Length > i)
                solution[i] = solution[i] == request[i];
            else
                solution[i] = solution[i] == false;
        }

        return solution;
    }
}