using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPFWebsitMVC.ActionFilters
{
    public class GlobalRouting : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context) 
        { 
            var controller = context.RouteData.Values["controller"]; 
            if (controller.Equals("Home")) 
            { 
                if (GlobalSettings.CurrentUserRole =="Client") 
                { 
                    context.Result = new RedirectToActionResult("Index", "Clients", null); 
                } 
                else if (GlobalSettings.CurrentUserRole == "Trainer") 
                { 
                    context.Result = new RedirectToActionResult("Index", "Trainers", null); 
                }
                else if (GlobalSettings.CurrentUserRole == "Admin")
                {
                    context.Result = new RedirectToActionResult("Index", "Admins", null);
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}
