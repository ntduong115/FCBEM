using Microsoft.AspNetCore.Authorization;

namespace FCBEM24.Commons.Policy
{
    public class LoginOnlyRequirement : AuthorizationHandler<LoginOnlyRequirement>, IAuthorizationRequirement
    {

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, LoginOnlyRequirement requirement)
        {
            if (context.User.Identity.IsAuthenticated)
                context.Succeed(requirement);
            await Task.CompletedTask;
            return;
        }
    }
}
