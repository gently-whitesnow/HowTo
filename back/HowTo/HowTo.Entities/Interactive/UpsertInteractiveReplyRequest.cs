using System.ComponentModel.DataAnnotations;
using HowTo.Entities.Interactive.Base;
using HowTo.Entities.Interactive.CheckList;
using HowTo.Entities.Interactive.ChoiceOfAnswers;
using HowTo.Entities.Interactive.ProgramWriting;
using HowTo.Entities.Interactive.WritingOfAnswer;

namespace HowTo.Entities.Interactive;

public class UpsertInteractiveReplyRequest
{
    [Required] 
    public int InteractiveId { get; set; }
    [Required] 
    public int CourseId { get; set; }
    [Required] 
    public int ArticleId { get; set; }
    
    public UpsertReplyCheckListRequest? UpsertReplyCheckList { get; set; }
    public UpsertReplyAnswerChoiceRequest? UpsertReplyChoiceOfAnswer { get; set; }
    public UpsertReplyProgramWritingRequest? UpsertReplyProgramWriting { get; set; }
    public UpsertReplyWritingOfAnswerRequest? UpsertReplyWritingOfAnswer { get; set; }
}