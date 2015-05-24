namespace Portal.Filter
{
    using System;
    using System.Web;
    using System.Web.Mvc;


    /// <summary>
    /// This class is to Filter authorization.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class AuthorizationFilterAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Handle Unauthorized request
        /// </summary>
        /// <param name="filterContext">filter the information</param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("~/Login");
        }

        /// <summary>
        /// Check authentication and HttpContext request
        /// </summary>
        /// <param name="httpContext">The http Context</param>
        /// <returns>
        /// True:Authorized
        /// False:UnAuthorized
        /// </returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authentication = httpContext.User.Identity.IsAuthenticated;

            if (authentication)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}