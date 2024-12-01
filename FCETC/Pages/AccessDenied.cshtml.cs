using FCCore.PageModels;

namespace FCETC.Pages
{
    public class AccessDeniedModel(IConfiguration configuration) : IPageModel(configuration)
    {
        public void OnGet()
        {
        }
    }
}
