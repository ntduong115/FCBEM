using System.Security.Claims;

using FCCore.ViewComponents;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FCAI.Commons.Authorizations
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
                context.Result = new ViewComponentResult
                {
                    ViewComponentName = MessagePageViewComponent.COMPONENTNAME,
                    Arguments = new MessagePageViewComponent.Message()
                    {
                        Title = "Access Denine",
                        Htmlcontent = "You don't have permission to access, Current Role: " + role,
                        Urlredirect = context.HttpContext.Request.PathBase.Value + "/Login",
                        ReturnUrl = context.HttpContext.Request.PathBase.Value + context.HttpContext.Request.Path.Value,
                        ProjectName = ProjectName,
                        ProjectYear = ProjectYear,
                    },
                    //ViewData = ViewData,
                    //TempData = TempData
                };
                return;
            }

            if (string.IsNullOrEmpty(role))
            {
                context.Result = new ViewComponentResult
                {
                    ViewComponentName = MessagePageViewComponent.COMPONENTNAME,
                    Arguments = new MessagePageViewComponent.Message()
                    {
                        Title = "Access Denine",
                        Htmlcontent = "You don't have permission to access, Current Role: " + role,
                        Urlredirect = context.HttpContext.Request.PathBase.Value + "/Login",
                        ReturnUrl = context.HttpContext.Request.PathBase.Value + context.HttpContext.Request.Path.Value,
                        ProjectName = ProjectName,
                        ProjectYear = ProjectYear,

                    },
                    //ViewData = ViewData,
                    //TempData = TempData
                };
                context.Result = new RedirectResult(context.HttpContext.Request.PathBase.Value + "/Login" + "?ReturnUrl=" + context.HttpContext.Request.PathBase.Value + context.HttpContext.Request.Path.Value);
                return;
            }
        }
    }
}

