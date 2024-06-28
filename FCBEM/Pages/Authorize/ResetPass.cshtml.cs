using Core.Models.Utility;

using FCBEM24.Commons.PageModels;

using FCBEMModel;
using FCBEMModel.Models.Authorize;

using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Mvc;

using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace FCBEM24.Pages.Authorize
{
    public class ResetPassModel(DatabaseContext context, UserManager<User> userManager, IConfiguration configuration) : IChangePageModel(context, configuration)
    {
        [BindProperty]
        public ResetPassInputModel Input { get; set; } = new ResetPassInputModel(); // Initialize the Input object

        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Token { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Email { get; set; }

        public class ResetPassInputModel
        {
            public string Password { get; set; }

            public string ConfirmPassword { get; set; }

            [NotMapped]
            public string Mail { get; set; }
        }
        public async Task<IActionResult> OnGetAsync()
        {
            ModelState.Clear();
            string email = HttpUtility.HtmlDecode(this.Email);

            if (email != null)
            {
                User user = await userManager.FindByEmailAsync(email);

                Input.Password = string.Empty;
                Input.Mail = user.Email;

                if (user != null)
                {
                    /*ModelState.AddModelError(string.Empty, "Người dùng không tồn tại");*/
                    StatusMessage = new StatusMessage("Hello " + user.Name + ", Please enter your new password", true).ToJSon();

                    return Page();
                }
                else
                {
                    return Redirect($"~/");
                }
            }
            else
            {
                return Redirect("~/AccessDenied");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Clear();
            if (Input.Password != Input.ConfirmPassword)
            {
                StatusMessage = new StatusMessage("Password and ConfirmPassword do not match", false).ToJSon();
                return Page();
            }
            else if (Input.Password == null && Input.ConfirmPassword == null)
            {
                StatusMessage = new StatusMessage("Password and Confirm Password cannot be left blank", false).ToJSon();
                return Page();
            }
            User user = await userManager.FindByEmailAsync(Input.Mail);
            if (user != null && Input.Password == Input.ConfirmPassword)
            {
                var roleUpdateRs = await userManager.ResetPasswordAsync(user, HttpUtility.HtmlDecode(Token), Input.Password);

                if (roleUpdateRs.Succeeded)
                {
                    StatusMessage = new StatusMessage("Change password successfully, Please login again", true).ToJSon();
                    return Redirect($"~/{ProjectName + ProjectYear}/Login");
                }
                else
                {
                    string error = "Error: ";
                    foreach (var er in roleUpdateRs.Errors)
                    {
                        error += er.Description;
                    }
                }
            }
            return Page();
        }
    }
}
