using Core.Interfaces;
using Model;
using Model.Models.Authorize;
using Microsoft.AspNetCore.Identity;

namespace FCBEM.Areas.Admin.Pages.Authors
{
    public class IndexModel(UserManager<User> userManager, IEmailSender emailSender, DatabaseContext context, IConfiguration configuration) : FCCore.Areas.Admin.Pages.Authors.IndexModel(userManager, emailSender, context, configuration)
    {
    }
}
