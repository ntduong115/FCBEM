using Microsoft.AspNetCore.Identity;

using Model;
using Model.Models.Authorize;

namespace FCETC.Areas.Admin.Pages.Manager
{
    public class CreateUserModel(UserManager<User> userManager, DatabaseContext context, IConfiguration configuration, ILogger<CreateUserModel> logger) : FCCore.Areas.Admin.Pages.Manager.CreateUserModel(userManager, context, configuration, logger)
    {
    }
}
