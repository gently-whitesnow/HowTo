using System.Linq;
using HowTo.Entities.Interactive.CheckList;
using HowTo.Entities.Interactive.ChoiceOfAnswer;
using HowTo.Entities.Interactive.ChoiceOfAnswers;
using HowTo.Entities.Interactive.ProgramWriting;
using HowTo.Entities.Interactive.WritingOfAnswer;

namespace HowTo.Entities.Interactive;

public class LastInteractivePublic
{
    public LastInteractivePublic(LastCheckListDto[] dto)
    {
        LastCheckList = dto.Select(d => new LastCheckListPublic(d)).ToArray();
    }

    public LastInteractivePublic(LastChoiceOfAnswerDto[] dto)
    {
        LastChoiceOfAnswer = dto.Select(d => new LastChoiceOfAnswerPublic(d)).ToArray();
    }

    public LastInteractivePublic(LastProgramWritingDto[] dto)
    {
        LastProgramWriting = dto.Select(d => new LastProgramWritingPublic(d)).ToArray();
    }

    public LastInteractivePublic(LastWritingOfAnswerDto[] dto)
    {
        LastWritingOfAnswer = dto.Select(d => new LastWritingOfAnswerPublic(d)).ToArray();
    }

    public LastInteractivePublic(LastCheckListPublic[]? lastCheckList,
        LastChoiceOfAnswerPublic[]? lastChoiceOfAnswer,
        LastProgramWritingPublic[]? lastProgramWriting,
        LastWritingOfAnswerPublic[]? lastWritingOfAnswer)
    {
        LastCheckList = lastCheckList;
        LastChoiceOfAnswer = lastChoiceOfAnswer;
        LastProgramWriting = lastProgramWriting;
        LastWritingOfAnswer = lastWritingOfAnswer;
    }

    public LastCheckListPublic[]? LastCheckList { get; init; }
    public LastChoiceOfAnswerPublic[]? LastChoiceOfAnswer { get; init; }
    public LastProgramWritingPublic[]? LastProgramWriting { get; init; }
    public LastWritingOfAnswerPublic[]? LastWritingOfAnswer { get; init; }
}