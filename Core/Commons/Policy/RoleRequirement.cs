using Microsoft.AspNetCore.Authorization;

using System.Security.Claims;

namespace Core.Commons.Policy
{
    //Chưa xử lý xong
    public class RoleRequirement : AuthorizationHandler<RoleRequirement>, IAuthorizationRequirement
    {

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.Role))
            {
                context.Fail();
                await Task.CompletedTask;
                return;
            }

            var role = context.User.FindFirst(c => c.Type == ClaimTypes.Role)?.Value;
            

            //if (age >= 18)
            //{
            //    context.Succeed(requirement);
            //}
            //else
            //{
            //    context.Fail();
            //}
            await Task.CompletedTask;
            return;
        }
    }
}
