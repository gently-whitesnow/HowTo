using HowTo.Entities.Interactive.CheckList;
using HowTo.Entities.Interactive.ChoiceOfAnswer;
using HowTo.Entities.Interactive.ChoiceOfAnswers;
using HowTo.Entities.Interactive.ProgramWriting;
using HowTo.Entities.Interactive.WritingOfAnswer;

namespace HowTo.Entities.Interactive;

public class InteractiveByIdPublic
{
    public InteractiveByIdPublic(CheckListDto dto, LastCheckListPublic? lastCheckList = null)
    {
        CheckList = new CheckListPublic(dto)
        {
            UserClausesChecked = lastCheckList?.Clauses
        };
    }

    public InteractiveByIdPublic(ChoiceOfAnswerDto dto, LastChoiceOfAnswerPublic? lastChoiceOfAnswer = null)
    {
        ChoiceOfAnswer = new ChoiceOfAnswerPublic(dto)
        {
            UserAnswers = lastChoiceOfAnswer?.Answers,
            UserSuccessAnswers = lastChoiceOfAnswer?.SuccessAnswers
        };
    }

    public InteractiveByIdPublic(ProgramWritingDto dto, LastProgramWritingPublic? lastProgramWriting = null)
    {
        ProgramWriting = new ProgramWritingPublic(dto)
        {
            UserCode = lastProgramWriting?.Code,
            UserSuccess = lastProgramWriting?.Success,
            Output = lastProgramWriting?.Output
        };
    }

    public InteractiveByIdPublic(WritingOfAnswerDto dto, LastWritingOfAnswerPublic? lastWritingOfAnswer = null)
    {
        WritingOfAnswer = new WritingOfAnswerPublic(dto)
        {
            UserAnswer = lastWritingOfAnswer?.Answer,
            UserSuccess = lastWritingOfAnswer?.Success
        };
    }
    
    public CheckListPublic CheckList { get; init; }
    public ChoiceOfAnswerPublic ChoiceOfAnswer { get; init; }
    public ProgramWritingPublic ProgramWriting { get; init; }
    public WritingOfAnswerPublic WritingOfAnswer { get; init; }
}