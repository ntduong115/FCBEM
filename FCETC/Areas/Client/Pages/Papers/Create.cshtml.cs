using Microsoft.AspNetCore.Identity;
using Model.Models.Authorize;
using Model;
using Core.Interfaces;

namespace FCETC.Areas.Client.Pages.Papers
{
    public class CreateModel(UserManager<User>? userManager, IWebHostEnvironment environment, ILogger<CreateModel> logger, IEmailSender iEmailSender, DatabaseContext context, IConfiguration configuration) : FCCore.Areas.Client.Pages.Papers.CreateModel(userManager, environment, logger, iEmailSender, context, configuration)
    {
        public override void CheckExpired()
        {
            IsExpired = DateTime.Now > new DateTime(2025, 10, 23, 6, 0, 0);
            IsExpiredSubmission = DateTime.Now > new DateTime(2025, 10, 23, 6, 0, 0);
        }
    }

}
