using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using OfficeOpenXml;
using FCCore.Commons.Authorizations;
using static Core.Commons.FCConstants;
using FCCore.PageModels;
using Model;

using Model.Models.Authorize;
using Core.Interfaces;
using Model.Registrations;
using Core.Models.Utility;
using MimeKit.Utils;
using MimeKit;
using System.IO.Compression;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;

namespace FCCore.Areas.Admin.Pages.Registrations
{
    [AuthorizeCustomize(RoleName.Admin)]
    public class IndexModel(UserManager<User> userManager, IEmailSender iEmailSender, DatabaseContext context, IConfiguration configuration, IWebHostEnvironment environment) : IReadPageModel<Registration>(userManager, context, configuration)
    {
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        public IActionResult OnGet()
        {
            HasListData = true;
            return Page();
        }
        public async Task<IActionResult> OnPostApprove()
        {
            Registration registration = context.Registrations.FirstOrDefault(i => i.Id == Id);
            User user = await userManager.FindByIdAsync(registration.UserId.ToString());
            registration.Status = RegistrationStatus.Completed;
            context.Entry(registration).State = EntityState.Modified;
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
                string fileContents = System.IO.File.ReadAllText(Path.Combine(environment.WebRootPath, PathSource.MailContent, FileName.RegistrationComplete));
                string subject = string.Format(System.IO.File.ReadAllText(Path.Combine(environment.WebRootPath, PathSource.MailContent, FileName.RegistrationCompleteSubject)), $"{ProjectName} 20{ProjectYear}");
                string htmlMessage = string.Format(fileContents, $"{ProjectName} 20{ProjectYear}", mimeEntity1.ContentId, mimeEntity2.ContentId, mimeEntity3.ContentId);
                builder.HtmlBody = htmlMessage;
                await iEmailSender.SendEmailAsync(user.Email, subject, builder);
            }
            catch (IOException ioExp)
            {
                StatusMessage = new StatusMessage(ioExp.Message).ToJSon();
                return Page();
            }

            StatusMessage = new StatusMessage("Approve Successfully!", true).ToJSon();
            return RedirectToPage("./Index");
        }

        public IActionResult OnPostDownload()
        {
            string zipFileName = $"Registrations_{DateTime.Now:yyyyMMddHHmmss}.zip";
            string zipFilePath = Path.Combine(environment.WebRootPath, PathUpload.TEMP, zipFileName);

            // Tạo tập tin zip

            using (FileStream zipToCreate = new(zipFilePath, FileMode.Create))
            {
                using ZipArchive archive = new(zipToCreate, ZipArchiveMode.Create);
                var query = context.Registrations.AsQueryable();
                List<Registration> registrations = [.. query];
                foreach (Registration registration in registrations)
                {
                    List<string> paths = JsonConvert.DeserializeObject<List<string>>(registration.Files);
                    int index = 0;
                    foreach (string path in paths)
                    {
                        archive.CreateEntryFromFile(Path.Combine(environment.WebRootPath, path), $"{registration.RegistrationIdText}_{++index}_{Path.GetExtension(path)}");
                    }

                }
            }

            // Trả về file zip cho người dùng
            var result = PhysicalFile(zipFilePath, "application/zip", zipFileName);

            return result;
        }

        static string Conv(string input)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            string output = textInfo.ToTitleCase(input);
            return output;
        }

        public async Task<IActionResult> OnPostRejectAsync()
        {
            Registration registration = context.Registrations.FirstOrDefault(i => i.Id == Id);
            User user = await userManager.FindByIdAsync(registration.UserId.ToString());
            registration.Status = RegistrationStatus.Reject;
            context.Entry(registration).State = EntityState.Modified;
            context.SaveChanges();

            StatusMessage = new StatusMessage("Reject Successfully!", true).ToJSon();
            return RedirectToPage("./Index");
        }

        public override IQueryable<Registration> Include(IQueryable<Registration> listData)
        {
            listData = listData.Include(i => i.User).Include(i => i.Paper);
            return base.Include(listData);
        }
        public override IQueryable<Registration> Where(IQueryable<Registration> listData)
        {
            return listData;
        }
        public override IQueryable<Registration> Sort(IQueryable<Registration> listData)
        {
            return base.Sort(listData.OrderByDescending(o => o.CreatedDate));
        }
        public IActionResult OnPostExcel()
        {

            var data1 = context.Registrations.Include(i => i.Paper).ToList();

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var sheet = package.Workbook.Worksheets.Add("Registration");
                // Điền dữ liệu vào sheet
                sheet.Cells[1, 1].Value = nameof(Registration.FirstName);
                sheet.Cells[1, 2].Value = nameof(Registration.MiddleName);
                sheet.Cells[1, 3].Value = nameof(Registration.LastName);
                sheet.Cells[1, 4].Value = nameof(Registration.Position);
                sheet.Cells[1, 5].Value = nameof(Registration.StudentId);
                sheet.Cells[1, 6].Value = nameof(Registration.Affilication);
                sheet.Cells[1, 7].Value = nameof(Registration.CountryOrRegion);
                sheet.Cells[1, 8].Value = nameof(Registration.Email);
                sheet.Cells[1, 9].Value = nameof(Registration.TelephoneNumber);
                sheet.Cells[1, 10].Value = nameof(Registration.RegistrationType);
                sheet.Cells[1, 11].Value = nameof(Registration.PaperId);
                sheet.Cells[1, 12].Value = nameof(Registration.Total);
                sheet.Cells[1, 13].Value = nameof(Registration.BillTo);
                sheet.Cells[1, 14].Value = nameof(Registration.AnyRemark);
                sheet.Cells[1, 15].Value = nameof(Registration.Diet);
                sheet.Cells[1, 16].Value = nameof(Registration.DietComment);
                sheet.Cells[1, 17].Value = nameof(Registration.Status);


                int rowIdx = 2;
                for (int i = 0; i < data1.Count; ++i)
                {
                    var ab = data1[i];
                    sheet.Cells[i + 2, 1].Value = ab.FirstName;
                    sheet.Cells[i + 2, 2].Value = ab.MiddleName;
                    sheet.Cells[i + 2, 3].Value = ab.LastName;
                    sheet.Cells[i + 2, 4].Value = ab.Position;
                    sheet.Cells[i + 2, 5].Value = ab.StudentId;
                    sheet.Cells[i + 2, 6].Value = ab.Affilication;
                    sheet.Cells[i + 2, 7].Value = ab.CountryOrRegion;
                    sheet.Cells[i + 2, 8].Value = ab.Email;
                    sheet.Cells[i + 2, 9].Value = ab.TelephoneNumber;
                    sheet.Cells[i + 2, 10].Value = ab.RegistrationType;
                    sheet.Cells[i + 2, 11].Value = ab.Paper == null ? string.Empty : ab.Paper.PaperIDText;
                    sheet.Cells[i + 2, 12].Value = ab.Total;
                    sheet.Cells[i + 2, 13].Value = ab.BillTo;
                    sheet.Cells[i + 2, 14].Value = ab.AnyRemark;
                    sheet.Cells[i + 2, 15].Value = ab.Diet;
                    sheet.Cells[i + 2, 16].Value = ab.DietComment;
                    sheet.Cells[i + 2, 17].Value = ab.Status;
                }
                // Lưu file Excel vào stream
                package.Save();
            }

            stream.Position = 0;
            var fileName = $"Registration_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";

            // Thêm header để trình duyệt tự động tải file xuống
            Response.Headers.Add("Content-Disposition", $"attachment; filename=\"{fileName}\"");

            // Trả về file với Content-Type phù hợp
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
    }
}
