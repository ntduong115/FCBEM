using Microsoft.AspNetCore.Identity;
using Model.Models.Authorize;
using Model;
using Core.Interfaces;

namespace FCBEM.Areas.Admin.Pages.Submission
{
    public class IndexModel(UserManager<User> userManager, IEmailSender iEmailSender, DatabaseContext context, IConfiguration configuration, IWebHostEnvironment environment) : FCCore.Areas.Admin.Pages.Submission.IndexModel(userManager, iEmailSender, context, configuration, environment)
    {
    }
}
