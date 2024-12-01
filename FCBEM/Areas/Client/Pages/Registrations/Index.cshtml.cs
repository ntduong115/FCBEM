using Model;
using Model.Models.Authorize;

using Microsoft.AspNetCore.Identity;

namespace FCBEM.Areas.Client.Pages.Registrations
{
    public class IndexModel(UserManager<User> userManager, RoleManager<Role> roleManager, DatabaseContext context, IConfiguration configuration) : FCCore.Areas.Client.Pages.Registrations.IndexModel(userManager, roleManager, context, configuration)
    {
    }
}
