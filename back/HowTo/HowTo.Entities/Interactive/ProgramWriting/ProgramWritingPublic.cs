using HowTo.Entities.Interactive.Base;

namespace HowTo.Entities.Interactive.ProgramWriting;

public class ProgramWritingPublic : InteractivePublicBase
{
    public ProgramWritingPublic(ProgramWritingDto dto, bool isAuthor = true)
    {
        Id = dto.Id;
        ArticleId = dto.ArticleId;
        CourseId = dto.CourseId;
        Description = dto.Description;
        Code = dto.Code;
        Output = isAuthor ? dto.Output : null;
        InteractiveType = InteractiveType.ProgramWriting;
    }

    public string Code { get; set; }
    public string? Output { get; set; }
}