using LogicLayer.Services;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace Last.Filters
{
    public class UnblockedOnlyAttribute : FilterAttribute, IAuthenticationFilter
    {
            public void OnAuthentication(AuthenticationContext filterContext)
            {
        
       
            }

            public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
            {
                var user = filterContext.HttpContext.User;
            
            if (user.IsInRole("blocked"))
            {
                    filterContext.Result = new RedirectToRouteResult(
                        new System.Web.Routing.RouteValueDictionary {
                    { "controller", "Account" }, { "action", "Error" }
                       });
                }
            }
      
    }
}