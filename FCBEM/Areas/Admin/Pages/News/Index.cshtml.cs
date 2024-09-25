using FCBEM.Commons.Authorizations;
using FCCore.PageModels;
using Model;
using Model.Models.Authorize;
using Model.PaperModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static Core.Commons.FCConstants;

namespace FCBEM.Areas.Admin.Pages.News
{
    [AuthorizeCustomize(RoleName.Admin)]
    public class IndexModel(UserManager<User> userManager, IWebHostEnvironment environment, DatabaseContext context, IConfiguration configuration) : IReadPageModel<PaperNew>(userManager, context, configuration)
    {
        public SelectList? AuthorId { set; get; }

        protected readonly IWebHostEnvironment environment = environment;

        public override IQueryable<PaperNew> Where(IQueryable<PaperNew> query)
        {
            if (Input != null)
            {

                if (Input.Title != null)
                {

                    query = query.Where(a => !string.IsNullOrEmpty(a.Title) && a.Title.Contains(Input.Title));


                }
                if (Input.Status != null)
                {
                    query = query.Where(a => a.Status == Input.Status);
                }
                if (Input.UserId != null)
                {
                    query = query.Where(a => a.UserId == Input.UserId);
                }
            }
            return base.Where(query);
        }

        public override IQueryable<PaperNew> Include(IQueryable<PaperNew> listData)
        {

            return base.Include(listData);
        }

        public override IQueryable<PaperNew> Sort(IQueryable<PaperNew> listData)
        {
            return base.Sort(listData.OrderByDescending(o => o.CreatedDate));
        }


        public async Task<IActionResult> OnGetAsync()
        {
            await GetSelectListAsync();
            HasListData = true;
            return Page();
        }

        public IActionResult OnPostDelete()
        {

            context.PaperNews.Remove(Input);
            context.SaveChanges();


            if (!string.IsNullOrEmpty(Input.ThumbImage))
            {
                try
                {
                    var pathFile = Path.Combine(environment.WebRootPath, Input.ThumbImage.Replace("/", "\\").Remove(0, 1));
                    // Check if file exists with its full path    
                    FileInfo file = new(pathFile);
                    if (file.Exists)//check file exsit or not  
                    {
                        file.Delete();
                    }
                    else
                    {
                    }
                }
                catch (IOException ioExp)
                {
                    Console.WriteLine(ioExp.Message);
                }
            }

            return RedirectToPage("./Index");
        }


        public async Task GetSelectListAsync()
        {
            var users = await userManager.GetUsersInRoleAsync(RoleName.Admin);
            AuthorId = new SelectList(users, "Id", "Name", User.Identity?.Name ?? string.Empty);

        }
    }
}
