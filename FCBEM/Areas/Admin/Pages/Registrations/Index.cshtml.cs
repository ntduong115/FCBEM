using Microsoft.AspNetCore.Identity;
using Model;

using Model.Models.Authorize;
using Core.Interfaces;

namespace FCBEM.Areas.Admin.Pages.Registrations
{
    public class IndexModel(UserManager<User> userManager, IEmailSender iEmailSender, DatabaseContext context, IConfiguration configuration, IWebHostEnvironment environment) : FCCore.Areas.Admin.Pages.Registrations.IndexModel(userManager, iEmailSender, context, configuration, environment)
    {
    }
}
