using System.ComponentModel.DataAnnotations;
namespace Core.Models.Systens
{

    public partial class Config(string configKey)
    {
        [Key]

        public string ConfigKey { get; set; } = configKey;
        public string ConfigValue { get; set; } = string.Empty;
    }
}
