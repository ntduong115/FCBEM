using Microsoft.AspNetCore.Identity;
using Model.Models.Authorize;
using Model;

namespace FCBEM.Areas.Client.Pages
{
    public class IndexModel(UserManager<User> userManager, DatabaseContext context, IConfiguration configuration, ILogger<IndexModel> logger) : FCCore.Areas.Client.Pages.IndexModel(userManager, context, configuration, logger)
    {
    }
}
