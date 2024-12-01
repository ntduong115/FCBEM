using Microsoft.AspNetCore.Identity;
using Model.Models.Authorize;
using Model;
using Core.Interfaces;

namespace FCETC.Areas.Client.Pages.Papers
{
    public class UpdateModel(UserManager<User>? userManager, IWebHostEnvironment environment, ILogger<UpdateModel> logger, IEmailSender iEmailSender, DatabaseContext context, IConfiguration configuration) : FCCore.Areas.Client.Pages.Papers.UpdateModel(userManager, environment, logger, iEmailSender, context, configuration)
    {
    }

}
