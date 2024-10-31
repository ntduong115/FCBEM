using Core.Commons;
using Core.Models.Utility;

using Core.Helpers;
using FCBEM.Commons.Authorizations;
using FCCore.PageModels;

using Model;
using Model.Models.Authorize;
using Model.Registrations;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using MimeKit.Utils;
using MimeKit;

using Newtonsoft.Json;

using System.Security.Claims;

using static Core.Commons.FCConstants;
using Core.Interfaces;


namespace FCBEM.Areas.Client.Pages.Registrations
{
    [AuthorizeCustomize(RoleName.Client)]
    public class CreateModel(UserManager<User>? userManager, IWebHostEnvironment environment, ILogger<CreateModel> logger, DatabaseContext context, IConfiguration configuration, IEmailSender iEmailSender) : IWritePageModel<Registration>(userManager, environment, context, configuration)
    {
        public DateTime Expired { get; set; }
        public SelectList Positions { get; set; }
        public SelectList PresentationTypes { get; set; }
        public SelectList RegistrationTypes { get; set; }
        public SelectList DietTypes { get; set; }
        public SelectList Papers { get; set; }

        public IActionResult OnGet()
        {
            if (DateTime.Now > new DateTime(2024, 11, 1, 6, 0, 0))
            {
                StatusMessage = new StatusMessage("Expire time", false).ToJSon();
                return RedirectToPage("./Index");
            }
            _ = Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid user);
            CreateSelectList(user);
            ModelState.Clear();
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid userId);
            if (!ModelState.IsValid)
            {
                CreateSelectList(userId);
                return Page();
            }

            logger.LogInformation("{userId} Create Registraion", userId);
            string error = string.Empty;
            if (Input.RegistrationType == RegistrationType.Author)
            {
                if (Input.PresentationType == null)
                {
                    error += "The Presentation Type field is required.<br/>";
                }
                if (Input.PaperId == null)
                {
                    error += "The Paper Id field is required.<br/>";
                }

                if (Input.AuthorRegular5VND == 0 && Input.AuthorRegular45VND == 0 && Input.AuthorRegular3USD == 0 && Input.AuthorRegular25USD == 0)
                {
                    error += "Registration fee is required.<br/>";
                }
                else if ((Input.AuthorRegular5VND > 0 || Input.AuthorRegular45VND > 0) && (Input.AuthorRegular3USD > 0 || Input.AuthorRegular25USD > 0))
                {
                    error += "Registration fee is only USD or VND.<br/>";
                }

                if (!string.IsNullOrEmpty(error))
                {
                    CreateSelectList(userId);
                    StatusMessage = new StatusMessage(error, false).ToJSon();
                    return Page();
                }

                Input.Listener1USD = 0;
                Input.Listener1VND = 0;
            }
            else
            {

                if (Input.Listener1USD == 0 && Input.Listener1VND == 0)
                {
                    error += "Registration fee is required.<br/>";
                }
                else if (Input.Listener1USD > 0 && Input.Listener1VND > 0)
                {
                    error += "Registration fee is only USD or VND.<br/>";
                }

                if (!string.IsNullOrEmpty(error))
                {
                    CreateSelectList(userId);
                    StatusMessage = new StatusMessage(error, false).ToJSon();
                    return Page();
                }
                Input.PaperId = null;
                Input.PresentationType = null;

                Input.AuthorRegular5VND = 0;
                Input.AuthorRegular45VND = 0;
                Input.AuthorRegular3USD = 0;
                Input.AuthorRegular25USD = 0;
            }

            if (Input.FormFile != null)
            {
                string userPath = userId.ToString().Replace("-", "");
                string path = Path.Combine(PathUpload.REGISTRAION, userPath);
                bool exists = Directory.Exists(Path.Combine(environment.WebRootPath, path));

                if (!exists)
                    Directory.CreateDirectory(Path.Combine(environment.WebRootPath, path));
                List<string> urls = await FileManager.SaveFilesAsyncToServer(Input.FormFile, path, environment.WebRootPath);
                Input.Files = JsonConvert.SerializeObject(urls.Select(u => Url.Content(Path.Combine(PathUpload.REGISTRAION, userPath, u))));
            }

            Input.Status = RegistrationStatus.Pending;
            string message = "Registration has been submitted";
            Input.UserId = userId;

            context.Registrations.Add(Input);
            context.SaveChanges();
            Input.RegistrationId = context.Registrations.Count(c => c.VersionCode == ProjectName + ProjectYear);
            context.Entry(Input).Property(p => p.RegistrationId).IsModified = true;
            context.SaveChanges();

            try
            {
                var builder = new BodyBuilder();
                MimeEntity mimeEntity1 = builder.LinkedResources.Add(Path.Combine(environment.WebRootPath, PathSource.MailImage, FileName.Image1));
                MimeEntity mimeEntity2 = builder.LinkedResources.Add(Path.Combine(environment.WebRootPath, PathSource.MailImage, FileName.Image2));
                MimeEntity mimeEntity3 = builder.LinkedResources.Add(Path.Combine(environment.WebRootPath, PathSource.MailImage, FileName.Image3));
                mimeEntity1.ContentId = MimeUtils.GenerateMessageId();
                mimeEntity2.ContentId = MimeUtils.GenerateMessageId();
                mimeEntity3.ContentId = MimeUtils.GenerateMessageId();
                string fileContents = System.IO.File.ReadAllText(Path.Combine(environment.WebRootPath, PathSource.MailContent, FileName.RegistrationSent));
                string subject = string.Format(System.IO.File.ReadAllText(Path.Combine(environment.WebRootPath, PathSource.MailContent, FileName.RegistrationSentSubject)), $"{ProjectName} 20{ProjectYear}");
                string htmlMessage = string.Format(fileContents, $"{ProjectName} 20{ProjectYear}", mimeEntity1.ContentId, mimeEntity2.ContentId, mimeEntity3.ContentId);
                builder.HtmlBody = htmlMessage;
                User user = await userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty) ?? new User();
                await iEmailSender.SendEmailAsync(Input.Email, subject, builder);
            }
            catch (Exception ioExp)
            {
                StatusMessage = new StatusMessage(ioExp.Message).ToJSon();
                logger.LogError(ioExp, "send mail");
                return Page();
            }

            StatusMessage = new StatusMessage(message).ToJSon();
            return RedirectToPage("./Index");
        }

        private void CreateSelectList(Guid user)
        {
            Positions = new SelectList(FCConstantsHelpers.PositionSelectList, "Value", "Name", null);
            PresentationTypes = new SelectList(FCConstantsHelpers.PresentationTypeSelectList, "Value", "Name", null);
            RegistrationTypes = new SelectList(FCConstantsHelpers.RegistrationTypeSelectList, "Value", "Name");
            DietTypes = new SelectList(FCConstantsHelpers.DietTypeSelectList, "Value", "Name");
            Papers = new SelectList(context.Papers.Where(p => p.UserId == user && p.VersionCode == ProjectName + ProjectYear && p.Status == PaperStatus.Accepted), "Id", "TitleSelectList");
        }
    }
}
