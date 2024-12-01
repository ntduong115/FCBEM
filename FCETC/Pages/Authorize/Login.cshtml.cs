using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Core.Models.Utility;

using FCCore.PageModels;
using FCCore.ViewComponents;

using Model;
using Model.Models.Authorize;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using static Core.Commons.FCConstants;

namespace FCETC.Pages.Authorize
{
    [AllowAnonymous]
    public class LoginModel(SignInManager<User> signInManager, ILogger<LoginModel> logger, UserManager<User> userManager, DatabaseContext context, IConfiguration configuration) : IChangePageModel(context, configuration)
    {

        [BindProperty]
        public LoginInputModel Input { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Kiểm tra xem có người dùng đã đăng nhập và không có truy cập bị từ chối
            //if (HttpContext.User.Identity.IsAuthenticated)
            //{
            //    var role = HttpContext.User.FindFirstValue(ClaimTypes.Role);

            //    if (role == RoleName.Client)
            //    {
            //        return Redirect("~/");
            //    }
            //    else if (role == RoleName.Admin)
            //    {
            //        return Redirect("~/Admin/News");
            //    }
            //}

            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            //ReturnUrl ??= HttpContext.Request.PathBase.Value != string.Empty ? HttpContext.Request.PathBase.Value : $"/{ProjectName}{ProjectYear}";

            // Xóa cookie ngoại vi hiện có để đảm bảo quá trình đăng nhập sạch sẽ
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            // Xóa đăng nhập người dùng hiện tại
            await signInManager.SignOutAsync();



            ModelState.Clear();
            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                User? user = await userManager.FindByEmailAsync(Input.Email);

                if (user == null)
                {
                    StatusMessage = new StatusMessage("Invalid credentials", false).ToJSon();
                    return Page();
                }

                if (await userManager.IsLockedOutAsync(user))
                {
                    // Người dùng bị khóa, xử lý tùy thuộc vào yêu cầu của bạn
                    // Ví dụ: Hiển thị thông báo cho người dùng
                    StatusMessage = new StatusMessage("Account is locked! (Tài khoản của bạn đã bị khóa.)", false).ToJSon();
                    return Page();
                }
                // Gỡ bỏ việc theo dõi đối tượng User
                //_context.Entry(user).State = EntityState.Detached;

                var result = await signInManager.PasswordSignInAsync(user, Input.Password, false, false);

                if (result.Succeeded)
                {

                    await signInManager.SignInAsync(user, new AuthenticationProperties
                    {
                        IsPersistent = false,
                        ExpiresUtc = DateTime.Now.AddDays(1)
                    });
                    return RedirectToPage("/Index");

                }
                StatusMessage = new StatusMessage("Wrong password. Please try again or click Forget Password", false).ToJSon();
            }
            catch (Exception ex)
            {
                StatusMessage = new StatusMessage(ex.Message, false).ToJSon();
                logger.LogError(ex, ex.Message);
            }
            return Page();
        }

    }

    public class LoginInputModel
    {
        public required string Email { get; set; }
        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}
