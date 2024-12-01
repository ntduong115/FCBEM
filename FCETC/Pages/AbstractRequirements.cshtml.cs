using FCCore.PageModels;

namespace FCETC.Pages
{
    public class AbstractRequirementsModel(IConfiguration configuration) : IPageModel(configuration)
    {
        public void OnGet()
        {
        }
    }
}
