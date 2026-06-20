using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Http;

namespace _4task4.Filters;

public class SelectionRequiredFilter : Attribute, IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        List<Guid>? selectedIds = null;
        if (context.ActionArguments.ContainsKey("selectedIds"))
        {
            selectedIds = context.ActionArguments["selectedIds"] as List<Guid>;
        }

        if (selectedIds == null || selectedIds.Count == 0)
        {
            var factory = context.HttpContext.RequestServices.GetService(typeof(ITempDataDictionaryFactory)) as ITempDataDictionaryFactory;
            var temp = factory?.GetTempData(context.HttpContext);
            if (temp != null)
            {
                temp["Error"] = "No users selected.";
            }
            context.Result = new RedirectToActionResult("Index", "UserCrud", null);
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}
