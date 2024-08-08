using FCBEM24.Commons.PageModels;
using FCBEM24.ViewComponents;

using FCBEMModel.Models.Authorize;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace FCBEM24.Pages.Authorize
{

    [AllowAnonymous]
    public class LogoutModel(SignInManager<User> signInManager, ILogger<LogoutModel> logger, IConfiguration configuration) : IPageModel(configuration)
    {
        public IActionResult OnGet() => RedirectToPage($"~/");

        public async Task<IActionResult> OnPost(string? returnUrl = null)
        {
            if (!signInManager.IsSignedIn(User)) return RedirectToPage($"~/");

            await signInManager.SignOutAsync();
            logger.LogInformation($"User logged out {User.Identity.Name}");


            return ViewComponent(MessagePageViewComponent.COMPONENTNAME,
                new MessagePageViewComponent.Message()
                {
                    Title = "Signed out",
                    Htmlcontent = "Signed out successfully",
                    Urlredirect = (returnUrl != null) ? returnUrl : (HttpContext.Request.PathBase.Value != string.Empty ? HttpContext.Request.PathBase.Value : "/"),
                    ProjectName = ProjectName,
                    ProjectYear = ProjectYear,
                }
            );
        }
    }
}
