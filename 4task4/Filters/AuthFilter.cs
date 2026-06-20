using _4task4.DataBase;
using _4task4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace _4task4.Filters;

public class AuthFilter : Attribute, IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var http = context.HttpContext;
        var id = http.Session.GetString("UserId");

        UserDataModel? user = null;
        var db = http.RequestServices.GetService(typeof(UserDBContext)) as UserDBContext;
        if (id != null && db != null)
        {
            user = db.Users.FirstOrDefault(u => u.Id.ToString() == id);
        }

        if (id == null)
        {
            SetMessage(http, "Please login to access that page.");
            context.Result = new RedirectToActionResult("Login", "Account", null);
            return;
        }

        if (user == null || user.IsBlocked)
        {
            http.Session.Clear();
            SetMessage(http, "Your account has been blocked or removed, so you were signed out.");
            context.Result = new RedirectToActionResult("Login", "Account", null);
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }

    private static void SetMessage(HttpContext http, string message)
    {
        var factory = http.RequestServices.GetService(typeof(ITempDataDictionaryFactory)) as ITempDataDictionaryFactory;
        var temp = factory?.GetTempData(http);
        if (temp != null)
        {
            temp["Error"] = message;
        }
    }
}
