using System.ComponentModel.DataAnnotations.Schema;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using static Core.Commons.FCConstants;
using Model.Models.Authorize;

namespace Model.PaperModels
{
    public class Paper : IEntityVersion
    {
        public int PaperId { get; set; }

        [Required]
        public string ManuscriptTitle { get; set; }

        [NotMapped]
        public string? FullName { get; set; }
        [Required]
        [Display(Name = "Abstract", Prompt = "Write the abstract in English. The Abstract should be informative and completely self-explanatory. The Abstract should be 250 to 300 words in length. The abstract should be written in the past tense. Standard nomenclature should be used, and abbreviations should be avoided. No literature should be cited.")]
        public string Abstract { get; set; }

        public string Submission { get; set; }
        public string File { get; set; }

        public PaperStatus Status { get; set; }

        [Display(Name = "Keywords", Prompt = "Type 3-5 keywords here, separated by commas")]
        public string Keywords { get; set; }

        [ForeignKey("User")]
        public Guid? UserId { get; set; }

        [Display(Name = "User", Prompt = "User")]
        public User? User { get; set; }


        public virtual List<Author>? Authors { get; set; }

        [NotMapped]
        [Display(Name = "Form File", Prompt = "FormFile")]
        [DataType(DataType.Upload)]
        public IFormFile? FormFile { get; set; }

        [NotMapped]
        public string PaperIDText
        {
            get
            {
                return $"{VersionCode}-{PaperId:D3}";
            }
        }

        [NotMapped]
        public string TitleSelectList
        {
            get
            {
                return $"{PaperIDText} : {ManuscriptTitle}";
            }
        }

    }
}
