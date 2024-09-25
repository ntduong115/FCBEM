using FCCore.DataAccess;
using Model;
using Model.PaperModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Builder;

namespace FCCore.ViewComponents
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
