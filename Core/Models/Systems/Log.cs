using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models.Systems
{
    public class Log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string? LogLevel { get; set; }
        public int ThreadId { get; set; }
        public int EventId { get; set; }
        public string? EventName { get; set; }
        public string? Message { get; set; }
        public string? Values { get; set; }
        public DateTime Created { get; set; }
    }
}
