using Core.Interfaces;

using FCBEMModel.Systems;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCBEMModel
{
    public class IEntityVersion : IEntity
    {

        [ForeignKey("Version")]
        public string? VersionCode { get; set; }

        public ProjectVersion? Version { get; set; }
    }
}
