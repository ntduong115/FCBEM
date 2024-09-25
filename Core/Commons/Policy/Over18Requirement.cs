using Microsoft.AspNetCore.Authorization;

using System.Security.Claims;

namespace Core.Commons.Policy
{
    public class Over18Requirement : AuthorizationHandler<Over18Requirement>, IAuthorizationRequirement
    {

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, Over18Requirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.DateOfBirth))
            {
                context.Fail();
                await Task.CompletedTask;
                return;
            }

            var dobVal = context.User.FindFirst(c => c.Type == ClaimTypes.DateOfBirth).Value;
            var dateOfBirth = Convert.ToDateTime(dobVal);
            int age = DateTime.Today.Year - dateOfBirth.Year;
            if (dateOfBirth > DateTime.Today.AddYears(-age))
            {
                age--;
            }

            if (age >= 18)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
            await Task.CompletedTask;
            return;
        }
    }
}
