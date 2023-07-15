using HowTo.Entities.Interactive.Base;

namespace HowTo.Entities.Interactive.WritingOfAnswer;

public class WritingOfAnswerPublic : InteractivePublicBase
{
    public WritingOfAnswerPublic(WritingOfAnswerDto dto, bool isAuthor = true)
    {
        Id = dto.Id;
        Description = dto.Description;
        Answer = isAuthor ? dto.Answer : null;
        InteractiveType = InteractiveType.WritingOfAnswer;
    }

    public string? Answer { get; set; }
    public string? UserAnswer { get; set; }
    public bool? UserSuccess { get; set; }
}