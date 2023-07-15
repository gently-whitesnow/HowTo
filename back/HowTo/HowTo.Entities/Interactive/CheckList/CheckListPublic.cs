using HowTo.Entities.Interactive.Base;
using Newtonsoft.Json;

namespace HowTo.Entities.Interactive.CheckList;

public class CheckListPublic : InteractivePublicBase
{
    public CheckListPublic(CheckListDto dto)
    {
        Id = dto.Id;
        Description = dto.Description;
        Clauses = JsonConvert.DeserializeObject<string[]>(dto.ClausesJsonStringArray);
        InteractiveType = InteractiveType.CheckList;
    }
    public string[] Clauses { get; init; }
    public bool?[] UserClausesChecked { get; set; }
}