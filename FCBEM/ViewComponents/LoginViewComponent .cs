using Microsoft.AspNetCore.Mvc;


namespace SMIA.ViewComponents
{
    [ViewComponent(Name = "LoginViewComponent")]
    public class LoginViewComponent : ViewComponent
    {
        public LoginViewComponent()
        {
        }

        public IViewComponentResult Invoke()
        {
            return View();
        }

    }
}
