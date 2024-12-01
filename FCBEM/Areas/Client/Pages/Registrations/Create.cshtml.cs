using Model;
using Model.Models.Authorize;

using Microsoft.AspNetCore.Identity;
using Core.Interfaces;

namespace FCBEM.Areas.Client.Pages.Registrations
{
    public class CreateModel(UserManager<User>? userManager, IWebHostEnvironment environment, ILogger<CreateModel> logger, DatabaseContext context, IConfiguration configuration, IEmailSender iEmailSender) : FCCore.Areas.Client.Pages.Registrations.CreateModel(userManager, environment, logger, context, configuration, iEmailSender)
    {
        public override void CheckExpired()
        {
            IsExpired = DateTime.Now > new DateTime(2024, 12, 02, 0, 0, 0);
        }
    }
}
