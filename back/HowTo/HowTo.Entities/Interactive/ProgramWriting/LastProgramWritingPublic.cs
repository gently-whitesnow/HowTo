using HowTo.Entities.Interactive.Base;

namespace HowTo.Entities.Interactive.ProgramWriting;

public class LastProgramWritingPublic : LastInteractivePublicBase
{
    public LastProgramWritingPublic(UpsertInteractiveReplyRequest request, ProgramWritingDto dto)
    {
        InteractiveId = request.InteractiveId;
        ArticleId = request.ArticleId;
        CourseId = request.CourseId;
        Code = request.UpsertReplyProgramWriting.Code;
        Success = ValidateProgramWriting(request.UpsertReplyProgramWriting.Code, dto.Output);
        InteractiveType = InteractiveType.ProgramWriting;
    }
    
    public LastProgramWritingPublic(LastProgramWritingDto dto)
    {
        InteractiveId = dto.InteractiveId;
        ArticleId = dto.ArticleId;
        CourseId = dto.CourseId;
        Code = dto.Code;
        Success = dto.Success;
        InteractiveType = InteractiveType.ProgramWriting;
    }
    public string Code { get; init; }
    public bool Success { get; init; }
    
    // TODO разработка сервиса под компиляцию и запуск кода
    private bool ValidateProgramWriting(string code, string output) =>
        code.Trim().ToLower().Contains(output.Trim().ToLower());
}