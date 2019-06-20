using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace MyWebPage.Filters
{
    public class BasicAuthenticationAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            var authorization = request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(authorization))
            {
                var credentials = ASCIIEncoding.ASCII.GetString(
                    Convert.FromBase64String(authorization.Substring(6))).Split(':');
                var username = credentials[0];
                var password = credentials[1];

                // Web API's sample user login.
                var isValid = username == "sampleuser@gmail.com" && password == "Password@123";
                if (isValid)
                {
                    var principal = new GenericPrincipal(new GenericIdentity(username), null);
                    Thread.CurrentPrincipal = principal;
                    return;
                }
            }
            filterContext.HttpContext.Response.Headers.Add(
                "WWW-Authenticate", "Basic Scheme='Data' location='http://localhost'");
            filterContext.Result = new HttpUnauthorizedResult();
        }
    }
}