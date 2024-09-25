using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using FCCore.PageModels;
using Model;
using Model.Models.Authorize;
using FCCore.ViewComponents;

namespace FCBEM.Pages.Authorize
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
                return RedirectToPage($"/RegisterConfirmation");
            }

            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"No User with email: '{email}'.");
            }

            if (user.EmailConfirmed)
            {
                return RedirectToPage("/Authorize/Register");
            }

            Email = email;

            UrlContinue = Url.Page($"/RegisterConfirmation", new { email = Email });


            return Page();
        }
    }
}
