using System.Drawing;
using System.Drawing.Imaging;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using FCCore.Commons.Authorizations;
using FCCore.PageModels;
using static Core.Commons.FCConstants;
using Model.Models.Authorize;
using Model;
using Core.Models.Utility;
using Core.Commons;
using Model.PaperModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace FCCore.Areas.Admin.Pages.News
{
    [AuthorizeCustomize(RoleName.Admin)]
    public class EditModel(UserManager<User> userManager, IWebHostEnvironment environment, DatabaseContext context, IConfiguration configuration) : INewsPageModel(userManager, context, configuration)
    {
        protected readonly IWebHostEnvironment environment = environment;

        public string? Slug { get; private set; }

        public IActionResult OnGet()
        {
            StatusMessage = string.Empty;
            if (Id == null)
            {
                StatusMessage = new StatusMessage("Không có thông tin về Tin Tức", false).ToJSon();
                return Page();
            }
            Input = context.PaperNews.Include(a => a.User).FirstOrDefault(a => a.Id == Id);
            if (Input == null)
            {
                return RedirectToPage("/Error");
            }

            Input.Content = Input.Content;
            Input.Abstract = Input.Abstract;
            Input.Title = Input.Title;


            return Page();
        }



        //// Cập nhật 
        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {

                if (Input == null)
                {
                    StatusMessage = new StatusMessage("Không Tìm Thấy Tin Tức", false).ToJSon();
                    return RedirectToPage("./Index");
                }
                PaperNew? paperNew = context.PaperNews.Include(a => a.User).FirstOrDefault(a => a.Id == Id);
                if (paperNew == null)
                {
                    StatusMessage = new StatusMessage("Không Tìm Thấy Tin Tức", false).ToJSon();
                    return RedirectToPage("./Index");
                }

                //tải lên ảnh bé cho bài viết
                if (FileUpload != null)
                {
                    Image newImage = Image.FromStream(FileUpload.OpenReadStream());
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
                    if (!string.IsNullOrEmpty(paperNew.ThumbImage))
                    {
                        try
                        {
                            var pathFile = System.IO.Path.Combine(environment.WebRootPath, paperNew.ThumbImage.Replace("/", "\\").Remove(0, 1));
                            // Check if file exists with its full path    
                            FileInfo file = new(pathFile);
                            if (file.Exists)//check file exsit or not  
                            {
                                file.Delete();
                            }
                            else
                            {
                            }
                        }
                        catch (IOException ioExp)
                        {
                            Console.WriteLine(ioExp.Message);
                        }
                    }
                    paperNew.ThumbImage = urlDirect;
                }


                paperNew.Slug = Input.Title.Slugify();
                paperNew.Title = Input.Title;
                paperNew.Abstract = Input.Abstract;
                paperNew.Content = Input.Content;
                paperNew.ViewCount = Input.ViewCount;



                //đặt ngày giờ sửa bài
                Input.UpdatedDate = DateTime.Now;
                paperNew.UpdatedDate = Input.UpdatedDate;


                //lưu vào db
                context.Entry(paperNew).State = EntityState.Modified;
                context.SaveChanges();


                //sửa thành công -> chuyển đến trang index
                StatusMessage = new StatusMessage("Updated", true).ToJSon();
                return RedirectToPage("./Index");
            }

            return Page();
        }
    }
}
