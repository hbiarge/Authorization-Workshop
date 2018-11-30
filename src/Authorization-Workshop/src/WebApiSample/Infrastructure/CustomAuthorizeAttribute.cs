using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace WebApiSample.Infrastructure
{
    public class CustomAuthorizeAttribute: AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var isAuthorizedByDefault = base.IsAuthorized(actionContext);

            if (isAuthorizedByDefault == false)
            {
                return false;
            }

            // Custom authorization logic... as complex as you want :)

            return true;
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException(nameof(actionContext));
            }

            if (actionContext.ControllerContext.RequestContext.Principal.Identity?.IsAuthenticated ?? false)
            {
                actionContext.Response = actionContext.ControllerContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unauthorized");
            }
            else
            {
                actionContext.Response = actionContext.ControllerContext.Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Forbidden");
            }
        }
    }
}