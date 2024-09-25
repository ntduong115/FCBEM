using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Model.PaperModels;
using Model.Models.Authorize;
using Model;
using static Core.Commons.FCConstants;
using Microsoft.AspNetCore.Builder;

namespace FCCore.ViewComponents
{
    [ViewComponent(Name = "PaperAdjust")]
    public class PaperAdjustViewComponent(IOptions<RequestLocalizationOptions> options, UserManager<User> userManager, DatabaseContext context) : ViewComponent
    {
        protected readonly UserManager<User> userManager = userManager;

        public async Task<IViewComponentResult> InvokeAsync(PaperNew paper, bool isEdit = false)
        {
            var users = await userManager.GetUsersInRoleAsync(RoleName.Admin);
            SelectList AuthorId = new(users, "Id", "Name", User.Identity.Name);

            /*  SelectList EditorId = new(users, "Id", "Name", User.Identity.Name);
              SelectList Tags = new(context.Tags.ToList(), "Id", "Name");*/
            return View(new PaperAdjustModel { Input = paper, AuthorId = AuthorId });
        }

        public class PaperAdjustModel
        {

            public PaperNew Input { get; set; }

            public SelectList AuthorId { get; set; }

        }
    }
}
