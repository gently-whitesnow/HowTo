using HowTo.Entities.Interactive.Base;
using Newtonsoft.Json;

namespace HowTo.Entities.Interactive.CheckList;

public class CheckListPublic : InteractivePublicBase
{
    public CheckListPublic(CheckListDto dto)
    {
        Id = dto.Id;
        ArticleId = dto.ArticleId;
        CourseId = dto.CourseId;
        Description = dto.Description;
        Clauses = JsonConvert.DeserializeObject<string[]>(dto.ClausesJsonStringArray);
        InteractiveType = InteractiveType.CheckList;
    }
    public string[] Clauses { get; init; }
}