using FCAI.Commons.Authorizations;
using FCCore.PageModels;

using Model;
using Model.Models.Authorize;
using Model.Registrations;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Security.Claims;

using static Core.Commons.FCConstants;

namespace FCAI.Areas.Client.Pages.Registrations
{
    [AuthorizeCustomize(RoleName.Client)]
    public class IndexModel(UserManager<User> userManager, RoleManager<Role> roleManager, DatabaseContext context, IConfiguration configuration) : IReadPageModel<Registration>(userManager, roleManager, context, configuration)
    {
        public override IQueryable<Registration> Where(IQueryable<Registration> query)
        {
            Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid user);
            query = query.Where(x => x.UserId == user);
            return base.Where(query);
        }

        public override IQueryable<Registration> Include(IQueryable<Registration> listData)
        {
            listData = listData.Include(i => i.Paper);
            return base.Include(listData);
        }

        public override IQueryable<Registration> Sort(IQueryable<Registration> listData)
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
