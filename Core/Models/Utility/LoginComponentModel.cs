using System.Globalization;

namespace Core.Models.Utility
{
    public class LoginComponentModel
    {
        public CultureInfo CurrentUICulture { get; set; }
        public List<CultureInfo> SupportedCultures { get; set; }
    }
}
