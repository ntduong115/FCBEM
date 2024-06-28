using System.ComponentModel.DataAnnotations;

namespace FCBEMModel.Systems
{
    public class ProjectVersion
    {
        [Key]
        public string Code { get; set; }
        public int Year { get; set; }
    }
}
