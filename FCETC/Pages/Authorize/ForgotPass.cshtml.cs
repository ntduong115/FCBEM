using Core.Interfaces;
using Core.Models.Utility;

using FCCore.PageModels;

using Model;
using Model.Models.Authorize;

using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Mvc;

using MimeKit;
using MimeKit.Utils;

using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

using static Core.Commons.FCConstants;


namespace FCETC.Pages.Authorize
{

    public class ForgotPassModel(DatabaseContext context, IWebHostEnvironment environment, UserManager<User> userManager, IEmailSender iEmailSender, IConfiguration configuration) : IChangePageModel(context, configuration)
    {
        protected readonly IWebHostEnvironment environment = environment;

        [BindProperty]
        public ForgotPassInputModel Input { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }


        public class ForgotPassInputModel
        {

            public string Email { get; set; }

            [NotMapped]
            public string Domain { get; set; }



        }

        public IActionResult OnGetAsync()
        {
            ModelState.Clear();
            StatusMessage = null;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            User user = await userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                /*ModelState.AddModelError(string.Empty, "Người dùng không tồn tại");*/
                StatusMessage = new StatusMessage("Email does not exist", false).ToJSon();
                return Page();
            }

            try
            {
                string pathBase = HttpContext.Request.PathBase.Value != string.Empty ? HttpContext.Request.PathBase.Value : $"/{ProjectName}{ProjectYear}";
                string Domain = $"{Request.Scheme}://{Request.Host.Value}{pathBase}";
                string url = Domain + "/ResetPassword?token=" + HttpUtility.UrlEncode(await userManager.GeneratePasswordResetTokenAsync(user)) + "&email=" + HttpUtility.UrlEncode(Input.Email);

                var builder = new BodyBuilder();
                MimeEntity mimeEntity = builder.LinkedResources.Add(Path.Combine(environment.WebRootPath, PathSource.MailImage, "image.png"));
                mimeEntity.ContentId = MimeUtils.GenerateMessageId();
                string htmlMessage = $@"
                        Dear {user.Name},
                        <br/>
                        Recover password?, click here to recover:  <a href='{url}'>here</a>
                        <br/>
                        <br/>
                        Best regards,
                        <br/>
                        {ProjectName} 20{ProjectYear} Organizing Committee.
                        <img src=""cid:{mimeEntity.ContentId}"" />
                        ";
                string fileContents = System.IO.File.ReadAllText(Path.Combine(environment.WebRootPath, PathSource.MailContent, FileName.ForgetPassword));

                htmlMessage = string.Format(fileContents, user.Name, url, ProjectName, ProjectYear);

                builder.HtmlBody = htmlMessage;
                await iEmailSender.SendEmailAsync(user.Email, $"{ProjectName} 20{ProjectYear}", builder);

            }
            catch (Exception)
            {
                StatusMessage = new StatusMessage("An error occurred while sending mail", false).ToJSon();
                return Page();
            }

            StatusMessage = new StatusMessage("Password reset email has been sent, please check your inbox", true).ToJSon();
            return Page();
        }
    }
}
