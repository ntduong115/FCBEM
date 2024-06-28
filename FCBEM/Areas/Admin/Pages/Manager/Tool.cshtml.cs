using FCBEM24.BusinessLogic;
using FCBEM24.Commons.Authorizations;
using FCBEM24.Commons.PageModels;

using FCBEMModel;

using Microsoft.AspNetCore.Mvc;

using static Core.Commons.FCBEMConstants;

namespace FCBEM24.Areas.Admin.Pages.Manager
{
    [AuthorizeCustomize(RoleName.Admin)]
    public class ToolModel(DatabaseContext context, IConfiguration configuration, IWebHostEnvironment environment) : IChangePageModel(context, configuration)
    {
        protected readonly IWebHostEnvironment environment = environment;

        public void OnGet()
        {
        }

        public IActionResult OnPostGenerate()
        {
            FileManager.CreateFolderIfNotExists(Path.Combine(environment.WebRootPath, PathUpload.PAPER));
            FileManager.CreateFolderIfNotExists(Path.Combine(environment.WebRootPath, PathUpload.NEWS));
            FileManager.CreateFolderIfNotExists(Path.Combine(environment.WebRootPath, PathUpload.NEWS_THUMB));
            FileManager.CreateFolderIfNotExists(Path.Combine(environment.WebRootPath, PathUpload.TEMP));
            FileManager.CreateFolderIfNotExists(Path.Combine(environment.WebRootPath, PathUpload.REGISTRAION));
            return RedirectToPage("./Tool");
        }
        public IActionResult OnPostReset()
        {
            return RedirectToPage("./Tool");
        }
    }
}
