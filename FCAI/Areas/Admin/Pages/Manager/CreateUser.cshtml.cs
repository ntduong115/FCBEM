using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using static Core.Commons.FCConstants;
using FCCore.PageModels;
using Model;
using Model.Models.Authorize;
using Core.Models.Utility;
using FCAI.Commons.Authorizations;

namespace FCAI.Areas.Admin.Pages.Manager
{
    [AuthorizeCustomize(RoleName.Admin)]
    public class CreateUserModel(UserManager<User> userManager, DatabaseContext context, IConfiguration configuration, ILogger<CreateUserModel> logger) : IChangePageModel(context, configuration)
    {

        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string Name { get; set; }
        [BindProperty]
        public string Password { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Password != null && Email != null && Name != null)
            {
                var i = Guid.NewGuid().ToString().Replace("-", "");
                var existingUser = await userManager.FindByEmailAsync(Email);
                if (existingUser == null)
                {
                    // Tạo AppUser sau đó tạo User mới (cập nhật vào db)
                    var user = new User
                    {
                        UserName = i,
                        Password = Password,
                        Email = Email,
                        Code = i,
                        Name = Name,
                        //PhoneNumber = GenerateRandomNumber(10),

                    };
                    var result = await userManager.CreateAsync(user, Password);

                    if (result.Succeeded)
                    {
                        logger.LogInformation("Create success.");

                        var resultAddRole = await userManager.AddToRoleAsync(user, RoleName.Admin);

                        if (resultAddRole.Succeeded)
                        {
                            StatusMessage = new StatusMessage("Create success.").ToJSon();
                            return Page();
                        }
                        foreach (var error in resultAddRole.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                    // Có lỗi, đưa các lỗi thêm user vào ModelState để hiện thị ở html heleper: asp-validation-summary
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                else
                {
                    var resultAddRole = await userManager.AddToRoleAsync(existingUser, RoleName.Admin);
                    if (resultAddRole.Succeeded)
                    {
                        StatusMessage = new StatusMessage("Create success.").ToJSon();
                        return Page();
                    }
                    foreach (var error in resultAddRole.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

            }

            return Page();
        }
    }
}
