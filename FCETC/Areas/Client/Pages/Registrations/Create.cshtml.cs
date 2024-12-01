using Model;
using Model.Models.Authorize;

using Microsoft.AspNetCore.Identity;
using Core.Interfaces;

namespace FCETC.Areas.Client.Pages.Registrations
{
    public class CreateModel(UserManager<User>? userManager, IWebHostEnvironment environment, ILogger<CreateModel> logger, DatabaseContext context, IConfiguration configuration, IEmailSender iEmailSender) : FCCore.Areas.Client.Pages.Registrations.CreateModel(userManager, environment, logger, context, configuration, iEmailSender)
    {
        public override void CheckExpired()
        {
            IsExpired = DateTime.Now > new DateTime(2025, 10, 23, 6, 0, 0);
        }
    }
}
