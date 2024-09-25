using Model.PaperModels;
using Model.Registrations;

using Microsoft.AspNetCore.Identity;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Models.Authorize
{
    public class User : IdentityUser<Guid>
    {
        public string Code { get; set; }

        public string Name { get; set; }

        [DataType(DataType.Password)]
        public virtual string? Password { get; set; }

        [NotMapped]
        [DataType(DataType.Password)]
        public virtual string? ConfirmPassword { get; set; }

        [StringLength(255)]
        public override string? Email { get; set; }

        public override string? PhoneNumber { get; set; }

        [StringLength(255)]
        public string? IdentityNumber { get; set; }


        public string? UpdatedPIC { get; set; }
        public DateTime UpdatedDate { get; set; }

        public virtual ICollection<Paper>? Papers { get; set; }
        public virtual ICollection<Registration>? Registrations { get; set; }
    }
}
