using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using static Core.Commons.FCConstants;
using FCCore.PageModels;
using Model.PaperModels;
using Model.Models.Authorize;
using FCAI.Commons.Authorizations;
using Model;
using Core.Interfaces;
using MimeKit.Utils;
using MimeKit;
using Core.Models.Utility;
using Core.Commons;
using OfficeOpenXml.Style;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO.Compression;

namespace FCAI.Areas.Admin.Pages.Submission
{
    [AuthorizeCustomize(RoleName.Admin)]
    public class IndexModel : IReadPageModel<Paper>
    {
        readonly IEmailSender iEmailSender;
        private readonly IWebHostEnvironment environment;
        public SelectList SubmissionTypes { get; set; }
        [BindProperty]
        public PaperStatus? Status { get; set; }
        public SelectList PaperStatuses { get; set; }

        [BindProperty]
        public bool IsAllSubmissonType { get; set; }



        public IndexModel(UserManager<User> userManager, IEmailSender iEmailSender, DatabaseContext context, IConfiguration configuration, IWebHostEnvironment environment) : base(userManager, context, configuration)
        {
            this.iEmailSender = iEmailSender;
            this.environment = environment;
            this.configuration = configuration;
        }
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }


        public IActionResult OnGet()
        {
            HasListData = true;
            if (Input == null || Input.Submission == null)
            {
                Input = new Paper()
                {
                    Submission = SubmissionType.AbstractPO,
                };
            }
            SubmissionTypes = new SelectList(FCConstantsHelpers.SubmissionTypeSelectList, "Value", "Name", Input.Submission);
            PaperStatuses = new SelectList(FCConstantsHelpers.PaperStatusSelectList, "Value", "Name");
            return Page();
        }
        //public IActionResult OnPost()
        //{
        //    HasListData = true;
        //    SubmissionTypes = new SelectList(FCConstantsHelpers.SubmissionTypeSelectList, "Value", "Name", Input.Submission);
        //    return Page();
        //}
        public async Task<IActionResult> OnPostApprove(int PageNumber)
        {
            Paper paper = context.Papers.FirstOrDefault(i => i.Id == Id);
            Author author = context.Authors.FirstOrDefault(i => i.PaperId == paper.Id && i.IsCorresponding);
            paper.Status = PaperStatus.Accepted;
            context.Entry(paper).State = EntityState.Modified;
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
                string fileContents = System.IO.File.ReadAllText(Path.Combine(environment.WebRootPath, PathSource.MailContent, paper.Submission == SubmissionType.FullPaper ? FileName.FullpaperAccept : FileName.AbstractAccept));
                string subject = string.Format(System.IO.File.ReadAllText(Path.Combine(environment.WebRootPath, PathSource.MailContent, paper.Submission == SubmissionType.FullPaper ? FileName.FullpaperAcceptSubject : FileName.AbstractAcceptSubject)), $"{ProjectName} 20{ProjectYear}");
                string htmlMessage = string.Format(fileContents, $"{ProjectName} 20{ProjectYear}", mimeEntity1.ContentId, mimeEntity2.ContentId, mimeEntity3.ContentId,
                    paper.PaperIDText, paper.ManuscriptTitle);
                builder.HtmlBody = htmlMessage;
                await iEmailSender.SendEmailAsync(author.Email, subject, builder);
            }
            catch (IOException ioExp)
            {
                StatusMessage = new StatusMessage(ioExp.Message).ToJSon();
                return RedirectToPage("./Index", new { Submission = Input.Submission, PageNumber });
            }


            return RedirectToPage("./Index", new { Submission = Input.Submission, PageNumber });
        }

        public async Task<IActionResult> OnPostRejectAsync(int PageNumber)
        {
            Paper paper = context.Papers.FirstOrDefault(i => i.Id == Id);
            Author author = context.Authors.FirstOrDefault(i => i.PaperId == paper.Id && i.IsCorresponding);
            paper.Status = PaperStatus.Reject;
            context.Entry(paper).State = EntityState.Modified;
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
                string fileContents = System.IO.File.ReadAllText(Path.Combine(environment.WebRootPath, PathSource.MailContent, paper.Submission == SubmissionType.FullPaper ? FileName.FullpaperReject : FileName.AbstractReject));
                string subject = string.Format(System.IO.File.ReadAllText(Path.Combine(environment.WebRootPath, PathSource.MailContent, paper.Submission == SubmissionType.FullPaper ? FileName.FullpaperRejectSubject : FileName.AbstractRejectSubject)), $"{ProjectName} 20{ProjectYear}");
                string htmlMessage = string.Format(fileContents, $"{ProjectName} 20{ProjectYear}", mimeEntity1.ContentId, mimeEntity2.ContentId, mimeEntity3.ContentId,
                    paper.PaperIDText, paper.ManuscriptTitle);
                builder.HtmlBody = htmlMessage;
                await iEmailSender.SendEmailAsync(author.Email, subject, builder);
            }
            catch (IOException ioExp)
            {
                StatusMessage = new StatusMessage(ioExp.Message).ToJSon();
                return RedirectToPage("./Index", new { Submission = Input.Submission, PageNumber });
            }



            return RedirectToPage("./Index", new { Submission = Input.Submission, PageNumber });
        }

        public IActionResult OnPostDownload()
        {
            string zipFileName = $"papers_{(Status.HasValue ? Enum.GetName(typeof(PaperStatus), Status) : "All")}_{(IsAllSubmissonType ? "All" : Input.Submission)}_{DateTime.Now:yyyyMMddHHmmss}.zip";
            string zipFilePath = Path.Combine(environment.WebRootPath, PathUpload.TEMP, zipFileName);

            // Tạo tập tin zip

            using (FileStream zipToCreate = new(zipFilePath, FileMode.Create))
            {
                using ZipArchive archive = new(zipToCreate, ZipArchiveMode.Create);
                var query = context.Papers.AsQueryable();
                if (Status.HasValue)
                {
                    query = query.Where(p => p.Status == Status);
                }
                if (!IsAllSubmissonType)
                {
                    query = query.Where(p => p.Submission == Input.Submission);
                }
                List<Paper> papers = [.. query];
                foreach (Paper paper in papers)
                {
                    archive.CreateEntryFromFile(Path.Combine(environment.WebRootPath, paper.File), paper.PaperIDText + Path.GetExtension(paper.File));
                }
            }

            // Trả về file zip cho người dùng
            var result = PhysicalFile(zipFilePath, "application/zip", zipFileName);

            return result;
        }

        public IActionResult OnPostExcel()
        {
            MemoryStream stream = ExportExcel(Input.Submission);


            var fileName = $"{Input.Submission}_{DateTime.Now:yyyyMMddHHmmss}.xlsx";

            // Thêm header để trình duyệt tự động tải file xuống
            Response.Headers.Add("Content-Disposition", $"attachment; filename=\"{fileName}\"");

            // Trả về file với Content-Type phù hợp
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
        public IActionResult OnPostExcelAll()
        {
            MemoryStream stream = ExportExcel();


            var fileName = $"All_{DateTime.Now:yyyyMMddHHmmss}.xlsx";

            // Thêm header để trình duyệt tự động tải file xuống
            Response.Headers.Add("Content-Disposition", $"attachment; filename=\"{fileName}\"");

            // Trả về file với Content-Type phù hợp
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        private MemoryStream ExportExcel(string submission = null)
        {
            var query = context.Papers.AsQueryable();
            if (submission != null)
            {
                query = query.Where(paper => paper.Submission == submission);
            }
            var data = query.Include(i => i.Authors).ToList();

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var sheet = package.Workbook.Worksheets.Add("Abstract");
                // Điền dữ liệu vào sheet
                sheet.Cells[1, 1].Value = "ManuscriptTitle";
                sheet.Cells[1, 2].Value = "Abstract";
                sheet.Cells[1, 3].Value = "FileName";
                sheet.Cells[1, 4].Value = "Status";
                sheet.Cells[1, 5].Value = "Keywords";
                sheet.Cells[1, 6].Value = "CreateDate";
                sheet.Cells[1, 7].Value = "PaperId";
                sheet.Cells[1, 8].Value = "SubmissionType";

                sheet.Cells[1, 12].Value = "FullName";
                sheet.Cells[1, 13].Value = "Country";
                sheet.Cells[1, 14].Value = "Affiliation";
                sheet.Cells[1, 15].Value = "Email";
                sheet.Cells[1, 16].Value = "Phone";
                sheet.Cells[1, 17].Value = "Role";

                sheet.Column(1).Width = 40;
                sheet.Column(2).Width = 80;
                sheet.Column(5).Width = 40;
                sheet.Column(6).Width = 25;
                sheet.Column(7).Width = 15;
                sheet.Column(8).Width = 40;
                sheet.Column(12).Width = 30;
                sheet.Column(13).Width = 15;
                sheet.Column(14).Width = 30;
                sheet.Column(15).Width = 45;
                sheet.Column(16).Width = 20;
                sheet.Column(17).Width = 25;
                sheet.Cells[1, 6].Style.Numberformat.Format = "";
                sheet.Column(6).Style.Numberformat.Format = "yyyy-MM-dd HH:mm:ss";
                SetBorder(sheet, 1, 17);

                int rowIdx = 2;
                for (int i = 0; i < data.Count; i++)
                {
                    var ab = data[i];
                    sheet.Cells[rowIdx, 1].Value = ab.ManuscriptTitle;
                    sheet.Cells[rowIdx, 2].Value = ab.Abstract;
                    sheet.Cells[rowIdx, 3].Value = ab.File;
                    sheet.Cells[rowIdx, 4].Value = ab.Status;
                    sheet.Cells[rowIdx, 5].Value = ab.Keywords;
                    sheet.Cells[rowIdx, 6].Value = ab.CreatedDate;
                    sheet.Cells[rowIdx, 7].Value = ab.PaperIDText;
                    sheet.Cells[rowIdx, 8].Value = FCConstantsHelpers.SubmissionTypeSelectList.FirstOrDefault(s => s.Value.ToString() == ab.Submission).Name;
                    for (int j = 0; j < ab.Authors.Count; j++)
                    {
                        var au = ab.Authors.ElementAt(j);

                        string fullName = $"{au.FirstName} {au.MiddleName} {au.LastName}";
                        sheet.Cells[rowIdx, 12].Value = fullName;
                        sheet.Cells[rowIdx, 13].Value = au.Country;
                        sheet.Cells[rowIdx, 14].Value = au.Affiliation;
                        sheet.Cells[rowIdx, 15].Value = au.Email;
                        sheet.Cells[rowIdx, 16].Value = au.Phone;
                        if (au.IsCorresponding)
                        {
                            sheet.Cells[rowIdx, 17].Value = "Corresponding author";
                        }
                        else
                        {
                            sheet.Cells[rowIdx, 17].Value = "Author";
                        }
                        sheet.Cells[rowIdx, 12].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        SetBorder(sheet, rowIdx, 17, 12);
                        ++rowIdx;
                    }
                    SetBorder(sheet, rowIdx - 1, 17);
                }
                // Lưu file Excel vào stream
                package.Save();
            }

            stream.Position = 0;

            return stream;
        }

        void SetBorder(ExcelWorksheet sheet, int row, int col, int colStart = 1)
        {
            for (int i = colStart; i <= col; ++i)
            {
                sheet.Cells[row, i].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            }
        }

        public override IQueryable<Paper> Include(IQueryable<Paper> listData)
        {
            listData = listData.Include(i => i.User);
            return base.Include(listData);
        }

        public override IQueryable<Paper> Where(IQueryable<Paper> listData)
        {
            listData = listData
                .Where(p => p.Submission == Input.Submission);
            return listData;
        }

        public override IQueryable<Paper> Sort(IQueryable<Paper> listData)
        {
            return base.Sort(listData.OrderByDescending(o => o.CreatedDate));
        }
    }
}
