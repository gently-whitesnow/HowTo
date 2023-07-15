using System.ComponentModel.DataAnnotations;
using HowTo.Entities.Interactive.Base;
using HowTo.Entities.Interactive.CheckList;
using HowTo.Entities.Interactive.ChoiceOfAnswers;
using HowTo.Entities.Interactive.ProgramWriting;
using HowTo.Entities.Interactive.WritingOfAnswer;

namespace HowTo.Entities.Interactive;

public class UpsertInteractiveRequest
{
    public int? InteractiveId { get; set; }
    [Required]
    public int ArticleId { get; set; }
    [Required]
    public int CourseId { get; set; }
    [Required]
    public string Description { get; set; }
    public UpsertCheckListRequest? UpsertCheckList { get; set; }
    public UpsertChoiceOfAnswerRequest? UpsertChoiceOfAnswer { get; set; }
    public UpsertProgramWritingRequest? UpsertProgramWriting { get; set; }
    public UpsertWritingOfAnswerRequest? UpsertWritingOfAnswer { get; set; }
}