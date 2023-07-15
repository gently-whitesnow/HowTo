using HowTo.Entities.Interactive.Base;

namespace HowTo.Entities.Interactive.ProgramWriting;

public class ProgramWritingPublic : InteractivePublicBase
{
    public ProgramWritingPublic(ProgramWritingDto dto, bool isAuthor = true)
    {
        Id = dto.Id;
        Description = dto.Description;
        Code = dto.Code;
        Output = isAuthor ? dto.Output : null;
        InteractiveType = InteractiveType.ProgramWriting;
    }

    public string Code { get; set; }
    public string? Output { get; set; }

    public string? UserCode { get; set; }
    public bool? UserSuccess { get; set; }
}