﻿using Core.Commons.Validations;

using Model;
using Model.Models.Authorize;
using Model.PaperModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;


using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace FCCore.ViewComponents
{
    [ViewComponent(Name = "PaperViewComponent")]
    public class PaperViewComponent(IOptions<RequestLocalizationOptions> options, DatabaseContext context) : ViewComponent
    {
        protected readonly DatabaseContext context = context;
        protected readonly UserManager<User> userManager;

        public IViewComponentResult Invoke(PaperNew paper, IFormFile fileUpload)
        {
            return View(new PaperViewModel { Input = paper, FileUpload = fileUpload });
        }

        public class PaperViewModel
        {
            public PaperNew Input { get; set; }


            [BindProperty]
            [DataType(DataType.Upload)]
            [MaxFileSize(5 * 1024 * 1024)]
            [AllowedExtensions(Extensions = [".jpg", ".png"], ErrorMessage = "File format is not valid")]
            public IFormFile FileUpload { get; set; }
        }
    }
}
