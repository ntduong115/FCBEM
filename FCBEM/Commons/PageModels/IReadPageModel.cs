
using Core.Commons;
using Core.Commons.Validations;

using FCBEM24.Commons.Validations;
using FCBEMModel;
using FCBEMModel.Models.Authorize;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;


using System.ComponentModel.DataAnnotations;

namespace FCBEM24.Commons.PageModels
{
    public class IReadPageModel<T> : IChangePageModel where T : class
    {
        protected readonly UserManager<User>? userManager;
        protected readonly RoleManager<Role>? roleManager;

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; }

        public int CountPages { get; set; }

        [BindProperty]
        public int TotalItem { get; set; }

        [BindProperty(SupportsGet = true)]
        public T? Input { set; get; }

        [BindProperty]
        public IList<T> ListData { get; set; }
        public bool HasListData { get; set; }
        public int ItemPerPage { get; set; } = Constants.ITEMS_PER_PAGE;


        [BindProperty]
        [DataType(DataType.Upload)]
        [MaxFileSize(5 * 1024 * 1024)]
        [AllowedExtensions(Extensions = [".jpg", ".png"], ErrorMessage = "File format is not valid")]
        public IFormFile? FileUpload { get; set; }
        public IReadPageModel(DatabaseContext context, IConfiguration configuration) : base(context, configuration)
        {
            ListData = new List<T>();
        }
        public IReadPageModel(UserManager<User> userManager, DatabaseContext context, IConfiguration configuration) : base(context, configuration)
        {
            this.userManager = userManager;
            ListData = new List<T>();
        }
        public IReadPageModel   (RoleManager<Role> roleManager, DatabaseContext context, IConfiguration configuration) : base(context, configuration)
        {
            this.roleManager = roleManager;
            ListData = new List<T>();
        }
        public IReadPageModel(UserManager<User> userManager, RoleManager<Role> roleManager, DatabaseContext context, IConfiguration configuration) : base(context, configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            ListData = new List<T>();
        }

        public virtual IQueryable<T> Where(IQueryable<T> listData)
        {
            return listData;
        }

        public virtual IQueryable<T> Include(IQueryable<T> listData)
        {
            return listData;
        }

        public virtual List<T> LastSelect(List<T> listData)
        {
            return listData;
        }

        public virtual IQueryable<T> Sort(IQueryable<T> listData)
        {
            return listData;
        }

        public override void OnPageHandlerSelected(PageHandlerSelectedContext context)
        {
            base.OnPageHandlerSelected(context);
        }

        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            base.OnPageHandlerExecuting(context);
        }

        public override void OnPageHandlerExecuted(PageHandlerExecutedContext context)
        {
            base.OnPageHandlerExecuted(context);
            if (HasListData)
            {
                var query = Where(this.context.Set<T>().AsQueryable());

                TotalItem = query.Count();
                CountPages = (int)Math.Ceiling((double)TotalItem / ItemPerPage);
                if (PageNumber < 1)
                {
                    PageNumber = 1;
                }
                if (PageNumber > CountPages && CountPages != 0)
                {
                    PageNumber = CountPages;
                }
                ListData = LastSelect(Sort(Include(query)).Skip((PageNumber - 1) * ItemPerPage).Take(ItemPerPage).ToList());
            }
        }

        public override Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        {
            return base.OnPageHandlerSelectionAsync(context);
        }

        public override Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            return base.OnPageHandlerExecutionAsync(context, next);
        }
    }
}
