using FCCore.PageModels;

namespace FCETC.Pages
{
    public class AbstractBooksModel(IConfiguration configuration) : IPageModel(configuration)
    {
        public void OnGet()
        {
        }
    }
}
