using FCBEM24.Commons.PageModels;

using Microsoft.AspNetCore.Mvc;

namespace FCBEM24.Pages
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
