using Model;
using Model.Models.Authorize;
using Microsoft.AspNetCore.Identity;

namespace FCBEM.Areas.Admin.Pages.News
{
    public class CreateModel(UserManager<User> userManager, IWebHostEnvironment environment, DatabaseContext context, IConfiguration configuration) : FCCore.Areas.Admin.Pages.News.CreateModel(userManager, environment, context, configuration)
    {
    }

}
