using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;

namespace MVCPaswords.Models
{
    public class LoginControl : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return httpContext.Session["UserID"] != null;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
                                  new RouteValueDictionary
                                  {
                                   { "action", "Index" },
                                   { "controller", "Login" }
                                  });
        }
        //public override void OnResultExecuting(ResultExecutingContext filterContext)
        //{
        //    base.OnResultExecuting(filterContext);

        //    /// we set a field 'IsAjaxRequest' in ViewBag according to the actual request type
        //    filterContext.Controller.ViewBag.IsAjaxRequest = filterContext.HttpContext.Request.IsAjaxRequest();
        //}

    }
}

//public override void OnActionExecuting(ActionExecutingContext filterContext)
//{

//    /// user is logged in (the "loggedIn" should be set in Login action upon a successful login request)
//    try
//    {
//        if (string.IsNullOrEmpty(HttpContext.Current.Session["UserID"].ToString()))
//        {
//            //
//        }
//        else
//        {
//            HttpContext.Current.Session.Timeout = 120;
//            Entities db = new Entities();
//            int userid = (int)HttpContext.Current.Session["UserID"];
//            var user = db.Kullanicis.FirstOrDefault(x => x.ID.Equals(userid));
//            base.OnActionExecuting(filterContext);
//        }
//    }
//    catch (Exception ex)
//    {
//        HttpContext.Current.Response.Redirect("/Login/Index");
//    }
//    /// if the request is ajax then we return a json object
//    //if (filterContext.HttpContext.Request.IsAjaxRequest())
//    //    {
//    //        filterContext.Result = new JsonResult
//    //        {
//    //            Data = "UnauthorizedAccess",
//    //            JsonRequestBehavior = JsonRequestBehavior.AllowGet
//    //        };
//    //    }
//    //    /// otherwise we redirect the user to the login page
//    //    else
//    //    {
//    //        var redirectTarget = new RouteValueDictionary { { "Controller", "Login" }, { "Action", "Index" } };
//    //        filterContext.Result = new RedirectToRouteResult(redirectTarget);
//    //    }

//}