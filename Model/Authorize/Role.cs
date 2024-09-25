using Microsoft.AspNetCore.Identity;

namespace Model.Models.Authorize
{
    public class Role : IdentityRole<Guid>
    {

        public string? UpdatePIC { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
