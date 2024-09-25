using System.Security.Claims;

using FCCore.ViewComponents;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FCBEM.Commons.Authorizations
{
    public class AuthorizeCustomizeAttribute : TypeFilterAttribute
    {
        public AuthorizeCustomizeAttribute(string role)
        : base(typeof(AuthorizeActionFilter))
        {
            Arguments = [role];

        }
    }
    public class AuthorizeActionFilter(string role, IConfiguration configuration) : IAuthorizationFilter
    {
        private readonly string ProjectName = configuration["ProjectName"];
        private readonly string ProjectYear = configuration["ProjectYear"];

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var roles = context.HttpContext.User.FindAll(ClaimTypes.Role);//FindFirstValue(ClaimTypes.Role);
            if (!roles.Any(r => r.Value.ToString() == role))
            {
                context.Result = new RedirectToPageResult("/Errors/UnAuthorized", new { ReturnUrl = context.HttpContext.Request.Path.Value ?? "/Index" });
                return;
            }

            if (string.IsNullOrEmpty(role))
            {
                context.Result = new RedirectToPageResult("/Errors/UnAuthorized", new { ReturnUrl = context.HttpContext.Request.Path.Value ?? "/Index" });
                return;
            }
        }
    }
}

