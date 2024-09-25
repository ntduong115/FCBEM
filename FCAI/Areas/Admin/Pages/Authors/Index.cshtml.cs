using Core.Interfaces;

using FCAI.Commons.Authorizations;
using FCCore.PageModels;
using Model;
using Model.Models.Authorize;
using Model.PaperModels;
using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Core.Commons.FCConstants;

namespace FCAI.Areas.Admin.Pages.Authors
{
    [AuthorizeCustomize(RoleName.Admin)]
    public class IndexModel(UserManager<User> userManager, IEmailSender emailSender, DatabaseContext context, IConfiguration configuration) : IReadPageModel<Author>(userManager, context, configuration)
    {
        public IActionResult OnGet()
        {
            HasListData = true;
            return Page();
        }

        public override IQueryable<Author> Include(IQueryable<Author> listData)
        {
            listData = listData
                .Include(i => i.Paper);
            return base.Include(listData);
        }
        public override IQueryable<Author> Where(IQueryable<Author> listData)
        {
            listData = listData
                .Where(p => p.PaperId == Id);
            return listData;
        }
        public override IQueryable<Author> Sort(IQueryable<Author> listData)
        {
            return base.Sort(listData.OrderBy(o => o.AuthorNum));
        }
    }
}
