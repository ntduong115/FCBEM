using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using System.Globalization;
using FCBEM.Commons.Authorizations;
using Model.PaperModels;
using Model.Models.Authorize;
using Model;
using FCCore.PageModels;
using static Core.Commons.FCConstants;
using Core.Commons;
using Core.Helpers;
using Core.Models.Utility;
using Core.Interfaces;
using MimeKit.Utils;
using MimeKit;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace FCBEM.Areas.Client.Pages.Papers
{
    // Yêu cầu quyền truy cập bằng cách sử dụng AuthorizeCustomize và chỉ cho phép viết dữ liệu
    [AuthorizeCustomize(RoleName.Client)]
    public class CreateModel(UserManager<User>? userManager, IWebHostEnvironment environment, ILogger<CreateModel> logger, IEmailSender iEmailSender, DatabaseContext context, IConfiguration configuration) : IWritePageModel<Paper>(userManager, environment, context, configuration)
    {

        public List<Author> Authors { get; set; }
        public int TotalAuthors { get; set; }
        public DateTime Expired { get; set; }
        public SelectList SubmissionTypes { get; set; }
        [BindProperty]
        public int? AuthorRole { get; set; }

        // Thuộc tính được gắn thuộc tính BindProperty để gắn kết với dữ liệu nhập vào từ form
        [BindProperty]
        public decimal SumActualAmountSpent { get; set; }

        // Xử lý khi trang được tải lên

        public async Task<IActionResult> OnGetAsync()
        {
            if (DateTime.Now > new DateTime(2024, 10, 23, 6, 0, 0))
            {
                StatusMessage = new StatusMessage("Expire time",false).ToJSon();
                return RedirectToPage("./Index");
            }
            ModelState.Clear();
            StatusMessage = null;
            User user = await userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty) ?? new User();


            //Input = context.Papers.FirstOrDefault(p => p.UserId == user.Id);
            if (Input == null)
            {
                Input = new Paper
                {
                    FullName = user.Name
                };
            }
            Input.Submission = SubmissionType.FullPaper;
            SubmissionTypes = new SelectList(FCConstantsHelpers.SubmissionTypeSelectList, "Value", "Name", Input.Submission);
            return Page();
        }


        static string Conv(string input)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            string output = textInfo.ToTitleCase(input);
            return output;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Input.Submission = SubmissionType.FullPaper;
            // Lấy thông tin user từ post
            // Thực hiện gửi đề nghị
            try
            {

                var correspondingAuthor = AuthorRole.HasValue ? Input.Authors.ElementAt(AuthorRole.Value) : null;
                if (correspondingAuthor == null || correspondingAuthor.IsHidden)
                {
                    ModelState.Clear();
                    StatusMessage = new StatusMessage("Please choose Corresponding author.", false).ToJSon();
                    SubmissionTypes = new SelectList(FCConstantsHelpers.SubmissionTypeSelectList, "Value", "Name", Input.Submission);
                    return Page();
                }
                string mail = correspondingAuthor.Email;
                correspondingAuthor.IsCorresponding = true;
                var authors = Input.Authors.Where(c => !c.IsHidden).ToList();

                if (authors.GroupBy(g => g.Email).Count() < authors.Count)
                {
                    ModelState.Clear();
                    StatusMessage = new StatusMessage("Duplicate author mail.", false).ToJSon();
                    SubmissionTypes = new SelectList(FCConstantsHelpers.SubmissionTypeSelectList, "Value", "Name", Input.Submission);
                    return Page();
                }


                Input.Authors = authors;

                if (Input.Keywords.Split(',').Length is < 3 or > 5 || Input.Keywords.Split(',').Any(a => a.Trim().Length < 3))
                {
                    StatusMessage = new StatusMessage("Type 3-5 keywords here, separated by commas.", false).ToJSon();
                    SubmissionTypes = new SelectList(FCConstantsHelpers.SubmissionTypeSelectList, "Value", "Name", Input.Submission);
                    return Page();
                }

                Input.Status = PaperStatus.Pending;
                string message = "Paper has been submitted";
                User user = await userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty) ?? new User();
                Input.UserId = user.Id;


                if (Input.FormFile != null)
                {
                    string path = Path.Combine(PathUpload.PAPER, Methods.CombineSHA256(user.Id.ToString()));
                    string url = await FileManager.SaveFileAsync(Input.FormFile, path, environment.WebRootPath);
                    Input.File = Url.Content(Path.Combine(path, url));
                }

                logger.LogInformation("{userId} create paper", user.Id);

                context.Papers.Add(Input);
                context.SaveChanges();
                Input.PaperId = context.Papers.Count(c => c.VersionCode == ProjectName + ProjectYear);
                context.Entry(Input).Property(p => p.PaperId).IsModified = true;
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
                    string fileContents = System.IO.File.ReadAllText(Path.Combine(environment.WebRootPath, PathSource.MailContent, FileName.Submission));
                    string subject = string.Format(System.IO.File.ReadAllText(Path.Combine(environment.WebRootPath, PathSource.MailContent, FileName.SubmissionSubject)), $"{ProjectName} 20{ProjectYear}");
                    string htmlMessage = string.Format(fileContents, $"{ProjectName} 20{ProjectYear}", mimeEntity1.ContentId, mimeEntity2.ContentId, mimeEntity3.ContentId);
                    builder.HtmlBody = htmlMessage;
                    await iEmailSender.SendEmailAsync(mail, subject, builder);
                }
                catch (IOException ioExp)
                {
                    SubmissionTypes = new SelectList(FCConstantsHelpers.SubmissionTypeSelectList, "Value", "Name", Input.Submission);
                    StatusMessage = new StatusMessage(ioExp.Message).ToJSon();
                    return Page();
                }
                StatusMessage = new StatusMessage(message).ToJSon();
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                SubmissionTypes = new SelectList(FCConstantsHelpers.SubmissionTypeSelectList, "Value", "Name", Input.Submission);
                // Xử lý lỗi nếu có
                StatusMessage = new StatusMessage("Error: " + ex.Message, false).ToJSon();
                return Page();
            }
        }

    }

}
