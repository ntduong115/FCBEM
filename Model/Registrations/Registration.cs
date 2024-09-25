using Core.Commons.Validations;

using Model.Models.Authorize;
using Model.PaperModels;

using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static Core.Commons.FCConstants;

namespace Model.Registrations
{
    public class Registration : IEntityVersion
    {
        public int RegistrationId { get; set; }

        [Required]
        [Display(Prompt = "First Name")]
        public required string FirstName { get; set; }

        [Display(Prompt = "Middle Name")]
        public string? MiddleName { get; set; }

        [Required]
        [Display(Prompt = "Last Name")]
        public required string LastName { get; set; }

        public Position? Position { get; set; }

        [Display(Prompt = "Student ID")]
        public string? StudentId { get; set; }

        [Required]
        [Display(Prompt = "Affilication")]
        public required string Affilication { get; set; }

        [Required]
        [Display(Prompt = "Country Or Region")]
        public required string CountryOrRegion { get; set; }

        [Required]
        [Display(Prompt = "Email")]
        public required string Email { get; set; }

        [Required]
        [Display(Prompt = "Telephone Number")]
        public required string TelephoneNumber { get; set; }

        [ForeignKey("Paper")]
        public Guid? PaperId { get; set; }

        public Paper? Paper { get; set; }

        public PresentationType? PresentationType { get; set; }

        public RegistrationType RegistrationType { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Value must be a non-negative integer.")]
        public int AuthorRegular5VND { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Value must be a non-negative integer.")]
        public int AuthorRegular3USD { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Value must be a non-negative integer.")]
        public int AuthorRegular45VND { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Value must be a non-negative integer.")]
        public int AuthorRegular25USD { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Value must be a non-negative integer.")]
        public int Listener1VND { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Value must be a non-negative integer.")]
        public int Listener1USD { get; set; }

        [Required]
        [Display(Prompt = "Bill To")]
        public required string BillTo { get; set; }

        [Display(Prompt = "Any Remark")]
        public string? AnyRemark { get; set; }

        public DietType Diet { get; set; }

        [Display(Prompt = "Comment")]
        public string? DietComment { get; set; }

        public string? Files { get; set; }

        public RegistrationStatus Status { get; set; }

        [ForeignKey("User")]
        public Guid? UserId { get; set; }

        public User? User { get; set; }

        [NotMapped]
        public string Total
        {
            get
            {
                int totalVND = (AuthorRegular5VND * 5000000) + (AuthorRegular45VND * 4500000) +
                (Listener1VND * 1000000);


                int totalUSD = (AuthorRegular3USD * 300) + (AuthorRegular25USD * 250) +
                    (Listener1USD * 100);

                return totalVND == 0 ? string.Format("{0:#,##0} USD", totalUSD) : string.Format("{0:#,##0} VND", totalVND);
            }
        }

        [NotMapped]
        [Display(Name = "FormFile", Prompt = "Files Upload")]
        [DataType(DataType.Upload)]
        [AllowedExtensions(Extensions = [".pdf", ".PDF", ".png", ".PNG", ".jpeg", ".JPEG", ".jpg", ".JPG", ".docx", ".DOCX", ".doc", ".DOCX"], ErrorMessage = "File format is not valid")]
        public List<IFormFile>? FormFile { get; set; }



        [NotMapped]
        public string RegistrationIdText
        {
            get
            {
                return $"{VersionCode}-R{RegistrationId:D3}";
            }
        }
    }
}
