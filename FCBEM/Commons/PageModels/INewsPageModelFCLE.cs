using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


using System.ComponentModel.DataAnnotations;
using FCBEMModel.PaperModels;
using FCBEMModel;
using FCBEMModel.Models.Authorize;
using FCBEM24.Commons.Validations;

namespace FCBEM24.Commons.PageModels
{
    public class INewsPageModelFCBEM(UserManager<User> userManager, DatabaseContext context, IConfiguration configuration) : IChangePageModel(context, configuration)
    {
        protected readonly UserManager<User> userManager = userManager;

        [BindProperty(SupportsGet = true)]
        public PaperNew? Input { set; get; } = new PaperNew();

        [BindProperty]
        [DataType(DataType.Upload)]
        [MaxFileSize(5 * 1024 * 1024)]
        public IFormFile? FileUpload { get; set; }
    }
}
