using HowTo.Entities.Interactive.Base;
using Newtonsoft.Json;

namespace HowTo.Entities.Interactive.CheckList;

public class LastCheckListPublic : LastInteractivePublicBase
{
    public LastCheckListPublic(UpsertInteractiveReplyRequest request)
    {
        InteractiveId = request.InteractiveId;
        CourseId = request.CourseId;
        ArticleId = request.ArticleId;
        Clauses = request.UpsertReplyCheckList.Clauses;
        InteractiveType = InteractiveType.CheckList;
    }
    
    public LastCheckListPublic(LastCheckListDto dto)
    {
        InteractiveId = dto.InteractiveId;
        CourseId = dto.CourseId;
        ArticleId = dto.ArticleId;
        Clauses = JsonConvert.DeserializeObject<bool?[]>(dto.CheckedClausesJsonBoolArray);
        InteractiveType = InteractiveType.CheckList;
    }
    
    public bool?[] Clauses { get; set; }
}