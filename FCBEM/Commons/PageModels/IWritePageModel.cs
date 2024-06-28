using FCBEMModel;
using FCBEMModel.Models.Authorize;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;


namespace FCBEM24.Commons.PageModels
{
    public class IWritePageModel<T> : IChangePageModel where T : class
    {
        protected readonly UserManager<User>? userManager;
        protected readonly IWebHostEnvironment? environment;

        [BindProperty(SupportsGet = true)]
        public T? Input { set; get; }



        public IWritePageModel(UserManager<User>? userManager, IWebHostEnvironment environment, DatabaseContext context, IConfiguration configuration) : base(context, configuration)
        {
            this.userManager = userManager;
            this.environment = environment;
        }

        public IWritePageModel(IWebHostEnvironment? environment, DatabaseContext context, IConfiguration configuration) : base(context, configuration)
        {
            this.environment = environment;
        }

        public IWritePageModel(DatabaseContext context, IConfiguration configuration) : base(context, configuration)
        {
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
