using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Core.Models.Utility;

using FCBEM24.Commons.PageModels;
using FCBEM24.ViewComponents;

using FCBEMModel;
using FCBEMModel.Models.Authorize;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using static Core.Commons.FCBEMConstants;

namespace FCBEM24.Pages.Authorize
{
    public class LoginModel(SignInManager<User> signInManager, ILogger<LoginModel> logger, UserManager<User> userManager, DatabaseContext context, IConfiguration configuration) : IChangePageModel(context, configuration)
    {
        public class LoginInputModel
        {
            public string Email { get; set; }
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        [BindProperty]
        public LoginInputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool AccesDenine { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Kiểm tra xem có người dùng đã đăng nhập và không có truy cập bị từ chối
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var role = HttpContext.User.FindFirstValue(ClaimTypes.Role);

                if (role == RoleName.Client)
                {
                    return Redirect("~/");
                }
                else if (role == RoleName.Admin)
                {
                    return Redirect("~/Admin/News");
                }
            }

            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            ReturnUrl ??= HttpContext.Request.PathBase.Value != string.Empty ? HttpContext.Request.PathBase.Value : $"/{ProjectName}{ProjectYear}";

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
                ReturnUrl ??= HttpContext.Request.PathBase.Value != string.Empty ? HttpContext.Request.PathBase.Value : $"/{ProjectName}{ProjectYear}";

                if (!ModelState.IsValid)
                {
                    return Page();
                }

                User user = await userManager.FindByEmailAsync(Input.Email);
                if (user != null)
                {
                    // Gỡ bỏ việc theo dõi đối tượng User
                    //_context.Entry(user).State = EntityState.Detached;

                    var result = await signInManager.PasswordSignInAsync(user, Input.Password, true, false);

                    if (result.Succeeded)
                    {
                        return ViewComponent(MessagePageViewComponent.COMPONENTNAME, new MessagePageViewComponent.Message()
                        {
                            Title = "Logged in",
                            Htmlcontent = "Log in success",
                            Urlredirect = ReturnUrl,
                            ReturnUrl = null,
                            ProjectName = ProjectName,
                            ProjectYear = ProjectYear,
                        });
                    }
                }
                StatusMessage = new StatusMessage("Wrong password. Please try again or click Forget Password", false).ToJSon();
            }
            catch (Exception ex)
            {
                //StatusMessage = new StatusMessage(ex.Message, false).ToJSon();
                //_logger.LogError(ex, ex.Message);
            }
            return Page();
        }

    }
}
