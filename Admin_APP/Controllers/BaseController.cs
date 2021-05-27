using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin_APP.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var session = HttpContext.Session.GetString("Token");
            if (session == null)
                context.Result = new RedirectToActionResult("Index", "Login", null);
            base.OnActionExecuted(context);
        }
    }
}