using FCBEM24.DataAccess;
using FCBEMModel;
using FCBEMModel.PaperModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FCBEM24.ViewComponents
{
    [ViewComponent(Name = "MostViewPaper")]
    public class MostViewPaper(IOptions<RequestLocalizationOptions> options, DatabaseContext context) : ViewComponent
    {
        public IList<PaperNew> PaperNews { get; set; }

        public IViewComponentResult Invoke()
        {
            PaperNews = context.GetMostViewedPaperNews(5).ToList();
            return View(PaperNews);
        }

    }
}
