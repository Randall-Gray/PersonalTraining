using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using SPFWebsitMVC.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SPFWebsitMVC.ActionFilters
{
    public class GlobalRouting : IActionFilter
    {
        public async void OnActionExecuting(ActionExecutingContext context) 
        { 
            var controller = context.RouteData.Values["controller"];
            string action = context.RouteData.Values["action"].ToString();

            if (controller.Equals("Home")) 
            { 
                if (GlobalSettings.CurrentUserRole =="Client") 
                { 
                    context.Result = new RedirectToActionResult(action, "Clients", null); 
                } 
                else if (GlobalSettings.CurrentUserRole == "Trainer") 
                { 
                    context.Result = new RedirectToActionResult(action, "Trainers", null); 
                }
                else if (GlobalSettings.CurrentUserRole == "Admin")
                {
                    context.Result = new RedirectToActionResult(action, "Admins", null);
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}
