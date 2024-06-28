using FCBEMModel;
using FCBEMModel.Models.Authorize;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;


using System.ComponentModel.DataAnnotations;


namespace FCBEM24.Commons.PageModels
{
    public class IUserPageModel(UserManager<User> userManager, DatabaseContext context, IConfiguration configuration) : IChangePageModel(context, configuration)
    {

        protected readonly UserManager<User> userManager = userManager;
        public SelectList Unit { set; get; }
        public SelectList Area { set; get; }

        public class InputModel
        {
            public Guid ID { get; set; }

            [Display(Name = "Code", Prompt = "Promt_Code")]
            public string Code { get; set; }

            [Display(Name = "Name", Prompt = "Promt_Name")]
            public string Name { get; set; }

            [StringLength(255)]
            [Display(Name = "Email", Prompt = "Promt_Email")]
            public string Email { get; set; }

            [Display(Name = "PhoneNumber", Prompt = "Promt_PhoneNumber")]
            public string PhoneNumber { get; set; }

            [Display(Name = "IdentityNumber", Prompt = "Promt_IdentityNumber")]
            [StringLength(255)]
            public string IdentityNumber { get; set; }

            [Display(Name = "BankAccountNumber", Prompt = "Promt_BankAccountNumber")]
            [StringLength(255)]
            public string BankAccountNumber { get; set; }

            [Display(Name = "BankAccountName", Prompt = "Promt_BankAccountName")]
            [StringLength(255)]
            public string BankAccountName { get; set; }

            [Display(Name = "BankName", Prompt = "Promt_BankName")]
            [StringLength(255)]
            public string BankName { get; set; }

            [Display(Name = "BankBranch", Prompt = "Promt_BankBranch")]
            [StringLength(255)]
            public string BankBranch { get; set; }

            [Display(Name = "IsLector", Prompt = "Promt_IsLector")]
            public bool IsLector { get; set; }
        }
    }
}
