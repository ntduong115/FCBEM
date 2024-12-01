using Model;

namespace FCBEM.Areas.Admin.Pages.Manager
{
    public class ToolModel(DatabaseContext context, IConfiguration configuration, IWebHostEnvironment environment) : FCCore.Areas.Admin.Pages.Manager.ToolModel(context, configuration, environment)
    {
    }
}
