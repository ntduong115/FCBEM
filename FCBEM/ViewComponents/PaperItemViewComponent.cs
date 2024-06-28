using FCBEMModel.PaperModels;
using Microsoft.AspNetCore.Mvc;


namespace FCBEM24.ViewComponents
{
    [ViewComponent(Name = "PaperItem")]
    public class PaperItemViewComponent : ViewComponent
    {
        public PaperItemViewComponent()
        {
        }

        public IViewComponentResult Invoke(PaperNew paper, string domain)
        {
            return View(new PaperItemModel { PaperNew = paper, Domain = domain });
        }

        public class PaperItemModel
        {
            public PaperNew? PaperNew { get; set; }
            public string? Domain { get; set; }
        }
    }
}
