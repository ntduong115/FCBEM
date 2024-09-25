using Core.Interfaces;

using FCCore.PageModels;

using Model;
using Model.Models.Authorize;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;


using System.Text;

using static Core.Commons.FCConstants;



namespace FCBEM.Pages.Authorize
{
    public class RegisterModel(
        UserManager<User> userManager,
        SignInManager<User> signInManager, RoleManager<Role> roleManager,
        ILogger<RegisterModel> logger,
        IEmailSender emailSender, DatabaseContext context, IConfiguration configuration) : IChangePageModel(context, configuration)
    {
        [BindProperty]
        public User Input { get; set; }


        public async Task OnGetAsync()
        {

        }

        private string GenerateRandomNumber(int length)
        {
            const string digits = "0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(digits, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        // Đăng ký tài khoản theo dữ liệu form post tới
        public async Task<IActionResult> OnPostAsync()
        {
            if (Input.Password != Input.ConfirmPassword)
            {
                ModelState.AddModelError(string.Empty, "ConfirmPassword and password do not match.");
                return Page();
            }

            if (Input.Password != null && Input.Email != null && Input.Name != null && Input.ConfirmPassword == Input.Password)
            {
                var i = Guid.NewGuid().ToString().Replace("-", "");
                var existingUser = await userManager.FindByEmailAsync(Input.Email);
                if (existingUser == null)
                {
                    // Tạo AppUser sau đó tạo User mới (cập nhật vào db)
                    var user = new User
                    {
                        UserName = i,
                        Password = Input.Password,
                        Email = Input.Email,
                        Code = i,
                        Name = Input.Name,
                        //PhoneNumber = GenerateRandomNumber(10),

                    };
                    var result = await userManager.CreateAsync(user, Input.Password);

                    if (result.Succeeded)
                    {
                        logger.LogInformation("Create success.");

                        var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                        var callbackUrl = Url.Page(
                            "/Authorize/RegisterConfirmation",
                            pageHandler: null,
                            values: new {userId = user.Id, code },
                            protocol: Request.Scheme);

                        //// Gửi email    
                        //await _emailSender.SendEmailAsync(Input.Email, "confirm mail email",
                        //    $"Create success <a href='{callbackUrl}'>click here</a>.");

                        //var role = roleManager.Roles.Where(r => r.Name == ).Select(r => r.Name).FirstOrDefault();

                        var resultAddRole = await userManager.AddToRoleAsync(user, RoleName.Client);

                        if (resultAddRole.Succeeded)
                        {

                            if (userManager.Options.SignIn.RequireConfirmedEmail)
                            {
                                return RedirectToPage($"/Authorize/RegisterConfirmation", new { email = Input.Email });
                            }
                            else
                            {
                                // Không cần xác thực - đăng nhập luôn
                                await signInManager.SignInAsync(user, isPersistent: false);
                                return RedirectToPage("/Index");
                            }
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
                    var resultAddRole = await userManager.AddToRoleAsync(existingUser, RoleName.Client);
                    if (resultAddRole.Succeeded)
                    {

                        if (userManager.Options.SignIn.RequireConfirmedEmail)
                        {
                            return RedirectToPage("/Index", new { email = Input.Email });
                        }
                        else
                        {
                            // Không cần xác thực - đăng nhập luôn
                            await signInManager.SignInAsync(existingUser, isPersistent: false);
                            return RedirectToPage("/Index");
                        }
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



