using System.Drawing;
using System.Drawing.Imaging;
using Core.Commons;
using Core.Models.Utility;
using FCBEM24.Commons.Authorizations;
using FCBEM24.Commons.PageModels;
using FCBEMModel;
using FCBEMModel.Models.Authorize;
using FCBEMModel.PaperModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using static Core.Commons.FCBEMConstants;

namespace FCBEM24.Areas.Admin.Pages.News
{
    [AuthorizeCustomize(RoleName.Admin)]
    public class CreateModel(UserManager<User> userManager, IWebHostEnvironment environment, DatabaseContext context, IConfiguration configuration) : INewsPageModelFCBEM(userManager, context, configuration)
    {
        protected readonly IWebHostEnvironment environment = environment;

        public IActionResult OnGet()
        {
            StatusMessage = new StatusMessage("Please enter information to create News").ToJSon();
            ModelState.Clear();
            Input = new PaperNew
            {
                CreatedDate = DateTime.Now
            };
            return Page();
        }



        // Cập nhật hoặc thêm mới tùy thuộc vào IsUpdate
        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                //tải lên ảnh bé cho bài viết
                if (FileUpload != null)
                {
                    using (Image newImage = Image.FromStream(FileUpload.OpenReadStream()))
                    {
                        //giảm chất lượng ảnh xuống
                        ImageCodecInfo encoder = Methods.GetEncoder(newImage.RawFormat);
                        var encParams = new EncoderParameters(1);
                        encParams.Param[0] = new EncoderParameter(Encoder.Quality, 90L);
                        string fileName = DateTime.Now.ToString("yyyyMMddhhmmssfff") + "_" + FileUpload.FileName;
                        fileName = fileName.Replace(" ", string.Empty);
                        string Path = System.IO.Path.Combine(environment.WebRootPath, PathUpload.NEWS, fileName);

                        string Path2 = System.IO.Path.Combine(PathUpload.NEWS_THUMB, "M_" + fileName);
                        //if (article.IsEmagazine == true)
                        Methods.ResizeImage(newImage, ImageSize.SMALL, false).Save(Path, encoder, encParams);
                        Methods.ResizeImage(newImage, ImageSize.MEDIUM, false).Save(System.IO.Path.Combine(environment.WebRootPath, Path2), encoder, encParams);
                        //else
                        //   newImage.Save(Path);
                        //string urlHost = Request.Url.GetLeftPart(UriPartial.Authority); //lấy địa chỉ host -- bỏ dòng này trong code insert
                        string urlDirect = "/" + Path2.Replace("\\", "/"); //lấy dòng này đưa vào img src
                                                                           // lấy đường dẫn local để lưu vào database
                        System.Diagnostics.Debug.WriteLine("Image Direct: " + urlDirect);
                        Input.ThumbImage = urlDirect;
                    }
                }

                Input.Title = Input.Title;
                Input.Abstract = Input.Abstract;
                Input.Content = Input.Content;

                //đặt ngày giờ tạo bài
                if (Input.CreatedDate == DateTime.MinValue)
                {
                    Input.CreatedDate = DateTime.Now;
                }

                Input.UpdatedDate = DateTime.Now;

                //đặt trạng thái bài viết
                //mặc định khi tạo là chưa được duyệt(0), admin thì đã đăng (1)

                Input.Status = ArticleStatus.Show;


                Input.Slug = Input.Title.Slugify();


                //lưu vào db
                context.PaperNews.Add(Input);
                context.SaveChanges();

                //lấy mảng các id của các bài liên quan
                //if (ars != null)
                //{
                //    int rows = addAllRelatedArticle(article.Id, ars);
                //    System.Diagnostics.Debug.WriteLine("Affected rows: " + rows);
                //}
                //else
                //{
                //    System.Diagnostics.Debug.WriteLine("No related articles was selected! -> leaving null");
                //}
                //tạo thành công -> chuyển đến trang index
                StatusMessage = new StatusMessage("Tạo tin tức thành công").ToJSon();
                return RedirectToPage("./Index");
            }

            //ViewBag.AuthorId = new SelectList(db.Users, "Id", "Name", article.AuthorId);
            //ViewBag.CatId = new SelectList(db.Categories, "Id", "Name", article.CatId);
            //ViewBag.EditorId = new SelectList(db.Users, "Id", "Name", article.EditorId);

            ////select list for status
            //List<SelectListItem> stats = new List<SelectListItem>();
            //stats.Add(new SelectListItem() { Text = "Đang chờ duyệt", Value = "0" });
            //stats.Add(new SelectListItem() { Text = "Đã hiển thị", Value = "1" });
            //stats.Add(new SelectListItem() { Text = "Đã bị ẩn", Value = "2" });
            //ViewBag.Status = new SelectList(stats, "Value", "Text", Input.Status);

            ////chế cái category list để có thêm tùy chọn tất cả
            ////dành cho mục tìm kiếm các bài viết liên quan
            //List<Category> cats = db.Categories.ToList();
            //cats.Insert(0, new Category() { Name = "Tất cả", Id = -1 });
            //ViewBag.CatRelatedId = new SelectList(cats, "Id", "Name");

            return Page();

        }
    }

}
