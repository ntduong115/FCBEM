using FCBEM.Commons.Authorizations;
using FCCore.PageModels;

using Model;
using Model.Models.Authorize;
using Model.PaperModels;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using System.Security.Claims;

using static Core.Commons.FCConstants;

namespace FCBEM.Areas.Client.Pages.Papers
{
    [AuthorizeCustomize(RoleName.Client)]
    public class IndexModel(UserManager<User> userManager, DatabaseContext context, IConfiguration configuration) : IReadPageModel<Paper>(userManager, context, configuration)
    {
        public SelectList? AuthorId { set; get; }
        public SelectList? CategoriesId { set; get; }

        public override IQueryable<Paper> Where(IQueryable<Paper> query)
        {
            Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid user);
            query = query.Where(x => x.UserId == user);
            return base.Where(query);
        }

        public override IQueryable<Paper> Include(IQueryable<Paper> listData)
        {

            return base.Include(listData);
        }

        public override IQueryable<Paper> Sort(IQueryable<Paper> listData)
        {
            return base.Sort(listData.OrderByDescending(o => o.CreatedDate));
        }

        public async Task<IActionResult> OnGetAsync()
        {
            HasListData = true;
            return Page();
        }

    }
}
