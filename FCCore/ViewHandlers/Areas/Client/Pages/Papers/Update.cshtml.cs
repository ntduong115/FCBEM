using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FCCore.Commons.Authorizations;
using Model.PaperModels;
using Model.Models.Authorize;
using Model;
using FCCore.PageModels;
using static Core.Commons.FCConstants;
using Core.Commons;
using Core.Helpers;
using Core.Models.Utility;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace FCCore.Areas.Client.Pages.Papers
{
    // Yêu cầu quyền truy cập bằng cách sử dụng AuthorizeCustomize và chỉ cho phép viết dữ liệu
    [AuthorizeCustomize(RoleName.Client)]
    public class UpdateModel(UserManager<User>? userManager, IWebHostEnvironment environment, ILogger logger, IEmailSender iEmailSender, DatabaseContext context, IConfiguration configuration) : IWritePageModel<Paper>(userManager, environment, context, configuration)
    {
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }
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
            ModelState.Clear();

            User user = await userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty) ?? new User();


            Input = context.Papers.Where(p => p.Id == Id).Include(i => i.Authors).FirstOrDefault();

            SubmissionTypes = new SelectList(FCConstantsHelpers.SubmissionTypeSelectList, "Value", "Name", SubmissionType.FullPaper);
            Input.Submission = SubmissionType.FullPaper;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {

                var correspondingAuthor = AuthorRole.HasValue ? Input.Authors.ElementAt(AuthorRole.Value) : null;
                if (correspondingAuthor == null || correspondingAuthor.IsHidden)
                {
                    ModelState.Clear();
                    StatusMessage = new StatusMessage("Please choose Corresponding author.", false).ToJSon();
                    SubmissionTypes = new SelectList(FCConstantsHelpers.SubmissionTypeSelectList, "Value", "Name", SubmissionType.FullPaper);
                    return Page();
                }
                correspondingAuthor.IsCorresponding = true;

                if (Input.Keywords.Split(',').Length is < 3 or > 5 && !Input.Keywords.Split(',').Any(a => a.Trim().Length < 3))
                {
                    StatusMessage = new StatusMessage("Type 3-5 keywords here, separated by commas.", false).ToJSon();
                    SubmissionTypes = new SelectList(FCConstantsHelpers.SubmissionTypeSelectList, "Value", "Name", SubmissionType.FullPaper);
                    return Page();
                }

                Input.Status = PaperStatus.Pending;
                string message = "Full Paper has been submitted";
                User user = await userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty) ?? new User();
                Input.UserId = user.Id;


                logger.LogInformation("{userId} update paper", user.Id);
                if (Input.FormFile != null)
                {
                    string path = Path.Combine(PathUpload.PAPER, Methods.CombineSHA256(user.Id.ToString()));
                    string url = await FileManager.SaveFileAsync(Input.FormFile, path, environment.WebRootPath);
                    Input.File = Url.Content(Path.Combine(path, url));
                }

                Paper paper = context.Papers.Where(p => p.Id == Id).Include(i => i.Authors).FirstOrDefault();
                string oldPath = paper.File;


                var authors = Input.Authors.Where(a => !a.IsHidden).Select((a, index) =>
                {
                    a.PaperId = Input.Id;
                    a.AuthorNum = index;
                    return a;
                }).ToList();

                context.Authors.RemoveRange(paper.Authors);

                paper.Authors = authors;
                paper.Submission = SubmissionType.FullPaper;
                paper.ManuscriptTitle = Input.ManuscriptTitle;
                paper.Abstract = Input.Abstract;
                paper.Keywords = Input.Keywords;
                paper.File = Input.File;
                paper.Status = PaperStatus.Pending;


                context.Papers.Attach(paper);
                context.Entry(paper).State = EntityState.Modified;
                await context.SaveChangesAsync();

                //   context.Authors.RemoveRange(oldPage.Authors);
                //   context.Authors.AddRange(authors.Select(a =>
                //   {
                //       a.PaperId = Input.Id;
                //       return a;
                //   }));
                //await   context.SaveChangesAsync();

                System.IO.File.Delete(Path.Combine(environment.WebRootPath, oldPath));

                //try
                //{
                //    var builder = new BodyBuilder();
                //    MimeEntity mimeEntity1 = builder.LinkedResources.Add(Path.Combine(environment.WebRootPath, PathSource.MailImage, FileName.Image1));
                //    MimeEntity mimeEntity2 = builder.LinkedResources.Add(Path.Combine(environment.WebRootPath, PathSource.MailImage, FileName.Image2));
                //    MimeEntity mimeEntity3 = builder.LinkedResources.Add(Path.Combine(environment.WebRootPath, PathSource.MailImage, FileName.Image3));
                //    mimeEntity1.ContentId = MimeUtils.GenerateMessageId();
                //    mimeEntity2.ContentId = MimeUtils.GenerateMessageId();
                //    mimeEntity3.ContentId = MimeUtils.GenerateMessageId();
                //    string fileContents = System.IO.File.ReadAllText(Path.Combine(environment.WebRootPath, PathSource.MailContent, FileName.Submission));
                //    string subject = string.Format(System.IO.File.ReadAllText(Path.Combine(environment.WebRootPath, PathSource.MailContent, FileName.SubmissionSubject)), $"{ProjectName} 20{ProjectYear}");
                //    string htmlMessage = string.Format(fileContents, $"{ProjectName} 20{ProjectYear}", mimeEntity1.ContentId, mimeEntity2.ContentId, mimeEntity3.ContentId);
                //    builder.HtmlBody = htmlMessage;
                //    await iEmailSender.SendEmailAsync(user.Email, subject, builder);
                //}
                //catch (IOException ioExp)
                //{
                //    SubmissionTypes = new SelectList(FCBEMConstantsHelpers.SubmissionTypeSelectList, "Value", "Name", SubmissionType.FullPaper);
                //    StatusMessage = new StatusMessage(ioExp.Message).ToJSon();
                //    return Page();
                //}
                StatusMessage = new StatusMessage(message).ToJSon();
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                SubmissionTypes = new SelectList(FCConstantsHelpers.SubmissionTypeSelectList, "Value", "Name", SubmissionType.FullPaper);
                // Xử lý lỗi nếu có
                StatusMessage = new StatusMessage("Error: " + ex.Message, false).ToJSon();
                return Page();
            }
        }

    }

}
