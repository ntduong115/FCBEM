using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Core.Commons.Validations;


namespace FCBEM24.ViewComponents
{
    [ViewComponent(Name = "FileUpload")]
    public class FileUploadViewComponent : ViewComponent
    {
        public FileUploadViewComponent()
        {
        }

        public IViewComponentResult Invoke(bool isEdit, List<string> urls)
        {
            var domain = Request.Scheme + "://" + Request.Host.Value + Request.PathBase.Value + "/";
            return View(new FileUploadModel { IsEdit = isEdit, Domain = domain, Urls = urls });
        }

        public class FileUploadModel
        {

            public InputClass Input { get; set; }
            public bool IsEdit { get; set; }
            public List<string> Urls { get; set; }
            public string Domain { get; set; }
        }

        public class InputClass
        {
            [DataType(DataType.Upload)]
            [AllowedExtensions(Extensions = [".docx", ".DOCX", ".doc", ".DOCX"], ErrorMessage = "File format is not valid")]
            [Display(Name = "Form File")]
            public List<IFormFile>? FormFile { get; set; }
        }

    }

}
