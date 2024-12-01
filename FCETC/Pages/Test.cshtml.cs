using FCCore.PageModels;

using Microsoft.AspNetCore.Mvc;

namespace FCETC.Pages
{
    public class TestModel(IConfiguration configuration) : IPageModel(configuration)
    {
        public string Text { get; set; }
        public IActionResult OnGet()
        {
            Text = HttpContext.Request.PathBase;
            return Redirect(HttpContext.Request.PathBase+"/Admin");
        }
    }
}
