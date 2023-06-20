using HowTo.Entities.Interactive.CheckList;
using HowTo.Entities.Interactive.ChoiceOfAnswer;
using HowTo.Entities.Interactive.ProgramWriting;
using HowTo.Entities.Interactive.WritingOfAnswer;

namespace HowTo.Entities.Interactive;

public class LastInteractiveByIdPublic
{
    public LastInteractiveByIdPublic(LastCheckListPublic model)
    {
        LastCheckList = model;
    }

    public LastInteractiveByIdPublic(LastChoiceOfAnswerPublic model)
    {
        LastChoiceOfAnswer = model;
    }

    public LastInteractiveByIdPublic(LastProgramWritingPublic model)
    {
        LastProgramWriting = model;
    }

    public LastInteractiveByIdPublic(LastWritingOfAnswerPublic model)
    {
        LastWritingOfAnswer = model;
    }
    public LastCheckListPublic LastCheckList { get; init; }
    public LastChoiceOfAnswerPublic LastChoiceOfAnswer { get; init; }
    public LastProgramWritingPublic LastProgramWriting { get; init; }
    public LastWritingOfAnswerPublic LastWritingOfAnswer { get; init; }
}