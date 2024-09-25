using Core.Interfaces;

using System.ComponentModel.DataAnnotations.Schema;

namespace Model.PaperModels
{
    public class Author : IEntity
    {
        public string FirstName { get; set; }

        public string? MiddleName { get; set; }

        public string LastName { get; set; }

        public string Country { get; set; }

        public string Affiliation { get; set; }

        public string Email { get; set; }

        public int AuthorNum { get; set; }

        public string Phone { get; set; }

        [ForeignKey("Paper")]
        public Guid? PaperId { get; set; }

        public Paper? Paper { get; set; }

        public bool IsCorresponding { get; set; }

        [NotMapped]
        public bool IsHidden { get; set; }


    }
}