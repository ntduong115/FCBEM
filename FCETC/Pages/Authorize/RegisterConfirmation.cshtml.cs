using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using FCCore.PageModels;
using Model;
using Model.Models.Authorize;
using FCCore.ViewComponents;

namespace FCETC.Pages.Authorize
{
    [AllowAnonymous]
    public class RegisterConfirmationModel(UserManager<User> userManager, DatabaseContext context, IConfiguration configuration) : IChangePageModel(context, configuration)
    {
        public string Email { get; set; }

        public string UrlContinue { set; get; }


        public async Task<IActionResult> OnGetAsync(string email, string? returnUrl = null)
        {
            if (email == null)
            {
                return RedirectToPage($"~/RegisterConfirmation");
            }


            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"No User with email: '{email}'.");
            }

            if (user.EmailConfirmed)
            {
                returnUrl ??= HttpContext.Request.PathBase.Value != string.Empty ? HttpContext.Request.PathBase.Value : "/";
                return ViewComponent(MessagePageViewComponent.COMPONENTNAME,
                        new MessagePageViewComponent.Message()
                        {
                            Title = "MessagePage",
                            Htmlcontent = "User is confirmed, please wait to redirect",
                            Urlredirect = returnUrl,
                            ProjectName = ProjectName,
                            ProjectYear = ProjectYear,
                        }

                );
            }

            Email = email;

            if (returnUrl != null)
            {
                UrlContinue = Url.Page($"~/RegisterConfirmation", new { email = Email, returnUrl });
            }
            else
                UrlContinue = Url.Page($"~/RegisterConfirmation", new { email = Email });


            return Page();
        }
    }
}
