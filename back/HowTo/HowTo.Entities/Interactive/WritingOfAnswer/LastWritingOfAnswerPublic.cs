using System;
using HowTo.Entities.Interactive.Base;

namespace HowTo.Entities.Interactive.WritingOfAnswer;

public class LastWritingOfAnswerPublic : LastInteractivePublicBase
{
    public LastWritingOfAnswerPublic(UpsertInteractiveReplyRequest request, WritingOfAnswerDto dto)
    {
        InteractiveId = request.InteractiveId;
        ArticleId = request.ArticleId;
        CourseId = request.CourseId;
        Answer = request.UpsertReplyWritingOfAnswer.Answer;
        Success = ValidateWritingOfAnswer(request.UpsertReplyWritingOfAnswer.Answer, dto.Answer);
        InteractiveType = InteractiveType.WritingOfAnswer;
    }
    
    public LastWritingOfAnswerPublic(LastWritingOfAnswerDto dto)
    {
        InteractiveId = dto.InteractiveId;
        ArticleId = dto.ArticleId;
        CourseId = dto.CourseId;
        Answer = dto.Answer;
        Success = dto.Success;
        InteractiveType = InteractiveType.WritingOfAnswer;
    }

    public string Answer { get; set; }
    public bool Success { get; set; }
    
    
    private bool ValidateWritingOfAnswer(string request, string solve) =>
        request.Equals(solve, StringComparison.CurrentCultureIgnoreCase);
}