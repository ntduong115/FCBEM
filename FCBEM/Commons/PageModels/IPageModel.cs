using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.PlatformAbstractions;

namespace FCBEM24.Commons.PageModels
{
    public class IPageModel : PageModel
    {
        [TempData] // Sử dụng Session
        public string? StatusMessage { get; set; }


        public string Language { get; set; }
        public string? Version { get; set; }
        public string? ProjectName { get; set; }
        public string? ProjectYear { get; set; }


        public IConfiguration configuration;

        public IPageModel(IConfiguration configuration)
        {
            ApplicationEnvironment app = PlatformServices.Default.Application;
            Version = app.ApplicationVersion;
            this.configuration = configuration;

            ProjectName = configuration["ProjectName"];
            ProjectYear = configuration["ProjectYear"];
        }


    }
}
