using FCBEMModel.Models.Authorize;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Core.Commons.FCBEMConstants;

namespace FCBEMModel.PaperModels
{

    public partial class PaperNew : IEntityVersion
    {
        public PaperNew()
        {
        }


        [Required]
        public string? Title { get; set; }

        [Required]
        public string? Abstract { get; set; }

        [StringLength(500)]
        public string? ThumbImage { get; set; }

        [DefaultValue(100)]
        public long ViewCount { get; set; }

        public string? Content { get; set; }

        [Required]
        public ArticleStatus? Status { get; set; }

        [StringLength(500)]
        public string? Slug { get; set; }

        [ForeignKey("User")]
        public Guid? UserId { get; set; }

        public User? User { get; set; }

    }
}
