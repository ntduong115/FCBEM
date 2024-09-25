using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using Core.Models.Utility;
using Microsoft.AspNetCore.Builder;


namespace FCCore.ViewComponents
{
    [ViewComponent(Name = "StatusMessage")]
    public class StatusMessageViewComponent(IOptions<RequestLocalizationOptions> options) : ViewComponent
    {
        public IViewComponentResult Invoke(string message)
        {
            return View(!string.IsNullOrEmpty(message) ? StatusMessage.FromJSon(message) : null);
        }

    }
}
