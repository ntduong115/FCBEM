using Core.Interfaces;

using Model.Systems;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
    public class IEntityVersion : IEntity
    {

        [ForeignKey("Version")]
        public string? VersionCode { get; set; }

        public ProjectVersion? Version { get; set; }
    }
}
