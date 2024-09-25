using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


using System.ComponentModel.DataAnnotations;
using Model.PaperModels;
using Model;
using Model.Models.Authorize;
using Core.Commons.Validations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace FCCore.PageModels
{
    public class INewsPageModel(UserManager<User> userManager, DatabaseContext context, IConfiguration configuration) : IChangePageModel(context, configuration)
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
