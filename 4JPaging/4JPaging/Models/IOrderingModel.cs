using System.Collections.Generic;

namespace DV.ProjectMaster.Common.Data.Models
{
    public interface IOrderingModel
    {
        IDictionary<string, string> OrderBy { get; set; }
    }
}
