using Model;
using Model.Models.Authorize;

using Microsoft.AspNetCore.Identity;

namespace FCBEM.Areas.Client.Pages.Papers
{
    public class IndexModel(UserManager<User> userManager, DatabaseContext context, IConfiguration configuration) : FCCore.Areas.Client.Pages.Papers.IndexModel(userManager, context, configuration)
    {
    }
}
