using Microsoft.AspNetCore.Identity;
using Model.Models.Authorize;
using Model;

namespace FCETC.Areas.Admin.Pages.News
{
    public class EditModel(UserManager<User> userManager, IWebHostEnvironment environment, DatabaseContext context, IConfiguration configuration) : FCCore.Areas.Admin.Pages.News.EditModel(userManager, environment, context, configuration)
    {
    }
}
