using System.ComponentModel.DataAnnotations.Schema;
using AnyApps.Core.Repository.Infrastructure;

namespace AnyApps.Core.Repository.Ef
{
    public abstract class Entity : IObjectState
    {
        [NotMapped]
        public ObjectState ObjectState { get; set; }
    }
}