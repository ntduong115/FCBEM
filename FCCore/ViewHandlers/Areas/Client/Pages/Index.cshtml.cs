using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FCCore.Commons.Authorizations;
using FCCore.PageModels;
using Model.Models.Authorize;
using Model;
using Core.Commons;
using Core.Models.Utility;
using static Core.Commons.FCConstants;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace FCCore.Areas.Client.Pages
{
    [AuthorizeCustomize(RoleName.Client)]
    public class IndexModel(UserManager<User> userManager, DatabaseContext context, IConfiguration configuration, ILogger logger) : IUserPageModel(userManager, context, configuration)
    {
        [BindProperty]
        public InputModel Input { set; get; }
        public async Task<IActionResult> OnGet()
        {
            StatusMessage = null;
            var id = userManager.GetUserId(User);
            var result = await userManager.FindByIdAsync(id);
            if (result != null)
            {
                Input = Methods.MapProperties(result, new InputModel());
                Input.ID = result.Id;
                ViewData["Title"] = "Cập nhật tài khoản : " + Input.Name;
                ModelState.Clear();
            }
            else
            {
                StatusMessage = new StatusMessage("Error: Không có thông tin về tài khoản ID = " + id, false).ToJSon();
            }

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {

            if (!ModelState.IsValid)
            {
                StatusMessage = null;
                return Page();
            }

            // CẬP NHẬT
            if (Input.ID == null)
            {
                ModelState.Clear();
                StatusMessage = new StatusMessage("Error: Không có thông tin về tài khoản").ToJSon();
                return Page();
            }

            logger.LogInformation("{userId} update information", User.Identity.Name);
            var id = userManager.GetUserId(User);
            var result = await userManager.FindByIdAsync(id);
            if (result != null)
            {
                Input.Code = null;
                result = Methods.MapProperties(Input, result);
                // Cập nhật tài khoản
                var roleUpdateRs = await userManager.UpdateAsync(result);
                if (roleUpdateRs.Succeeded)
                {
                    StatusMessage = new StatusMessage("Đã cập nhật thông tin tài khoản thành công").ToJSon();
                }
                else
                {
                    string error = "Error: ";
                    foreach (var er in roleUpdateRs.Errors)
                    {
                        error += er.Description;
                    }
                }
                return Page();
            }
            else
            {
                StatusMessage = new StatusMessage("Error: Không tìm thấy tài khoản cập nhật").ToJSon();
                return Page();
            }

        }
    }
}
