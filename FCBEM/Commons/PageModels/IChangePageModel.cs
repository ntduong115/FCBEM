using FCBEMModel;
using Microsoft.AspNetCore.Mvc;


namespace FCBEM24.Commons.PageModels
{
    public class IChangePageModel(DatabaseContext context, IConfiguration configuration) : IPageModel(configuration)
    {

        [BindProperty(SupportsGet = true)]
        public Guid? Id { get; set; }

        protected readonly DatabaseContext context = context;
    }
}
