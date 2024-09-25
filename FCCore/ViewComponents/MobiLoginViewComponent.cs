using Microsoft.AspNetCore.Mvc;


namespace FCCore.ViewComponents
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
