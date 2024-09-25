using Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;


namespace FCCore.PageModels
{
    public class IChangePageModel(DatabaseContext context, IConfiguration configuration) : IPageModel(configuration)
    {

        [BindProperty(SupportsGet = true)]
        public Guid? Id { get; set; }

        protected readonly DatabaseContext context = context;
    }
}
