using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HowTo.Entities.Extensions;

public class AdminRequiredAttribute: ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext actionContext)
    {
        var user = actionContext.HttpContext.GetUser();
        if (user.UserRole == UserRole.Admin)
            return;

        actionContext.Result = new UnauthorizedResult();
        base.OnActionExecuting(actionContext);
    }
}