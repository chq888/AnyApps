
using System.ComponentModel.DataAnnotations.Schema;

namespace AnyApps.Core.Repository.Infrastructure
{
    public interface IObjectState
    {
        [NotMapped]
        ObjectState ObjectState { get; set; }
    }
}