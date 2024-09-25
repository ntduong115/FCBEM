using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.PlatformAbstractions;

namespace FCCore.ViewComponents
{
    [ViewComponent(Name = "MessagePage")]
    public class MessagePageViewComponent : ViewComponent
    {
        public const string COMPONENTNAME = "MessagePage";
        public class Message
        {
            public string Title { set; get; } = "Message";
            public string Htmlcontent { set; get; } = "";
            public string Urlredirect { set; get; } = "/";
            public string ReturnUrl { set; get; } = "/";
            public int Secondwait { set; get; } = 0;
            public string? Version { get; set; }
            public string? Menus { get; set; }// Not use, refactor later
            public string? ProjectName { get; set; }
            public string? ProjectYear { get; set; }
        }
        public MessagePageViewComponent() { }
        public IViewComponentResult Invoke(Message message)
        {
            ApplicationEnvironment app = PlatformServices.Default.Application;
            message.Version = app.ApplicationVersion;
            this.HttpContext.Response.Headers.Add("REFRESH", $"{message.Secondwait};URL={message.Urlredirect}?ReturnUrl={message.ReturnUrl}");
            return View(message);
        }
    }

}
