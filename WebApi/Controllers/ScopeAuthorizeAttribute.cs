using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace WebApi.Controllers
{
    public class ScopeAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string scope;

        public ScopeAuthorizeAttribute(string scope)
        {
            this.scope = scope;
        }
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            base.OnAuthorization(actionContext);
            var x = actionContext.ControllerContext;
            var y = x.RequestContext;
            var z = y.Principal;

            ClaimsPrincipal principal = z as ClaimsPrincipal;
            if (principal != null && principal.HasClaim(c => c.Type == "scope"))
            {
                // Split the scopes string into an array
                var scopes = principal.Claims.FirstOrDefault(c => c.Type == "scope")?.Value.Split(' ');

                // Succeed if the scope array contains the required scope
                if (scopes.Any(s => s == scope))
                    return;
            }

            HandleUnauthorizedRequest(actionContext);
        }
    }
}