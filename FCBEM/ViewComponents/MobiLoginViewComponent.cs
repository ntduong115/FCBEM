using Microsoft.AspNetCore.Mvc;


namespace SMIA.ViewComponents
{
    [ViewComponent(Name = "mobiLoginViewComponent")]
    public class MobiLoginViewComponent : ViewComponent
    {
        public MobiLoginViewComponent()
        {
        }

        public IViewComponentResult Invoke()
        {
            return View();
        }

    }
}
